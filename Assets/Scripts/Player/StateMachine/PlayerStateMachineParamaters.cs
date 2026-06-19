

using UnityEngine;

namespace Game.Player
{
    public class PlayerStateMachineParamaters
    {
        public PlayerStateMachine StateMachine;
        public PlayerBody Body;
        
        public float MoveSpeed;
        public float JumpSpeed;
        
        public int MoveDirection;

        //地面标记
        public bool IsGrounded;
    }
}