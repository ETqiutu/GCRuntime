using UnityEngine;

namespace GCRuntime.BTree
{
    public abstract class BTNode : ScriptableObject
    {
        /// <summary>
        /// 当前节点状态
        /// </summary> 
        [HideInInspector] public BTState State = BTState.Running;

        /// <summary>
        /// 检测是否开始
        /// </summary>
        [HideInInspector] public bool Started = false;

        /// <summary>
        /// GUI标识
        /// </summary> 
        [HideInInspector] public string Guid;

        /// <summary>
        /// 节点在行为树编辑器中的位置
        /// </summary> 
        [HideInInspector] public Vector2 Position;

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

        /// <summary>
        /// 克隆节点
        /// </summary>
        /// <returns></returns>
        public virtual BTNode Clone()
        {
            return Instantiate(this);
        }
    }
}
