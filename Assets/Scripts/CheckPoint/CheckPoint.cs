using Game.CheckPoint.Events;
using Game.InteractableObject;
using Game.Tool;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.CheckPoint
{
    public class CheckPoint : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private CheckPointMenu _menu;

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

        #region 交互

        public float GetDistance() { return (InstanceFinder.Player.transform.position - transform.position).magnitude; }

        public void Interact()
        {
            //打开QTE界面
            _menu.Open();
        }

        #endregion
    }
}