using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        public Animator Animator => _animator;

        [SerializeField] private Animator _animator;

        #region 工具

        public void Play(string aniName) { _animator.Play(aniName); }

        #endregion

        #region 动画状态

        public void PlayDead() { _animator.Play("Dead"); }

        public void PlayIdle() { _animator.Play("Idle"); }

        public void PlayWalk() { _animator.Play("Walk"); }

        public void PlayCrouchIdle() { _animator.Play("CrouchIdle"); }

        public void PlayCrouchWalk() { _animator.Play("CrouchWalk"); }

        public void PlayJump() { _animator.Play("Jump"); }

        public void PlayFall() { _animator.Play("Fall"); }

        public void PlayLand() { _animator.Play("Land"); }

        public void PlayClimbIdle() { _animator.Play("ClimbIdle"); }

        public void PlayClimb() { _animator.Play("Climb"); }

        #endregion
    }
}