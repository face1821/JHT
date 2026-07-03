using Maxy.GameFramework.Game2D.Tool;
using UnityEngine;

namespace Game.Stuff
{
    public class StairDisableColliderWhenDetect : MonoBehaviour
    {
        [SerializeField] private BoxDetection2D _upDetection;
        [SerializeField] private BoxDetection2D _detection;

        private Collider2D _collider;

        private void Awake() { _collider = GetComponent<Collider2D>(); }

        private void FixedUpdate()
        {
            //上面没人的时候，才根据下方是否有人来关闭碰撞
            //换言之，上面有人的时候，碰撞不发生启用关闭变化
            if (!_upDetection.Detect())
            {
                _collider.enabled = !_detection.Detect();
            }
        }
    }
}