using System.Diagnostics;
using Maxy.GameFramework.Common.System;

namespace Game.Player
{
    public class PlayerStateClimb : PlayerStateBase
    {
        public override bool CanEnter() => Paramaters.ClimbingObject != null;

        public override void OnEnter()
        {
            Body.ZeroVelocity();
            Body.SetGravityEnabled(false);
            Body.SetPositionX(Paramaters.ClimbingObject.transform.position.x);

            MLogger.Log($"玩家：攀爬到 {Paramaters.ClimbingObject.transform.name}");
        }

        public override void OnFixedUpdate()
        {
            //攀爬物消失后，自动脱离，变为下坠状态
            var climbingObject = Paramaters.ClimbingObject;
            if (climbingObject == null)
            {
                StateMachine.RequestToChangeState(StateMachine.StateFall);
                return;
            }

            //当在攀爬时，按下左右键也会立刻脱离攀爬状态
            if (PlayerInput.MoveDirection != 0)
            {
                StateMachine.RequestToChangeState(StateMachine.StateFall);
                return;
            }


            //攀爬速度
            var climbSpeed = Paramaters.MoveSpeed * Paramaters.ClimbSpeedMultiplier;
            //攀爬速度*=攀爬方向
            climbSpeed *= PlayerInput.UpDownMoveDirection;

            //应用最终攀爬方向
            Body.SetVelocityY(climbSpeed);
        }

        public override void OnExit()
        {
            Body.SetGravityEnabled(true);
            Paramaters.ClimbingObject = null;
        }
    }
}