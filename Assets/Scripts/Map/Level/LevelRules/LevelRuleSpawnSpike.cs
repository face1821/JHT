using System;
using System.Collections;
using Game.Player;
using Game.Stuff;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleSpawnSpike : LevelRuleBase
    {
        [SerializeField] private Spike _spike;

        private bool _started;

        private void OnEnable() { PlayerStateMachine.OnDead += OnPlayerDead; }

        private void OnDisable()
        {
            //当规则关闭时，尖刺就放置在末尾，不让玩家回去了
            _spike.TpToEnd();

            PlayerStateMachine.OnDead -= OnPlayerDead;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (_started) return;

            //当玩家进入该规则区时，立刻召唤尖刺
            _spike.StartMove();
            _started = true;
        }

        private void OnPlayerDead()
        {
            //当玩家死亡时，尖刺就延迟关闭，等待下次启动
            StartCoroutine(nameof(DelayOnPlayerDead));
        }

        private IEnumerator DelayOnPlayerDead()
        {
            yield return new WaitForSeconds(1f);

            _started = false;
            _spike.Close();
        }
    }
}