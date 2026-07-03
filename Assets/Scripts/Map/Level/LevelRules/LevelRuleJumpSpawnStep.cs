using System;
using System.Collections.Generic;
using Game.Player;
using Game.Stuff;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleJumpSpawnStep : LevelRuleBase
    {
        [SerializeField] private List<GameObject> _steps;

        private bool _alreadySpawned;
        private int _index;

        private void OnDisable() { _alreadySpawned = false; }

        private void FixedUpdate()
        {
            if (!_runing) return;

            if (_playerStateMachine.CurrentState is PlayerStateFall) _alreadySpawned = false;
            if (_alreadySpawned) return;

            //当玩家跳跃时，召唤箭矢，以及台阶
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                MLogger.LogWarning("规则：玩家跳跃了，触犯了规则");

                //放置新台阶
                _index = Mathf.Clamp(_index + 2, 0, _steps.Count);
                if (_index < _steps.Count)
                {
                    //一次性加俩台阶，快一点
                    _steps.ForEach(x => x.SetActive(false));
                    _index += 2;
                    _steps[_index].SetActive(true);
                }
            }
        }
    }
}