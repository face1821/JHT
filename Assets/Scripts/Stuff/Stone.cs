using System;
using System.Collections;
using Game.Tool;
using UnityEngine;

namespace Game.Stuff
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class Stone : MonoBehaviour
    {
        private Animator _animator;

        private void Awake() { _animator = GetComponent<Animator>(); }

        private void OnEnable()
        {
            //当激活时，播放石头的动画
            _animator.Play("Roll");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //当碰到玩家时，让玩家死亡
            if (!other.gameObject.CompareTag("Player")) return;
            //当不在滚动动画时，就没伤害
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Roll")) return;

            InstanceFinder.Player.StateMachine.RequestChangeState(InstanceFinder.Player.StateMachine.StateDead);

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