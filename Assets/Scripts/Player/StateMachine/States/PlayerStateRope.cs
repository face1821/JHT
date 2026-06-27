using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;

public class PlayerStateRope : PlayerStateBase
{
    public override void OnFixedUpdate()
    {
        base.OnUpdate();
            
        // Body.SetVelocityY(Paramaters.MoveDirection * Paramaters.MoveSpeed);
        //TODO：还没写完，先写蹲下状态
    }
}
