namespace Maxy.GameFramework.Common.Events
{
    public struct PlayButtonAudioEvent
    {
        public string ButtonAudioName;

        public PlayButtonAudioEvent(string buttonAudioName) { ButtonAudioName = buttonAudioName; }
    }
}