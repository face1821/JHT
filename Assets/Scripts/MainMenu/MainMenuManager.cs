using System;
using System.Collections;
using Game.LoadingMenu;
using Game.Map;
using Game.System;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private OverlayFadeEffect _overlay;
        [SerializeField] private GameObject _btnContinue;
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
            if (ES3.Load($"Level-0", false))
            {
                _btnContinue.SetActive(true);
            }
        }

        #region 主界面

        public void NewGame()
        {
            //先删除所有关卡和所有成就的记录
            for (int i = 0; i < LevelCount; i++)
            {
                ES3.Save($"Level-{i + 1}", false);
            }

            var achievementConfig = Resources.Load<AchievementConfig>("Datas/AchievementConfig");
            foreach (var item in achievementConfig.Achievements)
            {
                ES3.Save($"Achievement-{item.Name}", false);
            }

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
            var value = ES3.Load("SfxToggle", true);
            ES3.Save("SfxToggle", toggle.interactable);

            _audioSystem.SetSfxVolume(value ? 1f : 0f);
            MLogger.Log($"音频系统：音效（{(value ? "开启" : "关闭")}）");
        }

        public void ToggleMusic(Toggle toggle)
        {
            var value = ES3.Load("MusicToggle", true);
            ES3.Save("MusicToggle", toggle.interactable);

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