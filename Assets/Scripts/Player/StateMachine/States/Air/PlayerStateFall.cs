namespace Game.Player
{
    public class PlayerStateFall : PlayerStateAir
    {
        public override void OnExit()
        {
            base.OnEnter();

            Paramaters.IsGrounded = true;
        }
    }
}