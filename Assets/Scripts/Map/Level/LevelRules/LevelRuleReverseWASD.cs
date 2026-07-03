using System;
using Game.Player;
using Game.Stuff;
using Game.Tool;
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

            SetReverseUIShowState(true);
        }

        public override void OnExit()
        {
            base.OnExit();

            SetReverseUIShowState(false);
        }

        private void SetReverseUIShowState(bool isReverse)
        {
            //切换反向WASD的GUI
            _normalMoveGUI.SetActive(!isReverse);
            _normalJumpCrouchGUI.SetActive(!isReverse);

            _reverseMoveGUI.SetActive(isReverse);
            _reverseJumpCrouchGUI.SetActive(isReverse);

            //重置按键的状态
            InstanceFinder.Player.Input.BtnReleaseMoveLeft();
            InstanceFinder.Player.Input.BtnReleaseMoveRight();
            InstanceFinder.Player.Input.BtnReleaseCrouch();
        }
    }
}