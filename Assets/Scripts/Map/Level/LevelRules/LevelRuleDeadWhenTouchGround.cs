using Game.Player;

namespace Game.Map
{
    public class LevelRuleDeadWhenTouchGround : LevelRuleBase
    {
        private void FixedUpdate()
        {
            if (!_runing) return;

            //如果玩家在该规则区是地面状态，就死亡（这里的X轴设置是防止玩家死后重生的第一帧依然被判定，因为物理引擎的脱离检测是可能慢一帧的）
            if (_playerStateMachine.transform.position.x > 142 && _playerStateMachine.CurrentState is PlayerStateGround)
            {
                _playerStateMachine.Die();
            }
        }
    }
}