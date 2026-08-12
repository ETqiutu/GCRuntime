using System.Collections.Generic;
using System;
using System.Linq;

namespace GCRuntime.Event
{
    public static class EventSystem 
    {
        private static Dictionary<Type, object> listeners = new Dictionary<Type, object>();

        private static readonly object _lock = new object();

        #region 订阅

        /// <summary>
        /// 订阅事件
        /// </summary>
        public static string Subscribe<T>(Action<T> callback, int priority = 0, string id = null) 
            where T : struct, IEvent
        {
            return Subscribe(callback, priority, id, false, 0);
        }

        /// <summary>
        /// 订阅一次性事件
        /// </summary>
        public static string SubscribeOnce<T>(Action<T> callback, int priority = 0, string id = null) 
            where T : struct, IEvent
        {
            return Subscribe(callback, priority, id, true, 1);
        }

        /// <summary>
        /// 订阅事件（完整参数）
        /// </summary>
        public static string Subscribe<T>(Action<T> callback, int priority, string id, bool isOnce, int maxExecutionCount) 
            where T : struct, IEvent
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            lock (_lock)
            {
                var type = typeof(T);
                var wrapper = new EventSubscription<T>(callback, priority, id, isOnce, maxExecutionCount);

                if (!listeners.TryGetValue(type, out var obj))
                {
                    listeners[type] = new List<EventSubscription<T>> { wrapper };
                }
                else
                {
                    var list = obj as List<EventSubscription<T>>;
                    list.Add(wrapper);
                    list.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                }

                return wrapper.Id;
            }
        }

        #endregion

        #region 取消订阅

        /// <summary>
        /// 取消订阅（通过回调）
        /// </summary>
        public static bool Unsubscribe<T>(Action<T> callback) where T : struct, IEvent
        {
            if (callback == null) return false;

            lock (_lock)
            {
                var type = typeof(T);
                if (!listeners.TryGetValue(type, out var obj)) return false;

                var list = obj as List<EventSubscription<T>>;
                var removed = list.RemoveAll(s => s.Callback == callback);

                if (list.Count == 0)
                    listeners.Remove(type);

                return removed > 0;
            }
        }

        /// <summary>
        /// 取消订阅（通过ID）
        /// </summary>
        public static bool Unsubscribe<T>(string id) where T : struct, IEvent
        {
            if (string.IsNullOrEmpty(id)) return false;

            lock (_lock)
            {
                var type = typeof(T);
                if (!listeners.TryGetValue(type, out var obj)) return false;

                var list = obj as List<EventSubscription<T>>;
                var removed = list.RemoveAll(s => s.Id == id);

                if (list.Count == 0)
                    listeners.Remove(type);

                return removed > 0;
            }
        }

        /// <summary>
        /// 取消所有订阅
        /// </summary>
        public static void UnsubscribeAll<T>() where T : struct, IEvent
        {
            lock (_lock)
            {
                listeners.Remove(typeof(T));
            }
        }

        /// <summary>
        /// 清空所有
        /// </summary>
        public static void Clear()
        {
            lock (_lock)
            {
                listeners.Clear();
            }
        }

        #endregion

        #region 发布

        /// <summary>
        /// 发布事件
        /// </summary>
        public static void Publish<T>(T eventData) where T : struct, IEvent
        {
            var type = typeof(T);
            List<EventSubscription<T>> list;

            lock (_lock)
            {
                if (!listeners.TryGetValue(type, out var obj)) return;
                list = (obj as List<EventSubscription<T>>).ToList();
            }

            var toRemove = new List<EventSubscription<T>>();

            foreach (var sub in list)
            {
                if (!sub.CanExecute()) continue;

                try
                {
                    sub.Callback?.Invoke(eventData);
                    sub.ExecutionCount++;
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"[EventSystem] {typeof(T).Name}: {ex.Message}");
                }

                if (sub.ShouldRemove())
                    toRemove.Add(sub);
            }

            if (toRemove.Count > 0)
            {
                lock (_lock)
                {
                    if (listeners.TryGetValue(type, out var obj))
                    {
                        var currentList = obj as List<EventSubscription<T>>;
                        currentList.RemoveAll(s => toRemove.Contains(s));
                        if (currentList.Count == 0)
                            listeners.Remove(type);
                    }
                }
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 是否有监听器
        /// </summary>
        public static bool HasListeners<T>() where T : struct, IEvent
        {
            lock (_lock)
            {
                return listeners.TryGetValue(typeof(T), out var obj) && 
                       (obj as List<EventSubscription<T>>).Count > 0;
            }
        }

        /// <summary>
        /// 获取监听器数量
        /// </summary>
        public static int GetListenerCount<T>() where T : struct, IEvent
        {
            lock (_lock)
            {
                return listeners.TryGetValue(typeof(T), out var obj) 
                    ? (obj as List<EventSubscription<T>>).Count 
                    : 0;
            }
        }

        /// <summary>
        /// 获取监听器信息
        /// </summary>
        public static List<EventListenerInfo> GetListenerInfos<T>() where T : struct, IEvent
        {
            lock (_lock)
            {
                if (!listeners.TryGetValue(typeof(T), out var obj)) 
                    return new List<EventListenerInfo>();

                var list = obj as List<EventSubscription<T>>;
                return list.Select(s => new EventListenerInfo
                {
                    Id = s.Id,
                    Priority = s.Priority,
                    IsOnce = s.IsOnce,
                    ExecutionCount = s.ExecutionCount,
                    MaxExecutionCount = s.MaxExecutionCount,
                    RegisterTime = s.RegisterTime
                }).ToList();
            }
        }

        #endregion
    }
}
