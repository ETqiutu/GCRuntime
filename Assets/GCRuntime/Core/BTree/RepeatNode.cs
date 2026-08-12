using UnityEngine;

namespace GCRuntime.BTree
{
    /// <summary>
    /// 循环节点，无论如何返回当前节点正在运行
    /// </summary> 
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }

        protected override BTState OnUpdate()
        {
            Child.Update();
            return BTState.Running;
        }
    }
}
