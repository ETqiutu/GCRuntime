using UnityEngine;

namespace GCRuntime.Dialogue
{
    public class DialogueRunner : MonoBehaviour
    {
        /// <summary>
        /// 运行目标
        /// </summary>
        [Header("行为树")] public DialogueTree Tree;
        
        public void Update()
        {
            if (Tree != null)
                Tree.Update();
        }
        
        /// <summary>
        /// 继续当前对话
        /// </summary>
        public void Continue()
        {
            if (Tree?.RunningNode is DialogueEntry entry)
            {
                entry.Continue();
            }
        }

        /// <summary>
        /// 选择分支选项
        /// </summary>
        public void Select(int index)
        {
            if (Tree?.RunningNode is DialogueBranch branch)
            {
                branch.Select(index);
            }
        }

        /// <summary>
        /// 获取当前对话内容
        /// </summary>
        public string GetCurrentContent()
        {
            if (Tree?.RunningNode is DialogueEntry entry)
                return entry.Content;
            return null;
        }

        /// <summary>
        /// 判断对话是否结束
        /// </summary>
        public bool IsFinished()
        {
            return Tree?.Status == Status.Waiting;
        }

        /// <summary>
        /// 获取当前运行的节点
        /// </summary>
        public DialogueNode GetCurrentNode()
        {
            return Tree?.RunningNode;
        }

        public void StartDialogue() => Tree?.OnTreeStart();
        public void StopDialogue() => Tree?.OnTreeStop();
    }
}