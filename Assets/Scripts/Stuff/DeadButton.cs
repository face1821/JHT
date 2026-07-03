using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.Map;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Stuff
{
    [RequireComponent(typeof(Light2D))]
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class DeadButton : MonoBehaviour, IInteractableObject
    {
        public bool IsActive => gameObject.activeSelf;

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

        public void Interact()
        {
            //玩家交互这个按钮就会死
            InstanceFinder.Player.StateMachine.Die();
        }
    }
}