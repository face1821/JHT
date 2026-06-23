using Game.InteractableObject;
using Maxy.GameFramework.Common.System;

namespace Game.CheckPoint.Events
{
    public struct RemovePlayerInteractableObjectEvent
    {
        public IInteractableObject Object;

        public RemovePlayerInteractableObjectEvent(IInteractableObject obj)
        {
            Object = obj;
            MLogger.Log($"交互对象：移除了 {obj}");
        }
    }
}