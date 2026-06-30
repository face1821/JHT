using System;
using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Map
{
    public class LevelRule1 : LevelRuleBase
    {
        private PlayerStateMachine _playerStateMachine;

        private void Awake() { _playerStateMachine = InstanceFinder.Player.StateMachine; }

        private void Update()
        {
            //当玩家处于跳跃状态时，死亡
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                _playerStateMachine.RequestToChangeState(_playerStateMachine.StateDead);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            MLogger.Log("规则1：玩家进入规则区域");
            enabled = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            MLogger.Log("规则1：玩家离开规则区域");
            enabled = false;
        }
    }
}