using System;
using Maxy.GameFramework.Common.System;
using UnityEngine;
using UnityEngine.Events;

namespace Maxy.GameFramework.Common.Tool
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxColliderTrigger2D : MonoBehaviour
    {
        public UnityEvent OnEnter;
        public UnityEvent OnStay;
        public UnityEvent OnExit;

        private void OnValidate()
        {
            Collider2D collier = GetComponent<Collider2D>();
            if (collier != null && !collier.isTrigger)
            {
                collier.isTrigger = true;

                MLogger.LogWarning($"{gameObject.name} 的Collider2D必须开启触发器！", this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) { OnEnter?.Invoke(); }

        private void OnTriggerStay2D(Collider2D other) { OnStay?.Invoke(); }

        private void OnTriggerExit2D(Collider2D other) { OnExit?.Invoke(); }
    }
}