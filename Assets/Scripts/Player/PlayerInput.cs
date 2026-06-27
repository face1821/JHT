using System;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Common.Tool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        #region 事件

        public static event Action OnIdle;
        public static event Action<int> OnMove;
        public static event Action OnJump;
        public static event Action OnCrouch;
        public static event Action OnInteract;

        #endregion

        #region 输入状态

        public static int MoveDirection => (IsMoveLeft ? -1 : 0) + (IsMoveRight ? 1 : 0);
        public static bool IsMoveLeft { get; private set; }
        public static bool IsMoveRight { get; private set; }
        
        public static int UpDownMoveDirection => (IsJumpHeld ? -1 : 0) + (IsJumpHeld ? 1 : 0);
        public static bool IsJumpHeld { get; private set; }
        public static bool IsCrouchHeld { get; private set; }

        #endregion

        private void Update() { PhoneInputHandle(); }

        private void PhoneInputHandle()
        {
            //输入的优先级由这里的事件发送顺序来表现

            if (!IsMoveLeft && !IsMoveRight || IsMoveLeft && IsMoveRight)
                OnIdle?.Invoke();
            else if (IsMoveLeft)
                OnMove?.Invoke(-1);
            else if (IsMoveRight)
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

        public void BtnPressMoveLeft() { IsMoveLeft = true; }
        public void BtnReleaseMoveLeft() { IsMoveLeft = false; }

        public void BtnPressMoveRight() { IsMoveRight = true; }
        public void BtnReleaseMoveRight() { IsMoveRight = false; }

        public void SetJumpHeld(bool held)
        {
            IsJumpHeld = held;

            if (held)
            {
                OnJump?.Invoke();
            }
        }

        public void SetCrouchHeld(bool held)
        {
            IsCrouchHeld = held;

            if (held)
            {
                OnCrouch?.Invoke();
            }
        }

        public void BtnInteract() { OnInteract?.Invoke(); }

        #endregion
    }
}