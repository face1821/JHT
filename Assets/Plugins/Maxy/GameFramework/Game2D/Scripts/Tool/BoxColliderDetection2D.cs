using System;
using UnityEngine;

namespace Maxy.GameFramework.Game2D.Tool
{
    public class BoxColliderDetection2D : MonoBehaviour
    {
        public event Action<Collider2D> OnTouched;
        public event Action<Collider2D> OnLeave;

        public LayerMask LayerMask;
        public bool AutoRun;
        public bool Touched;

        public Transform ObjectTouched => _ColliderArrayCache[0].transform;

        private Collider2D[] _ColliderArrayCache = new Collider2D[1];

        private BoxCollider2D _boxCollider2D;

        private void Awake() { _boxCollider2D = GetComponent<BoxCollider2D>(); }

        public bool Detect()
        {
            int size = Physics2D.OverlapBoxNonAlloc(
                _boxCollider2D.bounds.center,
                _boxCollider2D.bounds.size,
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!AutoRun) return;
            if ((LayerMask.value & (1 << other.gameObject.layer)) == 0) return;

            Touched = true;
            _ColliderArrayCache[0] = other;
            OnTouched?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!AutoRun) return;
            if ((LayerMask.value & (1 << other.gameObject.layer)) == 0) return;

            Touched = false;
            _ColliderArrayCache[0] = null;
            OnLeave?.Invoke(other);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!AutoRun) return;
            if ((LayerMask.value & (1 << other.gameObject.layer)) == 0) return;

            Touched = true;
            _ColliderArrayCache[0] = other.collider;
            OnTouched?.Invoke(other.collider);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!AutoRun) return;
            if ((LayerMask.value & (1 << other.gameObject.layer)) == 0) return;

            Touched = false;
            _ColliderArrayCache[0] = null;
            OnLeave?.Invoke(other.collider);
        }
    }
}