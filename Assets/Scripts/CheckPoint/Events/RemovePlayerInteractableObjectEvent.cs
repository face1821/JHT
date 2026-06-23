using Game.InteractableObject;

namespace Game.CheckPoint.Events
{
    public struct RemovePlayerInteractableObjectEvent
    {
        public IInteractableObject Object;

        public RemovePlayerInteractableObjectEvent(IInteractableObject obj) { Object = obj; }
    }
}