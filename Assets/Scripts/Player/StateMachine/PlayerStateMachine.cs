using System;
using Game.InteractableObject;
using Maxy.GameFramework.Common.System;
using Maxy.GameFramework.Game2D.Tool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    [Serializable]
    [RequireComponent(typeof(PlayerInput), typeof(PlayerBody), typeof(PlayerAnimator))]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region 事件

        public static event Action OnDead;
        public static event Action OnIdle;
        public static event Action<int> OnMove;
        public static event Action OnJump;
        public static event Action OnCrouch;
        public static event Action OnFall;
        public static event Action OnClimb;

        #endregion

        #region 状态

        [ShowInInspector, ReadOnly] public string CurrentStateName => _currentState?.GetType().Name;
        public PlayerStateBase StateDead { get; private set; }
        public PlayerStateBase StateIdle { get; private set; }
        public PlayerStateBase StateMove { get; private set; }
        public PlayerStateBase StateCrouch { get; private set; }
        public PlayerStateBase StateJump { get; private set; }
        public PlayerStateBase StateFall { get; private set; }
        public PlayerStateBase StateLand { get; private set; }
        public PlayerStateBase StateClimb { get; private set; }

        #endregion

        #region 组件

        [SerializeField] private BoxColliderDetection2D _crouchHeadDetection;
        [SerializeField] private BoxColliderDetection2D _groundDetection;
        private PlayerBody _body;
        private PlayerAnimator _animator;

        #endregion

        public PlayerStateMachineParamaters Paramaters => _paramaters;
        public PlayerStateBase CurrentState => _currentState;

        [SerializeField] private PlayerStateMachineParamaters _paramaters;

        private PlayerStateBase _currentState;

        #region Mono方法

        private void Start()
        {
            //组件获取
            _body = GetComponent<PlayerBody>();
            _animator = GetComponent<PlayerAnimator>();

            //上下文参数配置
            _paramaters.StateMachine = this;
            _paramaters.Body = _body;
            _paramaters.Animator = _animator;
            _paramaters.MoveSpeed = _body.MoveSpeed;
            _paramaters.JumpSpeed = _body.JumpSpeed;

            //状态配置
            StateDead = new PlayerStateDead() { Paramaters = _paramaters };
            StateIdle = new PlayerStateIdle() { Paramaters = _paramaters };
            StateMove = new PlayerStateMove() { Paramaters = _paramaters };
            StateCrouch = new PlayerStateCrouch() { Paramaters = _paramaters };
            StateJump = new PlayerStateJump() { Paramaters = _paramaters };
            StateFall = new PlayerStateFall() { Paramaters = _paramaters };
            StateLand = new PlayerStateLand() { Paramaters = _paramaters };
            StateClimb = new PlayerStateClimb() { Paramaters = _paramaters };

            ChangeState(StateIdle);
        }

        private void OnEnable()
        {
            PlayerInput.OnIdle += OnInputIdle;
            PlayerInput.OnMove += OnInputMove;
            PlayerInput.OnJump += OnInputJump;
            PlayerInput.OnCrouch += OnInputCrouch;

            _crouchHeadDetection.OnTouched += OnCrouchHeadTouched;
            _crouchHeadDetection.OnLeave += OnCrouchHeadLeave;

            _groundDetection.OnTouched += OnGroundTouched;
            _groundDetection.OnLeave += OnGroundLeave;
        }

        private void OnDisable()
        {
            PlayerInput.OnIdle -= OnInputIdle;
            PlayerInput.OnMove -= OnInputMove;
            PlayerInput.OnJump -= OnInputJump;
            PlayerInput.OnCrouch -= OnInputCrouch;

            _crouchHeadDetection.OnTouched += OnCrouchHeadTouched;
            _crouchHeadDetection.OnLeave += OnCrouchHeadLeave;

            _groundDetection.OnTouched -= OnGroundTouched;
            _groundDetection.OnLeave -= OnGroundLeave;
        }

        private void Update() { _currentState.OnUpdate(); }

        private void FixedUpdate() { _currentState.OnFixedUpdate(); }

        private void OnDrawGizmos()
        {
            if (_currentState is PlayerStateClimb)
            {
                Gizmos.DrawWireSphere(transform.position + new Vector3(_paramaters.ClimbingDetectOffset.x, _paramaters.ClimbingDetectOffset.y), _paramaters.ClimbingDetectRadius);
            }
        }

        #endregion

        #region 输入接收

        private void OnInputIdle()
        {
            _paramaters.MoveDirection = 0;

            //只有为地面移动状态时，才会请求切为站立待机动画
            if (_currentState is PlayerStateMove && _currentState is not PlayerStateJump && _currentState is not PlayerStateLand
                || !PlayerInput.IsCrouchHeld && _currentState is PlayerStateCrouch)
            {
                RequestChangeState(StateIdle);
            }
        }

        private void OnInputMove(int moveDir)
        {
            _paramaters.MoveDirection = moveDir;
            _paramaters.FaceDirection = moveDir;

            //只有为地面待机状态时，才会请求切换
            if (_currentState is PlayerStateIdle && _currentState is not PlayerStateJump && _currentState is not PlayerStateLand
                || !PlayerInput.IsCrouchHeld && _currentState is PlayerStateCrouch)
            {
                RequestChangeState(StateMove);
            }
        }

        private void OnInputJump()
        {
            //只有为地面状态时，才会请求切换
            if (_currentState is PlayerStateGround)
            {
                RequestChangeState(StateJump);
            }
        }

        private void OnInputCrouch(int moveDirection)
        {
            _paramaters.MoveDirection = moveDirection;
            _paramaters.FaceDirection = moveDirection != 0 ? moveDirection : _paramaters.FaceDirection;

            //只有为地面状态时，才会请求切换
            if (_currentState is PlayerStateIdle or PlayerStateMove)
            {
                RequestChangeState(StateCrouch);
            }
        }

        #endregion

        #region 外部状态申请

        public void TryToClimb(IClimbingObject climbingObject)
        {
            _paramaters.ClimbingObject = climbingObject;

            //如果不能攀爬，就重置攀爬物体对象引用
            if (!RequestChangeState(StateClimb))
            {
                _paramaters.ClimbingObject = null;
            }
        }

        public void Die() { RequestChangeState(StateDead); }

        #endregion

        #region 碰撞检测接收

        private void OnCrouchHeadTouched(Collider2D collision) { _paramaters.IsCrouchHead = true; }

        private void OnCrouchHeadLeave(Collider2D collision) { _paramaters.IsCrouchHead = false; }

        private void OnGroundTouched(Collider2D collision) { _paramaters.IsGrounded = true; }

        private void OnGroundLeave(Collider2D collision) { _paramaters.IsGrounded = false; }

        #endregion

        public void Respawn()
        {
            //重置为站立待机状态
            _currentState = StateIdle;
            _currentState.OnEnter();
        }

        public bool RequestChangeState(PlayerStateBase state)
        {
            if (_currentState == state) return false;

            MLogger.Log($"状态机：申请从 {_currentState}切换为 {state}");
            MLogger.Log($"状态机：是否可被该状态打断（{_currentState.CanBeInterrupt(state)}），是否可进入（{state.CanEnter()}）");
            if (state is PlayerStateDead || _currentState.CanBeInterrupt(state) && state.CanEnter())
            {
                MLogger.Log($"状态机：切换成功！");
                ChangeState(state);
                return true;
            }

            return false;
        }

        private void ChangeState(PlayerStateBase state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _paramaters.CurrentState = state;
            _currentState.OnEnter();

            //状态变化的事件调用，供外部使用
            switch (_currentState)
            {
                case PlayerStateDead:
                    OnDead?.Invoke();
                    return;
                case PlayerStateIdle:
                    OnIdle?.Invoke();
                    return;
                case PlayerStateMove:
                    OnMove?.Invoke(_paramaters.MoveDirection);
                    return;
                case PlayerStateJump:
                    OnJump?.Invoke();
                    return;
                case PlayerStateCrouch:
                    OnCrouch?.Invoke();
                    return;
                case PlayerStateFall:
                    OnFall?.Invoke();
                    return;
                case PlayerStateClimb:
                    OnClimb?.Invoke();
                    return;
            }
        }
    }
}