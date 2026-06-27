using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using UnityEngine;

namespace Game.Prop
{
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
                playerStateMachine.RequestToChangeState(playerStateMachine.StateFall);
                return;
            }

            playerStateMachine.TryToClimb(this);
        }
    }
}