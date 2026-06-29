namespace Game.Player
{
    public class PlayerStateJump : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Body.SetVelocityY(Paramaters.JumpSpeed);
            Animator.PlayJump();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();


            if (Body.Velocity.y <= 0f)
            {
                StateMachine.RequestToChangeState(StateMachine.StateFall);
            }
        }

        public override bool CanBeInterrupt(PlayerStateBase nextState) => nextState is PlayerStateFall;
    }
}