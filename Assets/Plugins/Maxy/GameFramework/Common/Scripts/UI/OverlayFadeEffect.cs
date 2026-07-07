using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maxy.GameFramework.Common.Tool
{
    public class OverlayFadeEffect : MonoBehaviour
    {
        [ReadOnly]
        public bool IsFinished;
        public bool ForceStartFromEdgeValue;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue = 1f;
        [SerializeField] private float _childMinValue;
        [SerializeField] private float _childMaxValue = 1f;
        [SerializeField] private List<Image> _childImages;
        [SerializeField] private List<TextMeshProUGUI> _childTexts;

        private Image _selfImage;
        private TextMeshProUGUI _selfText;

        private void Awake()
        {
            _selfImage = GetComponent<Image>();
            _selfText = GetComponentInChildren<TextMeshProUGUI>();
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

        public void PlayFadeIn(float duration = 1f)
        {
            StopAllCoroutines();

            StartCoroutine(OnFadeIn(duration == 0f ? _duration : duration));
        }

        public void PlayFadeOut(float duration = 1f)
        {
            StopAllCoroutines();

            StartCoroutine(OnFadeOut(duration == 0f ? _duration : duration));
        }

        public void PlayFadeOutAndIn(float fadeOutDuration = 1f, float keepDuration = 1f, float fadeInDuration = 1f)
        {
            StopAllCoroutines();

            StartCoroutine(OnFadeOutAndIn(fadeOutDuration, keepDuration, fadeInDuration));
        }

        private IEnumerator OnFadeIn(float duration)
        {
            IsFinished = false;

            var startTime = Time.time;

            //从头开始播放
            if (ForceStartFromEdgeValue)
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

            //开始
            while (_selfImage != null && _selfImage.color.a > _minValue || _selfText != null && _selfText.color.a > _minValue)
            {
                yield return null;

                if (_selfImage != null)
                    _selfImage.color = new Color(_selfImage.color.r, _selfImage.color.g, _selfImage.color.b, _maxValue - (Time.time - startTime) / duration);

                if (_selfText != null)
                    _selfText.color = new Color(_selfText.color.r, _selfText.color.g, _selfText.color.b, _maxValue - (Time.time - startTime) / duration);

                foreach (var child in _childImages)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, _childMaxValue - (Time.time - startTime) / duration);
                }

                foreach (var child in _childTexts)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, _childMaxValue - (Time.time - startTime) / duration);
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

            IsFinished = true;
        }

        private IEnumerator OnFadeOut(float duration)
        {
            IsFinished = false;

            var startTime = Time.time;

            //从头开始播放
            if (ForceStartFromEdgeValue)
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

            //开始
            while (_selfImage != null && _selfImage.color.a < _maxValue || _selfText != null && _selfText.color.a < _maxValue)
            {
                yield return null;

                if (_selfImage != null)
                    _selfImage.color = new Color(_selfImage.color.r, _selfImage.color.g, _selfImage.color.b, (Time.time - startTime) / duration);

                if (_selfText != null)
                    _selfText.color = new Color(_selfText.color.r, _selfText.color.g, _selfText.color.b, (Time.time - startTime) / duration);

                foreach (var child in _childImages)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, (Time.time - startTime) / duration);
                }

                foreach (var child in _childTexts)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, (Time.time - startTime) / duration);
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

            IsFinished = true;
        }

        private IEnumerator OnFadeOutAndIn(float fadeOutDuration, float keepDuration, float fadeInDuration)
        {
            StartCoroutine(OnFadeOut(fadeOutDuration));
            yield return new WaitForSeconds(fadeOutDuration + keepDuration);

            StartCoroutine(OnFadeIn(fadeInDuration));
        }
    }
}