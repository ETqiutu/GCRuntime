namespace GCRuntime.BTree
{
    /// <summary>
    /// 装饰节点：仅有一个子节点，为这个子节点的前置子节点
    /// </summary> 
    public abstract class DecoratorNode : BTNode
    {
        public BTNode Child;
    }
}
