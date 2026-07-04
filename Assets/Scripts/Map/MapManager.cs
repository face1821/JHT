using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
        [SerializeField] private AudioClip _music;
        [SerializeField] private AudioClip _uiEmptyClick;

        private IAudioSystem _audioSystem;

        private void OnEnable() { PlayerStateMachine.OnDead += OnPlayerDead; }

        private void OnDisable() { PlayerStateMachine.OnDead -= OnPlayerDead; }

        private void Start()
        {
            _audioSystem = SystemCenter.Get<IAudioSystem>();

            //渐入场景
            _overlay.PlayFadeIn();
            
            //播放音乐
            _audioSystem.PlayMusic(_music);

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
            var lastPassedLevelIndex = SaveSystem.Load("LastPassedLevel", -1) - 1;

            //如果没有存档点位置，就不管了
            if (lastPassedLevelIndex < 0) return;

            //传送到存档点位置
            MLogger.LogWarning($"系统：玩家有记录，传送到第{lastPassedLevelIndex + 1}个存档点");
            InstanceFinder.Player.transform.position = _levelInfos[lastPassedLevelIndex].SpawnPos;
        }

        private void Update()
        {
            //如果按到了UI对象
            var ui = GetClickUIObj();
            if (ui != null)
            {
                //点非按钮UI时发出空击音效
                if (!ui.CompareTag("UIButton"))
                    _audioSystem.PlaySfx(_uiEmptyClick, "_uiEmptyClick", null, 0f);
            }
        }

        public void ClosePassedLevels()
        {
            //遍历每个关卡的记录
            for (int i = 0; i < _levelInfos.Count; i++)
            {
                _levelInfos[i].Init(this);
                var passed = SaveSystem.Load($"Level-{i + 1}", false);
                if (passed)
                {
                    _levelInfos[i].InactiveLevel();
                }
            }
        }

        // 获取触屏点击到的UI物体
        private GameObject GetClickUIObj()
        {
            if (Input.touchCount == 0) return null;

            Touch t = Input.GetTouch(0);
            if (t.phase != TouchPhase.Began) return null;

            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = t.position;
            List<RaycastResult> resList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, resList);

            if (resList.Count == 0) return null;

            // 取视觉最顶层的（resList[0] 就是）
            GameObject topmost = resList[0].gameObject;

            return topmost;
        }

        #region 设置

        public void ToggleSfx(Toggle toggle)
        {
            var value = toggle.isOn;
            SaveSystem.Save("SfxToggle", value);

            _audioSystem.SetSfxVolume(value ? 1f : 0f);
            MLogger.Log($"音频系统：音效（{(value ? "开启" : "关闭")}）");
        }

        public void ToggleMusic(Toggle toggle)
        {
            var value = toggle.isOn;
            SaveSystem.Save("MusicToggle", value);

            _audioSystem.SetMusicVolume(value ? 1f : 0f);
            MLogger.Log($"音频系统：音乐（{(value ? "开启" : "关闭")}）");
        }

        #endregion

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
            var lastPassedLevelIndex = SaveSystem.Load("LastPassedLevel", -1) - 1;

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
            yield return new WaitUntil(() => !_openingStoryVideoPlayer.isPlaying);
            
            //然后显示开场剧情对话
            Destroy(_openingStoryVideoPlayer.gameObject);

            var dialogue = SystemCenter.Get<IDialogueSystem>();
            dialogue.StartDialog("OpeningStory");
            _storyCanvas.SetActive(false);
            
            yield return new WaitUntil(() => !dialogue.IsPlaying);

            //启用玩家输入
            PlayerInput.Instance.enabled = true;
        }
    }
}