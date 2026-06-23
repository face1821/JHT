using Game.System;
using Maxy.GameFramework.Common.System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private int LevelCount;

        private IAudioSystem _audioSystem;

        private void Awake() { _audioSystem = SystemCenter.Get<IAudioSystem>(); }

        #region 主界面

        public void NewGame()
        {
            //先删除所有关卡和所有成就的记录
            for (int i = 0; i < LevelCount; i++)
            {
                ES3.Save($"Level-{i}", false);
            }

            var achievementConfig = Resources.Load<AchievementConfig>("Datas/AchievementConfig");
            foreach (var item in achievementConfig.Achievements)
            {
                ES3.Save($"Achievement-{item.Name}", false);
            }

            //然后进入地图
            SceneManager.LoadScene("Map");
        }

        public void Continue()
        {
            //直接进入地图
            SceneManager.LoadScene("Map");
        }

        #endregion

        #region 设置

        public void ToggleSfx()
        {
            var value = ES3.Load("SfxToggle", true);
            ES3.Save("SfxToggle", !value);

            _audioSystem.SetMusicVolume(value ? 1f : 0f);
        }

        public void ToggleMusic()
        {
            var value = ES3.Load("MusicToggle", true);
            ES3.Save("MusicToggle", !value);

            _audioSystem.SetMusicVolume(value ? 1f : 0f);
        }

        #endregion
    }
}