using System;
using System.Collections.Generic;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _deadClip;
        [SerializeField]
        private AudioClip _deadTransformClip;
        [SerializeField]
        private AudioClip _jumpClip;
        [SerializeField]
        private AudioClip _landClip;

        [SerializeField]
        private List<AudioClip> _climbRopeClipList;
        [SerializeField]
        private List<AudioClip> _crouchMoveClipList;

        [FoldoutGroup("Move"), SerializeField]
        private List<AudioClip> _moveGroundClipList;
        [FoldoutGroup("Move"), SerializeField]
        private List<AudioClip> _movePlatformClipList;
        [FoldoutGroup("Move"), SerializeField]
        private List<AudioClip> _moveStairClipList;

        private IAudioSystem _audioSystem;

        private void Awake() { _audioSystem = SystemCenter.Get<IAudioSystem>(); }

        private void PlayRandomMoveOnGround()
        {
            var randomClip = _moveGroundClipList[Random.Range(0, _moveGroundClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "MoveGround", transform);
        }

        private void PlayRandomMoveOnPlatform()
        {
            var randomClip = _movePlatformClipList[Random.Range(0, _movePlatformClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "MovePlatform", transform);
        }

        private void PlayRandomMoveOnStair()
        {
            var randomClip = _moveStairClipList[Random.Range(0, _moveStairClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "MoveStair", transform);
        }

        #region 公开方法

        public void PlayDeadSfx() { _audioSystem.PlaySfx(_deadClip, "_dead", transform); }

        public void PlayDeadTransformSfx() { _audioSystem.PlaySfx(_deadTransformClip, "_deadTransfrom", transform); }

        public void PlayFootStepSfx(FootStepAudioType type)
        {
            if (type == FootStepAudioType.Ground)
                PlayRandomMoveOnGround();
            else if (type == FootStepAudioType.Platform)
                PlayRandomMoveOnPlatform();
            else
                PlayRandomMoveOnStair();
        }

        public void PlayCrouchMoveSfx()
        {
            var randomClip = _crouchMoveClipList[Random.Range(0, _crouchMoveClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "_crouchMove", transform);
        }

        public void PlayJumpSfx() { _audioSystem.PlaySfx(_jumpClip, "_jump", transform); }

        public void PlayLandSfx() { _audioSystem.PlaySfx(_landClip, "_land", transform); }

        public void PlayRandomClimbRopeSfx()
        {
            var randomClip = _climbRopeClipList[Random.Range(0, _climbRopeClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "_climbRope", transform);
        }

        #endregion
    }
}