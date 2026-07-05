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

        private void OnEnable()
        {
            //当玩家的存档点是第三个，也就是尖刺后面时，就将尖刺放置在末尾位置
            if (SaveSystem.Load("LastPassedLevel", 0) - 1 > 1)
            {
                _spike.TpToEnd();
                return;
            }

            PlayerStateMachine.OnDead += OnPlayerDead;
        }

        private void OnDisable() { PlayerStateMachine.OnDead -= OnPlayerDead; }

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

            //当玩家的存档点是第三个，也就是尖刺后面时，就将尖刺放置在末尾位置
            if (SaveSystem.Load("LastPassedLevel", 0) - 1 > 1)
            {
                _spike.TpToEnd();
                yield break;
            }

            _started = false;
        }
    }
}