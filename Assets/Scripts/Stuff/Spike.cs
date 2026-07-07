using System.Collections;
using Cinemachine;
using DG.Tweening;
using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Stuff
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Spike : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _playerVCam;
        [SerializeField, LabelText("幅度")] private float _camShakeAmplitude = 10f;
        [SerializeField, LabelText("频率")] private float _camShakeFrequency = 10f;
        [Space]
        [SerializeField] private DeadAreaTrap SpikeTrap;
        [SerializeField] private float _time;
        [SerializeField] private Vector2 _startPos;
        [SerializeField] private float _endX;
        [SerializeField] private AudioClip _clip;

        private CinemachineBasicMultiChannelPerlin _noise;

        private void Awake()
        {
            _noise = _playerVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            // 初始关闭抖动
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }

        private void OnEnable() { PlayerStateMachine.OnDead += OnPlayerDead; }

        private void OnDisable() { PlayerStateMachine.OnDead -= OnPlayerDead; }

        private void FixedUpdate()
        {
            if (!SpikeTrap.gameObject.activeSelf || SpikeTrap.transform.position.x >= _endX) return;

            var distance = Vector3.Distance(InstanceFinder.Player.transform.position, SpikeTrap.transform.position);

            _noise.m_AmplitudeGain = Mathf.Clamp(_camShakeAmplitude / distance, 0f, 5f);
            _noise.m_FrequencyGain = Mathf.Clamp(_camShakeFrequency / distance, 0f, 5f);
        }

        public void StartMove()
        {
            //播放音效
            SystemCenter.Get<IAudioSystem>().PlaySfx(_clip, "_spike_clip", InstanceFinder.Player.transform);

            SpikeTrap.transform.DOKill();
            SpikeTrap.gameObject.SetActive(true);
            SpikeTrap.transform.position = new Vector3(_startPos.x, _startPos.y);
            SpikeTrap.transform.DOMoveX(_endX, _time).SetEase(Ease.Linear).OnComplete(() =>
            {
                _noise.m_AmplitudeGain = 0f;
                _noise.m_FrequencyGain = 0f;
            });
        }

        public void TpToEnd()
        {
            SystemCenter.Get<IAudioSystem>().StopSfx("_spike_clip");
            SpikeTrap.transform.DOKill();
            SpikeTrap.gameObject.SetActive(true);
            SpikeTrap.transform.position = new Vector3(_endX, _startPos.y);
            _noise.m_AmplitudeGain = 0f;
            _noise.m_FrequencyGain = 0f;
        }

        private void OnPlayerDead() { StartCoroutine(nameof(DelayOnPlayerDead)); }

        private IEnumerator DelayOnPlayerDead()
        {
            yield return new WaitForSeconds(2.5f);

            //如果玩家的存档点已经大于尖刺的这个位置
            if (SaveSystem.Load("LastPassedLevel", 0) > 2)
            {
                TpToEnd();

                yield break;
            }

            //尖刺消失
            SystemCenter.Get<IAudioSystem>().StopSfx("_spike_clip");
            SpikeTrap.gameObject.SetActive(false);
            SpikeTrap.transform.DOKill();
            MLogger.LogWarning("尖刺消失");
            _noise.m_AmplitudeGain = 0f;
            _noise.m_FrequencyGain = 0f;
        }
    }
}