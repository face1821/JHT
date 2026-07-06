using System;
using System.Collections.Generic;
using Game.Stuff;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Map
{
    public class LevelRuleTurnBackSpawnArrow : LevelRuleBase
    {
        [SerializeField] private FloatingPlatform _platform;
        [SerializeField] private Arrow _arrow;
        [SerializeField] private List<AudioClip> _arrowClipList;
        [SerializeField] private Transform _arrowSpawnPoint;

        private int _lastFaceDirection;

        public override void OnEnter()
        {
            base.OnEnter();
            
            _lastFaceDirection = _playerStateMachine.Paramaters.FaceDirection;
        }

        private void FixedUpdate()
        {
            if (!_runing) return;

            //当玩家转向时，召唤箭矢
            if (_playerStateMachine.Paramaters.FaceDirection == -_lastFaceDirection)
            {
                MLogger.LogWarning("规则：玩家转向了，触犯了规则");

                var obj = GameObject.Instantiate(_arrow);
                obj.Platform = _platform;
                obj.transform.position = _arrowSpawnPoint.position;
                MTool.LookAt2D(obj.transform, _playerStateMachine.transform.position);
                SystemCenter.Get<IAudioSystem>().PlaySfx(_arrowClipList[Random.Range(0, _arrowClipList.Count)], "arrow_clip", InstanceFinder.Player.transform, 0f);
            }

            _lastFaceDirection = _playerStateMachine.Paramaters.FaceDirection;
        }
    }
}