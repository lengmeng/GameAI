using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Navigation
{
    /// <summary>
    /// 基于Dirichlet的Vertex对象
    /// 必须具备有Vertex 标记 和collider 组件，可以是图元或网格，会最大的覆盖视为节点的区域
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