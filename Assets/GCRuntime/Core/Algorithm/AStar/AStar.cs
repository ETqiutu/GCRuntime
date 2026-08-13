using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GCRuntime.Algorithm
{
    /// <summary>
    /// A* 寻路算法核心
    /// </summary>
    public class AStarPathfinder
    {
        private readonly PathfindingConfig config;

        public AStarPathfinder(PathfindingConfig config = null)
        {
            this.config = config ?? new PathfindingConfig();
        }

        /// <summary>
        /// 寻找路径，返回路径点列表
        /// </summary>
        public List<IPathNode> FindPath(IPathNode start, IPathNode target)
        {
            if (!start.IsWalkable || !target.IsWalkable)
                return null;

            var openSet = new PriorityQueue<IPathNode>();
            var cameFrom = new Dictionary<IPathNode, IPathNode>();
            var gScore = new Dictionary<IPathNode, float>();
            var fScore = new Dictionary<IPathNode, float>();

            openSet.Enqueue(start, 0);
            gScore[start] = 0;
            fScore[start] = start.GetHeuristic(target);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                // 到达目标
                if (current.Equals(target))
                {
                    return ReconstructPath(cameFrom, current);
                }

                foreach (var neighbor in current.GetNeighbors())
                {
                    if (!neighbor.IsWalkable) continue;

                    float tentativeGScore = gScore[current] + current.GetCost(neighbor);
                    
                    // 应用移动惩罚（如倾斜惩罚）
                    if (config.MovementPenalty != null)
                    {
                        tentativeGScore += config.MovementPenalty(current, neighbor);
                    }

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        float heuristic = neighbor.GetHeuristic(target);
                        
                        // 应用启发式权重
                        if (config.HeuristicWeight != 1.0f)
                        {
                            heuristic *= config.HeuristicWeight;
                        }
                        
                        fScore[neighbor] = gScore[neighbor] + heuristic;
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }

                // 检查是否超出迭代限制
                if (config.MaxIterations > 0 && openSet.Count > config.MaxIterations)
                {
                    Debug.LogWarning($"A* 寻路超出最大迭代次数: {config.MaxIterations}");
                    return null;
                }
            }

            // 没有找到路径
            return null;
        }

        /// <summary>
        /// 寻找路径并返回世界坐标点列表
        /// </summary>
        public List<Vector3> FindPathWorld(IPathNode start, IPathNode target)
        {
            var path = FindPath(start, target);
            if (path == null) return null;
            
            return path.Select(node => node.Position).ToList();
        }

        /// <summary>
        /// 重建路径
        /// </summary>
        private List<IPathNode> ReconstructPath(Dictionary<IPathNode, IPathNode> cameFrom, IPathNode current)
        {
            var path = new List<IPathNode> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }
            return path;
        }

        /// <summary>
        /// 平滑路径（可选后处理）
        /// </summary>
        public List<Vector3> SmoothPath(List<Vector3> path, float smoothingRadius = 0.5f, int iterations = 5)
        {
            if (path == null || path.Count < 3) return path;

            var smoothed = new List<Vector3>(path);
            
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 1; i < smoothed.Count - 1; i++)
                {
                    Vector3 newPos = smoothed[i];
                    Vector3 left = smoothed[i - 1];
                    Vector3 right = smoothed[i + 1];
                    
                    // 向两侧拉平
                    newPos += (left - newPos) * 0.25f;
                    newPos += (right - newPos) * 0.25f;
                    
                    // 限制移动距离
                    float maxMove = smoothingRadius;
                    float distance = Vector3.Distance(smoothed[i], newPos);
                    if (distance > maxMove)
                    {
                        newPos = smoothed[i] + (newPos - smoothed[i]).normalized * maxMove;
                    }
                    
                    smoothed[i] = newPos;
                }
            }
            
            return smoothed;
        }

        /// <summary>
        /// 路径点简化（删除共线点）
        /// </summary>
        public List<Vector3> SimplifyPath(List<Vector3> path, float tolerance = 0.1f)
        {
            if (path == null || path.Count < 3) return path;

            var simplified = new List<Vector3> { path[0] };
            
            for (int i = 1; i < path.Count - 1; i++)
            {
                Vector3 prev = path[i - 1];
                Vector3 curr = path[i];
                Vector3 next = path[i + 1];
                
                // 检查是否共线
                Vector3 dir1 = (curr - prev).normalized;
                Vector3 dir2 = (next - curr).normalized;
                float angle = Vector3.Angle(dir1, dir2);
                
                if (angle > tolerance)
                {
                    simplified.Add(curr);
                }
            }
            
            simplified.Add(path[path.Count - 1]);
            return simplified;
        }
    }

    /// <summary>
    /// 寻路配置
    /// </summary>
    public class PathfindingConfig
    {
        public float HeuristicWeight = 1.0f; // 启发式权重（<1 更准确但慢，>1 更快但不保证最优）
        public int MaxIterations = 10000; // 最大迭代次数，防止死循环
        public Func<IPathNode, IPathNode, float> MovementPenalty; // 自定义移动惩罚函数

        public PathfindingConfig()
        {
            // 默认对角线移动有额外惩罚
            MovementPenalty = (current, neighbor) =>
            {
                Vector3 diff = neighbor.Position - current.Position;
                if (Mathf.Abs(diff.x) > 0.1f && Mathf.Abs(diff.z) > 0.1f)
                {
                    return 0.414f; // 对角线惩罚 (√2 - 1)
                }
                return 0f;
            };
        }
    }

    /// <summary>
    /// 优先队列（使用二叉堆）
    /// </summary>
    public class PriorityQueue<T>
    {
        private readonly List<(T item, float priority)> heap = new List<(T, float)>();

        public int Count => heap.Count;

        public void Enqueue(T item, float priority)
        {
            heap.Add((item, priority));
            int i = heap.Count - 1;
            
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (heap[parent].priority <= heap[i].priority)
                    break;
                
                Swap(i, parent);
                i = parent;
            }
        }

        public T Dequeue()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("队列为空");

            T result = heap[0].item;
            int lastIndex = heap.Count - 1;
            heap[0] = heap[lastIndex];
            heap.RemoveAt(lastIndex);
            
            int i = 0;
            while (true)
            {
                int left = i * 2 + 1;
                int right = i * 2 + 2;
                int smallest = i;

                if (left < heap.Count && heap[left].priority < heap[smallest].priority)
                    smallest = left;
                if (right < heap.Count && heap[right].priority < heap[smallest].priority)
                    smallest = right;
                
                if (smallest == i)
                    break;
                
                Swap(i, smallest);
                i = smallest;
            }

            return result;
        }

        public T Peek()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("队列为空");
            return heap[0].item;
        }

        private void Swap(int i, int j)
        {
            var temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }
}
