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

        private void FixedUpdate()
        {
            if (!_runing) return;

            //当玩家转向时，召唤箭矢
            if (_playerStateMachine.Paramaters.FaceDirection == -_lastFaceDirection)
            {
                MLogger.LogWarning("规则：玩家转向了，触犯了规则");

                var obj = GameObject.Instantiate(_arrow);
                obj.Platform = _platform;
                obj.transform.position = _arrowSpawnPoint.position;
                MTool.LookAt2D(obj.transform, _playerStateMachine.transform.position);
            }

            _lastFaceDirection = _playerStateMachine.Paramaters.FaceDirection;
        }
    }
}