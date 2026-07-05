using System.Collections.Generic;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.Map
{
    public class LevelInfo : MonoBehaviour
    {
        public Vector2 SpawnPos => transform.position;
        public int LevelIndex => GameObject.FindWithTag("MapManager").GetComponent<MapManager>().LevelInfos.IndexOf(this);

        [SerializeField] private bool _canBeReset;
        [SerializeField] private bool _canRuleBeClosed;
        [SerializeField] private List<LevelRuleBase> _levelRules;
        [SerializeField] private List<GameObject> _activeObjs;
        [SerializeField] private List<GameObject> _inactiveObjs;
        [SerializeField] private List<GameObject> _ruleLights;

        public void ResetLevel()
        {
            if (!_canBeReset) return;

            //玩家死亡后，重置规则
            foreach (var item in _levelRules)
            {
                item.ResetRule();
            }

            //重置关卡
            foreach (var item in _activeObjs)
            {
                item.SetActive(true);
            }

            foreach (var item in _inactiveObjs)
            {
                item.SetActive(false);
            }
        }

        public void ActiveLevel()
        {
            foreach (var item in _levelRules)
            {
                item.enabled = true;
            }

            foreach (var item in _ruleLights)
            {
                item.SetActive(true);
            }
        }

        public void InactiveLevel()
        {
            if (!_canRuleBeClosed) return;

            foreach (var item in _levelRules)
            {
                item.enabled = false;
            }

            foreach (var item in _ruleLights)
            {
                item.SetActive(false);
            }
        }
    }
}