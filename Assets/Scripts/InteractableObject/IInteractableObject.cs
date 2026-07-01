
namespace Game.InteractableObject
{
    public interface IInteractableObject
    {
        public void SetHighLight(bool state);
        public float GetDistance();
        public void Interact();
    }
}