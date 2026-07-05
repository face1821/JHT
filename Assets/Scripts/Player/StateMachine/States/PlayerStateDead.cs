using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;

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
            AudioPlayer.PlayRandomDeadVoice();

            //记录死亡次数
            SaveSystem.Save("DeadCount", SaveSystem.Load("DeadCount", 0) + 1);
        }

        public override bool CanBeInterrupt(PlayerStateBase nextState) => false;
    }
}