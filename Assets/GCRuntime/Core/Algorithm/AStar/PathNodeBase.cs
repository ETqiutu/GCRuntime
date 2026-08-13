using System.Collections.Generic;
using UnityEngine;

namespace GCRuntime.Algorithm
{
    public abstract class PathNodeBase : IPathNode
    {
        public abstract Vector3 Position { get; }
        public abstract bool IsWalkable { get; set; }
        
        // 默认使用欧几里得距离作为代价
        public virtual float GetCost(IPathNode neighbor)
        {
            return Vector3.Distance(Position, neighbor.Position);
        }

        // 默认使用曼哈顿距离作为启发式
        public virtual float GetHeuristic(IPathNode target)
        {
            return Vector3.Distance(Position, target.Position);
        }

        public abstract List<IPathNode> GetNeighbors();
    }
}
