using System;
using System.Collections.Generic;

namespace GCRuntime.Event
{
    public static class EventExtensions
    {
        /// <summary>
        /// 批量订阅
        /// </summary>
        public static List<string> SubscribeAll<T>(this object _, IEnumerable<Action<T>> callbacks, int priority = 0) 
            where T : struct, IEvent
        {
            var ids = new List<string>();
            foreach (var cb in callbacks)
            {
                ids.Add(EventSystem.Subscribe(cb, priority));
            }
            return ids;
        }

        /// <summary>
        /// 批量取消订阅
        /// </summary>
        public static void UnsubscribeAll<T>(this object _, IEnumerable<Action<T>> callbacks) 
            where T : struct, IEvent
        {
            foreach (var cb in callbacks)
            {
                EventSystem.Unsubscribe(cb);
            }
        }

        /// <summary>
        /// 安全发布事件（带异常处理）
        /// </summary>
        public static void PublishSafe<T>(this object _, T eventData) where T : struct, IEvent
        {
            try
            {
                EventSystem.Publish(eventData);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[EventSystem] 发布事件异常: {ex.Message}");
            }
        }
    }
}