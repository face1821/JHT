using Game.Player;
using Maxy.GameFramework.Common.System;

namespace Game.Map
{
    public class LevelRuleJumpDead : LevelRuleBase
    {
        private void FixedUpdate()
        {
            if (!_runing) return;

            //当玩家处于跳跃状态时，死亡
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                MLogger.LogWarning("规则1：玩家跳跃了，触犯了规则");
                _playerStateMachine.RequestChangeState(_playerStateMachine.StateDead);
            }
        }
    }
}