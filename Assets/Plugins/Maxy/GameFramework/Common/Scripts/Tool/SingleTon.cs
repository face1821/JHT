using UnityEngine;


namespace Maxy.GameFramework.Common.Tool
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //查找
                    _instance = GameObject.FindObjectOfType<T>();
                    
                    //找不到就创建
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
        private static T _instance;

        protected virtual void Awake()
        {
            // 防止重复创建
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}