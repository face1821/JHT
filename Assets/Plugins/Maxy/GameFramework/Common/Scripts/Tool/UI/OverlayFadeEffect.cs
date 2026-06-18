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
        if (duration != 0f)
            _duration = duration;
        StartCoroutine(nameof(OnFadeIn));
    }

    public void PlayFadeOut(float duration = 1f)
    {
        if (duration != 0f)
            _duration = duration;
        StartCoroutine(nameof(OnFadeOut));
    }

    private IEnumerator OnFadeIn()
    {
        IsFinished = false;
        Image pic = GetComponent<Image>();

        var startTime = Time.time;

        while (pic.color.a > 0f)
        {
            yield return null;
            pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 1f - (Time.time - startTime) / _duration);

            foreach (var child in _childImages)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, 1f - (Time.time - startTime) / _duration);
            }

            foreach (var child in _childTexts)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, 1f - (Time.time - startTime) / _duration);
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

    private IEnumerator OnFadeOut()
    {
        IsFinished = false;
        Image pic = GetComponent<Image>();

        var startTime = Time.time;

        while (pic.color.a < 1f)
        {
            yield return null;
            pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, (Time.time - startTime) / _duration);

            foreach (var child in _childImages)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, (Time.time - startTime) / _duration);
            }

            foreach (var child in _childTexts)
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, (Time.time - startTime) / _duration);
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
}
}