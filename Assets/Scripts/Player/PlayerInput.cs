using System;
using Cinemachine;
using Maxy.GameFramework.Common.Tool;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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

        [SerializeField] private CinemachineVirtualCamera _vCam;

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

            //判断是不是平板，如果是，就将相机尺寸变更
            if (CheckIfTablet())
            {
                _vCam.m_Lens.OrthographicSize *= 2f;
                _vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset *= 2.5f;
            }
        }

        private void Update()
        {
            // if (Application.isEditor || Application.platform != RuntimePlatform.Android)
            //     PCInputHandle();
            // else
                PhoneInputHandle();
        }

        #region 平板设备判断

        private bool CheckIfTablet()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string model = SystemInfo.deviceModel.ToLower();
                if (model.Contains("ipad"))
                    return true;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (IsTabletScreen())
                    return true;
            }

            return false;
        }

        // 判断是否为平板屏幕（≥6.5英寸 且 最小宽度≥600dp）
        private static bool IsTabletScreen()
        {
            // 物理尺寸（英寸）
            float screenInch = GetScreenInch();
            // 最小宽度dp（更稳）
            float minWidthDp = GetSmallestWidthDp();

            return screenInch >= 6.5f || minWidthDp >= 600f;
        }

        // 计算屏幕对角线英寸
        private static float GetScreenInch()
        {
            float w = Screen.width / Screen.dpi;
            float h = Screen.height / Screen.dpi;
            return Mathf.Sqrt(w * w + h * h);
        }

        // 获取Android最小宽度dp（适配系统）
        private static float GetSmallestWidthDp()
        {
            // 用Screen.dpi近似，也可调用Android原生API更准
            float shortSide = Mathf.Min(Screen.width, Screen.height);
            return shortSide / Screen.dpi * 160f;
        }

        #endregion

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