using UnityEngine;
using System;

namespace GCRuntime.Utility
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[{typeof(T).Name}] 应用程序正在退出，返回 null");
                    return null;
                }

                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = FindAnyObjectByType<T>();

                        if (instance == null)
                        {
                            var singletonObject = new GameObject();
                            instance = singletonObject.AddComponent<T>();
                            singletonObject.name = $"{typeof(T).Name} (Singleton)";
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// 手动创建单例实例（用于在Awake前预创建）
        /// </summary>
        public static T CreateInstance()
        {
            if (_applicationIsQuitting)
                return null;

            lock (_lock)
            {
                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = $"{typeof(T).Name} (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
                return instance;
            }
        }

        /// <summary>
        /// 销毁单例实例
        /// </summary>
        public static void DestroyInstance()
        {
            if (instance != null)
            {
                lock (_lock)
                {
                    if (instance != null)
                    {
                        if (Application.isPlaying)
                        {
                            Destroy(instance.gameObject);
                        }
                        else
                        {
                            DestroyImmediate(instance.gameObject);
                        }
                        instance = null;
                        _applicationIsQuitting = false;
                    }
                }
            }
        }

        /// <summary>
        /// 在Awake中注册实例
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = this as T;
                        DontDestroyOnLoad(gameObject);
                    }
                    else if (instance != this)
                    {
                        Debug.LogWarning($"[GCRuntime]: 检测到重复的 {typeof(T).Name} 实例，将被销毁");
                        Destroy(gameObject);
                    }
                }
            }
            else if (instance != this)
            {
                Debug.LogWarning($"[GCRuntime]: 检测到重复的 {typeof(T).Name} 实例，将被销毁");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 应用退出时的清理
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        /// <summary>
        /// 销毁时的清理
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
