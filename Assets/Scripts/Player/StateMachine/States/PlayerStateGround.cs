namespace Game.Player
{
    public class PlayerStateGround : PlayerStateBase
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            //TODO: 当不在地面时
            if (false)
            {
                Paramaters.IsGrounded = false;
                StateMachine.RequestToChangeState(StateMachine.StateFall);
            }
        }

        //任何地面状态都至少需要地面标记为true时，才能进入
        public override bool CanEnter() => Paramaters.IsGrounded;
    }
}