namespace GCRuntime.BTree
{
    /// <summary>
    /// 反转节点：将子节点的成功/失败结果反转
    /// Running状态保持不变
    /// </summary>
    public class InverterNode : DecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override BTState OnUpdate()
        {
            BTState state = Child.Update();

            switch (state)
            {
                case BTState.Success:
                    return BTState.Failure;
                case BTState.Failure:
                    return BTState.Success;
                case BTState.Running:
                    return BTState.Running;
                default:
                    return BTState.Failure;
            }
        }
    }
}