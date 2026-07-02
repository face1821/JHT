using System;
using System.Collections;
using System.Collections.Generic;
using Game.LoadingMenu;
using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Map
{
    public class MapManager : MonoBehaviour
    {
        public static bool IsNewGame;

        public List<LevelInfo> LevelInfos => _levelInfos;

        [Header("LevelInfo自身就是复活点，而且它还可以关闭自身的规则们")]
        [Header("而存档点是为了记录到达哪里了，然后根据LevelInfo位置来复活")]
        [SerializeField] private OverlayFadeEffect _overlay;
        [SerializeField] List<LevelInfo> _levelInfos;

        private void OnEnable() { PlayerStateMachine.OnDead += OnPlayerDead; }

        private void OnDisable() { PlayerStateMachine.OnDead -= OnPlayerDead; }

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
            for (int i = 0; i < _levelInfos.Count; i++)
            {
                _levelInfos[i].Init(this);
                var passed = ES3.Load($"Level-{i + 1}", false);
                if (passed)
                {
                    _levelInfos[i].InactiveLevel();
                }
            }

            //将玩家传送到上一次刚通关的关卡的通关位置
            var lastPassedLevelIndex = ES3.Load("LastPassedLevel", -1);

            //如果没有存档点位置，就不管了
            if (lastPassedLevelIndex == -1) return;

            //传送到存档点位置
            InstanceFinder.Player.transform.position = _levelInfos[lastPassedLevelIndex].SpawnPos;
        }

        private void OnPlayerDead() { StartCoroutine(nameof(DelayRespawn)); }

        private IEnumerator DelayRespawn()
        {
            yield return new WaitForSeconds(1f);

            _overlay.PlayFadeOutAndIn();
            yield return new WaitForSeconds(1.5f);

            //将玩家传送到上一次刚通关的关卡的通关位置
            var lastPassedLevelIndex = ES3.Load("LastPassedLevel", -1);

            InstanceFinder.Player.StateMachine.Respawn();
            
            //如果没有存档点位置，就回到起始点
            if (lastPassedLevelIndex == -1)
            {
                InstanceFinder.Player.transform.position = new Vector3(-5f, -2.5f, 0f);
                yield break;
            }

            //传送到存档点位置
            InstanceFinder.Player.transform.position = _levelInfos[lastPassedLevelIndex].SpawnPos;
        }
    }
}