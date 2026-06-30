namespace Game.Player
{
    public class PlayerStateLand : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Body.ZeroVelocityX();
            Animator.PlayLand();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            //如果落地动画已经过渡到待机动画了，就切换为待机状态
            if (Animator.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                StateMachine.RequestToChangeState(StateMachine.StateIdle);
            }
        }

        public override bool CanBeInterrupt(PlayerStateBase nextState) => Animator.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }
}