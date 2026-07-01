using System;
using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Prop
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Light2D), typeof(BoxCollider2D))]
    public class Rope : MonoBehaviour, IClimbingObject
    {
        protected Light2D _highLight;

        private void Awake() { _highLight = GetComponent<Light2D>(); }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            EventBus.Publish(new AddPlayerInteractableObjectEvent(this));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            EventBus.Publish(new RemovePlayerInteractableObjectEvent(this));
        }

        public void SetHighLight(bool state) { _highLight.enabled = state; }

        public float GetDistance() { return Vector3.Distance(transform.position, InstanceFinder.Player.transform.position); }

        public void Interact()
        {
            var playerStateMachine = InstanceFinder.Player.StateMachine;

            //如果玩家已经在攀爬这条绳子
            if (ReferenceEquals(playerStateMachine.Paramaters.ClimbingObject, this))
            {
                MLogger.Log("脱离攀爬");
                playerStateMachine.RequestToChangeState(playerStateMachine.StateFall);
                return;
            }

            MLogger.Log("尝试攀爬");
            playerStateMachine.TryToClimb(this);
        }
    }
}