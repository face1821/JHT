using System;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    public class ComponentSystem<T> : MonoBehaviour where T : ComponentSystem<T>
    {
        protected static T _instance;

        protected static void BindToRoot(string systemName)
        {
            _instance = new GameObject(systemName).AddComponent<T>();

            //寻找SystemRoot
            var root = GameObject.FindWithTag("SystemList");
            if (root == null)
            {
                root = new GameObject("SystemList");
                GameObject.DontDestroyOnLoad(root);
            }

            _instance.transform.SetParent(root.transform);
        }
    }
}