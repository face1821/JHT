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

        //任何空中状态都至少需要地面标记为false时，才能进入
        public override bool CanEnter() => !Paramaters.IsGrounded || Paramaters.CurrentState is PlayerStateClimb;
    }
}