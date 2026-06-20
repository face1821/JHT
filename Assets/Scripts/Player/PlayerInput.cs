using System;
using Maxy.GameFramework.Common.Tool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static event Action OnIdle;
        public static event Action<int> OnMove;
        public static event Action OnJump;

        private void Update()
        {
            //输入的优先级由这里的事件发送顺序来表现
            
            //移动
            var moveDir = MTool.GetMoveInput();

            if (moveDir.x != 0f)
            {
                OnMove?.Invoke((int)moveDir.x);
            }
            else
            {
                OnIdle?.Invoke();
            }

            //跳跃
            if (Input.GetKey(KeyCode.W))
            {
                OnJump?.Invoke();
            }
        }
    }
}