using System.Collections.Generic;

namespace GCRuntime.BTree
{   
    /// <summary>
    /// 组合节点：它可以由多个子节点构成
    /// </summary> 
    public abstract class CompositeNode : BTNode
    {
        public List<BTNode> Children = new List<BTNode>();
    }
}
