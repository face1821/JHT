namespace Game.Player
{
    public class PlayerStateDead : PlayerStateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Animator.PlayDead();
        }

        public override bool CanBeInterrupt(PlayerStateBase nextState) => false;
    }
}