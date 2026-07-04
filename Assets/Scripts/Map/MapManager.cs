using System;
using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

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
        [Space]
        [SerializeField] private GameObject _storyCanvas;
        [SerializeField] private VideoPlayer _openingStoryVideoPlayer;
        [SerializeField] private AudioClip _uiEmptyClick;

        private IAudioSystem _audioSystem;

        private void OnEnable() { PlayerStateMachine.OnDead += OnPlayerDead; }

        private void OnDisable() { PlayerStateMachine.OnDead -= OnPlayerDead; }

        private void Awake()
        {
            _audioSystem = SystemCenter.Get<IAudioSystem>();

            //渐入场景
            _overlay.PlayFadeIn();

            if (IsNewGame)
            {
                IsNewGame = false;

                //但这表示这是新游戏，所以开启进入新游戏的剧情
                StartCoroutine(nameof(ShowOpeningStory));

                return;
            }

            //关闭已经通过的关卡的规则
            //不需要了
            //ClosePassedLevels();
            //初始化
            _levelInfos.ForEach(x => x.Init(this));

            //将玩家传送到上一次刚通关的关卡的通关位置
            var lastPassedLevelIndex = ES3.Load("LastPassedLevel", -1) - 1;

            //如果没有存档点位置，就不管了
            if (lastPassedLevelIndex < 0) return;

            //传送到存档点位置
            MLogger.LogWarning($"系统：玩家有记录，传送到第{lastPassedLevelIndex + 1}个存档点");
            InstanceFinder.Player.transform.position = _levelInfos[lastPassedLevelIndex].SpawnPos;
        }

        private void Update()
        {
            if (IsAnyFingerJustDownToUI())
            {
                var ui = EventSystem.current.currentSelectedGameObject;

                //点非按钮UI时发出空击音效
                if (ui == null || !ui.CompareTag("UIButton"))
                    _audioSystem.PlaySfx(_uiEmptyClick, "_uiEmptyClick", null, 0f);
            }
        }

        public void ClosePassedLevels()
        {
            //遍历每个关卡的记录
            for (int i = 0; i < _levelInfos.Count; i++)
            {
                _levelInfos[i].Init(this);
                var passed = ES3.Load($"Level-{i + 1}", false);
                if (passed)
                {
                    _levelInfos[i].InactiveLevel();
                }
            }
        }

        public bool IsAnyFingerJustDownToUI()
        {
            // 是否有任意手指刚刚按到UI对象
            foreach (var t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                    return EventSystem.current.IsPointerOverGameObject();
            }

            return false;
        }

        private void OnPlayerDead()
        {
            //延迟复活
            StartCoroutine(nameof(DelayRespawn));
        }

        private IEnumerator DelayRespawn()
        {
            yield return new WaitForSeconds(1f);

            _overlay.PlayFadeOutAndIn();
            yield return new WaitForSeconds(1.5f);

            //重置每一个关卡
            foreach (var item in _levelInfos)
            {
                item.ResetLevel();
            }

            //将玩家传送到上一次刚通关的关卡的通关位置
            var lastPassedLevelIndex = ES3.Load("LastPassedLevel", -1) - 1;

            InstanceFinder.Player.StateMachine.Respawn();

            //如果没有存档点位置，就回到起始点
            if (lastPassedLevelIndex < 0)
            {
                InstanceFinder.Player.transform.position = new Vector3(-2.09f, -2.5f, 0f);
                yield break;
            }

            //传送到存档点位置
            MLogger.LogWarning($"系统：玩家重生到 第{lastPassedLevelIndex + 1}个复活点");
            InstanceFinder.Player.transform.position = _levelInfos[lastPassedLevelIndex].SpawnPos;
        }

        private IEnumerator ShowOpeningStory()
        {
            //禁用玩家输入
            PlayerInput.Instance.enabled = false;

            //显示开场剧情CG
            _storyCanvas.SetActive(true);
            _openingStoryVideoPlayer.gameObject.SetActive(true);
            yield return new WaitUntil(() => !_openingStoryVideoPlayer.isPlaying);

            //然后显示开场剧情对话
            Destroy(_openingStoryVideoPlayer.gameObject);

            var dialogue = SystemCenter.Get<IDialogueSystem>();
            dialogue.StartDialog("OpeningStory");
            yield return new WaitUntil(() => dialogue.IsPlaying);

            //启用玩家输入
            _storyCanvas.SetActive(false);
            PlayerInput.Instance.enabled = true;
        }
    }
}