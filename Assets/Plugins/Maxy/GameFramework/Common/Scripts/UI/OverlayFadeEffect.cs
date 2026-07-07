using System.Collections;
using System.Collections.Generic;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maxy.GameFramework.Common.Tool
{
    public class OverlayFadeEffect : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] public bool IsPlaying => !IsFinished;
        [HideInInspector] public bool IsFinished = true;

        [SerializeField] private bool UseSmoothFadeStart = true;
        [ShowInInspector, ShowIf(nameof(UseSmoothFadeStart))]
        private bool UseScaledDuration;

        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue = 1f;
        [SerializeField] private float _childMinValue;
        [SerializeField] private float _childMaxValue = 1f;
        [SerializeField] private List<Image> _childImages = new List<Image>();
        [SerializeField] private List<TextMeshProUGUI> _childTexts = new List<TextMeshProUGUI>();

        private Image _selfImage;
        private TextMeshProUGUI _selfText;

        private float _realDuration;
        private float _realMinValue;
        private float _realMaxValue;
        private float _realChildMinValue;
        private float _realChildMaxValue;

        private void Awake()
        {
            _selfImage = GetComponent<Image>();
            _selfText = GetComponentInChildren<TextMeshProUGUI>();

            _realDuration = _duration;
            _realMinValue = _minValue;
            _realMaxValue = _maxValue;
            _realChildMinValue = _childMinValue;
            _realChildMaxValue = _childMaxValue;
        }

        public void SetAlpha(float alpha)
        {
            if (_selfImage != null)
                _selfImage.color = new Color(_selfImage.color.r, _selfImage.color.g, _selfImage.color.b, alpha);
            if (_selfText != null)
                _selfText.color = new Color(_selfText.color.r, _selfText.color.g, _selfText.color.b, alpha);

            foreach (var child in _childImages)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, alpha);
            }

            foreach (var child in _childTexts)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, alpha);
            }
        }

        public void Stop()
        {
            StopAllCoroutines();

            if (UseSmoothFadeStart)
            {
                _duration = _realDuration;
                _minValue = _realMinValue;
                _maxValue = _realMaxValue;
                _childMinValue = _realChildMinValue;
                _childMaxValue = _realChildMaxValue;
            }
        }

        public void PlayFadeIn(float duration = -1f)
        {
            Stop();

            duration = duration < 0f ? _duration : duration;

            //是否平滑中间态过渡
            if (UseSmoothFadeStart)
            {
                //是否根据中间态的已过渡比例来调节过渡时间
                if (UseScaledDuration)
                {
                    duration = _selfImage != null
                        ? (_selfImage.color.a - _minValue) / (_maxValue - _minValue) * duration
                        : (_selfText != null
                            ? (_selfText.color.a - _minValue) / (_maxValue - _minValue) * duration
                            : duration);
                }

                _realMaxValue = _maxValue;
                _maxValue = _selfImage != null ? _selfImage.color.a : (_selfText != null ? _selfText.color.a : _maxValue);

                _realChildMaxValue = _childMaxValue;
                _childMaxValue = _childImages.Count > 0 ? _childImages[0].color.a : (_childTexts.Count > 0 ? _childTexts[0].color.a : _childMaxValue);
            }

            StartCoroutine(OnFadeIn(duration));
        }

        public void PlayFadeOut(float duration = -1f)
        {
            Stop();

            duration = duration < 0f ? _duration : duration;

            if (UseSmoothFadeStart)
            {
                if (UseScaledDuration)
                {
                    duration = _selfImage != null
                        ? (_maxValue - _selfImage.color.a) / (_maxValue - _minValue) * duration
                        : (_selfText != null
                            ? (_maxValue - _selfText.color.a) / (_maxValue - _minValue) * duration
                            : duration);
                }

                _realMinValue = _minValue;
                _minValue = _selfImage != null ? _selfImage.color.a : (_selfText != null ? _selfText.color.a : _minValue);

                _realChildMinValue = _childMinValue;
                _childMinValue = _childImages.Count > 0 ? _childImages[0].color.a : (_childTexts.Count > 0 ? _childTexts[0].color.a : _childMinValue);
            }

            StartCoroutine(OnFadeOut(duration));
        }

        public void PlayFadeOutAndIn(float fadeOutDuration = -1f, float keepDuration = 1f, float fadeInDuration = -1f)
        {
            Stop();

            StartCoroutine(OnFadeOutAndIn(fadeOutDuration, keepDuration, fadeInDuration));
        }

        private IEnumerator OnFadeIn(float duration)
        {
            IsFinished = false;

            var startTime = Time.time;

            //开始
            while (_selfImage != null && _selfImage.color.a > _minValue || _selfText != null && _selfText.color.a > _minValue)
            {
                yield return null;

                if (_selfImage != null)
                    _selfImage.color = new Color(_selfImage.color.r, _selfImage.color.g, _selfImage.color.b, _maxValue - (_maxValue - _minValue) * (Time.time - startTime) / duration);

                if (_selfText != null)
                    _selfText.color = new Color(_selfText.color.r, _selfText.color.g, _selfText.color.b, _maxValue - (_maxValue - _minValue) * (Time.time - startTime) / duration);

                foreach (var child in _childImages)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, _childMaxValue - (_childMaxValue - _childMinValue) * (Time.time - startTime) / duration);
                }

                foreach (var child in _childTexts)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, _childMaxValue - (_childMaxValue - _childMinValue) * (Time.time - startTime) / duration);
                }

                //结束工作
                if (_selfImage != null && _selfImage.color.a <= _minValue || _selfText != null && _selfText.color.a <= _minValue)
                {
                    if (_selfImage != null)
                        _selfImage.color = new Color(_selfImage.color.r, _selfImage.color.g, _selfImage.color.b, _minValue);

                    if (_selfText != null)
                        _selfText.color = new Color(_selfText.color.r, _selfText.color.g, _selfText.color.b, _minValue);

                    foreach (var child in _childImages)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, _childMinValue);
                    }

                    foreach (var child in _childTexts)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, _childMinValue);
                    }
                }
            }

            if (UseSmoothFadeStart)
            {
                _maxValue = _realMaxValue;
                _childMaxValue = _realChildMaxValue;
            }

            IsFinished = true;
        }

        private IEnumerator OnFadeOut(float duration)
        {
            IsFinished = false;

            var startTime = Time.time;

            //开始
            while (_selfImage != null && _selfImage.color.a < _maxValue || _selfText != null && _selfText.color.a < _maxValue)
            {
                yield return null;

                if (_selfImage != null)
                    _selfImage.color = new Color(_selfImage.color.r, _selfImage.color.g, _selfImage.color.b, _minValue + (_maxValue - _minValue) * (Time.time - startTime) / duration);

                if (_selfText != null)
                    _selfText.color = new Color(_selfText.color.r, _selfText.color.g, _selfText.color.b, _minValue + (_maxValue - _minValue) * (Time.time - startTime) / duration);

                foreach (var child in _childImages)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, _childMinValue + (_childMaxValue - _childMinValue) * (Time.time - startTime) / duration);
                }

                foreach (var child in _childTexts)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, _childMinValue + (_childMaxValue - _childMinValue) * (Time.time - startTime) / duration);
                }

                if (_selfImage != null && _selfImage.color.a >= _maxValue || _selfText != null && _selfText.color.a >= _maxValue)
                {
                    if (_selfImage != null)
                        _selfImage.color = new Color(_selfImage.color.r, _selfImage.color.g, _selfImage.color.b, _maxValue);

                    if (_selfText != null)
                        _selfText.color = new Color(_selfText.color.r, _selfText.color.g, _selfText.color.b, _maxValue);

                    foreach (var child in _childImages)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, _childMaxValue);
                    }

                    foreach (var child in _childTexts)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, _childMaxValue);
                    }
                }
            }

            if (UseSmoothFadeStart)
            {
                _minValue = _realMinValue;
                _childMinValue = _realChildMinValue;
            }

            IsFinished = true;
        }

        private IEnumerator OnFadeOutAndIn(float fadeOutDuration, float keepDuration, float fadeInDuration)
        {
            fadeOutDuration = fadeOutDuration < 0f ? _duration : fadeOutDuration;
            StartCoroutine(OnFadeOut(fadeOutDuration));
            yield return new WaitForSeconds(fadeOutDuration + keepDuration);

            fadeInDuration = fadeInDuration < 0f ? _duration : fadeInDuration;
            StartCoroutine(OnFadeIn(fadeInDuration));
        }
    }
}