using System.Diagnostics;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerStateClimb : PlayerStateBase
    {
        private float _cantReleaseTime = 0.25f;
        private float _currentCatReleaseTime;

        private float _timer;
        
        public override bool CanEnter()
        {
            if (Paramaters.ClimbingObject == null) return false;

            //保存当前的状态信息
            var oldFace = Paramaters.FaceDirection;
            var oldPos = StateMachine.transform.position;

            //将玩家放置到攀爬的位置
            int faceToClimbObject = (Paramaters.ClimbingObject.transform.position.x - InstanceFinder.Player.transform.position.x) > 0f ? 1 : -1;
            var offset = -faceToClimbObject * 0.5f;
            Body.SetFaceX(faceToClimbObject);
            Body.SetPositionX(Paramaters.ClimbingObject.transform.position.x + offset);

            //当玩家碰不到攀爬物体时，不能进入该状态
            var hit = Physics2D.OverlapCircle(
                StateMachine.transform.position + new Vector3(Paramaters.ClimbingDetectOffset.x, Paramaters.ClimbingDetectOffset.y),
                Paramaters.ClimbingDetectRadius,
                LayerMask.GetMask("ClimbingObject"));
            if (hit is null || hit.transform != Paramaters.ClimbingObject.transform)
            {
                //恢复之前的状态
                Body.SetFaceX(oldFace);
                Body.SetPosition(oldPos);

                return false;
            }

            return true;
        }

        public override void OnEnter()
        {
            //手动解除玩家的移动输入几秒，这样玩家就不会刚攀爬到物体后由于手没及时松开移动键而又掉下去的情况
            _currentCatReleaseTime = 0f;

            Body.ZeroVelocity();
            Body.SetGravityEnabled(false);

            int faceToClimbObject = (Paramaters.ClimbingObject.transform.position.x - InstanceFinder.Player.transform.position.x) > 0f ? 1 : -1;
            var offset = -faceToClimbObject * 0.5f;
            Body.SetFaceX(faceToClimbObject);
            Body.SetPositionX(Paramaters.ClimbingObject.transform.position.x + offset);

            //设置身体碰撞大小
            var bodyCollider = StateMachine.GetComponent<CapsuleCollider2D>();
            bodyCollider.offset = new Vector2(bodyCollider.offset.x, 0f);
            bodyCollider.size = new Vector2(bodyCollider.size.x, 2f);

            Animator.PlayClimbIdle();

            MLogger.Log($"玩家：攀爬到 {Paramaters.ClimbingObject.transform.name}");
        }

        public override void OnFixedUpdate()
        {
            //无法主动脱离的时间计时
            if (_currentCatReleaseTime < _cantReleaseTime)
                _currentCatReleaseTime += Time.fixedDeltaTime;

            //攀爬物消失后，自动脱离，变为下坠状态
            var climbingObject = Paramaters.ClimbingObject;
            if (climbingObject == null)
            {
                StateMachine.RequestChangeState(StateMachine.StateFall);
                return;
            }

            //当在攀爬时，按下左右键也会立刻脱离攀爬状态
            if (_currentCatReleaseTime >= _cantReleaseTime && PlayerInput.MoveDirection != 0)
            {
                StateMachine.RequestChangeState(StateMachine.StateFall);
                return;
            }

            //当玩家碰不到绳子时，脱离攀爬状态
            var hit = Physics2D.OverlapCircle(
                StateMachine.transform.position + new Vector3(Paramaters.ClimbingDetectOffset.x, Paramaters.ClimbingDetectOffset.y),
                Paramaters.ClimbingDetectRadius,
                LayerMask.GetMask("ClimbingObject"));
            if (hit is null || hit.transform != climbingObject.transform)
            {
                StateMachine.RequestChangeState(StateMachine.StateFall);
                return;
            }

            //攀爬速度
            var climbSpeed = Paramaters.MoveSpeed * Paramaters.ClimbSpeedMultiplier;
            //攀爬速度*=攀爬方向
            climbSpeed *= PlayerInput.UpDownMoveDirection;

            //动画设置
            if (climbSpeed != 0f)
            {
                Animator.PlayClimb();
                
                //音效计时播放
                if (_timer > 0f)
                {
                    var deltaTime = Time.time - _timer;

                    if (deltaTime >= Paramaters.ClimbAudioInterval)
                    {
                        AudioPlayer.PlayRandomClimbRopeSfx();
                        _timer = Time.time;
                    }
                }
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
            //设置身体碰撞大小
            var bodyCollider = StateMachine.GetComponent<CapsuleCollider2D>();
            bodyCollider.offset = new Vector2(bodyCollider.offset.x, -0.7f);
            bodyCollider.size = new Vector2(bodyCollider.size.x, 2.5f);

            Body.SetGravityEnabled(true);
            Paramaters.ClimbingObject = null;
        }
    }
}