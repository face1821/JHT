using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleTurnBackSpawnArrow : LevelRuleBase
    {
        [SerializeField] private GameObject _arrow;
        [SerializeField] private Transform _arrowSpawnPoint;

        private int _lastMoveDirection;
        
        private void FixedUpdate()
        {
            //当玩家转向时，召唤箭矢
            if (PlayerInput.MoveDirection == -_lastMoveDirection)
            {
                MLogger.LogWarning("规则1：玩家转向了，触犯了规则");
                
                var obj = GameObject.Instantiate(_arrow);
                obj.transform.position = _arrowSpawnPoint.position;
                MTool.LookAt2D(obj.transform, InstanceFinder.Player.transform.position);
            }

            _lastMoveDirection = PlayerInput.MoveDirection;
        }
    }
}