using Game.Player;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Map
{
    public class LevelRule2 : LevelRuleBase
    {
        [SerializeField] private GameObject _stone;
        
        private void Update()
        {
            //当玩家跳跃时，激活石头
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                MLogger.LogWarning("规则1：玩家跳跃了，触犯了规则");
                _stone.SetActive(true);
            }
        }
    }
}