namespace GCRuntime.BTree
{
    /// <summary>
    /// 选择器节点：按顺序执行子节点，直到有一个返回Success或Running
    /// 类似于"或"逻辑，常用于优先级选择
    /// </summary>
    public class SelectorNode : CompositeNode
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
            for (int i = Current; i < Children.Count; i++)
            {
                var child = Children[i];
                BTState state = child.Update();

                switch (state)
                {
                    case BTState.Running:
                        Current = i;
                        return BTState.Running;
                    case BTState.Success:
                        Current = 0;
                        return BTState.Success;
                    case BTState.Failure:
                        Current = i + 1;
                        break;
                }
            }

            Current = 0;
            return BTState.Failure;
        }
    }
}