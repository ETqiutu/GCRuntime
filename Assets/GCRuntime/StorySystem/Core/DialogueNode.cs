using UnityEngine;

namespace GCRuntime.StorySystem
{
    public abstract class DialogueNode : ScriptableObject
    {
        [Header("节点基础信息")]
        public string Name;
        public string ID;
        public string Character; 
        public System.Action<DialogueNode> OnNodeEnter;
        public System.Action<DialogueNode> OnNodeExit;

        
        /// <summary>
        /// 进入节点时调用
        /// </summary>
        public virtual void Enter()
        {
            OnNodeEnter?.Invoke(this);
        }
        
        /// <summary>
        /// 执行节点逻辑（核心方法，由子类重写）
        /// </summary>
        public abstract void Execute();
        
        /// <summary>
        /// 退出节点时调用
        /// </summary>
        public virtual void Exit()
        {
            OnNodeExit?.Invoke(this);
        }
        
        /// <summary>
        /// 克隆节点
        /// </summary>
        public virtual DialogueNode Clone()
        {
            DialogueNode node = Instantiate(this);
            return node;
        }
    }
}
