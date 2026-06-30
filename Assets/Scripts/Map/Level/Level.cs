using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    public class Level : MonoBehaviour
    {
        public Vector2 SpawnPos => _spawnPos;

        [SerializeField] private Vector2 _spawnPos;
        [SerializeField] private List<LevelRuleBase> _levelRules;
        [SerializeField] private List<GameObject> _activeObjs;
        [SerializeField] private List<GameObject> _inactiveObjs;

        private MapManager _mapManager;
        private int _levelIndex;

        public void Init(MapManager mapManager)
        {
            _mapManager = mapManager;
            _levelIndex = mapManager.Levels.IndexOf(this);
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

        public void InactiveLevel()
        {
            //每个规则是一个独立的游戏对象
            foreach (var item in _levelRules)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}