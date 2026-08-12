using UnityEngine;

namespace GCRuntime.FSM
{
    public abstract class State<T> where T : class
    {
        protected T Machine;
        protected MonoBehaviour Owner;

        public virtual string StateName => GetType().Name;

        public virtual void Initialize(T Machine, MonoBehaviour Owner)
        {
            this.Machine = Machine;
            this.Owner = Owner;
        }

        /// <summary>
        /// 进入状态时调用
        /// </summary>
        public virtual void OnEnter() { }
        
        /// <summary>
        /// 每帧更新
        /// </summary>
        public virtual void OnUpdate() { }
        
        /// <summary>
        /// 固定频率更新（物理）
        /// </summary>
        public virtual void OnFixedUpdate() { }
        
        /// <summary>
        /// 离开状态时调用
        /// </summary>
        public virtual void OnExit() { }
    }
}
