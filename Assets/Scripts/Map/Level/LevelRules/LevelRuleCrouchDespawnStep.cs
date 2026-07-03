using System;
using System.Collections.Generic;
using Game.Player;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleCrouchDespawnStep : LevelRuleBase
    {
        [SerializeField] private List<GameObject> _steps;

        private bool _alreadySpawned;
        private int _index;

        private void Awake() { _index = _steps.Count - 1; }

        private void FixedUpdate()
        {
            if (!_runing) return;

            if (_playerStateMachine.CurrentState is PlayerStateIdle) _alreadySpawned = false;
            if (_alreadySpawned) return;

            //当玩家下蹲时，销毁台阶
            if (_playerStateMachine.CurrentState is PlayerStateCrouch)
            {
                MLogger.LogWarning("规则：玩家下蹲了，触犯了规则");
                _alreadySpawned = true;

                //销毁新台阶
                //一次性加俩台阶，快一点
                _index = Mathf.Clamp(_index - 2, -1, _steps.Count - 1);
                _steps.ForEach(x => x.SetActive(false));

                if (_index >= 0)
                {
                    _steps[_index].SetActive(true);
                }
            }
        }

        private void OnDisable() { _alreadySpawned = false; }
    }
}