using UnityEngine;

namespace GCRuntime.BTree
{
    /// <summary>
    /// 运行器
    /// </summary> <summary>
    /// 
    /// </summary>
    public class BTRunner : MonoBehaviour
    {
        [Header("行为树 - 运行器")]
        public BTree Tree;

        private void Start()
        {
            Tree = Tree.Clone();
        }

        public void Update()
        {
            Tree.Update();
        }
    }
}
