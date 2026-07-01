using System;
using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.Map;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.CheckPoint
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Light2D), typeof(BoxCollider2D))]
    public class CheckPoint : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private LevelInfo currentLevelInfo;
        [SerializeField] private GameObject _openedCheckPoint;
        private OverlayFadeEffect _overlayTip;
        private Light2D _highLight;

        private void Awake()
        {
            _overlayTip = GameObject.FindWithTag("OverlaySaveTip").GetComponent<OverlayFadeEffect>();
            _highLight = GetComponent<Light2D>();
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
            gameObject.SetActive(false);
            EventBus.Publish(new RemovePlayerInteractableObjectEvent(this));
            _openedCheckPoint.SetActive(true);

            //存档，记录当前关卡已经通关
            //因为该检查点是为了告知玩家这个关卡已经完成，因此：该检查点应该放置在该关卡的终点位置
            ES3.Save($"Level-{currentLevelInfo.LevelIndex + 1}", true);
        }

        #endregion
    }
}