using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Tool
{
    public class ButtonScaleEffecct : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float PressScaleRate = 0.8f;
        public float Duration = 0.1f;

        private RectTransform _rect;
        private Vector3 _defaultScale;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _defaultScale = _rect.localScale;
        }

        public void OnPointerDown(PointerEventData eventData) { _rect.DOScale(PressScaleRate * _defaultScale, Duration); }

        public void OnPointerUp(PointerEventData eventData) { _rect.DOScale(_defaultScale, Duration); }
    }
}