using UnityEngine;

namespace Game.InteractableObject
{
    public interface IClimbingObject : IInteractableObject
    {
        public Transform transform { get; }
    }
}