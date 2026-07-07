using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Maxy.GameFramework.Common.System
{
    public class EasyAudioSystem : System<EasyAudioSystem>, IAudioSystem
    {
        #region 字段

        public AudioMixer GlobalAudioMixer;

        #region 公开属性

        public bool IsMuteMusic
        {
            get => _isMuteMusic;
            set
            {
                _isMuteMusic = value;
                if (_isMuteMusic)
                    MuteMusic();
                else
                    UnmuteMusic();
            }
        }
        private bool _isMuteMusic;
        public bool IsMuteSfx
        {
            get => _isMuteSfx;
            set
            {
                _isMuteSfx = value;
                if (_isMuteSfx)
                    MuteSfx();
                else
                    UnmuteSfx();
            }
        }
        private bool _isMuteSfx;
        public bool IsMuteVoice
        {
            get => _isMuteVoice;
            set
            {
                _isMuteVoice = value;
                if (_isMuteVoice)
                    MuteVoice();
                else
                    UnmuteVoice();
            }
        }
        private bool _isMuteVoice;
        public bool IsMuteAmbient
        {
            get => _isMuteAmbient;
            set
            {
                _isMuteAmbient = value;
                if (_isMuteAmbient)
                    MuteAmbient();
                else
                    UnmuteAmbient();
            }
        }
        private bool _isMuteAmbient;

        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                SaveSystem.Save("MasterVolume", value);
                _masterVolume = value;
                GlobalAudioMixer.SetFloat("MasterVolume", ToDB(_masterVolume));
            }
        }
        private float _masterVolume;
        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                SaveSystem.Save("MusicVolume", value);
                _musicVolume = value;
                GlobalAudioMixer.SetFloat("MusicVolume", ToDB(_musicVolume));
            }
        }
        private float _musicVolume;
        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                SaveSystem.Save("SfxVolume", value);
                _sfxVolume = value;
                GlobalAudioMixer.SetFloat("SfxVolume", ToDB(_sfxVolume));
            }
        }
        private float _sfxVolume;
        public float VoiceVolume
        {
            get => _voiceVolume;
            set
            {
                SaveSystem.Save("VoiceVolume", value);
                _voiceVolume = value;
                GlobalAudioMixer.SetFloat("VoiceVolume", ToDB(_voiceVolume));
            }
        }
        private float _voiceVolume;
        public float AmbientVolume
        {
            get => _ambientVolume;
            set
            {
                SaveSystem.Save("AmbientVolume", value);
                _ambientVolume = value;
                GlobalAudioMixer.SetFloat("AmbientVolume", ToDB(_ambientVolume));
            }
        }
        private float _ambientVolume;

        #endregion

        private AudioSourceWrapper _musicSource;
        private AudioSourceWrapper _sfxSource;
        private AudioSourceWrapper _voiceSource;
        private AudioSourceWrapper _ambientSource;

        private List<AudioSourceWrapper> _sfxSourceList;
        private List<AudioSourceWrapper> _voiceSourceList;
        private List<AudioSourceWrapper> _ambientSourceList;

        #endregion

        # region Init

        public override void Init()
        {
            base.Init();

            _masterVolume = SaveSystem.Load("MasterVolume", 1f);
            _musicVolume = SaveSystem.Load("MusicVolume", 1f);
            _sfxVolume = SaveSystem.Load("SfxVolume", 1f);
            _voiceVolume = SaveSystem.Load("VoiceVolume", 1f);
            _ambientVolume = SaveSystem.Load("AmbientVolume", 1f);

            if (GlobalAudioMixer == null)
            {
                GlobalAudioMixer = Resources.Load<AudioMixer>("Datas/GlobalAudioMixer");
                GlobalAudioMixer.SetFloat("MasterVolume", ToDB(_masterVolume));
                GlobalAudioMixer.SetFloat("MusicVolume", ToDB(_musicVolume));
                GlobalAudioMixer.SetFloat("SfxVolume", ToDB(_sfxVolume));
                GlobalAudioMixer.SetFloat("VoiceVolume", ToDB(_voiceVolume));
                GlobalAudioMixer.SetFloat("AmbientVolume", ToDB(_ambientVolume));
            }

            _musicSource = new GameObject("MusicSource").AddComponent<AudioSourceWrapper>();
            _musicSource.transform.SetParent(transform);
            _musicSource.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Music")[0];

            _sfxSource = new GameObject("SfxSource").AddComponent<AudioSourceWrapper>();
            _sfxSource.transform.SetParent(transform);
            _sfxSource.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
            _sfxSourceList = new List<AudioSourceWrapper>();

            _voiceSource = new GameObject("VoiceSource").AddComponent<AudioSourceWrapper>();
            _voiceSource.transform.SetParent(transform);
            _voiceSource.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
            _voiceSourceList = new List<AudioSourceWrapper>();

            _ambientSource = new GameObject("AmbientSource").AddComponent<AudioSourceWrapper>();
            _ambientSource.transform.SetParent(transform);
            _ambientSource.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
            _ambientSourceList = new List<AudioSourceWrapper>();

            //空间混合都为0
            _musicSource.SpatialBlend = 0f;
            _sfxSource.SpatialBlend = 0f;
            _voiceSource.SpatialBlend = 0f;
            _ambientSource.SpatialBlend = 0f;
        }

        #endregion

        #region Tool

        private float ToDB(float Volume) => Mathf.Log10(Mathf.Clamp(Volume, 0.0001f, 10f)) * 20;

        private IEnumerator DestroyWhenEnd(AudioSourceWrapper target, List<AudioSourceWrapper> list)
        {
            //对象为空 或 对象播放到结尾了
            yield return new WaitUntil(() => target == null || target.IsEnded);

            if (target != null)
            {
                Destroy(target.gameObject);
            }

            list.Remove(target);
        }

        #endregion

        #region Music

        [FoldoutGroup("Music"), Button]
        public void MuteMusic()
        {
            _isMuteMusic = true;
            GlobalAudioMixer.SetFloat("MusicVolume", ToDB(0f));
        }

        [FoldoutGroup("Music"), Button]
        public void UnmuteMusic()
        {
            _isMuteMusic = false;
            GlobalAudioMixer.SetFloat("MusicVolume", ToDB(_musicVolume));
        }

        public void PlayMusic(AudioClip Clip, bool loop = true, bool withFadeOutAndIn = true)
        {
            if (!withFadeOutAndIn)
            {
                _musicSource.Clip = Clip;
                _musicSource.Loop = loop;
                _musicSource.Volume = 1f;

                _musicSource.Play();
                return;
            }

            _musicSource.DOKill();
            _musicSource.DOFade(0f, 0.5f).OnComplete(() =>
            {
                _musicSource.Clip = Clip;
                _musicSource.Loop = loop;

                _musicSource.Play();
                _musicSource.DOFade(1f, 0.5f);
            });
        }

        [FoldoutGroup("Music"), Button]
        public void StopMusic(bool withFadeOut = true)
        {
            if (!withFadeOut)
            {
                _musicSource.Stop();

                return;
            }

            _musicSource.DOKill();
            _musicSource.DOFade(0f, 0.5f)
                .OnComplete(() => _musicSource.Stop());
        }

        [FoldoutGroup("Music"), Button]
        public void PauseMusic(bool withFadeOut = true)
        {
            if (!withFadeOut)
            {
                _musicSource.Pause();

                return;
            }

            _musicSource.DOKill();
            _musicSource.DOFade(0f, 0.5f)
                .OnComplete(() => _musicSource.Pause());
        }

        [FoldoutGroup("Music"), Button]
        public void UnPauseMusic(bool withFadeIn = true)
        {
            if (!withFadeIn)
            {
                _musicSource.UnPause();

                return;
            }

            _musicSource.DOKill();
            _musicSource.DOFade(1f, 0.5f)
                .OnComplete(() => _musicSource.UnPause());
        }

        #endregion

        #region Sfx

        [FoldoutGroup("Sfx"), Button]
        public void MuteSfx()
        {
            _isMuteSfx = true;
            GlobalAudioMixer.SetFloat("SfxVolume", ToDB(0f));
        }

        [FoldoutGroup("Sfx"), Button]
        public void UnmuteSfx()
        {
            _isMuteSfx = false;
            GlobalAudioMixer.SetFloat("SfxVolume", ToDB(_sfxVolume));
        }

        public void PlaySfx(AudioClip Clip, string ClipName = "Sfx_Clip", Transform objectToFollow = null, float Volume = -1f)
        {
            if (objectToFollow == null)
            {
                var newSfxSource = new GameObject("SfxSource").AddComponent<AudioSourceWrapper>();
                newSfxSource.transform.SetParent(_sfxSource.transform);
                newSfxSource.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
                newSfxSource.Clip = Clip;
                newSfxSource.SpatialBlend = 0f;
                newSfxSource.Volume = Volume < 0f ? _sfxVolume : Volume;
                newSfxSource.Play();

                if (ClipName != null && ClipName != String.Empty)
                {
                    newSfxSource.gameObject.name = $"SfxSource-{ClipName}";
                }

                _sfxSourceList.Add(newSfxSource);

                _instance.StartCoroutine(DestroyWhenEnd(newSfxSource, _sfxSourceList));
                return;
            }

            var obj = new GameObject($"SfxSource-{ClipName}").AddComponent<AudioSourceWrapper>();
            obj.transform.SetParent(objectToFollow);
            obj.transform.localPosition = Vector3.zero;
            obj.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
            obj.Clip = Clip;
            obj.Volume = Volume < 0f ? _sfxVolume : Volume;
            obj.SpatialBlend = 0f;
            obj.Play();

            _sfxSourceList.Add(obj);

            _instance.StartCoroutine(DestroyWhenEnd(obj, _sfxSourceList));
        }

        public void PlaySfxAt(AudioClip Clip, Vector3 pos = default, string ClipName = "Sfx_Clip", float Volume = -1f)
        {
            var obj = new GameObject("SfxSourceFromEasyAudioSystem").AddComponent<AudioSourceWrapper>();
            obj.transform.position = pos;
            obj.SpatialBlend = 1f;
            obj.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
            obj.Clip = Clip;
            obj.Volume = Volume < 0f ? _sfxVolume : Volume;
            obj.Play();

            if (ClipName != null && ClipName != String.Empty)
            {
                obj.gameObject.name = $"SfxSource-{ClipName}";
                _sfxSourceList.Add(obj);
            }

            _instance.StartCoroutine(DestroyWhenEnd(obj, _sfxSourceList));
        }

        public void PauseSfx(string ClipName)
        {
            foreach (var item in _sfxSourceList)
            {
                if (item.name == ClipName)
                {
                    item.Pause();
                    break;
                }
            }
        }

        public void UnPauseSfx(string ClipName)
        {
            foreach (var item in _sfxSourceList)
            {
                if (item.name == ClipName)
                {
                    item.UnPause();
                    break;
                }
            }
        }

        public void StopSfx(string ClipName)
        {
            ClipName = $"SfxSource-{ClipName}";

            foreach (var item in _sfxSourceList)
            {
                if (item == null)
                    _sfxSourceList.Remove(item);

                if (item.name == ClipName)
                {
                    Destroy(item.gameObject);
                    break;
                }
            }
        }

        [FoldoutGroup("Sfx"), Button]
        public void StopAllSfxs()
        {
            foreach (var item in _sfxSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        [FoldoutGroup("Sfx"), Button]
        public void PauseAllSfxs()
        {
            foreach (var item in _sfxSourceList)
            {
                item.Pause();
            }
        }

        [FoldoutGroup("Sfx"), Button]
        public void UnPauseAllSfxs()
        {
            foreach (var item in _sfxSourceList)
            {
                item.UnPause();
            }
        }

        #endregion

        #region Voice

        [FoldoutGroup("Voice"), Button]
        public void MuteVoice()
        {
            _isMuteVoice = true;
            GlobalAudioMixer.SetFloat("VoiceVolume", ToDB(0f));
        }

        [FoldoutGroup("Voice"), Button]
        public void UnmuteVoice()
        {
            _isMuteVoice = false;
            GlobalAudioMixer.SetFloat("VoiceVolume", ToDB(_voiceVolume));
        }

        public void PlayVoice(AudioClip Clip, string voiceName = "Voice_Clip", Transform objectToFollow = null, float Volume = -1f)
        {
            if (objectToFollow == null)
            {
                var newVoiceSource = new GameObject("VoiceSource").AddComponent<AudioSourceWrapper>();
                newVoiceSource.transform.SetParent(_sfxSource.transform);
                newVoiceSource.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
                newVoiceSource.Clip = Clip;
                newVoiceSource.Volume = Volume < 0f ? _voiceVolume : Volume;
                newVoiceSource.SpatialBlend = 0f;
                newVoiceSource.Play();

                if (voiceName != null && voiceName != String.Empty)
                {
                    newVoiceSource.gameObject.name = $"VoiceSource-{voiceName}";
                }

                _voiceSourceList.Add(newVoiceSource);

                _instance.StartCoroutine(DestroyWhenEnd(newVoiceSource, _voiceSourceList));
                return;
            }

            var obj = new GameObject("VoiceSource").AddComponent<AudioSourceWrapper>();
            obj.transform.SetParent(objectToFollow);
            obj.transform.localPosition = Vector3.zero;
            obj.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
            obj.Clip = Clip;
            obj.Volume = Volume < 0f ? _voiceVolume : Volume;
            obj.SpatialBlend = 0f;
            obj.Play();

            _voiceSourceList.Add(obj);

            _instance.StartCoroutine(DestroyWhenEnd(obj, _voiceSourceList));
        }

        public void PlayVoiceAt(AudioClip Clip, Vector3 pos = default, string ClipName = "Voice_Clip", float Volume = -1f)
        {
            var obj = new GameObject("VoiceSourceFromEasyAudioSystem").AddComponent<AudioSourceWrapper>();
            obj.transform.position = pos;
            obj.SpatialBlend = 1f;
            obj.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
            obj.Clip = Clip;
            obj.Volume = Volume < 0f ? _voiceVolume : Volume;
            obj.Play();

            if (ClipName != null && ClipName != String.Empty)
            {
                obj.gameObject.name = $"VoiceSource-{ClipName}";
                _voiceSourceList.Add(obj);
            }

            _instance.StartCoroutine(DestroyWhenEnd(obj, _voiceSourceList));
        }

        public void StopVoice(string voiceName)
        {
            voiceName = $"VoiceSource-{voiceName}";

            foreach (var item in _voiceSourceList)
            {
                if (item.name == voiceName)
                {
                    Destroy(item.gameObject);
                    break;
                }
            }
        }

        public void PauseVoice(string voiceName)
        {
            voiceName = $"VoiceSource-{voiceName}";

            foreach (var item in _voiceSourceList)
            {
                if (item.name == voiceName)
                {
                    item.Pause();
                    break;
                }
            }
        }

        public void UnPauseVoice(string voiceName)
        {
            voiceName = $"VoiceSource-{voiceName}";

            foreach (var item in _voiceSourceList)
            {
                if (item.name == voiceName)
                {
                    item.UnPause();
                    break;
                }
            }
        }

        [FoldoutGroup("Voice"), Button]
        public void PauseAllVoices()
        {
            foreach (var item in _voiceSourceList)
            {
                item.Pause();
            }
        }

        [FoldoutGroup("Voice"), Button]
        public void UnPauseAllVoices()
        {
            foreach (var item in _voiceSourceList)
            {
                item.UnPause();
            }
        }

        [FoldoutGroup("Voice"), Button]
        public void StopAllVoices()
        {
            foreach (var item in _voiceSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        #endregion

        #region Ambient

        [FoldoutGroup("Ambient"), Button]
        public void MuteAmbient()
        {
            _isMuteAmbient = true;
            GlobalAudioMixer.SetFloat("AmbientVolume", ToDB(0f));
        }

        [FoldoutGroup("Ambient"), Button]
        public void UnmuteAmbient()
        {
            _isMuteAmbient = false;
            GlobalAudioMixer.SetFloat("AmbientVolume", ToDB(_ambientVolume));
        }

        public void PlayAmbient(AudioClip Clip, string ambientName = "Ambient_Clip", bool loop = false, Transform objectToFollow = null, float Volume = -1f)
        {
            if (objectToFollow == null)
            {
                var newSfxSource = new GameObject("AmbientSource").AddComponent<AudioSourceWrapper>();
                newSfxSource.transform.SetParent(_sfxSource.transform);
                newSfxSource.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
                newSfxSource.Clip = Clip;
                newSfxSource.SpatialBlend = 0f;
                newSfxSource.Play();

                if (ambientName != null && ambientName != String.Empty)
                {
                    newSfxSource.gameObject.name = $"AmbientSource-{ambientName}";
                }

                _ambientSourceList.Add(newSfxSource);

                _instance.StartCoroutine(DestroyWhenEnd(newSfxSource, _ambientSourceList));
                return;
            }

            var obj = new GameObject("AmbientSource").AddComponent<AudioSourceWrapper>();
            obj.transform.SetParent(objectToFollow);
            obj.transform.localPosition = Vector3.zero;
            obj.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
            obj.Clip = Clip;
            obj.SpatialBlend = 0f;
            obj.Play();

            _ambientSourceList.Add(obj);

            _instance.StartCoroutine(DestroyWhenEnd(obj, _ambientSourceList));
        }

        public void PlayAmbientAt(AudioClip Clip, Vector3 pos = default, string ClipName = "Ambient_Clip", float Volume = -1f)
        {
            var obj = new GameObject("AmbientSourceFromEasyAudioSystem").AddComponent<AudioSourceWrapper>();
            obj.transform.position = pos;
            obj.SpatialBlend = 1f;
            obj.OutputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
            obj.Clip = Clip;
            obj.Volume = Volume < 0f ? _ambientVolume : Volume;
            obj.Play();

            if (ClipName != null && ClipName != String.Empty)
            {
                obj.gameObject.name = $"AmbientSource-{ClipName}";
                _ambientSourceList.Add(obj);
            }

            _instance.StartCoroutine(DestroyWhenEnd(obj, _ambientSourceList));
        }

        public void StopAmbient(string ambientName)
        {
            ambientName = $"AmbientSource-{ambientName}";

            foreach (var item in _ambientSourceList)
            {
                if (item.name == ambientName)
                {
                    Destroy(item.gameObject);
                    break;
                }
            }
        }

        public void PauseAmbient(string voiceName)
        {
            voiceName = $"AmbientSource-{voiceName}";

            foreach (var item in _ambientSourceList)
            {
                if (item.name == voiceName)
                {
                    item.Pause();
                    break;
                }
            }
        }

        public void UnPauseAmbient(string voiceName)
        {
            voiceName = $"AmbientSource-{voiceName}";

            foreach (var item in _ambientSourceList)
            {
                if (item.name == voiceName)
                {
                    item.UnPause();
                    break;
                }
            }
        }

        [FoldoutGroup("Ambient"), Button]
        public void PauseAllAmbients()
        {
            foreach (var item in _ambientSourceList)
            {
                item.Pause();
            }
        }

        [FoldoutGroup("Ambient"), Button]
        public void UnPauseAllAmbients()
        {
            foreach (var item in _ambientSourceList)
            {
                item.UnPause();
            }
        }

        [FoldoutGroup("Ambient"), Button]
        public void StopAllAmbients()
        {
            foreach (var item in _ambientSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        #endregion

        #region Volume

        [Button]
        public void Mute(bool muteMusic = true, bool muteSfx = true, bool muteVoice = true, bool muteAmbient = true)
        {
            IsMuteMusic = muteMusic;
            IsMuteSfx = muteSfx;
            IsMuteVoice = muteVoice;
            IsMuteAmbient = muteAmbient;
        }

        [Button]
        public void UnMute()
        {
            IsMuteMusic = false;
            IsMuteSfx = false;
            IsMuteVoice = false;
            IsMuteAmbient = false;
        }


        [Button]
        public void Pause(bool pauseMusic = true, bool pauseSfx = true, bool pauseVoice = true, bool pauseAmbient = true)
        {
            if (pauseMusic) PauseMusic();
            if (pauseSfx) PauseAllSfxs();
            if (pauseVoice) PauseAllVoices();
            if (pauseAmbient) PauseAllAmbients();
        }

        [Button]
        public void UnPause() { }

        [Button]
        public void SetMasterVolume(float Volume) => MasterVolume = Volume;

        [Button]
        public void SetMusicVolume(float Volume) => MusicVolume = Volume;

        [Button]
        public void SetSfxVolume(float Volume) => SfxVolume = Volume;

        [Button]
        public void SetVoiceVolume(float Volume) => VoiceVolume = Volume;

        [Button]
        public void SetAmbientVolume(float Volume) => AmbientVolume = Volume;

        #endregion
    }
}