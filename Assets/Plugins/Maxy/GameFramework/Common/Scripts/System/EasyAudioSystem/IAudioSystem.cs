namespace Maxy.GameFramework.Common.System
{
    public interface IAudioSystem : ISystem
    {
        public void SetMasterVolume(float volume);
        public void SetMusicVolume(float volume);
        public void SetSfxVolume(float volume);
        public void SetVoiceVolume(float volume);
        public void SetAmbientVolume(float volume);
    }
}