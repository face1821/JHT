using System;
using Maxy.GameFramework.Common.System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Map
{
    public class SettingMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _musicPic;
        [SerializeField] private GameObject _sfxPic;

        private IAudioSystem _audioSystem;

        private void Awake()
        {
            _audioSystem = SystemCenter.Get<IAudioSystem>();

            _musicPic.SetActive(SaveSystem.Load("MusicToggle", true));
            _sfxPic.SetActive(SaveSystem.Load("SfxToggle", true));
        }

        public void ToggleSfx(Toggle toggle)
        {
            var value = toggle.isOn;
            SaveSystem.Save("SfxToggle", value);

            _audioSystem.SetSfxVolume(value ? 1f : 0f);
            MLogger.Log($"音频系统：音效（{(value ? "开启" : "关闭")}）");
        }

        public void ToggleMusic(Toggle toggle)
        {
            var value = toggle.isOn;
            SaveSystem.Save("MusicToggle", value);

            _audioSystem.SetMusicVolume(value ? 1f : 0f);
            MLogger.Log($"音频系统：音乐（{(value ? "开启" : "关闭")}）");
        }

        public void SetMusicPicAsToggleValue(Toggle toggle) { _musicPic.SetActive(toggle.isOn); }

        public void SetSfxPicAsToggleValue(Toggle toggle) { _sfxPic.SetActive(toggle.isOn); }
    }
}