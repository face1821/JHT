using System;
using Game.Stuff;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleTurnBackSpawnArrow : LevelRuleBase
    {
        [SerializeField] private FloatingPlatform _platform;
        [SerializeField] private Arrow _arrow;
        [SerializeField] private Transform _arrowSpawnPoint;

        private int _lastFaceDirection;

        private void OnDisable()
        {
            //当规则关闭时，顺带把箭矢都销毁掉
            var arrows = GameObject.FindObjectsByType(typeof(Arrow), FindObjectsSortMode.None);
            foreach (var item in arrows)
            {
                Destroy((item as Arrow)!.gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (!_runing) return;

            //当玩家转向时，召唤箭矢
            if (_playerStateMachine.Paramaters.FaceDirection == -_lastFaceDirection)
            {
                MLogger.LogWarning("规则1：玩家转向了，触犯了规则");

                var obj = GameObject.Instantiate(_arrow);
                obj.Platform = _platform;
                obj.transform.position = _arrowSpawnPoint.position;
                MTool.LookAt2D(obj.transform, _playerStateMachine.transform.position);
            }

            _lastFaceDirection = _playerStateMachine.Paramaters.FaceDirection;
        }

        public override void ResetRule()
        {
            base.ResetRule();

            //当规则重置时，顺带把箭矢都销毁掉
            var arrows = GameObject.FindObjectsByType(typeof(Arrow), FindObjectsSortMode.None);
            foreach (var item in arrows)
            {
                Destroy((item as Arrow)!.gameObject);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _lastFaceDirection = 1;
        }
    }
}