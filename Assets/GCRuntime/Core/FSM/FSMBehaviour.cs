using UnityEngine;

namespace GCRuntime.FSM
{
    public abstract class FSMBehaviour<T> : MonoBehaviour where T : class
    {
        protected StateMachine<T> stateMachine;
        
        protected virtual void Awake()
        {
            stateMachine = new StateMachine<T>();
            stateMachine.Initialize(this as T, this);
            RegisterStates();
            SetInitialState();
        }
        
        /// <summary>
        /// 主逻辑不断更新
        /// </summary> 
        protected virtual void Update()
        {
            stateMachine.Update();
        }
        
        /// <summary>
        /// 物理不断更新
        /// </summary>
        protected virtual void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }
        
        /// <summary>
        /// 注册所有状态（子类实现）
        /// </summary>
        protected abstract void RegisterStates();
        
        /// <summary>
        /// 设置初始状态（子类实现）
        /// </summary>
        protected abstract void SetInitialState();
        
        /// <summary>
        /// 切换状态（便捷方法）
        /// </summary>
        protected void ChangeState<U>() where U : State<T>
        {
            stateMachine.ChangeState<U>();
        }
        
        /// <summary>
        /// 获取状态（便捷方法）
        /// </summary>
        protected U GetState<U>() where U : State<T>
        {
            return stateMachine.GetState<U>();
        }
    }
}
