using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    public class LevelInfo : MonoBehaviour
    {
        public Vector2 SpawnPos => transform.position;
        public int LevelIndex => _levelIndex;

        [SerializeField] private bool _canRuleBeClosed;
        [SerializeField] private List<GameObject> _activeObjs;
        [SerializeField] private List<GameObject> _inactiveObjs;

        private MapManager _mapManager;
        private int _levelIndex;
        private LevelRuleBase _levelRule;

        private void Awake() { _levelRule = GetComponent<LevelRuleBase>(); }

        public void Init(MapManager mapManager)
        {
            _mapManager = mapManager;
            _levelIndex = mapManager.LevelInfos.IndexOf(this);
        }

        public void ResetLevel()
        {
            //玩家死亡后，重置关卡
            foreach (var item in _activeObjs)
            {
                item.SetActive(true);
            }

            foreach (var item in _inactiveObjs)
            {
                item.SetActive(false);
            }
        }

        public void PassLevel()
        {
            //通过关卡后，关卡不应当再响应任何事件
            //所以将关卡的所有规则全部关闭
            InactiveLevel();

            //并且存储下来
            ES3.Save($"Level-{_levelIndex}", true);
            ES3.Save($"LastPassedLevel", _levelIndex);
        }

        public void ActiveLevel() { _levelRule.enabled = true; }

        public void InactiveLevel()
        {
            if (!_canRuleBeClosed) return;

            _levelRule.enabled = false;
        }
    }
}