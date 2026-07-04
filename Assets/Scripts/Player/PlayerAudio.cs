using System.Collections.Generic;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [Header("人声")]
        [SerializeField]
        private List<AudioClip> _deadVoiceClipList;
        [SerializeField]
        private List<AudioClip> _happyVoiceClipList;
        [SerializeField]
        private List<AudioClip> _jumpVoiceClipList;
        [SerializeField]
        private AudioClip _confusedVoiceClip;

        [Header("音效")]
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

        private void Awake()
        {
            _audioSystem = SystemCenter.Get<IAudioSystem>();
            _audioSystem.SpaceBlend = 0.5f;
        }

        private void PlayRandomMoveOnGround()
        {
            var randomClip = _moveGroundClipList[Random.Range(0, _moveGroundClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "MoveGround", transform, 0f);
        }

        private void PlayRandomMoveOnPlatform()
        {
            var randomClip = _movePlatformClipList[Random.Range(0, _movePlatformClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "MovePlatform", transform, 0f);
        }

        private void PlayRandomMoveOnStair()
        {
            var randomClip = _moveStairClipList[Random.Range(0, _moveStairClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "MoveStair", transform, 0f);
        }

        #region 公开方法

        #region 人声

        public void PlayRandomDeadVoice()
        {
            var randomClip = _deadVoiceClipList[Random.Range(0, _deadVoiceClipList.Count)];

            _audioSystem.PlayVoice(randomClip, "_dead", transform, 0f);
        }

        public void PlayRandomHappyVoice()
        {
            var randomClip = _happyVoiceClipList[Random.Range(0, _happyVoiceClipList.Count)];

            _audioSystem.PlayVoice(randomClip, "_happy", transform, 0f);
        }

        public void PlayRandomJumpVoice()
        {
            var randomClip = _jumpVoiceClipList[Random.Range(0, _jumpVoiceClipList.Count)];

            _audioSystem.PlayVoice(randomClip, "_jump", transform, 0f);
        }

        public void PlayRandomConfusedVoice() { _audioSystem.PlayVoice(_confusedVoiceClip, "_confused", transform, 0f); }

        #endregion

        #region 音效

        public void PlayDeadSfx() { _audioSystem.PlaySfx(_deadClip, "_dead", transform, 0f); }

        public void PlayDeadTransformSfx() { _audioSystem.PlaySfx(_deadTransformClip, "_deadTransfrom", transform, 0f); }

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

            _audioSystem.PlaySfx(randomClip, "_crouchMove", transform, 0f);
        }

        public void PlayJumpSfx() { _audioSystem.PlaySfx(_jumpClip, "_jump", transform, 0f); }

        public void PlayLandSfx() { _audioSystem.PlaySfx(_landClip, "_land", transform, 0f); }

        public void PlayRandomClimbRopeSfx()
        {
            var randomClip = _climbRopeClipList[Random.Range(0, _climbRopeClipList.Count)];

            _audioSystem.PlaySfx(randomClip, "_climbRope", transform, 0f);
        }

        #endregion

        #endregion
    }
}