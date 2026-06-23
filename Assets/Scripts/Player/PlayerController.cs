using System;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(PlayerInteract))]
    [RequireComponent(typeof(PlayerBody), typeof(PlayerStateMachine))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerInput Input { get; private set; }
        public PlayerInteract Interact { get; private set; }
        public PlayerBody Body { get; private set; }
        public PlayerStateMachine StateMachine { get; private set; }

        private void Awake()
        {
            Input = GetComponent<PlayerInput>();
            Interact = GetComponent<PlayerInteract>();
            Body = GetComponent<PlayerBody>();
            StateMachine = GetComponent<PlayerStateMachine>();
        }
    }
}