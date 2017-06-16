using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Navigation
{
    /// <summary>
    /// 用于存储邻近点 具有价值属性 继承IComparable接口，使具有比较特性
    /// </summary>
    public class Edge : IComparable<Edge>
    {
        public float cost;
        public Vertex vertex;

        public Edge(Vertex vertex = null, float cost = 1f)
        {
            this.vertex = vertex;
            this.cost = cost;
        }

        /// <summary>
        /// 对比顶点价值
        /// </summary>
        /// <param name="other">对比的顶点</param>
        /// <returns>对比结果</returns>
        public int CompareTo(Edge other)
        {
            float result = cost - other.cost;
            int idA = vertex.GetInstanceID();
            int idB = other.vertex.GetInstanceID();

            if (idA == idB)
                return 0;

            return (int)result;
        }

        public bool Equals(Edge other)
        {
            return (other.vertex.id == this.vertex.id);
        }

        public override bool Equals(object obj)
        {
            Edge other = obj as Edge;
            return (other.vertex.id == this.vertex.id);
        }

        /// <summary>
        /// 获取哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.vertex.GetHashCode();
        }
    }
}