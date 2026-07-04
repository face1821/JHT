namespace Maxy.GameFramework.Common.System
{
    public interface IDialogueSystem : ISystem
    {
        public bool IsPlaying { get; }

        public void StartDialog(string dialogPathId);
        public void EndDialog();
    }
}