using System.Collections.Generic;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class MapManager : MonoBehaviour
    {
        public static bool IsNewGame;

        public List<LevelInfo> LevelInfos => _levelInfos;

        [SerializeField] private OverlayFadeEffect _overlay;
        [SerializeField] List<LevelInfo> _levelInfos;

        private void Start()
        {
            //渐入场景
            _overlay.PlayFadeIn();

            if (IsNewGame)
            {
                IsNewGame = false;

                return;
            }

            //先遍历每个关卡的记录
            foreach (var level in _levelInfos)
            {
                level.Init(this);

                var passed = ES3.Load($"Level-{_levelInfos.IndexOf(level) + 1}", false);
                if (passed)
                {
                    level.InactiveLevel();
                }
            }

            //将玩家传送到上一次刚通关的关卡的通关位置
            var lastPassedLevelIndex = ES3.Load("LastPassedLevel", -1);

            //如果没有存档点位置，就不管了
            if (lastPassedLevelIndex == -1) return;

            //传送到存档点位置
            InstanceFinder.Player.transform.position = _levelInfos[lastPassedLevelIndex].SpawnPos;
        }
    }
}