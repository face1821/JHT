using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    public class CircleDetection : MonoBehaviour
    {
        public bool AlwaysRun;

        public float Radius;
        public LayerMask LayerMask;
        public Color Color = Color.yellow;
        public bool Touched;

        public Transform ObjectTouched => _ColliderArrayCache[0].transform;

        private Collider[] _ColliderArrayCache = new Collider[1];

        private void FixedUpdate()
        {
            if (AlwaysRun)
                Detect();
        }

        public bool Detect()
        {
            // 检测球体区域内是否有其他物体存在
            var size = Physics.OverlapSphereNonAlloc(transform.position, Radius, _ColliderArrayCache, LayerMask.value);

            if (size > 0)
            {
                Touched = true;
                return true;
            }

            Touched = false;
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}