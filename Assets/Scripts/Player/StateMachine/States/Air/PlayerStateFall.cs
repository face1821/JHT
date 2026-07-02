namespace Game.Player
{
    public class PlayerStateFall : PlayerStateAir
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            Animator.PlayFall();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (Paramaters.IsGrounded)
            {
                StateMachine.RequestChangeState(StateMachine.StateLand);
            }
        }
    }
}