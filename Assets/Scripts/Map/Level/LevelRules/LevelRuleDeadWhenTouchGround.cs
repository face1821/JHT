using System;
using Game.Player;
using Game.Stuff;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleDeadWhenTouchGround : LevelRuleBase
    {
        private void FixedUpdate()
        {
            if (!_runing) return;

            //如果玩家在该规则区是地面状态，就死亡
            if (_playerStateMachine.CurrentState is PlayerStateGround)
            {
                _playerStateMachine.Die();
            }
        }
    }
}