using System.Collections.Generic;
using Game.Tool;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class MapManager : MonoBehaviour
    {
        public static bool IsNewGame;

        public List<Level> Levels => _levels;

        [SerializeField] private OverlayFadeEffect _overlay;
        [SerializeField] List<Level> _levels;

        private void Awake()
        {
            //渐入场景
            _overlay.PlayFadeIn();

            if (IsNewGame)
            {
                IsNewGame = false;

                return;
            }

            //先遍历每个关卡的记录
            foreach (var level in _levels)
            {
                level.Init(this);

                var passed = ES3.Load($"Level-{_levels.IndexOf(level) + 1}", false);
                if (passed)
                {
                    level.InactiveLevel();
                }
            }

            //将玩家传送到上一次刚通关的关卡的通关位置
            var lastPassedLevelIndex = ES3.Load("LastPassedLevel", 0);

            //关卡都是正数计数，玩家上次通关的关卡再往外侧拓展一下，就是玩家现在应该所处的通关位置
            lastPassedLevelIndex += 1;

            var levelInfo = _levels.Find(x => _levels.IndexOf(x) == lastPassedLevelIndex);
            InstanceFinder.Player.transform.position = levelInfo.SpawnPos;
        }
    }
}