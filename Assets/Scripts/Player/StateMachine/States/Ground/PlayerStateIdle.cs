using Maxy.GameFramework.Common.System;

namespace Game.Player
{
    public class PlayerStateIdle : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Body.ZeroVelocityX();

            if (Paramaters.IsLand)
            {
                Paramaters.IsLand = false;
                
                Animator.PlayLand();
            }
            else
            {
                Animator.PlayIdle();
            }
        }
    }
}