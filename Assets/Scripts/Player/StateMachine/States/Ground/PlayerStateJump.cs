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
                StateMachine.RequestChangeState(StateMachine.StateFall);
            }

            Body.SetVelocityX(Paramaters.MoveDirection * Paramaters.MoveSpeed);
            Body.SetFaceX(Paramaters.FaceDirection);
        }

        public override bool CanBeInterrupt(PlayerStateBase nextState) => nextState is PlayerStateFall or PlayerStateClimb;
    }
}