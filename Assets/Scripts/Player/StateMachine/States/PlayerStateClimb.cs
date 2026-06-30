using System.Diagnostics;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerStateClimb : PlayerStateBase
    {
        public override bool CanEnter() => Paramaters.ClimbingObject != null;

        public override void OnEnter()
        {
            //手动解除玩家的移动输入，这样玩家就不会刚攀爬到物体后由于手没及时松开移动键而又掉下去的情况
            PlayerInput.Instance.BtnReleaseMoveLeft();
            PlayerInput.Instance.BtnReleaseMoveRight();

            Body.ZeroVelocity();
            Body.SetGravityEnabled(false);

            int faceToClimbObject = (Paramaters.ClimbingObject.transform.position.x - InstanceFinder.Player.transform.position.x) > 0f ? 1 : -1;
            var offset = -faceToClimbObject * 0.5f;
            Body.SetFaceX((int)faceToClimbObject);
            Body.SetPositionX(Paramaters.ClimbingObject.transform.position.x + offset);
            Animator.PlayClimbIdle();

            MLogger.Log($"玩家：攀爬到 {Paramaters.ClimbingObject.transform.name}");
        }

        public override void OnFixedUpdate()
        {
            //攀爬物消失后，自动脱离，变为下坠状态
            var climbingObject = Paramaters.ClimbingObject;
            if (climbingObject == null)
            {
                MLogger.Log("状态机：攀爬物体消失，脱离攀爬状态");
                StateMachine.RequestToChangeState(StateMachine.StateFall);
                return;
            }

            //当在攀爬时，按下左右键也会立刻脱离攀爬状态
            if (PlayerInput.MoveDirection != 0)
            {
                MLogger.Log("状态机：左右动，脱离攀爬状态");
                StateMachine.RequestToChangeState(StateMachine.StateFall);
                return;
            }


            //攀爬速度
            var climbSpeed = Paramaters.MoveSpeed * Paramaters.ClimbSpeedMultiplier;
            //攀爬速度*=攀爬方向
            climbSpeed *= PlayerInput.UpDownMoveDirection;

            MLogger.LogWarning($"{Paramaters.MoveSpeed} * {Paramaters.ClimbSpeedMultiplier} * {PlayerInput.UpDownMoveDirection} = {climbSpeed}");

            //动画设置
            if (climbSpeed != 0f)
            {
                Animator.PlayClimb();
            }
            else
            {
                Animator.PlayClimbIdle();
            }

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