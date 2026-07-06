using System;
using System.Collections;
using System.Linq;
using System.Text;
using Game.LoadingMenu;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.PauseMenu
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField, LabelText("渐出（可为空）")] private OverlayFadeEffect _overlay;
        [SerializeField] private TextMeshProUGUI _deadCountText;

        private IEnumerator DelayReturnToMainMenu()
        {
            _overlay?.gameObject.SetActive(true);
            _overlay?.PlayFadeOut();
            yield return new WaitForSeconds(1f);

            LoadingMenuManager.LoadingScene = "MainMenu";
            SceneManager.LoadScene("LoadingMenu");
        }

        private void UpdateDeadCount()
        {
            var deadCount = SaveSystem.Load("DeadCount", 0);

            if (deadCount == 0)
            {
                _deadCountText.text = string.Empty;

                return;
            }

            var fiveCount = deadCount / 5;
            var lastCharCount = deadCount % 5;

            var result = new StringBuilder();
            while (fiveCount > 0)
            {
                result.Append("<sprite name=c5>");
                fiveCount--;
            }

            if (lastCharCount != 0)
            {
                result.Append($"<sprite name=c{lastCharCount}>");
            }

            //更新计算结果
            _deadCountText.text = result.ToString();
        }

        public void Pause()
        {
            _menu.SetActive(true);
            Time.timeScale = 0f;

            //每次打开暂停界面时，刷新一下死亡计数捏
            UpdateDeadCount();
        }

        public void Resume()
        {
            _menu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void Respawn()
        {
            _menu.SetActive(false);
            Time.timeScale = 1f;
            
            InstanceFinder.Player.StateMachine.Die();
        }

        public void Settings() { _settingsMenu.SetActive(true); }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            StartCoroutine(nameof(DelayReturnToMainMenu));
        }
    }
}