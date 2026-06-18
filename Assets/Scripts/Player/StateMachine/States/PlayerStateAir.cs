namespace Game.Player
{
    public class PlayerStateAir : PlayerStateBase
    {
        //任何空中状态都至少需要地面标记为false时，才能进入
        public override bool CanEnter() => !Paramaters.IsGrounded;
    }
}