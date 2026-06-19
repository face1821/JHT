namespace Game.Player
{
    public class PlayerStateIdle : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Body.ZeroVelocityX();
        }
    }
}