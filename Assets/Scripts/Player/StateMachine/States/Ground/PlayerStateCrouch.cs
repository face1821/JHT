using UnityEngine;

namespace Game.Player
{
    public class PlayerStateCrouch : PlayerStateGround
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var bodyCollider = StateMachine.GetComponent<CapsuleCollider2D>();
            bodyCollider.offset = new Vector2(bodyCollider.offset.x, -1.2f);
            bodyCollider.size = new Vector2(bodyCollider.size.x, 1.6f);
        }

        public override void OnFixedUpdate()
        {
            base.OnUpdate();

            Body.SetVelocityX(Paramaters.MoveDirection * Paramaters.MoveSpeed * Paramaters.CrouchSpeedMultiplier);
            Body.SetFaceX(Paramaters.FaceDirection);

            //蹲的动画设置
            if (Body.Velocity.x == 0f)
            {
                Animator.PlayCrouchIdle();
            }
            else
            {
                Animator.PlayCrouchWalk();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            var bodyCollider = StateMachine.GetComponent<CapsuleCollider2D>();
            bodyCollider.offset = new Vector2(bodyCollider.offset.x, -0.7f);
            bodyCollider.size = new Vector2(bodyCollider.size.x, 2.5f);
        }

        public override bool CanBeInterrupt(PlayerStateBase nextState) { return base.CanBeInterrupt(nextState) && !Paramaters.IsCrouchHead; }
    }
}