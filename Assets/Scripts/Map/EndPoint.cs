using System;
using System.Collections;
using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.LoadingMenu;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Game.Map
{
    [RequireComponent(typeof(Light2D), typeof(BoxCollider2D))]
    public class EndPoint : MonoBehaviour, IInteractableObject
    {
        public bool IsActive => gameObject.activeSelf;

        [SerializeField] private OverlayFadeEffect _overlay;
        private Light2D _light;

        private void Awake() { _light = GetComponent<Light2D>(); }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            EventBus.Publish(new AddPlayerInteractableObjectEvent(this));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            EventBus.Publish(new RemovePlayerInteractableObjectEvent(this));
        }

        public void SetHighLight(bool state) { _light.enabled = state; }

        public float GetDistance() => Vector3.Distance(transform.position, InstanceFinder.Player.transform.position);

        public void Interact() { StartCoroutine(nameof(DelayEnd)); }

        private IEnumerator DelayEnd()
        {
            _overlay.PlayFadeOut();
            yield return new WaitForSeconds(1f);

            LoadingMenuManager.LoadingScene = "MainMenu";
            SceneManager.LoadScene("LoadingMenu");
        }
    }
}