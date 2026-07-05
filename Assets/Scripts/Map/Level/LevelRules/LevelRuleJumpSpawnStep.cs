using System;
using System.Collections.Generic;
using Game.Player;
using Game.Stuff;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleJumpSpawnStep : LevelRuleBase
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private List<GameObject> _steps;

        private bool _alreadySpawned;
        private int _index;

        private void OnDisable() { _alreadySpawned = false; }

        private void FixedUpdate()
        {
            if (!_runing) return;

            if (_playerStateMachine.CurrentState is PlayerStateFall) _alreadySpawned = false;
            if (_alreadySpawned) return;

            //当玩家跳跃时，召唤台阶
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                MLogger.LogWarning("规则：玩家跳跃了，触犯了规则");
                _alreadySpawned = true;

                //放置新台阶
                var oldIndex = _index;
                _index = Mathf.Clamp(_index + 2, 0, _steps.Count - 1);
                if (_index < _steps.Count && _index != oldIndex)
                {
                    //一次性加俩台阶，快一点
                    _steps.ForEach(x => x.SetActive(false));
                    SystemCenter.Get<IAudioSystem>().PlaySfx(_clip, "stair_clip", InstanceFinder.Player.transform, 0f);
                    _steps[_index].SetActive(true);
                }
            }
        }
    }
}