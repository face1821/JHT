using Game.Player;
using Game.Tool;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Map
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelRuleBase : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] public bool PlayerEntered => enabled;

        protected PlayerStateMachine _playerStateMachine;

        private void Start() { _playerStateMachine = InstanceFinder.Player.StateMachine; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            MLogger.Log($"{name}：玩家进入规则区域");
            enabled = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            MLogger.Log($"{name}：玩家离开规则区域");
            enabled = false;
        }
    }
}