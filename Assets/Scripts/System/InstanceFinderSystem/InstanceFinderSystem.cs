using Game.Player;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.System
{
    public class InstanceFinderSystem : System<InstanceFinderSystem>, IInstanceFinderSystem
    {
        public PlayerController Player { get; private set; }

        public override void Init()
        {
            base.Init();

            Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
    }
}