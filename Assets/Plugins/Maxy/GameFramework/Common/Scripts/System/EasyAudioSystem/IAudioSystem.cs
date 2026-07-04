using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    public interface IAudioSystem : ISystem
    {
        public float SpaceBlend { get; set; }
        
        #region Music

        public void PlayMusic(AudioClip clip, bool loop = true, bool withFadeOutAndIn = true);
        public void StopMusic(bool withFadeOut = true);
        public void PauseMusic(bool withFadeOut = true);
        public void UnPauseMusic(bool withFadeIn = true);

        #endregion
        
        #region Sfx

        public void PlaySfx(AudioClip clip, string clipName = null, Transform objectToFollow = null);
        public void PlaySfxAt(AudioClip clip, Vector3 pos, string clipName);
        public void StopSfx(string clipName);
        public void StopAllSfxs();
        
        #endregion
        
        #region Voice

        public void PlayVoice(AudioClip clip, string voiceName = null, Transform objectToFollow = null);
        public void StopVoice(string voiceName);
        public void PauseVoice(string voiceName);
        public void UnPauseVoice(string voiceName);
        public void StopAllVoices();
        
        #endregion
        
        #region Ambient

        public void PlayAmbient(AudioClip clip, string ambientName, bool loop = false, Transform objectToFollow = null);
        public void StopAmbient(string ambientName);
        public void PauseAmbient(string voiceName);
        public void UnPauseAmbient(string voiceName);
        public void StopAllAmbients();
        
        #endregion

        public void SetMasterVolume(float volume);
        public void SetMusicVolume(float volume);
        public void SetSfxVolume(float volume);
        public void SetVoiceVolume(float volume);
        public void SetAmbientVolume(float volume);
    }
}