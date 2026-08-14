using UnityEngine;

namespace GCRuntime.BTree
{
    /// <summary>
    /// 日志节点辅助开发者调试代码
    /// </summary> 
    public class DebuggerNode : ActionNode
    {
        public enum LogType
        {
            Info,
            Error,
            Warn
        }

        [Header("打印基本设置")]
        public string Message;

        public LogType logType = LogType.Info;

        protected override void OnStart()
        {
            switch (logType)
            {
                case LogType.Info:
                    Debug.Log($"[GCRuntime - Behaviour Tree] OnStart: {Message}");
                    break;
                case LogType.Error:
                    Debug.LogError($"[GCRuntime - Behaviour Tree] OnStart: {Message}");
                    break;
                case LogType.Warn:
                    Debug.LogWarning($"[GCRuntime - Behaviour Tree] OnStart: {Message}");
                    break;
            }
        }

        protected override void OnStop()
        {
            switch (logType)
            {
                case LogType.Info:
                    Debug.Log($"[GCRuntime - Behaviour Tree] OnStop: {Message}");
                    break;
                case LogType.Error:
                    Debug.LogError($"[GCRuntime - Behaviour Tree] OnStop: {Message}");
                    break;
                case LogType.Warn:
                    Debug.LogWarning($"[GCRuntime - Behaviour Tree] OnStop: {Message}");
                    break;
            }
        }

        protected override BTState OnUpdate()
        {
            switch (logType)
            {
                case LogType.Info:
                    Debug.Log($"[GCRuntime - Behaviour Tree] OnUpdate: {Message}");
                    break;
                case LogType.Error:
                    Debug.LogError($"[GCRuntime - Behaviour Tree] OnUpdate: {Message}");
                    break;
                case LogType.Warn:
                    Debug.LogWarning($"[GCRuntime - Behaviour Tree] OnUpdate: {Message}");
                    break;
            }
            return BTState.Success;
        }
    }
}
