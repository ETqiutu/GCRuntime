using UnityEngine;

namespace GCRuntime.BTree
{
    /// <summary>
    /// 装饰节点：仅有一个子节点，为这个子节点的前置子节点
    /// </summary> 
    public abstract class DecoratorNode : BTNode
    {
        [HideInInspector] public BTNode Child;

        public override BTNode Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.Child = Child.Clone();
            return node;
        }
    }
}
