using Game.InteractableObject;

namespace Game.CheckPoint.Events
{
    public struct AddPlayerInteractableObjectEvent
    {
        public IInteractableObject Object;

        public AddPlayerInteractableObjectEvent(IInteractableObject obj) { Object = obj; }
    }
}