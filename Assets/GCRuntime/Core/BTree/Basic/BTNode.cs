using UnityEngine;


namespace GCRuntime.BTree
{
    public abstract class BTNode : ScriptableObject
    {
        /// <summary>
        /// 当前节点状态
        /// </summary> 
        public BTState State = BTState.Running;

        /// <summary>
        /// 检测是否开始
        /// </summary>
        public bool Started = false;

        /// <summary>
        /// 主要更新逻辑
        /// </summary>
        /// <returns></returns> 
        public BTState Update()
        {
            if (!Started)
            {
                OnStart();
                Started = true;
            }
            State = OnUpdate();
            if (State == BTState.Failure || State == BTState.Success)
            {
                OnStop();
                Started = false;
            }
            return State;
        }

        /// <summary>
        /// 可重写声明周期方法 => 节点未初始化时调用
        /// </summary> 
        protected abstract void OnStart();

        /// <summary>
        /// 可重写的生命周期方法 => 节点删除时调用
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// 可重写的生命周期方法 => 节点不断的更新逻辑
        /// </summary>
        /// <returns></returns>
        protected abstract BTState OnUpdate();
    }
}
