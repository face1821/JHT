using System;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    public class System<T> : MonoBehaviour where T : System<T>
    {
        protected static T _instance;

        private void BindToRoot()
        {
            _instance = new GameObject(this.GetType().Name).AddComponent<T>();

            //寻找SystemRoot
            var root = GameObject.FindWithTag("SystemList");
            if (root == null)
            {
                root = new GameObject("SystemList");
                GameObject.DontDestroyOnLoad(root);
            }

            _instance.transform.SetParent(root.transform);
        }

        public virtual void Init() { BindToRoot(); }

        public void Destory() { DestroyImmediate(gameObject); }
    }
}