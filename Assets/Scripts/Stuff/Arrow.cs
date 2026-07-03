using Game.Tool;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Stuff
{
    [RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour
    {
        [HideInInspector] public FloatingPlatform Platform;

        [SerializeField] private float _speed;

        private void FixedUpdate()
        {
            MTool.LookAt2D(transform, InstanceFinder.Player.transform.position);
            transform.position += transform.up * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("LevelRule")) return;

            if (!enabled) return;

            //碰到物体就让箭矢停下并不再接收碰撞回调
            enabled = false;

            //碰到气球的处理
            if (other.name == "气球")
            {
                Destroy(gameObject);
                Destroy(other.gameObject);

                Platform.DisableFloating();
            }

            //碰到玩家的处理
            if (!other.CompareTag("Player")) return;

            InstanceFinder.Player.StateMachine.RequestChangeState(InstanceFinder.Player.StateMachine.StateDead);
        }
    }
}