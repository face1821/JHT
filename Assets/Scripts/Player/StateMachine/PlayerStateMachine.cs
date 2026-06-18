using System;
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

        private PlayerStateMachineParamaters _paramaters;
        private PlayerStateBase _currentState;

        #region Mono方法

        private void Awake()
        {
            _paramaters = new PlayerStateMachineParamaters();
            _paramaters.StateMachine = this;

            StateIdle = new PlayerStateIdle() { Paramaters = _paramaters };
            StateMove = new PlayerStateMove() { Paramaters = _paramaters };
            StateJump = new PlayerStateJump() { Paramaters = _paramaters };
            StateFall = new PlayerStateFall() { Paramaters = _paramaters };
        }

        private void OnEnable()
        {
            PlayerInput.OnIdle += OnIdle;
            PlayerInput.OnMove += OnMove;
        }


        private void OnDisable() { PlayerInput.OnMove -= OnMove; }

        private void Update() { _currentState.OnUpdate(); }

        private void FixedUpdate() { _currentState.OnFixedUpdate(); }

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