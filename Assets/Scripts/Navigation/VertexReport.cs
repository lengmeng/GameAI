using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI.NavigationWithDirichlet
{
    /// <summary>
    /// Vertex反馈，用于传递Vertex的id和GameObject对象
    /// </summary>
    public class VertexReport
    {
        public int vertex;
        public GameObject obj;
        
        public VertexReport(int vertexId, GameObject obj)
        {
            vertex = vertexId;
            this.obj = obj;
        }
    }
}