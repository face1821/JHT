using System;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Bubble
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BubbleTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip _confusedVoiceClip;
        [SerializeField] private bool _onlyOnce;
        [SerializeField] private string _content;

        private IAudioSystem _audioSystem;

        private void Awake() { _audioSystem = SystemCenter.Get<IAudioSystem>(); }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            InstanceFinder.Player.Bubble.Speak(_content);
            _audioSystem.PlayVoice(_confusedVoiceClip, "_confused", InstanceFinder.Player.transform, 0f);

            if (_onlyOnce)
            {
                Destroy(gameObject);
            }
        }
    }
}