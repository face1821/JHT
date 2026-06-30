using System;
using Game.InteractableObject;
using Game.Prop;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    [Serializable]
    public class PlayerStateMachineParamaters
    {
        [HideInInspector] public PlayerStateMachine StateMachine;
        [HideInInspector] public PlayerStateBase CurrentState;
        [HideInInspector] public PlayerBody Body;
        [HideInInspector] public PlayerAnimator Animator;
        [HideInInspector] public PlayerInput Input;

        [Header("基本属性")]
        [LabelText("移动速度")] public float MoveSpeed;
        [LabelText("跳跃速度")] public float JumpSpeed;
        [LabelText("蹲下速度倍率")] public float CrouchSpeedMultiplier = 0.6f;
        [LabelText("攀爬速度倍率")] public float ClimbSpeedMultiplier = 0.3f;

        [Header("布尔参数")]
        [LabelText("地面标记")] public bool IsGrounded = true;

        [Header("整数参数")]
        [LabelText("朝向")] public int FaceDirection = -1;
        [LabelText("移动方向")] public int MoveDirection = -1;
        [LabelText("上下移动方向")] public int UpDownMoveDirection;

        [LabelText("攀爬物"), ReadOnly] public IClimbingObject ClimbingObject;
    }
}