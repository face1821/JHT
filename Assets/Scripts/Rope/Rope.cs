using Game.InteractableObject;
using Game.Player;
using Game.Tool;
using UnityEngine;

namespace Game.Rope
{
    public class Rope : MonoBehaviour, IInteractableObject
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            InstanceFinder.Player.StateMachine.Paramaters.NearbyRope = this;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            var stateMachine = InstanceFinder.Player.StateMachine;
            var paramaters = stateMachine.Paramaters;

            if (paramaters.NearbyRope == this)
                paramaters.NearbyRope = null;

            if (stateMachine.CurrentState is PlayerStateRope && paramaters.ClimbingRope == this)
                stateMachine.RequestToChangeState(stateMachine.StateFall, force: true);
        }

        public float GetDistance()
        {
            return (InstanceFinder.Player.transform.position - transform.position).magnitude;
        }

        public void Interact()
        {
            var stateMachine = InstanceFinder.Player.StateMachine;
            stateMachine.Paramaters.ClimbingRope = this;
            stateMachine.RequestToChangeState(stateMachine.StateRope);
        }
    }
}
