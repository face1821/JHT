

namespace Game.Player
{
    public class PlayerStateMachineParamaters
    {
        public PlayerStateMachine StateMachine;
        
        public int MoveDirection;

        //地面标记（跳跃时为false，落地后变true）
        public bool IsGrounded;
    }
}