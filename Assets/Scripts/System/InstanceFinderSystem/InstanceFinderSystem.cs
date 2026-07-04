using Game.Player;
using Maxy.GameFramework.Common.System;
using UnityEngine;

namespace Game.System
{
    public class InstanceFinderSystem : System<InstanceFinderSystem>, IInstanceFinderSystem
    {
        public PlayerController Player
        {
            get
            {
                if (_player == null)
                    _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

                return _player;
            }
        }

        private PlayerController _player;

        public override void Init()
        {
            base.Init();

            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
    }
}