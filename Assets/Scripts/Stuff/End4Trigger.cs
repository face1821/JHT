using System.Collections;
using Game.LoadingMenu;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Game.Stuff
{
    public class End4Trigger : MonoBehaviour
    {
        [SerializeField] private GameObject _endVideoCanvas;
        [SerializeField] private VideoPlayer _videoPlayer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            StartCoroutine(nameof(OnTrigger));
        }

        private IEnumerator OnTrigger()
        {
            //禁用玩家的交互和移动
            InstanceFinder.Player.Input.enabled = false;
            InstanceFinder.Player.Interact.enabled = false;

            //黑幕出现
            var overlay = GameObject.FindWithTag("SceneOverlay").GetComponent<OverlayFadeEffect>();
            overlay.PlayFadeOut();
            yield return new WaitForSeconds(1f);

            //暂停音乐
            SystemCenter.Get<IAudioSystem>().StopMusic();
            
            //播放视频
            _endVideoCanvas.SetActive(true);
            overlay.PlayFadeIn(0f);
            _videoPlayer.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            //等待视频播放完毕，黑幕出现
            yield return new WaitUntil(() => !_videoPlayer.isPlaying);
            overlay.PlayFadeOut();

            //然后回到主界面
            LoadingMenuManager.LoadingScene = "MainMenu";
            SceneManager.LoadScene("LoadingMenu");
        }
    }
}