using UnityEngine;
using UnityEngine.Events;
using System;

namespace GCRuntime.Dialogue
{
    public abstract class DialogueNode : ScriptableObject
    {
        [Header("节点基础信息")]
        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 节点ID
        /// </summary> 
        public string ID;

        /// <summary>
        /// GUI标识
        /// </summary> 
        [HideInInspector] public string Guid;

        /// <summary>
        /// 节点在行为树编辑器中的位置
        /// </summary> 
        [HideInInspector] public Vector2 Position;

        /// <summary>
        /// 节点详细介绍
        /// </summary> 
        [TextArea] public string Description;

        [Header("节点事件")]
        /// <summary>
        /// 节点进入事件回调
        /// </summary>
        public UnityEvent<DialogueNode> OnNodeEnter;
        
        /// <summary>
        /// 节点退出事件回调
        /// </summary> 
        public UnityEvent<DialogueNode> OnNodeStop;

        [Header("运行时状态")]
        [HideInInspector] public bool IsStarted = false;
        [HideInInspector] public Status Status = Status.Waiting;

        /// <summary>
        /// 节点逻辑判断
        /// </summary>
        /// <returns></returns>
        public DialogueNode Execute()
        {
            if (!IsStarted)
            {
                OnNodeEnter?.Invoke(this);
                OnStart();
                IsStarted = true;
                Status = Status.Running;
            }
            
            DialogueNode currentNode = OnUpdate();
            
            if (Status != Status.Running)
            {
                OnNodeStop?.Invoke(this);
                OnStop();
                IsStarted = false;
            }
            
            return currentNode;
        }

        /// <summary>
        /// 节点内部开始时调用
        /// </summary> 
        protected abstract void OnStart();

        /// <summary>
        /// 主要用于验证节点的判断
        /// </summary>
        /// <returns></returns> 
        protected abstract DialogueNode OnUpdate();

        /// <summary>
        /// 节点内部结束时调用
        /// </summary> 
        protected abstract void OnStop();

        /// <summary>
        /// 节点结束时等待调用
        /// </summary>
        protected void Finish()
        {
            Status = Status.Waiting;
        }
    }
}