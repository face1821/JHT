namespace Game.Player
{
    public class PlayerStateJump : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Body.SetVelocityY(Paramaters.JumpSpeed);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (Body.Velocity.y <= 0f)
            {
                StateMachine.RequestToChangeState(StateMachine.StateFall);
            }
        }
    }
}