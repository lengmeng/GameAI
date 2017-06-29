using GameAi.Tools;
using GameAI.TacticsSystem.Formation;
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

        protected virtual void Start()
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

        /// <summary>
        /// 获取相邻导航点的比较对象
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public virtual Edge[] GetEdges(Vertex v)
        {
            return null;
        }

        /// <summary>
        /// 创建路径
        /// </summary>
        /// <param name="srcId">起点ID</param>
        /// <param name="dstId">终点ID</param>
        /// <param name="prevList">顶点树型层级结构数据</param>
        /// <returns></returns>
        private List<Vertex> BuildPath(int srcId, int dstId, ref int[] prevList)
        {
            List<Vertex> path = new List<Vertex>();
            int prev = dstId;
            do
            {
                path.Add(vertices[prev]);
                prev = prevList[prev];
            } while (prev != srcId);
            return path;
        }

        /// <summary>
        /// 深度优先算法寻找路径 使用堆栈优先遍历最右边节点的子节点
        /// </summary>
        /// <param name="srcObj">起点</param>
        /// <param name="dstObj">终点</param>
        /// <returns>路径列表</returns>
        public List<Vertex> GetPathDFS(GameObject srcObj, GameObject dstObj)
        {
            if (srcObj == null || dstObj == null) return new List<Vertex>();
            // 获取与指定位置最近的导航点
            Vertex src = GetNearestVertex(srcObj.transform.position);
            Vertex dst = GetNearestVertex(dstObj.transform.position);

            Vertex[] neighbours;
            Vertex v;
            int[] previous = new int[vertices.Count]; // 用于记录是否已经被遍历过
            for (int i = 0; i < previous.Length; ++i)
                previous[i] = -1;

            previous[src.id] = src.id;
            Stack<Vertex> s = new Stack<Vertex>();
            s.Push(src);

            while (s.Count != 0)
            {
                v = s.Pop();
                if (ReferenceEquals(v, dst))
                    return BuildPath(src.id, v.id, ref previous);

                neighbours = GetNeighbours(v);
                foreach (Vertex n in neighbours)
                {
                    if (previous[n.id] != -1)
                        continue;
                    previous[n.id] = v.id; // 标记节点N的父节点是V
                    s.Push(n);
                }
            }

            return new List<Vertex>();
        }

        /// <summary>
        /// 广度遍历寻路 使用FIFO队列优先遍历兄弟节点
        /// </summary>
        /// <param name="srcObj"></param>
        /// <param name="dstObj"></param>
        /// <returns></returns>
        public List<Vertex> GetPathBFS(GameObject srcObj, GameObject dstObj)
        {
            if (srcObj == null && dstObj == null) return new List<Vertex>();

            Vertex[] neighbours;
            Queue<Vertex> q = new Queue<Vertex>();
            Vertex src = GetNearestVertex(srcObj.transform.position);
            Vertex dst = GetNearestVertex(dstObj.transform.position);
            Vertex v;
            int[] previous = new int[vertices.Count];
            for (int i = 0; i < previous.Length; ++i)
                previous[i] = -1;
            previous[src.id] = src.id;
            q.Enqueue(src);

            while (q.Count != 0)
            {
                v = q.Dequeue();
                if (ReferenceEquals(v, dst))
                    return BuildPath(src.id, v.id, ref previous);

                neighbours = GetNeighbours(v);
                foreach (Vertex n in neighbours)
                {
                    if (previous[n.id] != -1)
                        continue;
                    previous[n.id] = v.id; // 标记节点N的父节点是V
                    q.Enqueue(n);
                }
            }

            return new List<Vertex>();
        }

        #region 使用Dijkstra算法进行路径查询
        // 存储通过Dijkstra算法得到的计算结果
        List<int[]> routes = new List<int[]>();

        /// <summary>
        /// 使用Dijkstra算法进行寻路
        /// </summary>
        /// <param name="stcObj"></param>
        /// <param name="dstObj"></param>
        /// <returns></returns>
        public List<Vertex> GetPathDijkstra(GameObject srcObj, GameObject dstObj)
        {
            List<Vertex> path = new List<Vertex>();
            Vertex src = GetNearestVertex(srcObj.transform.position);
            Vertex dst = GetNearestVertex(dstObj.transform.position);
            int[] previous = routes[dst.id];
            return BuildPath(src.id, dst.id, ref previous);
        }

        /// <summary>
        /// 计算并生成所有点的路径树
        /// </summary>
        public void DijkstraProcessing()
        {
            int size = GetSize();
            for (int i = 0; i < size; ++i)
            {
                GameObject go = vertices[i].gameObject;
                routes.Add(Dijkstra(go));
            }
        }

        /// <summary>
        /// 指定对象为源节点，获得其到其他所有节点的最佳导航数据。
        /// 使用最小二叉堆作为缓存队列，从起点开始，将相邻节点排成最小二叉堆，然后获取
        /// 二叉堆顶点（最小值），再一次遍历相邻节点，补充/更新现有二叉堆，直到找到终点。
        /// 看起来有点像BFS算法，但是区别在于Dijkstra算法一次计算后可以得到从源节点到所
        /// 有点的最佳路径。
        /// </summary>
        /// <param name="srcObj"></param>
        /// <returns></returns>
        public int[] Dijkstra(GameObject srcObj)
        {
            if (srcObj == null) return null;

            Vertex src = GetNearestVertex(srcObj.transform.position);

            BinaryHeap<Edge> frontier = new BinaryHeap<Edge>(); // 二叉堆
            Edge[] edges;
            Edge node, child;
            int size = vertices.Count;
            float[] distValue = new float[size];    // 距离列表
            int[] previous = new int[size];         // 层级关系列表
            // 将源节点添加到二叉堆
            node = new Edge(src, 0);
            frontier.Add(node);
            distValue[src.id] = 0;
            previous[src.id] = src.id;
            // 将除了根节点以外的其他距离值设为无穷大
            for (int i = 0; i < size; ++i)
            {
                if (i == src.id) continue;
                distValue[i] = Mathf.Infinity;
                previous[i] = -1;
            }

            while (frontier.Count != 0)
            {
                node = frontier.Pop();
                int nodeId = node.vertex.id;

                edges = GetEdges(node.vertex);

                foreach (Edge e in edges)
                {
                    int eId = e.vertex.id;
                    if (previous[eId] != -1)
                        continue;
                    float cost = distValue[nodeId] + e.cost;
                    if (cost < distValue[eId])
                    {
                        distValue[eId] = cost;
                        previous[eId] = nodeId;
                        frontier.Remove(e);     // 不存在时失败
                        child = new Edge(e.vertex, cost);
                        frontier.Add(child);
                    }
                }
            }
            return previous;
        }
        #endregion

        #region A*寻路
        public delegate float Heuristic(Vertex a, Vertex b);
        /// <summary>
        /// 欧式启发式计算，作为默认的距离启发式计算
        /// </summary>
        public float EuclidDist(Vertex a, Vertex b)
        {
            Vector3 posA = a.transform.position;
            Vector3 posB = b.transform.position;
            return Vector3.Distance(posA, posB);
        }

        /// <summary>
        /// 曼哈顿启发式计算，作为不同的启发式计算选择
        /// </summary>
        public float ManhattanDist(Vertex a, Vertex b)
        {
            Vector3 posA = a.transform.position;
            Vector3 posB = b.transform.position;
            return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
        }

        /// <summary>
        /// A*算法寻路，同样使用二叉堆存储遍历节点
        /// A*并不会真正遍历所有的路径去得到最佳结果，而是通过使用一个启发式的期望值
        /// 认为最佳期望的节点就是所需的节点，然后往下搜索直到找到终点。
        /// </summary>
        public List<Vertex> GetPathAstar(GameObject srcObj, GameObject dstObj, Heuristic h = null)
        {
            if (srcObj == null || dstObj == null) return new List<Vertex>();
            if (ReferenceEquals(hideFlags, null)) h = EuclidDist;

            Vertex src = GetNearestVertex(srcObj.transform.position);
            Vertex dst = GetNearestVertex(dstObj.transform.position);
            BinaryHeap<Edge> frontier = new BinaryHeap<Edge>();
            Edge[] edges;
            Edge node, child;
            int size = vertices.Count;
            float[] distValue = new float[size];
            int[] previous = new int[size];
            // 初始化列表
            node = new Edge(src, 0);
            frontier.Add(node);
            distValue[src.id] = 0;
            previous[src.id] = src.id;
            for (int i = 0; i < size; ++i)
            {
                if (i == src.id)
                    continue;
                distValue[i] = Mathf.Infinity;
                previous[i] = -1;
            }

            while (frontier.Count != 0)
            {
                node = frontier.Pop();
                int nodeId = node.vertex.id;
                if (ReferenceEquals(node.vertex, dst))
                    return BuildPath(src.id, node.vertex.id, ref previous);
                edges = GetEdges(node.vertex);

                foreach (Edge e in edges)
                {
                    int eId = e.vertex.id;
                    if (previous[eId] != -1)
                        continue;
                    float cost = distValue[nodeId] + e.cost;
                    cost += h(node.vertex, e.vertex); // 关键之处 增加了期望值

                    if (cost < distValue[eId])
                    {
                        distValue[eId] = cost;
                        previous[eId] = nodeId;
                        child = new Edge(e.vertex, cost);
                        frontier.Add(child);
                    }
                }
            }
            return new List<Vertex>();
        }
        #endregion


        #region IDA*寻路
        /// <summary>
        /// 通过递归的方式进行查找，从而不需要额外的数据二叉堆结构用于存储搜索结果
        /// </summary>
        public List<Vertex> GetPathIDAstar(GameObject srcObj, GameObject dstObj, Heuristic h = null)
        {
            if (srcObj == null || dstObj == null) return new List<Vertex>();
            if (ReferenceEquals(hideFlags, null)) h = EuclidDist;

            List<Vertex> path = new List<Vertex>();
            Vertex src = GetNearestVertex(srcObj.transform.position);
            Vertex dst = GetNearestVertex(dstObj.transform.position);
            Vertex goal = null;
            bool[] visited = new bool[vertices.Count];
            for (int i = 0; i < visited.Length; ++i)
                visited[i] = false;
            visited[src.id] = true;

            float bound = h(src, dst);
            while (bound < Mathf.Infinity)
            {
                // 通过深度优先递归查找
                bound = RecursiveIDAstar(src, dst, bound, h, ref goal, ref visited);
            }
            if (ReferenceEquals(goal, null))
                return path;

            return BuildPath(goal);
        }

        /// <summary>
        /// 递归查找最大期望值的节点
        /// </summary>
        private float RecursiveIDAstar(Vertex v, Vertex dst, float bound, Heuristic h, ref Vertex goal, ref bool[] visited)
        {
            // 如果已经找到终点 则结束递归
            if (ReferenceEquals(v, dst))
                return Mathf.Infinity;
            Edge[] edges = GetEdges(v);
            // 若无相邻节点 则结束递归
            if (edges.Length == 0)
                return Mathf.Infinity;

            float fn = Mathf.Infinity;
            foreach (Edge e in edges)
            {
                int eId = e.vertex.id;
                if (visited[eId])
                    continue;
                visited[eId] = true;
                // prev属性用于存储当前节点的下一个期望节点
                e.vertex.prev = v;
                float f = h(v, dst);
                float b;
                if (f <= bound)
                {
                    b = RecursiveIDAstar(e.vertex, dst, bound, h, ref goal, ref visited);
                    fn = Mathf.Min(f, b);
                }
                else
                    fn = Mathf.Min(fn, f);
            }
            return fn;
        }

        private List<Vertex> BuildPath(Vertex v)
        {
            List<Vertex> path = new List<Vertex>();
            while (!ReferenceEquals(v, null))
            {
                path.Add(v);
                v = v.prev;
            }
            return path;
        }
        #endregion


        #region 基于时间片搜索的A*算法
        public List<Vertex> path;
        public bool isFinish;

        /// <summary>
        /// 基于时间片搜索的A*算法 通过协程分担计算压力
        /// </summary>
        public IEnumerator GetPathInFrames(GameObject srcObj, GameObject dstObj, Heuristic h = null)
        {
            isFinish = false;
            path = new List<Vertex>();

            if (srcObj == null || dstObj == null)
            {
                path = new List<Vertex>();
                isFinish = true;
                yield break;
            }
            if (ReferenceEquals(hideFlags, null)) h = EuclidDist;

            Vertex src = GetNearestVertex(srcObj.transform.position);
            Vertex dst = GetNearestVertex(dstObj.transform.position);
            BinaryHeap<Edge> frontier = new BinaryHeap<Edge>();
            Edge[] edges;
            Edge node, child;
            int size = vertices.Count;
            float[] distValue = new float[size];
            int[] previous = new int[size];
            // 初始化列表
            node = new Edge(src, 0);
            frontier.Add(node);
            distValue[src.id] = 0;
            previous[src.id] = src.id;
            for (int i = 0; i < size; ++i)
            {
                if (i == src.id)
                    continue;
                distValue[i] = Mathf.Infinity;
                previous[i] = -1;
            }

            while (frontier.Count != 0)
            {
                yield return null;      // 每帧停顿 防止短时间内的大量运算
                node = frontier.Pop();
                int nodeId = node.vertex.id;
                if (ReferenceEquals(node.vertex, dst))
                {
                    path = BuildPath(src.id, node.vertex.id, ref previous);
                    break;
                }
                edges = GetEdges(node.vertex);

                foreach (Edge e in edges)
                {
                    int eId = e.vertex.id;
                    if (previous[eId] != -1)
                        continue;
                    float cost = distValue[nodeId] + e.cost;
                    cost += h(node.vertex, e.vertex); // 关键之处 增加了期望值

                    if (cost < distValue[eId])
                    {
                        distValue[eId] = cost;
                        previous[eId] = nodeId;
                        child = new Edge(e.vertex, cost);
                        frontier.Add(child);
                    }
                }
            }
            isFinish = true;
            yield break;
        }
        #endregion

        #region 平滑路径
        public List<Vertex> Smooth(List<Vertex> path)
        {
            List<Vertex> newPath = new List<Vertex>();
            if (path.Count == 0) return newPath;
            if (path.Count < 3) return path;

            newPath.Add(path[0]);
            int i, j;
            for (i = 0; i < path.Count; ++i)
            {
                for (j = i + 1; i < path.Count; ++j)
                {
                    Vector3 origin = path[i].transform.position;
                    Vector3 destination = path[j].transform.position;
                    Vector3 direction = destination - origin;
                    float distance = direction.magnitude;
                    bool isWall = false;
                    direction.Normalize();

                    Ray ray = new Ray(origin, direction);
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(ray, distance);

                    foreach (RaycastHit hit in hits)
                    {
                        string tag = hit.collider.gameObject.tag;
                        if (tag.Equals("Wall"))
                        {
                            isWall = true;
                            break;
                        }
                    }
                    if (!isWall)
                        break;
                }
                // 抽出彼此可见的导航点，生成新的导航路线
                i = j - 1;
                newPath.Add(path[i]);
            }
            return newPath;
        }
        #endregion

        #region 基于协调的A*寻路
        /// <summary>
        /// 用于设置所有的埋伏路径
        /// </summary>
        /// <param name="dstObj"></param>
        /// <param name="lurkers"></param>
        public void SetPathAmbush(GameObject dstObj, List<Lurker> lurkers)
        {
            Vertex dst = GetNearestVertex(dstObj.transform.position);
            foreach (var lurker in lurkers)
            {
                Vertex src = GetNearestVertex(lurker.transform.position);
                lurker.path = AStartMbush(src, dst, lurker, lurkers);
            }

        }


        /// <summary>
        /// 寻找到目的地的每一条路径 写的不明不白 大致思路是增加被使用过的节点的成本
        /// 使得其他选择者去选择不重复的路径
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="lurker"></param>
        /// <param name="lurkers"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public List<Vertex> AStartMbush(Vertex src, Vertex dst, Lurker lurker, List<Lurker> lurkers, Heuristic h = null)
        {
            int graphSize = vertices.Count;
            float[] extra = new float[graphSize];
            float[] costs = new float[graphSize];

            // 初始化统计数组
            for (int i = 0; i < graphSize; i++)
            {
                extra[i] = 1f;
                costs[i] = Mathf.Infinity;
            }

            foreach (Lurker l in lurkers)
            {
                foreach (Vertex v in l.path)
                {
                    extra[v.id] += 1f;
                }
            }

            // 定义和初始化用于计算A*的变量
            Edge[] successirs;
            int[] previous = new int[graphSize];
            for (int i = 0; i < graphSize; i++)
            {
                previous[i] = -1;
            }
            previous[src.id] = src.id;
            float cost = 0;
            Edge node = new Edge(src, 0);
            BinaryHeap<Edge> frontier = new BinaryHeap<Edge>();
            // 加入根节点，开始进行A*计算
            frontier.Add(node);
            while (frontier.Count != 0)
            {
                if (frontier.Count == 0)
                    return new List<Vertex>();

                node = frontier.Pop();
                if (ReferenceEquals(node.vertex, dst))
                {
                    return BuildPath(src.id, node.vertex.id, ref previous);
                }
                int nodeId = node.vertex.id;
                // 该导航点在此路径中的代价高于在其他路径的代价 则不选用
                if (node.cost > costs[nodeId]) 
                    continue;

                successirs = GetEdges(node.vertex);
                foreach (Edge e in successirs)
                {
                    int eId = e.vertex.id;
                    if (previous[eId] != -1) // 排除已经记录过的导航点
                        continue;

                    cost = e.cost;
                    cost += costs[dst.id];  // 这个值没有赋值过吧？
                    cost += h(e.vertex, dst);

                    if (cost < costs[eId])
                    {
                        Edge child;
                        child = new Edge(e.vertex, cost);
                        costs[eId] = cost;
                        previous[eId] = nodeId;
                        frontier.Remove(e);
                        frontier.Add(child);
                    }
                }
            }
            return new List<Vertex>();
        }

        #endregion


    }
}