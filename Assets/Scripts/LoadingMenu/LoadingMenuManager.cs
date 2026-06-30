using System;
using System.Collections;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.LoadingMenu
{
    public class LoadingMenuManager : MonoBehaviour
    {
        public static string LoadingScene;

        [SerializeField] private float _delayLoadTime;
        [SerializeField] private OverlayFadeEffect OverLay;
        [SerializeField] private TextDisplayLoop TextDisplay;

        private void Start()
        {
            OverLay.PlayFadeIn();
            TextDisplay.Play();

            StartCoroutine(nameof(DelayToStart));
        }

        private IEnumerator DelayToStart()
        {
            yield return new WaitForSeconds(_delayLoadTime);

            //异步加载指定的场景
            var handle = SceneManager.LoadSceneAsync(LoadingScene, LoadSceneMode.Additive);
            
            if (handle == null) yield break;
            
            handle.allowSceneActivation = false;
            while (handle.progress < 0.9f)
            {
                Debug.Log("加载进度：" + handle.progress);
                yield return null;
            }

            // 3. 【核心】分帧激活，不要一帧跑完
            yield return new WaitForSeconds(0.1f);
            handle.allowSceneActivation = true;
            
            yield return new WaitUntil(() => handle.isDone);
            MLogger.Log("场景：加载完毕！");
            
            //加载完毕后移除加载界面场景
            handle.completed += _ => SceneManager.UnloadSceneAsync("LoadingMenu");
        }
    }
}