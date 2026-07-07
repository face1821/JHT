using Game.CheckPoint.Events;
using Game.Map;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.InteractableObject
{
    [RequireComponent(typeof(Light2D))]
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class LevelRuleToggleButton : MonoBehaviour, IInteractableObject
    {
        public bool IsActive => gameObject.activeSelf;

        [SerializeField] private AudioClip _openClip;
        [SerializeField] private AudioClip _closeClip;
        [SerializeField] private Sprite _openSprite;
        [SerializeField] private Sprite _closeSprite;
        [SerializeField] private LevelRuleBase _rule;
        [SerializeField] private GameObject _ruleLight;

        private SpriteRenderer _renderer;
        private Light2D _light;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _light = GetComponent<Light2D>();
        }

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

        public void Interact()
        {
            _rule.enabled = !_rule.enabled;
            _ruleLight.SetActive(_rule.enabled);

            var sprite = _rule.enabled ? _closeSprite : _openSprite;
            _renderer.sprite = sprite;
            _light.lightCookieSprite = sprite;

            if (_rule.enabled)
                SystemCenter.Get<IAudioSystem>().PlaySfx(_openClip, "_button_open_clip", InstanceFinder.Player.transform);
            else
                SystemCenter.Get<IAudioSystem>().PlaySfx(_closeClip, "_button_open_clip", InstanceFinder.Player.transform);
        }
    }
}