using System.Collections;
using Cinemachine;
using Game.Tool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Stuff
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class Stone : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _vCam;
        [SerializeField, LabelText("幅度")] private float _camShakeAmplitude = 10f;
        [SerializeField, LabelText("频率")] private float _camShakeFrequency = 10f;

        private Animator _animator;
        private CinemachineBasicMultiChannelPerlin _noise;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _noise = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            // 初始关闭抖动
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }

        private void OnEnable()
        {
            //当激活时，播放石头的动画
            _animator.Play("Roll");
        }

        private void FixedUpdate()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("EndIdle"))
            {
                _noise.m_AmplitudeGain = 0f;
                _noise.m_FrequencyGain = 0f;
            }

            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Roll")) return;

            var distance = Vector3.Distance(InstanceFinder.Player.transform.position, transform.position);

            _noise.m_AmplitudeGain = Mathf.Clamp(_camShakeAmplitude / distance, 1f, 5f);
            _noise.m_FrequencyGain = Mathf.Clamp(_camShakeFrequency / distance, 1f, 5f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //当碰到玩家时，让玩家死亡
            if (!other.gameObject.CompareTag("Player")) return;
            //当不在滚动动画时，就没伤害
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Roll")) return;

            InstanceFinder.Player.StateMachine.Die();

            //再将自己隐藏
            StartCoroutine(nameof(OnDelayDisable));
        }

        private IEnumerator OnDelayDisable()
        {
            yield return new WaitForSeconds(1.5f);

            _animator.Play("Idle");
            gameObject.SetActive(false);
        }
    }
}