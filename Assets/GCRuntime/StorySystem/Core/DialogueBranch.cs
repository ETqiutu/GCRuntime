using System.Collections.Generic;
using UnityEngine;

namespace GCRuntime.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueBranch", menuName = "GCRuntime/Dialogue/Branch")]
    public class DialogueBranch : DialogueNode
    {
        
        [Header("分支选项")]
        /// <summary>
        /// 说话者
        /// </summary>
        public string Speaker;

        /// <summary>
        /// 说话内容
        /// </summary> 
        [TextArea(3, 10)] public string Content;

        /// <summary>
        /// 选择所有的子节点
        /// </summary> 
        [HideInInspector] public List<DialogueNode> Options = new List<DialogueNode>();
        
        /// <summary>
        /// 选项内容
        /// </summary>
        public List<string> SelectionContent = new List<string>();
        
        /// <summary>
        /// 等待选择判断
        /// </summary> 
        private bool _waitingForSelection = true;

        /// <summary>
        /// 选择的节点
        /// </summary> 
        private DialogueNode _selectedNode = null;

        /// <summary>
        /// 开始时调用
        /// </summary> 
        protected override void OnStart()
        {
            _waitingForSelection = true;
            _selectedNode = null;
        }

        /// <summary>
        /// 判断节点执行状态
        /// </summary>
        /// <returns></returns> 
        protected override DialogueNode OnUpdate()
        {
            if (_waitingForSelection)
                return this;
            
            if (_selectedNode != null)
            {
                Finish();
                return _selectedNode;
            }
            
            return this;
        }

        /// <summary>
        /// 结束时调用
        /// </summary> 
        protected override void OnStop() {}

        /// <summary>
        /// 选择节点
        /// </summary>
        /// <param name="index"></param>
        public void Select(int index)
        {
            if (!_waitingForSelection)
                return;
                
            if (index >= 0 && index < Options.Count)
            {
                _selectedNode = Options[index];
                _waitingForSelection = false;
            }
        }
    }
}