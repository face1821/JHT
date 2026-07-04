using System.Collections;
using System.Collections.Generic;
using Game.LoadingMenu;
using Game.Map;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private OverlayFadeEffect _overlay;
        [SerializeField] private GameObject _btnContinue;
        [SerializeField] private AudioClip _uiEmptyClick;
        [Space]
        [SerializeField] private int LevelCount;

        private IAudioSystem _audioSystem;

        private void Awake()
        {
            _audioSystem = SystemCenter.Get<IAudioSystem>();

            Application.targetFrameRate = 240;
            _overlay.PlayFadeIn();
        }

        private void Start()
        {
            //如果玩家第一关有通过，那就显示继续按钮
            if (ES3.Load($"LastPassedLevel", 0) > 0)
            {
                _btnContinue.SetActive(true);
            }
        }

        private void Update()
        {
            //如果按到了UI对象
            var ui = GetClickUIObj();
            if (ui != null)
            {
                //点非按钮UI时发出空击音效
                if (!ui.CompareTag("UIButton"))
                    _audioSystem.PlaySfx(_uiEmptyClick, "_uiEmptyClick", null, 0f);
            }
        }

        // 获取触屏点击到的UI物体
        private GameObject GetClickUIObj()
        {
            GameObject delayResult = null;
            GameObject obj = null;

            foreach (var t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    PointerEventData data = new PointerEventData(EventSystem.current);
                    data.position = t.position;
                    List<RaycastResult> resList = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(data, resList);

                    //先遍历所有，优先返回带UIButton标签的
                    foreach (var res in resList)
                    {
                        if (res.gameObject.CompareTag("UIButton"))
                        {
                            obj = res.gameObject;
                            break;
                        }
                    }

                    // 没有按钮，就再等等
                    if (resList.Count > 0)
                        delayResult = resList[0].gameObject;
                }
            }

            //如果obj不为null，也就是说多个手指中存在UIButton，就返回它
            if (obj != null)
                return obj;
            
            //如果为null，就只返回最后一个遍历到的普通UI，反正都一样
            return delayResult;
        }

        #region 主界面

        public void NewGame()
        {
            //先删除所有关卡和所有成就的记录
            ES3.DeleteDirectory("");

            //然后进入地图
            MapManager.IsNewGame = true;
            StartCoroutine(nameof(DelayStartGame));
        }

        public void Continue()
        {
            //直接进入地图
            StartCoroutine(nameof(DelayStartGame));
        }

        public void Exit() { Application.Quit(); }

        #endregion

        #region 设置

        public void ToggleSfx(Toggle toggle)
        {
            var value = toggle.isOn;
            ES3.Save("SfxToggle", value);

            _audioSystem.SetSfxVolume(value ? 1f : 0f);
            MLogger.Log($"音频系统：音效（{(value ? "开启" : "关闭")}）");
        }

        public void ToggleMusic(Toggle toggle)
        {
            var value = toggle.isOn;
            ES3.Save("MusicToggle", value);

            _audioSystem.SetMusicVolume(value ? 1f : 0f);
            MLogger.Log($"音频系统：音乐（{(value ? "开启" : "关闭")}）");
        }

        #endregion

        private IEnumerator DelayStartGame()
        {
            _overlay.PlayFadeOut();

            yield return new WaitForSeconds(1f);

            LoadingMenuManager.LoadingScene = "Map";
            SceneManager.LoadScene("LoadingMenu");
        }
    }
}