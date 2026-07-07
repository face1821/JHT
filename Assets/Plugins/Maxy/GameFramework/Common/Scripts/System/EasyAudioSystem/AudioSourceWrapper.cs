using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Audio;

namespace Maxy.GameFramework.Common.System
{
    [Serializable]
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceWrapper : MonoBehaviour
    {
        public AudioSource AudioSource => _source;

        #region 原生字段

        public AudioClip Clip { get => _source.clip; set => _source.clip = value; }
        public AudioMixerGroup OutputAudioMixerGroup { get => _source.outputAudioMixerGroup; set => _source.outputAudioMixerGroup = value; }
        public bool IsPlaying => _source.isPlaying;
        public bool Loop { get => _source.loop; set => _source.loop = value; }
        public float Volume { get => _source.volume; set => _source.volume = value; }
        public float SpatialBlend { get => _source.spatialBlend; set => _source.spatialBlend = value; }

        #endregion

        #region 拓展字段

        public bool IsPaused;
        public bool IsEnded => !Loop && _source.time >= _source.clip.length;
        public float SpaceBlend { get => _source.spatialBlend; set => _source.spatialBlend = value; }

        #endregion

        [SerializeField] private AudioSource _source;

        private void Awake() { _source = GetComponent<AudioSource>(); }

        public void Play() { _source.Play(); }
        public void Stop() { _source.Stop(); }

        public void Pause()
        {
            _source.Pause();
            IsPaused = true;
        }

        public void UnPause()
        {
            _source.UnPause();
            IsPaused = false;
        }

        //DoTween
        public TweenerCore<float, float, FloatOptions> DOFade(float endValue, float duration)
        {
            if (endValue < 0) endValue = 0;
            else if (endValue > 1) endValue = 1;
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => Volume, x => Volume = x, endValue, duration);
            t.SetTarget(this);
            return t;
        }
    }
}