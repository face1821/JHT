namespace Game.Player
{
    public class PlayerStateFall : PlayerStateAir
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (Paramaters.IsGrounded)
            {
                StateMachine.RequestToChangeState(StateMachine.StateIdle);
            }
        }
    }
}