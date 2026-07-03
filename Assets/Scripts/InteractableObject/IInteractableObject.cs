
namespace Game.InteractableObject
{
    public interface IInteractableObject
    {
        public bool IsActive { get; }
        public void SetHighLight(bool state);
        public float GetDistance();
        public void Interact();
    }
}