using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Navigation
{
    /// <summary>
    /// 用于图形化表现的基类
    /// </summary>
    public class Graph : MonoBehaviour
    {
        public GameObject vertexPrefab;
        protected List<Vertex> vertices;
        protected List<List<Vertex>> neighbours;
        protected List<List<float>> costs;
        void Start()
        {
            Load();
        }

        public virtual void Load()
        {

        }

        /// <summary>
        /// 获取图形的个数
        /// </summary>
        /// <returns></returns>
        public virtual int GetSize()
        {
            if (ReferenceEquals(vertices, null))
                return 0;
            return vertices.Count;
        }

        /// <summary>
        /// 通过位置获得距离该位置最近的顶点
        /// </summary>
        /// <param name="position">位置信息</param>
        /// <returns>最近的顶点</returns>
        public virtual Vertex GetNearestVertex(Vector3 position)
        {
            return null;
        }

        /// <summary>
        /// 实现通过顶点的ID获取相应的顶点对象
        /// </summary>
        /// <param name="id">顶点ID</param>
        /// <returns>顶点对象</returns>
        public virtual Vertex GetVertexObj(int id)
        {
            if (ReferenceEquals(vertices, null) || vertices.Count == 0)
                return null;

            if (id < 0 || id >= vertices.Count)
                return null;

            return vertices[id];
        }

        /// <summary>
        /// 索引指定顶点的相邻顶点
        /// </summary>
        /// <param name="v">指定顶点</param>
        /// <returns>相邻顶点集</returns>
        public virtual Vertex[] GetNeighbours(Vertex v)
        {
            if (ReferenceEquals(neighbours, null) || neighbours.Count == 0)
                return null;

            if (v.id < 0 || v.id >= neighbours.Count)
                return null;

            return neighbours[v.id].ToArray();
        }
    }
}