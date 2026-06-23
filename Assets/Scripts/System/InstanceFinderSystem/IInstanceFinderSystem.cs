using Game.Player;
using Maxy.GameFramework.Common.System;

namespace Game.System
{
    public interface IInstanceFinderSystem : ISystem
    {
        public PlayerController Player { get; }
    }
}