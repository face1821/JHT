using System.Collections;
using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.LoadingMenu;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Game.Suff
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer), typeof(Light2D))]
    public class ChooseEndButton : MonoBehaviour, IInteractableObject
    {
        public bool IsActive => gameObject.activeSelf;

        [SerializeField] private AudioClip _clip;
        [SerializeField] private GameObject _endVideoCanvas;
        [SerializeField] private OverlayFadeEffect _videoOverlay;
        [SerializeField] private VideoPlayer _videoPlayer;

        private Light2D _light;

        private void Awake() { _light = GetComponent<Light2D>(); }


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

        public void SetHighLight(bool state) { _light.enabled = state; }

        public float GetDistance() => Vector3.Distance(transform.position, InstanceFinder.Player.transform.position);

        public void Interact() { StartCoroutine(nameof(DelayInteract)); }

        private IEnumerator DelayInteract()
        {
            //禁用玩家的交互和移动
            InstanceFinder.Player.Input.enabled = false;
            InstanceFinder.Player.Interact.enabled = false;
            SystemCenter.Get<IAudioSystem>().PlaySfx(_clip, "_button_open_clip", InstanceFinder.Player.transform, 0f);

            //黑幕出现
            var overlay = GameObject.FindWithTag("SceneOverlay").GetComponent<OverlayFadeEffect>();
            overlay.PlayFadeOut();
            yield return new WaitForSeconds(1f);

            //暂停音乐
            SystemCenter.Get<IAudioSystem>().StopMusic();

            //播放视频
            _endVideoCanvas.SetActive(true);
            _videoOverlay.PlayFadeIn();
            _videoPlayer.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            //等待视频播放完毕，黑幕出现
            yield return new WaitUntil(() => !_videoPlayer.isPlaying);
            overlay.PlayFadeOut();

            //然后回到主界面
            LoadingMenuManager.LoadingScene = "MainMenu";
            SceneManager.LoadScene("LoadingMenu");
        }

        #endregion
    }
}