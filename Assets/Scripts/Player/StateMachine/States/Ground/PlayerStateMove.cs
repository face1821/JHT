using Maxy.GameFramework.Common.Events;
using UnityEngine;

namespace Game.Player
{
    public class PlayerStateMove : PlayerStateGround
    {
        private float _timeInterval = 0.2f;
        private float _timer;

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            //音效计时播放
            if (_timer > 0f)
            {
                _timer = Time.time - _timer;

                if (_timer >= _timeInterval)
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