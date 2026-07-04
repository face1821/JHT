using System.Collections;
using Game.LoadingMenu;
using Game.Map;
using Game.Tool;
using Maxy.GameFramework.Common.Tool;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.PauseMenu
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField, LabelText("渐出（可为空）")] private OverlayFadeEffect _overlay;

        private IEnumerator DelayReturnToMainMenu()
        {
            _overlay?.gameObject.SetActive(true);
            _overlay?.PlayFadeOut();
            yield return new WaitForSeconds(1f);

            LoadingMenuManager.LoadingScene = "MainMenu";
            SceneManager.LoadScene("LoadingMenu");
        }

        public void Pause() { _menu.SetActive(true); }

        public void Resume() { _menu.SetActive(false); }

        public void Respawn()
        {
            _menu.SetActive(false);

            InstanceFinder.Player.StateMachine.Die();
        }

        public void Settings() { _settingsMenu.SetActive(true); }

        public void ReturnToMainMenu() { StartCoroutine(nameof(DelayReturnToMainMenu)); }
    }
}