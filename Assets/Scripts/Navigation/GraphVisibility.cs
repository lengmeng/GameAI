using GameAI.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.NavigationWithVisibility
{
    public class GraphVisibility : Graph
    {
        /// <summary>
        /// 读取所有加载Vertex的组件的对象
        /// </summary>
        public override void Load()
        {
            Vertex[] verts = GameObject.FindObjectsOfType<Vertex>();
            vertices = new List<Vertex>(verts);
            for(int i = 0; i < vertices.Count; ++i)
            {
                VertexVisibility vv = vertices[i] as VertexVisibility;
                vv.id = i;
                vv.FindNeighbours(vertices);
            }
        }

        /// <summary>
        /// 获取指定坐标最近的顶点
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Vertex GetNearestVertex(Vector3 position)
        {
            Vertex vertex = null;
            float dist = Mathf.Infinity;
            float distNear = dist;
            Vector3 posVertex = Vector3.zero;
            for(int i = 0; i < vertices.Count; ++i)
            {
                posVertex = vertices[i].transform.position;
                dist = Vector3.Distance(position, posVertex);
                if(dist < distNear)
                {
                    distNear = dist;
                    vertex = vertices[i];
                }
            }
            return vertex;
        }

        /// <summary>
        /// 获取相邻顶点 通过遍历指定顶点相邻的边
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override Vertex[] GetNeighbours(Vertex v)
        {
            List<Edge> edges = v.neighbours;
            Vertex[] ns = new Vertex[edges.Count];

            for(int i = 0; i < edges.Count; ++i)
            {
                ns[i] = edges[i].vertex;
            }
            return ns;
        }

        /// <summary>
        /// 获取顶点邻边
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override Edge[] GetEdges(Vertex v)
        {
            return vertices[v.id].neighbours.ToArray();
        }
    }
}