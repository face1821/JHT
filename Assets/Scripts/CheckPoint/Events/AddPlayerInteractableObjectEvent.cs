using Game.InteractableObject;
using Maxy.GameFramework.Common.System;

namespace Game.CheckPoint.Events
{
    public struct AddPlayerInteractableObjectEvent
    {
        public IInteractableObject Object;

        public AddPlayerInteractableObjectEvent(IInteractableObject obj)
        {
            Object = obj;
            MLogger.Log($"交互对象：添加了 {obj}");
        }
    }
}