using UnityEngine;

namespace Game.Player
{
    public class PlayerStateBase
    {
        public PlayerStateMachineParamaters Paramaters;
        public PlayerStateMachine StateMachine => Paramaters.StateMachine;
        public PlayerStateBase CurrentState => Paramaters.CurrentState;
        public PlayerBody Body => Paramaters.Body;
        public PlayerAnimator Animator => Paramaters.Animator;

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnExit() { }

        public virtual bool CanBeInterrupt(PlayerStateBase nextState) => true;
        public virtual bool CanEnter() => true;
    }
}