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
        [LabelText("移动速度"), ReadOnly] public float MoveSpeed;
        [LabelText("跳跃速度"), ReadOnly] public float JumpSpeed;
        [LabelText("蹲下速度倍率")] public float CrouchSpeedMultiplier;
        [LabelText("攀爬速度倍率")] public float ClimbSpeedMultiplier;

        [Header("布尔参数")]
        [LabelText("地面标记"), ReadOnly] public bool IsGrounded = true;
        [LabelText("下蹲头顶检测标记"), ReadOnly] public bool IsCrouchHead;
        [LabelText("WASD反向标记"), ReadOnly] public bool ReverseWASD;

        [Header("整数参数")]
        [LabelText("朝向"), ReadOnly] public int FaceDirection = -1;
        [LabelText("移动方向"), ReadOnly] public int MoveDirection = -1;
        [LabelText("上下移动方向"), ReadOnly] public int UpDownMoveDirection;

        [LabelText("攀爬物"), ReadOnly] public IClimbingObject ClimbingObject;
        [LabelText("攀爬脱离碰撞偏移检测")] public Vector2 ClimbingDetectOffset;
        [LabelText("攀爬脱离碰撞大小检测")] public float ClimbingDetectRadius;
    }
}