using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    [RequireComponent(typeof(BoxCollider))]
    public class BoxColliderDetection : MonoBehaviour
    {
        public event Action<Collider> OnTouched;
        public event Action<Collider> OnLeave;

        [ShowInInspector, ReadOnly] public bool Touched { get; private set; }
        public Transform ObjectTouched => _ColliderArrayCache[0].transform;

        public LayerMask LayerMask;
        public bool AutoRun;

        private Collider[] _ColliderArrayCache = new Collider[1];
        private BoxCollider _boxCollider;

        private void Awake() { _boxCollider = GetComponent<BoxCollider>(); }

        public bool Detect()
        {
            int size = Physics.OverlapBoxNonAlloc(
                _boxCollider.bounds.center,
                _boxCollider.bounds.extents,
                _ColliderArrayCache,
                transform.rotation,
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

        private void OnTriggerEnter(Collider other)
        {
            if (!AutoRun) return;
            if ((LayerMask.value & (1 << other.gameObject.layer)) == 0) return;

            Touched = true;
            _ColliderArrayCache[0] = other;
            OnTouched?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!AutoRun) return;
            if ((LayerMask.value & (1 << other.gameObject.layer)) == 0) return;

            Touched = false;
            _ColliderArrayCache[0] = null;
            OnLeave?.Invoke(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!AutoRun) return;
            if ((LayerMask.value & (1 << other.gameObject.layer)) == 0) return;

            Touched = true;
            _ColliderArrayCache[0] = other.collider;
            OnTouched?.Invoke(other.collider);
        }

        private void OnCollisionExit(Collision other)
        {
            if (!AutoRun) return;
            if ((LayerMask.value & (1 << other.gameObject.layer)) == 0) return;

            Touched = false;
            _ColliderArrayCache[0] = null;
            OnLeave?.Invoke(other.collider);
        }
    }
}