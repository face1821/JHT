using Game.Player;
using Maxy.GameFramework.Common.System;

namespace Game.Map
{
    public class LevelRule1 : LevelRuleBase
    {
        private void Update()
        {
            //当玩家处于跳跃状态时，死亡
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                MLogger.LogWarning("规则1：玩家跳跃了，触犯了规则");
                _playerStateMachine.RequestToChangeState(_playerStateMachine.StateDead);
            }
        }
    }
}