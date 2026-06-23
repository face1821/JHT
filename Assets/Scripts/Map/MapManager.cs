using System.Collections.Generic;
using Game.Tool;
using UnityEngine;

namespace Game.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private List<Level> _levels;

        private void Awake()
        {
            //先遍历每个关卡的记录
            foreach (var level in _levels)
            {
                var passed = ES3.Load($"Level-{level.LevelIndex}", false);
                if (passed)
                {
                    level.InactiveLevel();
                }
            }

            //将玩家传送到上一次刚通关的关卡的通关位置
            var lastPassedLevelIndex = ES3.Load("LastPassedLevel", 0);
            
            //左侧的关卡都是负数计数，右侧的关卡都是正数计数，玩家上次通关的关卡再往外侧拓展一下，就是玩家现在应该所处的通关位置
            lastPassedLevelIndex += lastPassedLevelIndex > 0 ? 1 : (lastPassedLevelIndex < 0 ? -1 : 0);
            
            var levelInfo = _levels.Find(x => x.LevelIndex == lastPassedLevelIndex);
            InstanceFinder.Player.transform.position = levelInfo.SpawnPos;
        }
    }
}