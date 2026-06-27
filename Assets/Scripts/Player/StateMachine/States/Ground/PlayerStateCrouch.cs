namespace Game.Player
{
    public class PlayerStateCrouch : PlayerStateGround
    {
        public override void OnFixedUpdate()
        {
            base.OnUpdate();

            if (Paramaters.MoveDirection != 0)
            {
                var speed = Paramaters.MoveSpeed * Paramaters.CrouchSpeedMultiplier;
                Body.SetVelocityX(Paramaters.MoveDirection * speed);
                Body.SetFaceX(Paramaters.FaceDirection);
            }
            else
            {
                Body.ZeroVelocityX();
            }
        }
    }
}
