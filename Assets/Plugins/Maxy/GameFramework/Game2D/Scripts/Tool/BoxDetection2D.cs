using System;
using UnityEngine;

namespace Maxy.GameFramework.Game2D.Tool
{
    public class BoxDetection2D : MonoBehaviour
    {
        public bool AutoRun;

        public LayerMask LayerMask;
        public Color Color = Color.yellow;
        public bool Touched;

        public Transform ObjectTouched => _ColliderArrayCache[0].transform;

        private Collider2D[] _ColliderArrayCache = new Collider2D[1];

        public bool Detect()
        {
            int size = Physics2D.OverlapBoxNonAlloc(
                transform.position,
                transform.localScale,
                transform.eulerAngles.z,
                _ColliderArrayCache,
                LayerMask.value
            );

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

            Gizmos.matrix = Matrix4x4.TRS(
                transform.position,
                transform.rotation,
                transform.localScale
            );

            Gizmos.color = Color;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            Gizmos.matrix = originalMatrix;
        }
    }
}