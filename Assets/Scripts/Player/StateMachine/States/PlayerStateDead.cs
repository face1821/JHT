using Maxy.GameFramework.Common.Events;

namespace Game.Player
{
    public class PlayerStateDead : PlayerStateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Body.SetGravityEnabled(false);
            Body.ZeroVelocity();
            Animator.PlayDead();
            
            AudioPlayer.PlayDeadSfx();
        }

        public override bool CanBeInterrupt(PlayerStateBase nextState) => false;
    }
}