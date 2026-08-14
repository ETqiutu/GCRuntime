using UnityEngine;

namespace GCRuntime.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueEntry", menuName = "GCRuntime/Dialogue/Entry")]
    public class DialogueEntry : DialogueNode
    {
        [Header("对话内容")]
        /// <summary>
        /// 说话者
        /// </summary>
        public string Speaker;

        /// <summary>
        /// 说话内容
        /// </summary> 
        [TextArea(3, 10)] public string Content;

        /// <summary>
        /// 下一个节点
        /// </summary> 
        [HideInInspector] public DialogueNode NextNode;
        
        /// <summary>
        /// 是否进行下一个节点
        /// </summary>
        private bool _waitingForInput = true;

        /// <summary>
        /// 开始时调用
        /// </summary> 
        protected override void OnStart()
        {
            _waitingForInput = true;
        }

        /// <summary>
        /// 判断节点运行状态
        /// </summary>
        /// <returns></returns> 
        protected override DialogueNode OnUpdate()
        {
            if (_waitingForInput)
                return this;
            
            Finish();
            return NextNode;
        }

        /// <summary>
        /// 节点结束时调用
        /// </summary> 
        protected override void OnStop() {}

        /// <summary>
        /// 外部调用：进入下一个节点
        /// </summary> 
        public void Continue()
        {
            if (_waitingForInput)
            {
                _waitingForInput = false;
            }
        }
    }
}