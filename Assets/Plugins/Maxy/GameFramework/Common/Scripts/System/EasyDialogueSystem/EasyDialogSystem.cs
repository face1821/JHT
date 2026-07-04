using System;
using System.Collections;
using Maxy.GameFramework.Common.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maxy.GameFramework.Common.System
{
    public class EasyDialogSystem : System<EasyDialogSystem>, IDialogSystem
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private OverlayFadeEffect _windowOverlay;
        [SerializeField] private TextMeshProUGUI _windowCharacterName;
        [SerializeField] private TextMeshProUGUI _windowContent;
        [Space]
        [SerializeField] private OverlayFadeEffect _characterOverlay;
        [SerializeField] private Image _characterImage;

        private DialogueStory _currentStory;
        private bool _isPlaying;
        private bool _isSkip;

        private void OnEnable() { }

        private void OnDisable() { }

        private void Update()
        {
            if (!_isPlaying) return;

            //运行对话框的时候，如果点击了，就跳过当前正在显示的对话
            if (Input.touchCount > 0)
            {
                OnGetClick();
            }
        }

        public void StartDialog(string dialogPathId)
        {
            _currentStory = Resources.Load<DialogueStory>($"Stories/{dialogPathId}");
            ShowDialog();

            StopAllCoroutines();
            StartCoroutine(nameof(PlayDialogue));
        }

        private void ShowDialog()
        {
            _canvas.SetActive(true);
            _windowOverlay.SetAlpha(0f);
            _windowOverlay.gameObject.SetActive(true);
        }

        private void HideDialog()
        {
            _canvas.SetActive(false);
            _windowOverlay.gameObject.SetActive(false);
        }

        private void OnGetClick()
        {
            //跳过一个当前的对话
            _isSkip = true;
        }

        private IEnumerator PlayDialogue()
        {
            //初始化
            int index = 0;
            _isSkip = false;

            //从头到尾一条条显示信息
            while (index < _currentStory.contentList.Count)
            {
                var currentInfo = _currentStory.contentList[index];

                //设置立绘
                if (!currentInfo.FollowTheLastCharacterSprite)
                    _characterImage.sprite = currentInfo.CharacterSprite;

                //设置窗口的文本
                _windowContent.maxVisibleCharacters = 0;
                _windowCharacterName.text = currentInfo.CharacterName;
                _windowContent.text = currentInfo.Content;

                //显示窗口
                _windowOverlay.PlayFadeOut(0.5f);
                _characterOverlay.PlayFadeOut(0.5f);
                yield return new WaitForSeconds(0.5f);

                //是否启用打字机效果
                if (currentInfo.EnableTypeWriterEffect)
                {
                    //通过调整最大显示字数来高性能实现
                    while (_windowContent.maxVisibleCharacters < currentInfo.Content.Length)
                    {
                        //逐个显示
                        _windowContent.maxVisibleCharacters++;
                        yield return new WaitForSeconds(1f / currentInfo.TypeCountPerSecond);

                        //如果要跳过
                        if (_isSkip)
                        {
                            _isSkip = false;
                            _windowContent.maxVisibleCharacters = currentInfo.Content.Length;
                            break;
                        }
                    }
                }
                else
                {
                    //持续显示时间
                    yield return new WaitForSeconds(currentInfo.Duration);
                }

                //是否自动隐藏窗口
                if (!_currentStory.AutoPlay)
                {
                    yield return new WaitUntil(() => _isSkip);

                    _isSkip = false;
                }

                //隐藏窗口
                _windowOverlay.PlayFadeIn(0.5f);
                _characterOverlay.PlayFadeIn(0.5f);
                yield return new WaitForSeconds(0.5f);

                //隐藏窗口后的延迟等待
                yield return new WaitForSeconds(currentInfo.DelayAfterThis);

                //进入下一句对话前
                index++;
            }
        }
    }
}