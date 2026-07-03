using System;
using Game.Stuff;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using UnityEngine;

namespace Game.Map
{
    public class LevelRuleReverseWASD : LevelRuleBase
    {
        [SerializeField] private GameObject _normalMoveGUI;
        [SerializeField] private GameObject _reverseMoveGUI;
        [SerializeField] private GameObject _normalJumpCrouchGUI;
        [SerializeField] private GameObject _reverseJumpCrouchGUI;

        public override void OnEnter()
        {
            base.OnEnter();

            //显示反向WASD的GUI
            _normalMoveGUI.SetActive(false);
            _normalJumpCrouchGUI.SetActive(false);

            _reverseMoveGUI.SetActive(true);
            _reverseJumpCrouchGUI.SetActive(false);
        }

        public override void OnExit()
        {
            base.OnExit();

            //恢复
            _normalMoveGUI.SetActive(true);
            _normalJumpCrouchGUI.SetActive(true);

            _reverseMoveGUI.SetActive(false);
            _reverseJumpCrouchGUI.SetActive(false);
        }
    }
}