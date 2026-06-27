namespace Game.Player
{
    public class PlayerStateCrouch : PlayerStateGround
    {
        public override void OnFixedUpdate()
        {
            base.OnUpdate();

            Body.SetVelocityX(Paramaters.MoveDirection * Paramaters.MoveSpeed * Paramaters.CrouchSpeedMultiplier);
            Body.SetFaceX(Paramaters.FaceDirection);
        }
    }
}