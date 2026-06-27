using System;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static event Action OnIdle;
        public static event Action<int> OnMove;
        public static event Action OnJump;
        public static event Action OnCrouch;
        public static event Action OnInteract;

        private bool _isMoveLeft;
        private bool _isMoveRight;

        public bool IsJumpHeld { get; private set; }
        public bool IsCrouchHeld { get; private set; }

        private void Update() { PhoneInputHandle(); MLogger.Log(IsJumpHeld + "  adad  " + IsCrouchHeld); }

        private void PhoneInputHandle()
        {
            if (!_isMoveLeft && !_isMoveRight || _isMoveLeft && _isMoveRight)
                OnIdle?.Invoke();
            else if (_isMoveLeft)
                OnMove?.Invoke(-1);
            else if (_isMoveRight)
                OnMove?.Invoke(1);
        }

        private void PCInputHandle()
        {
            //输入的优先级由这里的事件发送顺序来表现

            //移动
            var moveDir = MTool.GetMoveInput();

            if (moveDir.x != 0f)
            {
                OnMove?.Invoke((int)moveDir.x);
            }
            else
            {
                OnIdle?.Invoke();
            }

            //跳跃
            if (Input.GetKey(KeyCode.W))
            {
                OnJump?.Invoke();
            }
        }

        #region 按钮事件触发

        public void BtnIdle() { OnIdle?.Invoke(); }

        public void BtnPressMoveLeft() { _isMoveLeft = true; }
        public void BtnReleaseMoveLeft() { _isMoveLeft = false; }

        public void BtnPressMoveRight() { _isMoveRight = true; }
        public void BtnReleaseMoveRight() { _isMoveRight = false; }

        public void BtnJump() { OnJump?.Invoke(); }

        public void BtnCrouch() { OnCrouch?.Invoke(); }

        public void SetJumpHeld(bool held) => IsJumpHeld = held;

        public void SetCrouchHeld(bool held) => IsCrouchHeld = held;

        public void BtnInteract() { OnInteract?.Invoke(); }

        #endregion
    }
}