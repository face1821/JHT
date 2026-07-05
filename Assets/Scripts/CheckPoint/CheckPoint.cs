using System.Collections;
using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.Map;
using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.CheckPoint
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Light2D), typeof(BoxCollider2D))]
    public class CheckPoint : MonoBehaviour, IInteractableObject
    {
        public bool IsActive => gameObject.activeSelf;

        [SerializeField] private LevelInfo currentLevelInfo;
        [SerializeField] private GameObject _openedCheckPoint;
        [SerializeField] private AudioClip _clip;
        private OverlayFadeEffect _overlayTip;
        private Light2D _highLight;

        private void Awake()
        {
            _overlayTip = GameObject.FindWithTag("OverlaySaveTip").GetComponent<OverlayFadeEffect>();
            _highLight = GetComponent<Light2D>();
        }

        private void Start()
        {
            if (SaveSystem.Load("LastPassedLevel", 0) - 1 < currentLevelInfo.LevelIndex) return;
            MLogger.LogWarning($"{currentLevelInfo.name} 存档点{currentLevelInfo.LevelIndex + 1}：发现最新存档点是第{SaveSystem.Load("LastPassedLevel", 0)}个，已经存档过自己了", this);

            gameObject.SetActive(false);
            EventBus.Publish(new RemovePlayerInteractableObjectEvent(this));
            _openedCheckPoint.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            EventBus.Publish(new AddPlayerInteractableObjectEvent(this));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            EventBus.Publish(new RemovePlayerInteractableObjectEvent(this));
        }
        
        #region 交互

        public void SetHighLight(bool state) { _highLight.enabled = state; }

        public float GetDistance() { return (InstanceFinder.Player.transform.position - transform.position).magnitude; }

        public void Interact()
        {
            //提示存档成功
            _overlayTip.PlayFadeOutAndIn();

            //存档点只能存档一次
            EventBus.Publish(new RemovePlayerInteractableObjectEvent(this));
            gameObject.SetActive(false);
            _openedCheckPoint.SetActive(true);

            //存档，记录当前关卡已经通关
            //因为该检查点是为了告知玩家这个关卡已经完成，因此：该检查点应该放置在该关卡的终点位置
            SaveSystem.Save($"Level-{currentLevelInfo.LevelIndex + 1}", true);
            SaveSystem.Save("LastPassedLevel", currentLevelInfo.LevelIndex + 1);
            MLogger.LogWarning($"存档：到达第{currentLevelInfo.LevelIndex + 1}个存档点");

            SystemCenter.Get<IAudioSystem>().PlaySfx(_clip, "checkPoint_clip", InstanceFinder.Player.transform, 0f);

            //存档后，将之前的关卡都给关闭掉
            //不需要了
            //GameObject.FindWithTag("MapManager").GetComponent<MapManager>().ClosePassedLevels();
        }

        #endregion
    }
}