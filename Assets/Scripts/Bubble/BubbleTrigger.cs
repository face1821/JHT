using System;
using Game.Player;
using Game.Tool;
using UnityEngine;

namespace Game.Bubble
{
    public class BubbleTrigger : MonoBehaviour
    {
        [SerializeField] private bool _onlyOnce;
        [SerializeField] private string _content;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            InstanceFinder.Player.Bubble.Speak(_content);

            if (_onlyOnce)
            {
                Destroy(gameObject);
            }
        }
    }
}