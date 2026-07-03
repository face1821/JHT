using System;
using Game.Stuff;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleReverseWASD : LevelRuleBase
    {
        public override void OnEnter()
        {
            base.OnEnter();

            _playerStateMachine.Paramaters.ReverseWASD = true;
        }

        public override void OnExit()
        {
            base.OnExit();

            _playerStateMachine.Paramaters.ReverseWASD = false;
        }
    }
}