using System.Collections.Generic;
using UnityEngine;

namespace GCRuntime.BTree
{   
    /// <summary>
    /// 组合节点：它可以由多个子节点构成
    /// </summary> 
    public abstract class CompositeNode : BTNode
    {
        [HideInInspector] public List<BTNode> Children = new List<BTNode>();

        public override BTNode Clone()
        {
            CompositeNode node = Instantiate(this);
            node.Children = Children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}
