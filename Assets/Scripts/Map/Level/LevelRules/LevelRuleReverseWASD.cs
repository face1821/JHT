using System;
using System.Collections.Generic;
using DG.Tweening;
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
        [SerializeField] private List<RectTransform> _buttons;

        private List<Vector3> _buttonScales;

        private void Awake()
        {
            _buttonScales = new List<Vector3>();

            foreach (var rectTransform in _buttons)
            {
                _buttonScales.Add(rectTransform.localScale);
            }
        }

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
            //每次切换的时候，都将按钮全部重置大小
            for (int i = 0; i < _buttons.Count; i++)
            {
                DOTween.Kill(_buttons[i].gameObject);

                _buttons[i].localScale = _buttonScales[i];
            }
            
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