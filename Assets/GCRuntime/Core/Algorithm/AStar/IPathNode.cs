using System.Collections.Generic;
using UnityEngine;

namespace GCRuntime.Algorithm
{
    public interface IPathNode
    {
        public Vector3 Position { get; }  
        public bool IsWalkable { get; }   
        public float GetCost(IPathNode neighbor); 
        public float GetHeuristic(IPathNode target); 
        public List<IPathNode> GetNeighbors(); 
    }
}