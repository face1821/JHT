using System;
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
        [SerializeField] private float _duration = 1f;
        [SerializeField] private List<Image> _childImages;
        [SerializeField] private List<TextMeshProUGUI> _childTexts;

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
            Image pic = GetComponent<Image>();

            var startTime = Time.time;

            //预热
            pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 1f);

            foreach (var child in _childImages)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
            }

            foreach (var child in _childTexts)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
            }

            //开始
            while (pic.color.a > 0f)
            {
                yield return null;
                pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 1f - (Time.time - startTime) / duration);

                foreach (var child in _childImages)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, 1f - (Time.time - startTime) / duration);
                }

                foreach (var child in _childTexts)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, 1f - (Time.time - startTime) / duration);
                }

                //结束工作
                if (pic.color.a <= 0f)
                {
                    pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 0f);

                    foreach (var child in _childImages)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, 0f);
                    }

                    foreach (var child in _childTexts)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, 0f);
                    }
                }
            }

            IsFinished = true;
        }

        private IEnumerator OnFadeOut(float duration)
        {
            IsFinished = false;
            Image pic = GetComponent<Image>();

            var startTime = Time.time;

            //预热
            pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 0f);

            foreach (var child in _childImages)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, 0f);
            }

            foreach (var child in _childTexts)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, 0f);
            }

            //开始
            while (pic.color.a < 1f)
            {
                yield return null;
                pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, (Time.time - startTime) / duration);

                foreach (var child in _childImages)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, (Time.time - startTime) / duration);
                }

                foreach (var child in _childTexts)
                {
                    child.color = new Color(child.color.r, child.color.g, child.color.b, (Time.time - startTime) / duration);
                }

                if (pic.color.a >= 1f)
                {
                    pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 1f);

                    foreach (var child in _childImages)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
                    }

                    foreach (var child in _childTexts)
                    {
                        child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
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