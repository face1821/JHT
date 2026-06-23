using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    public static class SystemCenter
    {
        private static SystemConfig _systemConfig;
        private static Dictionary<Type, ISystem> _systemListCache = new Dictionary<Type, ISystem>();

        static SystemCenter() { _systemConfig = Resources.Load<SystemConfig>("Datas/SystemConfig"); }

        public static T Get<T>() where T : class, ISystem
        {
            //从缓存中返回
            if (_systemListCache.ContainsKey(typeof(T)))
            {
                return _systemListCache[typeof(T)] as T;
            }

            //创建对象并返回
            MLogger.Log($"系统中心：创建 {typeof(T).Name}");
            var gameObject = GameObject.Instantiate(_systemConfig.Get<T>());
            var component = gameObject.GetComponent<T>();
            component.Init();
            _systemListCache.Add(typeof(T), component);

            return component;
        }

        public static void ClearAll()
        {
            foreach (var system in _systemListCache.Values)
            {
                system.Destory();
            }

            _systemListCache.Clear();
        }
    }
}