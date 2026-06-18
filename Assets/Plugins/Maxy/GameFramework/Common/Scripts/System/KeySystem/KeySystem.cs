using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    public class KeySystem
    {
        private static bool _inited { get; set; }
        [ShowInInspector]
        private static Dictionary<string, KeyCode> _keys;

        private static void Init()
        {
            //键位管理器的初始化处理

            //如果游戏是第一次打开，那么先初始化键位配置再存储
            if (!ES3.Load("IsFirstGame", false))
            {
                _keys = new Dictionary<string, KeyCode>();

                _keys["Action-MoveLeft"] = KeyCode.A;
                _keys["Action-MoveRight"] = KeyCode.D;
                _keys["Action-MoveForward"] = KeyCode.W;
                _keys["Action-MoveBackward"] = KeyCode.S;
                ES3.Save("Keys", _keys);

                ES3.Save("IsFirstGame", true);
                _inited = true;

                return;
            }

            //从键位配置存储中读取数据
            _keys = ES3.Load("Keys", new Dictionary<string, KeyCode>());

            _inited = true;
        }

        public static void SetDefaultKeys(Dictionary<string, KeyCode> keys)
        {
            if(!_inited) Init();
            
            _keys = keys;
            ES3.Save("Keys", _keys);
        }

        public static void ResetKeyFromDefault()
        {
            //恢复键位配置为游戏默认键位配置
            //直接调用Init方法，来重新初始化配置，再读取配置
            ES3.Save("IsFirstGame", false);
            Init();
        }

        public static KeyCode GetKey(string action)
        {
            if (!_inited) Init();

            if (_keys.ContainsKey(action)) return _keys[action];

            Debug.LogError($"KeyManager：不存在<{action}>键位名，无法获取");
            return KeyCode.None;
        }

        public static void ChangeKey(string action, KeyCode key)
        {
            if (!_inited) Init();

            if (_keys.ContainsKey(action))
            {
                _keys[action] = key;
                ES3.Save("Keys", _keys);

                return;
            }

            Debug.LogError($"KeyManager：不存在<{action}>键位名，无法修改");
        }

        public static void SetKey(string action, KeyCode key)
        {
            if (!_inited) Init();

            _keys[action] = key;
            ES3.Save("Keys", _keys);
        }
    }
}