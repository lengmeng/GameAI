using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Navigation
{
    /// <summary>
    /// 基于Dirichlet的Graph
    /// </summary>
    public class GraphDirichlet : Graph
    {
        Dictionary<int, List<int>> objToVertex;

        /// <summary>
        /// 添加Vertex 
        /// </summary>
        /// <param name="report"></param>
        public void AddLocation(VertexReport report)
        {
            int objId = report.obj.GetInstanceID();
            if (!objToVertex.ContainsKey(objId))
            {
                objToVertex.Add(objId, new List<int>());
            }
            objToVertex[objId].Add(report.vertex);
        }

        public void RemoveLocation(VertexReport report)
        {
            int objId = report.obj.GetInstanceID();
            objToVertex[objId].Remove(report.vertex);
        }
    }
}