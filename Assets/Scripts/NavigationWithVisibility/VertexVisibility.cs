using GameAI.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.NavigationWithVisibility
{
    /// <summary>
    /// 基于可视化检测的顶点 
    /// </summary>
    public class VertexVisibility : Vertex
    {
        void Awake()
        {
            neighbours = new List<Edge>();
        }
        
        /// <summary>
        /// 使用射线物理检测自身与其他所有顶点的可视度，如可视则视为向量顶点
        /// </summary>
        /// <param name="vertices"></param>
        public void FindNeighbours(List<Vertex> vertices)
        {
            Collider c = gameObject.GetComponent<Collider>();
            c.enabled = false;
            Vector3 direction = Vector3.zero;
            Vector3 origin = transform.position;
            Vector3 target = Vector3.zero;

            RaycastHit[] hits;
            Ray ray;
            float distance = 0f;
            for(int i = 0; i < vertices.Count; ++i)
            {
                if (vertices[i] == this)
                    continue;
                target = vertices[i].transform.position;
                direction = target - origin;
                distance = direction.magnitude;
                ray = new Ray(origin, direction);
                hits = Physics.RaycastAll(ray, distance);
                if(hits.Length == 1 && hits[0].collider.gameObject.tag.Equals("Vertex"))
                {
                    Edge e = new Edge();
                    e.cost = distance;
                    GameObject go = hits[0].collider.gameObject;
                    Vertex v = go.GetComponent<Vertex>();
                    if (v != vertices[i])
                        continue;
                    e.vertex = v;
                    neighbours.Add(e);
                }
            }
            c.enabled = true;
        }
    }
}