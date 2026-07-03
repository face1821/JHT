using System;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Game2D.Tool;
using UnityEngine;

namespace Game.Stuff
{
    public class StairDisableColliderWhenDetect : MonoBehaviour
    {
        [SerializeField] private BoxColliderDetection2D _detection;

        private Collider2D _collider;

        private void Awake() { _collider = GetComponent<Collider2D>(); }

        private void OnEnable() { _detection.OnTouched += OnTouch; }

        private void OnDisable() { _detection.OnLeave += OnLeave; }

        private void OnTouch(Collider2D collision)
        {
            _collider.enabled = false;
            MLogger.LogError("关闭");
        }

        private void OnLeave(Collider2D collsision)
        {
            _collider.enabled = true;
            MLogger.LogError("激活");
        }
    }
}