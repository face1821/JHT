using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Prop
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Rope : MonoBehaviour, IClimbingObject
    {
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