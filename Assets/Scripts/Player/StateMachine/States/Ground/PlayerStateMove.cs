namespace Game.Player
{
    public class PlayerStateMove : PlayerStateGround
    {
        public override void OnFixedUpdate()
        {
            base.OnUpdate();
            
            Body.SetVelocityX(Paramaters.MoveDirection * Paramaters.MoveSpeed);
            Body.SetFaceX(Paramaters.FaceDirection);
        }
    }
}