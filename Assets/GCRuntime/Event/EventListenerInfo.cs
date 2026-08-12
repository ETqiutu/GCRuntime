using System;

namespace GCRuntime.Event
{
    /// <summary>
    /// 事件信息，无重要作用获取信息使用的数据结构
    /// </summary> 
    public class EventListenerInfo
    {
        public string Id { get; internal set; }
        public int Priority { get; internal set; }
        public bool IsOnce { get; internal set; }
        public int ExecutionCount { get; internal set; }
        public int MaxExecutionCount { get; internal set; }
        public DateTime RegisterTime { get; internal set; }

        public override string ToString()
        {
            return $"[{Id}] Priority:{Priority} Once:{IsOnce} Exec:{ExecutionCount}/{MaxExecutionCount}";
        }
    }
}
