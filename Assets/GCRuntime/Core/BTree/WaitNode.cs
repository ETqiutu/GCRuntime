using UnityEngine;

namespace GCRuntime.BTree
{
    public class WaitNode : ActionNode
    {
        [Header("等待时间")]
        public float Duration = 1;

        private float StartTime;

        protected override void OnStart()
        {
            StartTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override BTState OnUpdate()
        {
            if (Time.time - StartTime > Duration)
            {
                return BTState.Success;
            }
            return BTState.Running; 
        }
    }
}