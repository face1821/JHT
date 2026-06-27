using System.Diagnostics;
using Maxy.GameFramework.Common.System;

namespace Game.Player
{
    public class PlayerStateRope : PlayerStateBase
    {
        public override bool CanEnter() => Paramaters.NearbyRope != null;

        public override void OnEnter()
        {
            Paramaters.ClimbingRope = Paramaters.NearbyRope;
            Body.ZeroVelocity();
            Body.SetGravityEnabled(false);
            Body.SetPositionX(Paramaters.ClimbingRope.transform.position.x);
            Body.SetFreezePositionX(true);
        }

        public override void OnFixedUpdate()
        {
            var rope = Paramaters.ClimbingRope;
            if (rope == null)
            {
                StateMachine.RequestToChangeState(StateMachine.StateFall, force: true);
                return;
            }

            Body.SetPositionX(rope.transform.position.x);
            Body.SetVelocityX(0f);

            var input = Paramaters.Input;
            var climbSpeed = Paramaters.MoveSpeed * Paramaters.ClimbSpeedMultiplier;

            if (input.IsJumpHeld && !input.IsCrouchHeld)
                Body.SetVelocityY(climbSpeed);
            else if (input.IsCrouchHeld && !input.IsJumpHeld)
                Body.SetVelocityY(-climbSpeed);
            else
                Body.SetVelocityY(0f);
        }

        public override void OnExit()
        {
            Body.SetFreezePositionX(false);
            Body.SetGravityEnabled(true);
            Paramaters.ClimbingRope = null;
        }
    }
}
