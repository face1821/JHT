namespace Game.Player
{
    public class PlayerStateIdle : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Body.SetGravityEnabled(true);
            Body.ZeroVelocityX();
            Animator.PlayIdle();
        }
    }
}