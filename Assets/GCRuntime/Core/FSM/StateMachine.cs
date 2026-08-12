using System;
using System.Collections.Generic;
using UnityEngine;

namespace GCRuntime.FSM
{
    public class StateMachine<T> where T : class
    {
        private Dictionary<Type, State<T>> states = new Dictionary<Type, State<T>>();
        private State<T> currentState;
        private State<T> previousState;
        private State<T> globalState;
        
        private T owner;
        private MonoBehaviour monoBehaviour;
        
        /// <summary>
        /// 当前状态
        /// </summary>
        public State<T> CurrentState => currentState;
        
        /// <summary>
        /// 上一个状态
        /// </summary>
        public State<T> PreviousState => previousState;
        
        /// <summary>
        /// 初始化状态机
        /// </summary>
        public void Initialize(T owner, MonoBehaviour monoBehaviour)
        {
            this.owner = owner;
            this.monoBehaviour = monoBehaviour;
        }
        
        /// <summary>
        /// 注册状态
        /// </summary>
        public void RegisterState(State<T> state)
        {
            Type stateType = state.GetType();
            if (!states.ContainsKey(stateType))
            {
                state.Initialize(owner, monoBehaviour);
                states.Add(stateType, state);
            }
            else
            {
                Debug.LogWarning($"[GCRuntime]: 状态 {stateType.Name} 已存在，将被覆盖");
                states[stateType] = state;
            }
        }
        
        /// <summary>
        /// 批量注册状态
        /// </summary>
        public void RegisterStates(params State<T>[] stateList)
        {
            foreach (var state in stateList)
            {
                RegisterState(state);
            }
        }
        
        /// <summary>
        /// 切换状态
        /// </summary>
        public void ChangeState<U>() where U : State<T>
        {
            ChangeState(typeof(U));
        }
        
        /// <summary>
        /// 切换状态（通过类型）
        /// </summary>
        public void ChangeState(Type stateType)
        {
            if (!states.TryGetValue(stateType, out State<T> newState))
            {
                Debug.LogError($"状态 {stateType.Name} 未注册！");
                return;
            }
            
            if (currentState == newState)
                return;
            currentState?.OnExit();
            previousState = currentState;
            currentState = newState;
            currentState.OnEnter();
        }
        
        /// <summary>
        /// 设置全局状态（在任何状态下都会执行）
        /// </summary>
        public void SetGlobalState(State<T> state)
        {
            if (globalState != null)
            {
                globalState.OnExit();
            }
            
            globalState = state;
            globalState.Initialize(owner, monoBehaviour);
            globalState.OnEnter();
        }
        
        /// <summary>
        /// 更新（由MonoBehaviour的Update调用）
        /// </summary>
        public void Update()
        {
            globalState?.OnUpdate();
            currentState?.OnUpdate();
        }
        
        /// <summary>
        /// 固定更新（由MonoBehaviour的FixedUpdate调用）
        /// </summary>
        public void FixedUpdate()
        {
            globalState?.OnFixedUpdate();
            currentState?.OnFixedUpdate();
        }
        
        /// <summary>
        /// 回退到上一个状态
        /// </summary>
        public void RevertToPreviousState()
        {
            if (previousState != null)
            {
                ChangeState(previousState.GetType());
            }
        }
        
        /// <summary>
        /// 获取指定类型状态
        /// </summary>
        public U GetState<U>() where U : State<T>
        {
            Type type = typeof(U);
            if (states.TryGetValue(type, out State<T> state))
            {
                return state as U;
            }
            return null;
        }
        
        /// <summary>
        /// 检查是否处于某状态
        /// </summary>
        public bool IsInState<U>() where U : State<T>
        {
            return currentState is U;
        }
    }
}
