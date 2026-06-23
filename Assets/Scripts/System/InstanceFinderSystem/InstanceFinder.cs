using Game.Player;
using Game.System;
using Maxy.GameFramework.Common.System;

namespace Game.Tool
{
    public static class InstanceFinder
    {
        private static IInstanceFinderSystem _instanceFinderSystem;

        static InstanceFinder() { _instanceFinderSystem = SystemCenter.Get<IInstanceFinderSystem>(); }

        public static PlayerController Player => _instanceFinderSystem.Player;
    }
}