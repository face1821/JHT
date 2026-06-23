using System;
using Maxy.GameFramework.Game2D.Tool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    [Serializable]
    [RequireComponent(typeof(PlayerInput), typeof(PlayerBody))]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region 事件

        public static event Action OnIdle;
        public static event Action<int> OnMove;
        public static event Action OnJump;

        #endregion

        #region 状态

        [ShowInInspector, ReadOnly] public string CurrentStateName => _currentState?.GetType().Name;
        public PlayerStateBase StateIdle { get; private set; }
        public PlayerStateBase StateMove { get; private set; }
        public PlayerStateBase StateJump { get; private set; }
        public PlayerStateBase StateFall { get; private set; }

        #endregion

        #region 组件

        [SerializeField] private BoxColliderDetection2D _groundDetection;
        private PlayerBody _body;

        #endregion

        [ShowInInspector] public PlayerStateMachineParamaters Paramaters { get; private set; }
        private PlayerStateBase _currentState;

        #region Mono方法

        private void Start()
        {
            //组件获取
            _body = GetComponent<PlayerBody>();

            //上下文参数配置
            Paramaters = new PlayerStateMachineParamaters();
            Paramaters.StateMachine = this;
            Paramaters.Body = _body;
            Paramaters.MoveSpeed = _body.MoveSpeed;
            Paramaters.JumpSpeed = _body.JumpSpeed;

            //状态配置
            StateIdle = new PlayerStateIdle() { Paramaters = Paramaters };
            StateMove = new PlayerStateMove() { Paramaters = Paramaters };
            StateJump = new PlayerStateJump() { Paramaters = Paramaters };
            StateFall = new PlayerStateFall() { Paramaters = Paramaters };

            ChangeState(StateFall);
        }

        private void OnEnable()
        {
            PlayerInput.OnIdle += OnInputIdle;
            PlayerInput.OnMove += OnInputMove;
            PlayerInput.OnJump += OnInputJump;

            _groundDetection.OnTouched += OnGroundTouched;
            _groundDetection.OnLeave += OnGroundLeave;
        }

        private void OnDisable()
        {
            PlayerInput.OnIdle -= OnInputIdle;
            PlayerInput.OnMove -= OnInputMove;
            PlayerInput.OnJump -= OnInputJump;

            _groundDetection.OnTouched -= OnGroundTouched;
            _groundDetection.OnLeave -= OnGroundLeave;
        }

        private void Update() { _currentState.OnUpdate(); }

        private void FixedUpdate() { _currentState.OnFixedUpdate(); }

        #endregion

        #region 输入接收

        private void OnInputIdle()
        {
            Paramaters.MoveDirection = 0;

            RequestToChangeState(StateIdle);
        }

        private void OnInputMove(int moveDir)
        {
            Paramaters.MoveDirection = moveDir;
            Paramaters.FaceDirection = moveDir;

            RequestToChangeState(StateMove);
        }

        private void OnInputJump() { RequestToChangeState(StateJump); }

        #endregion

        #region 碰撞检测接收

        private void OnGroundTouched(Collider2D collision) { Paramaters.IsGrounded = true; }

        private void OnGroundLeave(Collider2D collision) { Paramaters.IsGrounded = false; }

        #endregion

        public bool RequestToChangeState(PlayerStateBase state)
        {
            if (_currentState == state) return false;

            if (_currentState.CanBeInterrupt() && state.CanEnter())
            {
                ChangeState(state);
                return true;
            }

            return false;
        }

        private void ChangeState(PlayerStateBase state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _currentState.OnEnter();

            //状态变化的事件调用，供外部使用
            switch (_currentState)
            {
                case PlayerStateIdle:
                    OnIdle?.Invoke();
                    return;
                case PlayerStateMove:
                    OnMove?.Invoke(Paramaters.MoveDirection);
                    return;
                case PlayerStateJump:
                    OnJump?.Invoke();
                    return;
            }
        }
    }
}