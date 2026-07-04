using Maxy.GameFramework.Common.Events;
using UnityEngine;

namespace Game.Player
{
    public class PlayerStateMove : PlayerStateGround
    {
        private float _timer;

        public override void OnEnter()
        {
            base.OnEnter();

            AudioPlayer.PlayFootStepSfx(Paramaters.GroundStandingType);
            _timer = Time.time;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            //音效计时播放
            if (_timer > 0f)
            {
                var deltaTime = Time.time - _timer;

                if (deltaTime >= Paramaters.MoveAudioInterval)
                {
                    AudioPlayer.PlayFootStepSfx(Paramaters.GroundStandingType);
                    _timer = Time.time;
                }
            }

            Body.SetVelocityX(Paramaters.MoveDirection * Paramaters.MoveSpeed);
            Body.SetFaceX(Paramaters.FaceDirection);
            Animator.PlayWalk();
        }
    }
}