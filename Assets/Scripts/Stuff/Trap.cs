using System;
using Game.Tool;
using UnityEngine;

namespace Game.Stuff
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Trap : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            InstanceFinder.Player.StateMachine.RequestChangeState(InstanceFinder.Player.StateMachine.StateDead);
        }
    }
}