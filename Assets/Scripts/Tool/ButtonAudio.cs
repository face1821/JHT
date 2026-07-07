using Maxy.GameFramework.Common.System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Tool
{
    public class ButtonAudio : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private AudioClip _audioClip;

        private IAudioSystem _audioSystem;

        private void Awake() { _audioSystem = SystemCenter.Get<IAudioSystem>(); }

        public void OnPointerDown(PointerEventData eventData) { _audioSystem.PlaySfx(_audioClip, "ButtonAudio"); }
    }
}