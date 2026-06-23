using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    [CreateAssetMenu(fileName = "SystemConfig", menuName = "Maxy/GameFramework/System Config", order = 0)]
    public class SystemConfig : ScriptableObject
    {
        [LabelText("系统列表")]
        public List<SystemInfo> Systems = new List<SystemInfo>();

        public GameObject Get<T>()
        {
            var index = Systems.FindIndex(x => x.InterfaceName == typeof(T).Name);

            return Systems[index].Prefab;
        }
    }

    [Serializable]
    public class SystemInfo
    {
        [LabelText("接口名")]
        public string InterfaceName;
        [LabelText("预制体")]
        public GameObject Prefab;
    }
}