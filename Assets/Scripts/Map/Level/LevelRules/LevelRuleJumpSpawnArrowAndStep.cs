using System;
using System.Collections.Generic;
using Game.Player;
using Game.Stuff;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleJumpSpawnArrowAndStep : LevelRuleBase
    {
        [SerializeField] private List<GameObject> _steps;
        [SerializeField] private Arrow _arrow;
        [SerializeField] private Transform _arrowSpawnPoint;

        private bool _alreadySpawned;
        private int _index;

        private void OnDisable()
        {
            //按钮关闭规则时，销毁箭矢
            var arrows = GameObject.FindObjectsByType<Arrow>(FindObjectsSortMode.None);
            foreach (var item in arrows)
            {
                Destroy(item.gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (!_runing) return;

            if (_playerStateMachine.CurrentState is PlayerStateFall) _alreadySpawned = false;
            if (_alreadySpawned) return;

            //当玩家跳跃时，召唤箭矢，以及台阶
            if (_playerStateMachine.CurrentState is PlayerStateJump)
            {
                MLogger.LogWarning("规则：玩家跳跃了，触犯了规则");

                //召唤箭矢
                _alreadySpawned = true;
                var obj = GameObject.Instantiate(_arrow);
                obj.transform.position = _arrowSpawnPoint.position;
                MTool.LookAt2D(obj.transform, _playerStateMachine.transform.position);

                //放置新台阶
                if (_index + 2 < _steps.Count)
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