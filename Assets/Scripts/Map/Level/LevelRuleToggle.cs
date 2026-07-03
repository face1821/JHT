using Game.InteractableObject;
using Game.Tool;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Map
{
    [RequireComponent(typeof(Light2D))]
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class LevelRuleToggle : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private LevelRuleBase _rule;
        [SerializeField] private GameObject _ruleLight;
        private Light2D _light;

        private void Awake() { _light = GetComponent<Light2D>(); }

        public void SetHighLight(bool state) { _light.enabled = state; }

        public float GetDistance() => Vector3.Distance(transform.position, InstanceFinder.Player.transform.position);

        public void Interact()
        {
            _rule.enabled = !_rule.enabled;
            _ruleLight.SetActive(_rule.enabled);
        }
    }
}