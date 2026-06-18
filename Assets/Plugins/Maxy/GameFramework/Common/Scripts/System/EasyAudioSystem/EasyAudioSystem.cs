using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Maxy.GameFramework.Common.System
{
    public class EasyAudioSystem : ComponentSystem<EasyAudioSystem>
    {
        #region 字段
        
        public static AudioMixer GlobalAudioMixer;

        #region 公开音量属性

        public static float MasterVolume
        {
            get => _masterVolume;
            set
            {
                ES3.Save("MasterVolume", value);
                _masterVolume = value;
            }
        }
        private static float _masterVolume;
        public static float MusicVolume
        {
            get => _musicVolume;
            set
            {
                ES3.Save("MusicVolume", value);
                _musicVolume = value;
            }
        }
        private static float _musicVolume;
        public static float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                ES3.Save("SfxVolume", value);
                _sfxVolume = value;
            }
        }
        private static float _sfxVolume;
        public static float VoiceVolume
        {
            get => _voiceVolume;
            set
            {
                ES3.Save("VoiceVolume", value);
                _voiceVolume = value;
            }
        }
        private static float _voiceVolume;
        public static float AmbientVolume
        {
            get => _ambientVolume;
            set
            {
                ES3.Save("AmbientVolume", value);
                _ambientVolume = value;
            }
        }
        private static float _ambientVolume;

        #endregion

        private static AudioSource _musicSource;
        private static AudioSource _sfxSource;
        private static AudioSource _voiceSource;
        private static AudioSource _ambientSource;

        private static List<AudioSource> _sfxSourceList;
        private static List<AudioSource> _voiceSourceList;
        private static List<AudioSource> _ambientSourceList;
        
        #endregion

        # region Awake
        
        private void Awake()
        {
            BindToRoot("EasyAudioSystem");

            _masterVolume = ES3.Load("MasterVolume", 1f);
            _musicVolume = ES3.Load("MusicVolume", 1f);
            _sfxVolume = ES3.Load("SfxVolume", 1f);
            _voiceVolume = ES3.Load("VoiceVolume", 1f);
            _ambientVolume = ES3.Load("AmbientVolume", 1f);

            GlobalAudioMixer = Resources.Load<AudioMixer>("Datas/GlobalAudioMixer");
            GlobalAudioMixer.SetFloat("MasterVolume", ToDB(_masterVolume));
            GlobalAudioMixer.SetFloat("MusicVolume", ToDB(_musicVolume));
            GlobalAudioMixer.SetFloat("SfxVolume", ToDB(_sfxVolume));
            GlobalAudioMixer.SetFloat("VoiceVolume", ToDB(_voiceVolume));
            GlobalAudioMixer.SetFloat("AmbientVolume", ToDB(_ambientVolume));

            _musicSource = new GameObject("MusicSource").AddComponent<AudioSource>();
            _musicSource.transform.SetParent(transform);
            _musicSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Music")[0];

            _sfxSource = new GameObject("SfxSource").AddComponent<AudioSource>();
            _sfxSource.transform.SetParent(transform);
            _sfxSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];

            _voiceSource = new GameObject("VoiceSource").AddComponent<AudioSource>();
            _voiceSource.transform.SetParent(transform);
            _voiceSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];

            _ambientSource = new GameObject("AmbientSource").AddComponent<AudioSource>();
            _ambientSource.transform.SetParent(transform);
            _ambientSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
        }
        
        #endregion

        #region Tool

        private static float ToDB(float volume) => Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 10f)) * 20;

        private static IEnumerator DestroyWhenEnd(AudioSource target)
        {
            yield return new WaitUntil(() => target.gameObject == null || !target.isPlaying);

            if (target.gameObject != null)
                Destroy(target.gameObject);
        }

        #endregion

        #region Music

        public static void PlayMusic(AudioClip clip, bool loop = true, bool withFadeOutAndIn = true)
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

        public static void StopMusic(bool withFadeOut = true)
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

        public static void PauseMusic(bool withFadeOut = true)
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

        public static void UnPauseMusic(bool withFadeIn = true)
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

        public static void PlaySfx(AudioClip clip, string clipName = null, Transform objectToFollow = null)
        {
            if (objectToFollow == null)
            {
                var newSfxSource = new GameObject("SfxSource").AddComponent<AudioSource>();
                newSfxSource.transform.SetParent(_sfxSource.transform);
                newSfxSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
                newSfxSource.clip = clip;
                newSfxSource.Play();

                if (clipName != null && clipName != String.Empty)
                {
                    newSfxSource.gameObject.name = $"SfxSource-{clipName}";
                }

                _sfxSourceList.Add(newSfxSource);

                _instance.StartCoroutine(DestroyWhenEnd(newSfxSource));
                return;
            }

            var obj = new GameObject("SfxSource").AddComponent<AudioSource>();
            obj.transform.SetParent(objectToFollow);
            obj.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
            obj.spatialBlend = 1f;
            obj.clip = clip;
            obj.Play();

            _sfxSourceList.Add(obj);

            _instance.StartCoroutine(DestroyWhenEnd(obj));
        }

        public static void PlaySfxAt(AudioClip clip, Vector3 pos, string clipName)
        {
            var obj = new GameObject("SfxSourceFromEasyAudioSystem").AddComponent<AudioSource>();
            obj.transform.position = pos;
            obj.spatialBlend = 1f;
            obj.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Sfx")[0];
            obj.clip = clip;
            obj.Play();

            if (clipName != null && clipName != String.Empty)
            {
                obj.gameObject.name = $"SfxSource-{clipName}";
                _sfxSourceList.Add(obj);
            }

            _instance.StartCoroutine(DestroyWhenEnd(obj));
        }

        public static void StopSfx(string clipName)
        {
            clipName = $"SfxSource-{clipName}";

            foreach (var item in _sfxSourceList)
            {
                if (item.name == clipName)
                {
                    Destroy(item.gameObject);
                    break;
                }
            }
        }

        public static void StopAllSfxs()
        {
            foreach (var item in _sfxSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        #endregion

        #region Voice

        public static void PlayVoice(AudioClip clip, string voiceName = null, Transform objectToFollow = null)
        {
            if (objectToFollow == null)
            {
                var newSfxSource = new GameObject("VoiceSource").AddComponent<AudioSource>();
                newSfxSource.transform.SetParent(_sfxSource.transform);
                newSfxSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
                newSfxSource.clip = clip;
                newSfxSource.Play();

                if (voiceName != null && voiceName != String.Empty)
                {
                    newSfxSource.gameObject.name = $"VoiceSource-{voiceName}";
                }

                _voiceSourceList.Add(newSfxSource);

                _instance.StartCoroutine(DestroyWhenEnd(newSfxSource));
                return;
            }

            var obj = new GameObject("VoiceSource").AddComponent<AudioSource>();
            obj.transform.SetParent(objectToFollow);
            obj.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Voice")[0];
            obj.spatialBlend = 1f;
            obj.clip = clip;
            obj.Play();

            _voiceSourceList.Add(obj);

            _instance.StartCoroutine(DestroyWhenEnd(obj));
        }

        public static void StopVoice(string voiceName)
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

        public static void PauseVoice(string voiceName)
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

        public static void UnPauseVoice(string voiceName)
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

        public static void StopAllVoices()
        {
            foreach (var item in _voiceSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        #endregion

        #region Ambient

        public static void PlayAmbient(AudioClip clip, string ambientName, bool loop = false, Transform objectToFollow = null)
        {
            if (objectToFollow == null)
            {
                var newSfxSource = new GameObject("AmbientSource").AddComponent<AudioSource>();
                newSfxSource.transform.SetParent(_sfxSource.transform);
                newSfxSource.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
                newSfxSource.clip = clip;
                newSfxSource.Play();

                if (ambientName != null && ambientName != String.Empty)
                {
                    newSfxSource.gameObject.name = $"AmbientSource-{ambientName}";
                }

                _ambientSourceList.Add(newSfxSource);

                _instance.StartCoroutine(DestroyWhenEnd(newSfxSource));
                return;
            }

            var obj = new GameObject("AmbientSource").AddComponent<AudioSource>();
            obj.transform.SetParent(objectToFollow);
            obj.outputAudioMixerGroup = GlobalAudioMixer.FindMatchingGroups("Ambient")[0];
            obj.spatialBlend = 1f;
            obj.clip = clip;
            obj.Play();

            _ambientSourceList.Add(obj);

            _instance.StartCoroutine(DestroyWhenEnd(obj));
        }

        public static void StopAmbient(string ambientName)
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

        public static void PauseAmbient(string voiceName)
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

        public static void UnPauseAmbient(string voiceName)
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

        public static void StopAllAmbients()
        {
            foreach (var item in _ambientSourceList)
            {
                Destroy(item.gameObject);
            }
        }

        #endregion

        #region Volume

        public static void SetMasterVolume(float volume) => MasterVolume = volume;
        public static void SetMusicVolume(float volume) => MusicVolume = volume;
        public static void SetSfxVolume(float volume) => SfxVolume = volume;
        public static void SetVoiceVolume(float volume) => VoiceVolume = volume;
        public static void SetAmbientVolume(float volume) => AmbientVolume = volume;

        #endregion
    }
}