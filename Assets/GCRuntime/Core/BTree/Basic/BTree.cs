using UnityEngine;

namespace GCRuntime.BTree
{
    [CreateAssetMenu(fileName = "BTree", menuName = "GCRuntime/BTree")]
    public class BTree : ScriptableObject
    {
        /// <summary>
        /// 根节点
        /// </summary> 
        public BTNode Root;

        /// <summary>
        /// 当前树状态
        /// </summary>
        public BTState TreeState = BTState.Running;

        /// <summary>
        /// 得到更新逻辑
        /// </summary>
        /// <returns></returns>
        public BTState Update()
        {
            if (Root.State == BTState.Running)
                TreeState = Root.Update();
            return TreeState;
        }
    }
}
