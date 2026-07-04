using System.Collections;
using Maxy.GameFramework.Common.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maxy.GameFramework.Common.System
{
    public class EasyDialogueSystem : System<EasyDialogueSystem>, IDialogueSystem
    {
        public bool IsPlaying => _isPlaying;

        [SerializeField] private GameObject _canvas;
        [SerializeField] private Image _globalBackground;
        [SerializeField] private Image _background;
        [SerializeField] private OverlayFadeEffect _windowOverlay;
        [SerializeField] private TextMeshProUGUI _windowCharacterName;
        [SerializeField] private TextMeshProUGUI _windowContent;
        [Space]
        [SerializeField] private OverlayFadeEffect _characterOverlay;
        [SerializeField] private Image _characterImage;

        private DialogueStory _currentStory;
        private bool _isPlaying;
        private bool _isSkip;

        private void Update()
        {
            if (!_isPlaying) return;

            //运行对话框的时候，如果点击了，就跳过当前正在显示的对话
            if (Input.touchCount > 0 || Input.anyKeyDown)
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
            _isPlaying = true;
            _canvas.SetActive(true);
            _windowOverlay.SetAlpha(0f);
            _characterOverlay.SetAlpha(0f);
            _windowOverlay.gameObject.SetActive(true);
        }

        private void HideDialog()
        {
            _isPlaying = false;
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

            //设置全局背景
            _globalBackground.sprite = _currentStory.GlobalBackGround;
            if (_currentStory.GlobalBackGround != null)
            {
                _globalBackground.color = Color.white;
                _globalBackground.sprite = _currentStory.GlobalBackGround;
            }
            else
            {
                _globalBackground.color = new Color(0, 0, 0, 50);
            }

            //从头到尾一条条显示信息
            while (index < _currentStory.contentList.Count)
            {
                var currentInfo = _currentStory.contentList[index];

                //设置立绘位置
                if (currentInfo.enablePresetPosition)
                {
                    var position = Vector2.zero;

                    switch (currentInfo.PresetPosition)
                    {
                        case DialogCharaterPresetPosition.Left:
                            position = new Vector2(-500, -100);
                            break;
                        case DialogCharaterPresetPosition.Middle:
                            position = new Vector2(0, 100);
                            break;
                        case DialogCharaterPresetPosition.Right:
                            position = new Vector2(500, 100);
                            break;
                    }

                    (_characterImage.transform as RectTransform)!.anchoredPosition = position;
                }
                else
                {
                    (_characterImage.transform as RectTransform)!.anchoredPosition = currentInfo.ScreenPosition;
                }

                //设置立绘图像
                //如果不保持上一个立绘图像，就更换图像
                if (!currentInfo.FollowLastCharacterSprite)
                {
                    //但更换时，需要更换的结果不是一个空图像
                    if (currentInfo.CharacterSprite != null)
                    {
                        _characterImage.color = Color.white;
                        _characterImage.sprite = currentInfo.CharacterSprite;
                        _characterImage.SetNativeSize();
                    }
                    else //如果是空图像，就设置空白颜色
                    {
                        _characterImage.color = new Color(0, 0, 0, 0);
                    }
                }

                //设置窗口的文本
                _isSkip = false;
                _windowContent.maxVisibleCharacters = 0;
                _windowCharacterName.text = currentInfo.CharacterName;
                _windowContent.text = currentInfo.Content;

                //设置背景
                if (!currentInfo.FollowLastBackGround)
                {
                    if (currentInfo.BackGround != null)
                    {
                        _background.color = Color.white;
                        _background.sprite = currentInfo.BackGround;
                    }
                    else
                    {
                        _background.color = new Color(0, 0, 0, 0);
                    }
                }

                //显示窗口
                _windowOverlay.PlayFadeOut(0.5f);
                //如果当前图像是保持上一个图像或上一个立绘图像和当前图像一样，就不显示立绘图像
                //下面这段代码是反向的逻辑
                if (_characterImage != null
                    && (index == 0
                        || !currentInfo.FollowLastCharacterSprite
                        || _currentStory.contentList[index - 1].CharacterSprite != currentInfo.CharacterSprite))
                {
                    if (_currentStory.EnableCharacterSpriteFadeEffect)
                        _characterOverlay.PlayFadeOut(0.5f);
                }

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

                //如果下一个立绘图像是保持当前的图像或一样，就不隐藏立绘图像
                //下面这段代码是反向的逻辑
                if (index + 1 >= _currentStory.contentList.Count
                    || (!_currentStory.contentList[index + 1].FollowLastCharacterSprite
                        && _currentStory.contentList[index + 1].CharacterSprite != currentInfo.CharacterSprite))
                {
                    if (_currentStory.EnableCharacterSpriteFadeEffect)
                        _characterOverlay.PlayFadeIn(0.5f);
                }

                yield return new WaitForSeconds(0.5f);

                //隐藏窗口后的延迟等待
                yield return new WaitForSeconds(currentInfo.DelayAfterThis);

                //进入下一句对话前
                index++;
            }

            //结束对话
            HideDialog();
        }
    }
}