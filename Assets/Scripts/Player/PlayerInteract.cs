using System;
using System.Collections.Generic;
using Game.CheckPoint.Events;
using Game.InteractableObject;
using Maxy.GameFramework.Common.Events;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] private List<IInteractableObject> _interactableObjects = new List<IInteractableObject>();
        [ShowInInspector, ReadOnly] private IInteractableObject _closestInteractableObject;

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

        private void OnAddInteractableObject(AddPlayerInteractableObjectEvent ctx)
        {
            foreach (var item in _interactableObjects)
            {
                item.SetHighLight(false);
            }

            _interactableObjects.Add(ctx.Object);
            _closestInteractableObject = GetClosestInteractableObject();
            _closestInteractableObject.SetHighLight(true);
        }

        private void OnRemoveInteractableObject(RemovePlayerInteractableObjectEvent ctx)
        {
            ctx.Object.SetHighLight(false);
            _interactableObjects.Remove(ctx.Object);

            if (_closestInteractableObject == ctx.Object)
            {
                _closestInteractableObject = null;
            }
        }

        private IInteractableObject GetClosestInteractableObject()
        {
            //获取最近交互对象
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

            return resultObj;
        }

        private void OnInteract()
        {
            if (_closestInteractableObject == null) return;

            MLogger.Log("交互中...");
            _closestInteractableObject.Interact();
        }
    }
}