using System;
using Maxy.GameFramework.Common.Tool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance { get; private set; }

        #region 事件

        public static event Action OnIdle;
        public static event Action<int> OnMove;
        public static event Action OnJump;
        public static event Action<int> OnCrouch;
        public static event Action OnInteract;

        #endregion

        #region 输入状态

        [ShowInInspector, ReadOnly] public static int MoveDirection => (IsMoveLeft ? -1 : 0) + (IsMoveRight ? 1 : 0);
        [ShowInInspector, ReadOnly] public static bool IsMoveLeft { get; private set; }
        [ShowInInspector, ReadOnly] public static bool IsMoveRight { get; private set; }

        [ShowInInspector, ReadOnly] public static int UpDownMoveDirection => (IsCrouchHeld ? -1 : 0) + (IsJumpHeld ? 1 : 0);
        [ShowInInspector, ReadOnly] public static bool IsJumpHeld { get; private set; }
        [ShowInInspector, ReadOnly] public static bool IsCrouchHeld { get; private set; }

        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.Android)
                PCInputHandle();
            else
                PhoneInputHandle();
        }

        private void PhoneInputHandle()
        {
            //输入的优先级由这里的事件发送顺序来表现

            //跳跃状态
            if (IsJumpHeld)
            {
                OnJump?.Invoke();
            }

            //下蹲状态
            if (IsCrouchHeld)
            {
                OnCrouch?.Invoke(MoveDirection);
            }
            else if (MoveDirection == 0) //移动状态
            {
                OnIdle?.Invoke();
            }
            else //待机状态
            {
                OnMove?.Invoke(MoveDirection);
            }
        }

        private void PCInputHandle()
        {
            //输入的优先级由这里的事件发送顺序来表现
            IsMoveLeft = Input.GetKey(KeyCode.A);
            IsMoveRight = Input.GetKey(KeyCode.D);
            IsJumpHeld = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
            IsCrouchHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftShift);

            //交互
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInteract?.Invoke();
            }

            //跳跃状态
            if (IsJumpHeld)
            {
                OnJump?.Invoke();
            }

            //下蹲状态
            if (IsCrouchHeld)
            {
                OnCrouch?.Invoke(MoveDirection);
            }
            else if (MoveDirection == 0) //移动状态
            {
                OnIdle?.Invoke();
            }
            else //待机状态
            {
                OnMove?.Invoke(MoveDirection);
            }
        }

        #region 按钮事件触发

        public void BtnPressMoveLeft() { IsMoveLeft = true; }
        public void BtnReleaseMoveLeft() { IsMoveLeft = false; }

        public void BtnPressMoveRight() { IsMoveRight = true; }
        public void BtnReleaseMoveRight() { IsMoveRight = false; }

        public void SetJumpHeld(bool held) { IsJumpHeld = held; }

        public void BtnPressCrouch() { IsCrouchHeld = true; }

        public void BtnReleaseCrouch() { IsCrouchHeld = false; }

        public void BtnInteract() { OnInteract?.Invoke(); }

        #endregion
    }
}