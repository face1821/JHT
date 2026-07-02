namespace Game.Player
{
    public class PlayerStateMove : PlayerStateGround
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            Body.SetVelocityX(Paramaters.MoveDirection * Paramaters.MoveSpeed);
            Body.SetFaceX(Paramaters.FaceDirection);
            Animator.PlayWalk();
        }
    }
}