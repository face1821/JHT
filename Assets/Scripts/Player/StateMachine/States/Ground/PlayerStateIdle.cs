namespace Game.Player
{
    public class PlayerStateIdle : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Body.SetGravityEnabled(true);
            Body.Lock();
            Animator.PlayIdle();
        }

        public override void OnExit()
        {
            base.OnExit();

            Body.UnLock();
        }
    }
}