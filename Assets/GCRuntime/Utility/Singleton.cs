using System;

namespace GCRuntime.Utility
{
    /// <summary>
    /// 泛型单例基类（普通单例）
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        private static T _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 销毁单例实例
        /// </summary>
        public static void Destroy()
        {
            if (_instance != null)
            {
                lock (_lock)
                {
                    if (_instance != null)
                    {
                        var disposable = _instance as IDisposable;
                        disposable?.Dispose();
                        _instance = null;
                    }
                }
            }
        }
    }
}