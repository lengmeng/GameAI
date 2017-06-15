using GameAI.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.NavigationWithDirichlet
{
    /// <summary>
    /// 基于Dirichlet的Graph——先铺设好地上的导航网格，每个网格拥有Vertex标记和collider组件，通过获取网格，生成导航信息
    /// </summary>
    public class GraphDirichlet : Graph
    {
        Dictionary<int, List<int>> objToVertex;

        #region 继承接口
        protected override void Start()
        {
            base.Start();
            objToVertex = new Dictionary<int, List<int>>();
        }

        /// <summary>
        /// 读取已有的导航网格
        /// </summary>
        public override void Load()
        {
            Vertex[] verts = GameObject.FindObjectsOfType<Vertex>();
            vertices = new List<Vertex>(verts);

            for (int i = 0; i < vertices.Count; ++i)
            {
                VertexVisibility vv = vertices[i] as VertexVisibility;
                vv.id = i;
                vv.FindNeighbours(vertices);
            }
        }
        #endregion

        #region 外部接口

        /// <summary>
        /// 子物体检测碰撞 添加相应vertex 记录着一个对象与哪个导航点发生了碰撞
        /// （因为离开后就删除记录，所以同一时间应该只有一个记录）
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

        /// <summary>
        /// 子物体检测到碰撞结束 删除相应vertex
        /// </summary>
        /// <param name="report"></param>
        public void RemoveLocation(VertexReport report)
        {
            int objId = report.obj.GetInstanceID();
            objToVertex[objId].Remove(report.vertex);
        }

        /// <summary>
        /// 获取位置最近的导航点 通过遍历所有的导航点 获取最短距离的点（尴尬）
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
        /// 遍历与当前对象发生碰撞的导航网格，选取最近的一个
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Vertex GetNearestVertex(GameObject obj)
        {
            int objId = obj.GetInstanceID();
            Vector3 objPos = obj.transform.position;

            if (!objToVertex.ContainsKey(objId))
                return null;

            List<int> vertIds = objToVertex[objId];
            Vertex vertex = null;
            float dist = Mathf.Infinity;

            for(int i = 0; i < vertIds.Count; ++i)
            {
                int id = vertIds[i];
                Vertex v = vertices[id];
                Vector3 vPos = v.transform.position;
                float d = Vector3.Distance(objPos, vPos);

                if(d < dist)
                {
                    vertex = v;
                    dist = d;
                }
            }
            return vertex;
        }

        /// <summary>
        /// 获取指定导航点的邻近导航点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override Vertex[] GetNeighbours(Vertex v)
        {
            List<Edge> edges = v.neihbours;
            Vertex[] ns = new Vertex[edges.Count];

            for(int i = 0; i < edges.Count; ++i)
            {
                ns[i] = edges[i].vertex;
            }
            return ns;
        }

        public override Edge[] GetEdges(Vertex v)
        {
            return vertices[v.id].neihbours.ToArray();
        }
        #endregion

    }
}