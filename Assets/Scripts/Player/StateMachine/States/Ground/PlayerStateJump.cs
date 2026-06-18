namespace Game.Player
{
    public class PlayerStateJump : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Paramaters.IsGrounded = false;
        }
    }
}