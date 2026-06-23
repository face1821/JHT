using System;
using System.Collections.Generic;
using Game.CheckPoint.Events;
using Game.InteractableObject;
using Maxy.GameFramework.Common.Events;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private List<IInteractableObject> _interactableObjects = new List<IInteractableObject>();

        private void OnEnable()
        {
            PlayerInput.OnInteract += OnInteract;
            EventBus.Subscribe<AddPlayerInteractableObjectEvent>(OnAddInteractableObject);
            EventBus.Subscribe<RemovePlayerInteractableObjectEvent>(OnRemoveInteractableObject);
        }

        private void OnDisable()
        {
            PlayerInput.OnInteract -= OnInteract;
            EventBus.Unsubscribe<AddPlayerInteractableObjectEvent>(OnAddInteractableObject);
            EventBus.Unsubscribe<RemovePlayerInteractableObjectEvent>(OnRemoveInteractableObject);
        }

        private void OnAddInteractableObject(AddPlayerInteractableObjectEvent ctx) { _interactableObjects.Add(ctx.Object); }

        private void OnRemoveInteractableObject(RemovePlayerInteractableObjectEvent ctx) { _interactableObjects.Remove(ctx.Object); }

        private void OnInteract()
        {
            var minDistance = 100f;
            IInteractableObject resultObj = null;
            foreach (var item in _interactableObjects)
            {
                var distance = item.GetDistance();
                if (distance < minDistance)
                {
                    minDistance = distance;
                    resultObj = item;
                }
            }

            if (resultObj == null) return;

            resultObj.Interact();
        }
    }
}