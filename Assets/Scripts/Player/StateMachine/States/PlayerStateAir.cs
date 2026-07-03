namespace Game.Player
{
    public class PlayerStateAir : PlayerStateBase
    {
        public override void OnFixedUpdate()
        {
            base.OnUpdate();

            Body.SetVelocityX(Paramaters.MoveDirection * Paramaters.MoveSpeed);
            Body.SetFaceX(Paramaters.FaceDirection);
        }

        public override bool CanEnter() => CurrentState is not PlayerStateLand && (CurrentState is PlayerStateGround or PlayerStateClimb);
    }
}