using System.Collections.Generic;
using Game.Player;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleJumpDeadAndCrouchSpawnStep : LevelRuleBase
    {
        [SerializeField] private List<GameObject> _steps;

        private bool _alreadySpawned;
        private int _index;

        private void FixedUpdate()
        {
            if (!_runing) return;

            //当玩家处于跳跃状态时，死亡
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                MLogger.LogWarning("规则：玩家跳跃了，触犯了规则");
                _playerStateMachine.Die();
            }

            if (_playerStateMachine.CurrentState is PlayerStateIdle) _alreadySpawned = false;
            if (_alreadySpawned) return;

            //当玩家下蹲时，召唤台阶
            if (_playerStateMachine.CurrentState is PlayerStateCrouch)
            {
                MLogger.LogWarning("规则：玩家跳跃了，触犯了规则");
                _alreadySpawned = true;

                //放置新台阶
                _index = Mathf.Clamp(_index + 2, 0, _steps.Count - 1);
                if (_index < _steps.Count)
                {
                    //一次性加俩台阶，快一点
                    _steps.ForEach(x => x.SetActive(false));
                    _steps[_index].SetActive(true);
                }
            }
        }

        private void OnDisable() { _alreadySpawned = false; }
    }
}