using System;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        private IAudioSystem _audioSystem;

        private void Awake() { _audioSystem = SystemCenter.Get<IAudioSystem>(); }

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
    }
}