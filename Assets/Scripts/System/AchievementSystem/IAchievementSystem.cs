using Maxy.GameFramework.Common.System;

namespace Game.System
{
    public interface IAchievementSystem : ISystem
    {
        public bool HasUnlockedAchievement(string achievementName);
        public void UnlockAchievement(string achievementName);
    }
}