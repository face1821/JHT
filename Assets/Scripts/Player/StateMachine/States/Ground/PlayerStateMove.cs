namespace Game.Player
{
    public class PlayerStateMove : PlayerStateGround
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            Body.SetVelocityX(Paramaters.MoveDirection * Paramaters.MoveSpeed);
        }
    }
}