using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    [CreateAssetMenu(fileName = "SystemConfig", menuName = "Maxy/GameFramework/System Config", order = 0)]
    public class SystemConfig : ScriptableObject
    {
        public List<string> SystemTypeReferences = new List<string>();
        public List<GameObject> SystemGameObjectReferences = new List<GameObject>();

        public GameObject Get<T>()
        {
            var index = SystemTypeReferences.IndexOf(typeof(T).Name);

            return SystemGameObjectReferences[index];
        }
    }
}