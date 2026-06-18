using System;
using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    public class BoxDetection : MonoBehaviour
    {
        public bool AutoRun;

        public LayerMask LayerMask;
        public Color Color = Color.yellow;
        public bool Touched;

        public Transform ObjectTouched => _ColliderArrayCache[0].transform;

        private Collider[] _ColliderArrayCache = new Collider[1];

        public bool Detect()
        {
            // 检测立方体区域内是否有其他物体存在
            var size = Physics.OverlapBoxNonAlloc(transform.position, transform.localScale / 2f, _ColliderArrayCache,
                transform.rotation, LayerMask.value);

            if (size > 0)
            {
                Touched = true;
                return true;
            }

            Touched = false;
            return false;
        }

        private void FixedUpdate()
        {
            if (!AutoRun) return;

            Detect();
        }

        private void OnDrawGizmosSelected()
        {
            Matrix4x4 originalMatrix = Gizmos.matrix;

            // 设置新的矩阵
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position,
                transform.rotation,
                transform.localScale
            );

            // 在Scene视图中绘制立方体区域
            Gizmos.color = Color;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            // 恢复Gizmos矩阵
            Gizmos.matrix = originalMatrix;
        }
    }
}