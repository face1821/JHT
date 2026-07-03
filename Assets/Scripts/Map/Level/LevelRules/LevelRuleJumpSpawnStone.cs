using Game.Player;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleJumpSpawnStone : LevelRuleBase
    {
        [SerializeField] private GameObject _stone;

        private bool _alreadySpawnThisJump;

        private void FixedUpdate()
        {
            //如果玩家跳跃状态过去了，就重置
            if (_playerStateMachine.CurrentState is PlayerStateFall)
            {
                _alreadySpawnThisJump = false;
            }

            //如果玩家这次跳跃召唤过了，就不管
            if (_alreadySpawnThisJump) return;

            //当玩家跳跃时，激活石头
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                _alreadySpawnThisJump = true;
                MLogger.LogWarning("规则1：玩家跳跃了，触犯了规则");
                _stone.SetActive(true);
            }
        }
    }
}