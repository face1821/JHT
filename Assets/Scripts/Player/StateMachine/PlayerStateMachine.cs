using System;
using Maxy.GameFramework.Game2D.Tool;
using UnityEngine;

namespace Game.Player
{
    [Serializable]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region 状态

        public PlayerStateBase StateIdle { get; private set; }
        public PlayerStateBase StateMove { get; private set; }
        public PlayerStateBase StateJump { get; private set; }
        public PlayerStateBase StateFall { get; private set; }

        #endregion

        #region 组件

        [SerializeField] private PlayerBody _body;
        [SerializeField] private BoxDetection2D _groundDetection;

        #endregion

        private PlayerStateMachineParamaters _paramaters;
        private PlayerStateBase _currentState;

        #region Mono方法

        private void Awake()
        {
            _paramaters = new PlayerStateMachineParamaters();
            _paramaters.StateMachine = this;
            _paramaters.Body = _body;
            _paramaters.MoveSpeed = _body.MoveSpeed;
            _paramaters.JumpSpeed = _body.JumpSpeed;

            StateIdle = new PlayerStateIdle() { Paramaters = _paramaters };
            StateMove = new PlayerStateMove() { Paramaters = _paramaters };
            StateJump = new PlayerStateJump() { Paramaters = _paramaters };
            StateFall = new PlayerStateFall() { Paramaters = _paramaters };
        }

        private void OnEnable()
        {
            PlayerInput.OnIdle += OnIdle;
            PlayerInput.OnMove += OnMove;
            PlayerInput.OnJump += OnJump;
        }


        private void OnDisable()
        {
            PlayerInput.OnIdle -= OnIdle;
            PlayerInput.OnMove -= OnMove;
            PlayerInput.OnJump -= OnJump;
        }

        private void Update() { _currentState.OnUpdate(); }

        private void FixedUpdate()
        {
            //更新地面标记
            _paramaters.IsGrounded = _groundDetection.Detect();


            _currentState.OnFixedUpdate();
        }

        #endregion

        #region 输入接收

        private void OnIdle() { RequestToChangeState(StateIdle); }

        private void OnMove(int moveDir)
        {
            if (RequestToChangeState(StateMove))
            {
                _paramaters.MoveDirection = moveDir;
            }
        }

        private void OnJump() { RequestToChangeState(StateJump); }

        #endregion

        public bool RequestToChangeState(PlayerStateBase state)
        {
            if (_currentState.CanBeInterrupt() && state.CanEnter())
            {
                ChangeState(state);
                return true;
            }

            return false;
        }

        private void ChangeState(PlayerStateBase state)
        {
            if (_currentState == state) return;

            _currentState?.OnExit();
            _currentState = state;
            _currentState.OnEnter();
        }
    }
}