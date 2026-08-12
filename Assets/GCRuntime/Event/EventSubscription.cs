using System;

namespace GCRuntime.Event
{
    /// <summary>
    /// 优先事件封装
    /// </summary>
    /// <typeparam name="T"></typeparam> 
    internal class EventSubscription<T> where T : struct, IEvent
    {
        /// <summary>
        /// 优先事件回调，在IEvent结构里定义传入参数
        /// </summary> 
        /// <value></value>
        public Action<T> Callback { get; private set; }

        /// <summary>
        /// 事件参数，可以使用事先枚举类型
        /// </summary> 
        /// <value></value>
        public int Priority { get; set; }

        /// <summary>
        /// 优先事件ID方便用于事件索引与调用
        /// </summary> 
        /// <value></value>
        public string Id { get; private set; }

        /// <summary>
        /// 是否为单次优先事件
        /// </summary> 
        /// <value></value>
        public bool IsOnce { get; private set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        /// <value></value>
        public int ExecutionCount { get; set; }

        /// <summary>
        /// 最大可执行次数
        /// </summary> 
        /// <value></value>
        public int MaxExecutionCount { get; set; }

        /// <summary>
        /// 注册
        /// </summary> 
        /// <value></value>
        public DateTime RegisterTime { get; private set; }

        /// <summary>
        /// 构造函数，仅在注册时初始化
        /// </summary>
        /// <param name="callback">事件回调</param>
        /// <param name="priority">时间</param>
        /// <param name="id">事件ID</param>
        /// <param name="isOnce">是否为单次事件如成就等</param>
        /// <param name="maxExecutionCount">最大执行次数</param>
        public EventSubscription(Action<T> callback, int priority, string id, bool isOnce, int maxExecutionCount)
        {
            Callback = callback;
            Priority = priority;
            Id = id ?? Guid.NewGuid().ToString();
            IsOnce = isOnce;
            MaxExecutionCount = maxExecutionCount;
            ExecutionCount = 0;
            RegisterTime = DateTime.Now;
        }

        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <returns></returns> 
        public bool CanExecute()
        {
            return MaxExecutionCount == 0 || ExecutionCount < MaxExecutionCount;
        }

        /// <summary>
        /// 是否应该移除事件列表
        /// </summary>
        /// <returns></returns> 
        public bool ShouldRemove()
        {
            return IsOnce || (MaxExecutionCount > 0 && ExecutionCount >= MaxExecutionCount);
        }
    }
}
