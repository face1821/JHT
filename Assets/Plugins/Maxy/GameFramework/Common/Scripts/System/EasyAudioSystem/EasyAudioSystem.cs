using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace Maxy.GameFramework.Common.System
{
    public class EasyAudioSystem : System<EasyAudioSystem>, IAudioSystem
    {
        public float SpaceBlend
        {
            get => _spaceBlend;
            set
            {
                _spaceBlend = value;

                _musicSource.spatialBlend = _spaceBlend;
                _sfxSource.spatialBlend = _spaceBlend;
                _voiceSource.spatialBlend = _spaceBlend;
                _ambientSource.spatialBlend = _spaceBlend;
            }
        }

        #region 字段

        [SerializeField] private float _spaceBlend;
        public AudioMixer GlobalAudioMixer;

        #region 公开音量属性

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

        private AudioSource _musicSource;
        private AudioSource _sfxSource;
        private AudioSource _voiceSource;
        private AudioSource _ambientSource;

        private List<AudioSource> _sfxSourceList;
        private List<AudioSource> _voiceSourceList;
        private List<AudioSource> _ambientSourceList;

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

            _musicSource = new GameObject("MusicSource").AddComponent<AudioSource>();
            _musicSource.transform.SetParent(transform);
            _musicSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Music")[0];

            _sfxSource = new GameObject("SfxSource").AddComponent<AudioSource>();
            _sfxSource.transform.SetParent(transform);
            _sfxSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
            _sfxSourceList = new List<AudioSource>();

            _voiceSource = new GameObject("VoiceSource").AddComponent<AudioSource>();
            _voiceSource.transform.SetParent(transform);
            _voiceSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
            _voiceSourceList = new List<AudioSource>();

            _ambientSource = new GameObject("AmbientSource").AddComponent<AudioSource>();
            _ambientSource.transform.SetParent(transform);
            _ambientSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
            _ambientSourceList = new List<AudioSource>();

            //空间混合
            //音乐强制为0
            _musicSource.spatialBlend = 0f;
            _sfxSource.spatialBlend = _spaceBlend;
            _voiceSource.spatialBlend = _spaceBlend;
            _ambientSource.spatialBlend = _spaceBlend;
        }

        #endregion

        #region Tool

        private float ToDB(float volume) => Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 10f)) * 20;

        private IEnumerator DestroyWhenEnd(AudioSource target, List<AudioSource> list)
        {
            yield return new WaitUntil(() => target.gameObject == null || !target.isPlaying);

            if (target.gameObject != null)
            {
                Destroy(target.gameObject);
                list.Remove(target);
            }
        }

        #endregion

        #region Music

        public void PlayMusic(AudioClip clip, bool loop = true, bool withFadeOutAndIn = true)
        {
            if (!withFadeOutAndIn)
            {
                _musicSource.clip = clip;
                _musicSource.loop = loop;
                _musicSource.volume = 1f;

                _musicSource.Play();
                return;
            }

            _musicSource.DOKill();
            _musicSource.DOFade(0f, 0.5f).OnComplete(() =>
            {
                _musicSource.clip = clip;
                _musicSource.loop = loop;

                _musicSource.Play();
                _musicSource.DOFade(1f, 0.5f);
            });
        }

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

        public void PlaySfx(AudioClip clip, string clipName = null, Transform objectToFollow = null, float spaceBlend = -1f)
        {
            if (objectToFollow == null)
            {
                var newSfxSource = new GameObject("SfxSource").AddComponent<AudioSource>();
                newSfxSource.transform.SetParent(_sfxSource.transform);
                newSfxSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
                newSfxSource.clip = clip;
                newSfxSource.spatialBlend = spaceBlend < 0f ? _spaceBlend : spaceBlend;
                newSfxSource.Play();

                if (clipName != null && clipName != String.Empty)
                {
                    newSfxSource.gameObject.name = $"SfxSource-{clipName}";
                }

                _sfxSourceList.Add(newSfxSource);

                _instance.StartCoroutine(DestroyWhenEnd(newSfxSource, _sfxSourceList));
                return;
            }

            var obj = new GameObject($"SfxSource-{clipName}").AddComponent<AudioSource>();
            obj.transform.SetParent(objectToFollow);
            obj.transform.localPosition = Vector3.zero;
            obj.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
            obj.spatialBlend = 1f;
            obj.clip = clip;
            obj.spatialBlend = spaceBlend < 0f ? _spaceBlend : spaceBlend;
            obj.Play();

            _sfxSourceList.Add(obj);

            _instance.StartCoroutine(DestroyWhenEnd(obj, _sfxSourceList));
        }

        public void PlaySfxAt(AudioClip clip, Vector3 pos, string clipName)
        {
            var obj = new GameObject("SfxSourceFromEasyAudioSystem").AddComponent<AudioSource>();
            obj.transform.position = pos;
            obj.spatialBlend = 1f;
            obj.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
            obj.clip = clip;
            obj.spatialBlend = _spaceBlend;
            obj.Play();

            if (clipName != null && clipName != String.Empty)
            {
                obj.gameObject.name = $"SfxSource-{clipName}";
                _sfxSourceList.Add(obj);
            }

            _instance.StartCoroutine(DestroyWhenEnd(obj, _sfxSourceList));
        }

        public void StopSfx(string clipName)
        {
            clipName = $"SfxSource-{clipName}";

            foreach (var item in _sfxSourceList)
            {
                if (item.IsDestroyed())
                    _sfxSourceList.Remove(item);

                if (item.name == clipName)
                {
                    Destroy(item.gameObject);
                    break;
                }
            }
        }

        public void StopAllSfxs()
        {
            foreach (var item in _sfxSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        #endregion

        #region Voice

        public void PlayVoice(AudioClip clip, string voiceName = null, Transform objectToFollow = null, float spaceBlend = -1f)
        {
            if (objectToFollow == null)
            {
                var newSfxSource = new GameObject("VoiceSource").AddComponent<AudioSource>();
                newSfxSource.transform.SetParent(_sfxSource.transform);
                newSfxSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
                newSfxSource.clip = clip;
                newSfxSource.spatialBlend = spaceBlend < 0f ? _spaceBlend : spaceBlend;
                newSfxSource.Play();

                if (voiceName != null && voiceName != String.Empty)
                {
                    newSfxSource.gameObject.name = $"VoiceSource-{voiceName}";
                }

                _voiceSourceList.Add(newSfxSource);

                _instance.StartCoroutine(DestroyWhenEnd(newSfxSource, _voiceSourceList));
                return;
            }

            var obj = new GameObject("VoiceSource").AddComponent<AudioSource>();
            obj.transform.SetParent(objectToFollow);
            obj.transform.localPosition = Vector3.zero;
            obj.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
            obj.spatialBlend = 1f;
            obj.clip = clip;
            obj.spatialBlend = spaceBlend < 0f ? _spaceBlend : spaceBlend;
            obj.Play();

            _voiceSourceList.Add(obj);

            _instance.StartCoroutine(DestroyWhenEnd(obj, _voiceSourceList));
        }

        public void StopVoice(string voiceName)
        {
            voiceName = $"SfxSource-{voiceName}";

            foreach (var item in _sfxSourceList)
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
            voiceName = $"SfxSource-{voiceName}";

            foreach (var item in _sfxSourceList)
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
            voiceName = $"SfxSource-{voiceName}";

            foreach (var item in _sfxSourceList)
            {
                if (item.name == voiceName)
                {
                    item.UnPause();
                    break;
                }
            }
        }

        public void StopAllVoices()
        {
            foreach (var item in _voiceSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        #endregion

        #region Ambient

        public void PlayAmbient(AudioClip clip, string ambientName, bool loop = false, Transform objectToFollow = null, float spaceBlend = -1f)
        {
            if (objectToFollow == null)
            {
                var newSfxSource = new GameObject("AmbientSource").AddComponent<AudioSource>();
                newSfxSource.transform.SetParent(_sfxSource.transform);
                newSfxSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
                newSfxSource.clip = clip;
                newSfxSource.spatialBlend = spaceBlend < 0f ? _spaceBlend : spaceBlend;
                newSfxSource.Play();

                if (ambientName != null && ambientName != String.Empty)
                {
                    newSfxSource.gameObject.name = $"AmbientSource-{ambientName}";
                }

                _ambientSourceList.Add(newSfxSource);

                _instance.StartCoroutine(DestroyWhenEnd(newSfxSource, _ambientSourceList));
                return;
            }

            var obj = new GameObject("AmbientSource").AddComponent<AudioSource>();
            obj.transform.SetParent(objectToFollow);
            obj.transform.localPosition = Vector3.zero;
            obj.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
            obj.spatialBlend = 1f;
            obj.clip = clip;
            obj.spatialBlend = spaceBlend < 0f ? _spaceBlend : spaceBlend;
            obj.Play();

            _ambientSourceList.Add(obj);

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

        public void StopAllAmbients()
        {
            foreach (var item in _ambientSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        #endregion

        #region Volume

        public void SetMasterVolume(float volume) => MasterVolume = volume;
        public void SetMusicVolume(float volume) => MusicVolume = volume;
        public void SetSfxVolume(float volume) => SfxVolume = volume;
        public void SetVoiceVolume(float volume) => VoiceVolume = volume;
        public void SetAmbientVolume(float volume) => AmbientVolume = volume;

        #endregion
    }
}