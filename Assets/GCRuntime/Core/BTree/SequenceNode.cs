namespace GCRuntime.BTree
{
    /// <summary>
    /// 顺序执行节点，返回Sucess的条件是所有的子节点按顺序遍历过去都是Success
    /// </summary> 
    public class SequenceNode : CompositeNode
    {
        private int Current;

        protected override void OnStart()
        {
            Current = 0;
        }

        protected override void OnStop()
        {
            
        }

        protected override BTState OnUpdate()
        {
            var child = Children[Current];
            switch (child.Update())
            {
                case BTState.Running:
                    return BTState.Running;
                case BTState.Failure:
                    return BTState.Failure;
                case BTState.Success:
                    Current ++;
                    break;
            }
            return Current == Children.Count ? BTState.Success : BTState.Running;
        }
    }
}
