using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GCRuntime.BTree
{
    [CreateAssetMenu(fileName = "BTree", menuName = "GCRuntime/BTree")]
    public class BTree : ScriptableObject
    {
        /// <summary>
        /// 根节点
        /// </summary> 
        public BTNode Root;

        /// <summary>
        /// 当前树状态
        /// </summary>
        public BTState TreeState = BTState.Running;

        /// <summary>
        /// 所有的节点列表
        /// </summary> 
        public List<BTNode> Nodes = new List<BTNode>();

        /// <summary>
        /// 得到更新逻辑
        /// </summary>
        /// <returns></returns>
        public BTState Update()
        {
            if (Root.State == BTState.Running)
                TreeState = Root.Update();
            return TreeState;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns> 
        public BTNode CreateNode(Type type)
        {
            BTNode node = ScriptableObject.CreateInstance(type) as BTNode;
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
        public void DeleteNode(BTNode node)
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
        public void AddChild(BTNode parent, BTNode child)
        {
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                decorator.Child = child;
            }
            RootNode root = parent as RootNode;
            if (root != null)
            {
                root.Child = child;
            }
            CompositeNode composite = parent as CompositeNode;
            if (composite != null)
            {
                composite.Children.Add(child);
            }
        }

        /// <summary>
        /// 移除子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param> 
        public void RemoveChild(BTNode parent, BTNode child)
        {
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                decorator.Child = null;
            }
            RootNode root = parent as RootNode;
            if (root != null)
            {
                root.Child = null;
            }
            CompositeNode composite = parent as CompositeNode;
            if (composite != null)
            {
                composite.Children.Remove(child);
            }
        }

        /// <summary>
        /// 得到父节点下的所有子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns> 
        public List<BTNode> GetChildren(BTNode parent)
        {
            List<BTNode> bTNodes = new List<BTNode>();
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null && decorator.Child != null)
            {
                bTNodes.Add(decorator.Child);
            }
            RootNode root = parent as RootNode;
            if (root != null && root.Child != null)
            {
                bTNodes.Add(root.Child);
            }
            CompositeNode composite = parent as CompositeNode;
            if (composite != null)
            {
                return composite.Children;
            }
            return bTNodes;
        }
#endif

        public BTree Clone()
        {
            BTree tree = Instantiate(this);
            tree.Root = tree.Root.Clone();
            return tree;
        }
    }
}
