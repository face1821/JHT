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

            //如果碰到地面了，就进入落地状态
            if (Paramaters.IsGrounded)
            {
                StateMachine.RequestChangeState(StateMachine.StateLand);
            }

            //如果开始下坠了，就进入下坠状态
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