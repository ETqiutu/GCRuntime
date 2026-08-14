using System.Collections.Generic;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GCRuntime.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueTree", menuName = "GCRuntime/Dialogue/Tree")]
    public class DialogueTree : ScriptableObject
    {
        /// <summary>
        /// 开始节点
        /// </summary> 
        public DialogueNode Root;

        /// <summary>
        /// 正在运行节点
        /// </summary> 
        public DialogueNode RunningNode;

        /// <summary>
        /// 对话树运行状态
        /// </summary>
        public Status Status = Status.Waiting;

        /// <summary>
        /// 所有节点列表
        /// </summary> 
        public List<DialogueNode> Nodes = new List<DialogueNode>();

        /// <summary>
        /// 运行状态树
        /// </summary> 
        public virtual void Update()
        {
            if (Status != Status.Running || RunningNode == null)
                return;
            DialogueNode next = RunningNode.Execute();
            if (next != RunningNode && next != null && RunningNode.Status == Status.Waiting)
            {
                RunningNode = next;
            }

            if (next == null && RunningNode.Status == Status.Waiting)
                OnTreeStop();
        }

        /// <summary>
        /// 开始运行状态树
        /// </summary> 
        public virtual void OnTreeStart()
        {
            Status = Status.Running;
            RunningNode = Root;
        }

        /// <summary>
        /// 结束运行状态树
        /// </summary>
        public virtual void OnTreeStop()
        {
            Status = Status.Waiting;
            RunningNode = null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns> 
        public DialogueNode CreateNode(Type type)
        {
            DialogueNode node = ScriptableObject.CreateInstance(type) as DialogueNode;
            node.name = type.Name;
            node.Guid = GUID.Generate().ToString();
            Nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }


        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param> 
        public void DeleteNode(DialogueNode node)
        {
            Nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param> 
        public void AddChild(DialogueNode parent, DialogueNode child)
        {
            DialogueEntry entry = parent as DialogueEntry;
            if (entry != null)
            {
                entry.NextNode = child;
            }
            DialogueBranch branch = parent as DialogueBranch;
            if (branch != null)
            {
                branch.Options.Add(child);
                branch.SelectionContent.Add(string.Empty);
            }
            DialogueRoot root = parent as DialogueRoot;
            if (root != null)
            {
                root.NextNode = child;
            }
        }

        /// <summary>
        /// 移除子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param> 
        public void RemoveChild(DialogueNode parent, DialogueNode child)
        {
            DialogueEntry entry = parent as DialogueEntry;
            if (entry != null)
            {
                entry.NextNode = null;
            }
            DialogueBranch branch = parent as DialogueBranch;
            if (branch != null)
            {
                branch.Options.Remove(child);
            }
            DialogueRoot root = parent as DialogueRoot;
            if (root != null)
            {
                root.NextNode = null;
            }
        }

        /// <summary>
        /// 得到父节点下的所有子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns> 
        public List<DialogueNode> GetChildren(DialogueNode parent)
        {
            List<DialogueNode> nodes = new List<DialogueNode>();
            DialogueEntry entry = parent as DialogueEntry;
            if (entry != null && entry.NextNode != null)
            {
                nodes.Add(entry.NextNode);
            }
            DialogueBranch branch = parent as DialogueBranch;
            if (branch != null && branch.Options != null && branch.Options.Count > 0)
            {
                return branch.Options;
            }
            DialogueRoot root = parent as DialogueRoot;
            if (root != null && root.NextNode != null)
            {
                nodes.Add(root.NextNode);
            }
            return nodes;
        }
#endif
    }
}