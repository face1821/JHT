using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    public interface IAudioSystem : ISystem
    {
        public bool IsMuteMusic { get; set; }
        public bool IsMuteSfx { get; set; }
        public bool IsMuteVoice { get; set; }
        public bool IsMuteAmbient { get; set; }

        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SfxVolume { get; set; }
        public float VoiceVolume { get; set; }
        public float AmbientVolume { get; set; }

        #region Music

        public void MuteMusic();
        public void UnmuteMusic();
        public void PlayMusic(AudioClip clip, bool loop = true, bool withFadeOutAndIn = true);
        public void StopMusic(bool withFadeOut = true);
        public void PauseMusic(bool withFadeOut = true);
        public void UnPauseMusic(bool withFadeIn = true);

        #endregion

        #region Sfx

        public void MuteSfx();
        public void UnmuteSfx();
        public void PlaySfx(AudioClip clip, string clipName = "Sfx_Clip", Transform objectToFollow = null, float volume = -1f);
        public void PlaySfxAt(AudioClip clip, Vector3 pos = default, string clipName = "Sfx_Clip", float volume = -1f);
        public void PauseSfx(string clipName);
        public void UnPauseSfx(string clipName);
        public void StopSfx(string clipName);
        public void StopAllSfxs();
        public void PauseAllSfxs();
        public void UnPauseAllSfxs();

        #endregion

        #region Voice

        public void MuteVoice();
        public void UnmuteVoice();
        public void PlayVoice(AudioClip clip, string voiceName = "Voice_Clip", Transform objectToFollow = null, float volume = -1f);
        public void PlayVoiceAt(AudioClip clip, Vector3 pos = default, string clipName = "Voice_Clip", float volume = -1f);
        public void StopVoice(string voiceName);
        public void PauseVoice(string voiceName);
        public void UnPauseVoice(string voiceName);
        public void PauseAllVoices();
        public void UnPauseAllVoices();
        public void StopAllVoices();

        #endregion

        #region Ambient

        public void MuteAmbient();
        public void UnmuteAmbient();
        public void PlayAmbient(AudioClip clip, string ambientName = "Ambient_Clip", bool loop = false, Transform objectToFollow = null, float volume = -1f);
        public void PlayAmbientAt(AudioClip clip, Vector3 pos = default, string clipName = "Ambient_Clip", float volume = -1f);
        public void StopAmbient(string ambientName);
        public void PauseAmbient(string voiceName);
        public void UnPauseAmbient(string voiceName);
        public void PauseAllAmbients();
        public void UnPauseAllAmbients();
        public void StopAllAmbients();

        #endregion

        public void Mute(bool muteMusic = true, bool muteSfx = true, bool muteVoice = true, bool muteAmbient = true);
        public void UnMute();
        public void Pause(bool pauseMusic = true, bool pauseSfx = true, bool pauseVoice = true, bool pauseAmbient = true);
        public void UnPause();
        
        public void SetMasterVolume(float volume);
        public void SetMusicVolume(float volume);
        public void SetSfxVolume(float volume);
        public void SetVoiceVolume(float volume);
        public void SetAmbientVolume(float volume);
    }
}