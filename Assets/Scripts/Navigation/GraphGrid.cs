using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameAI.Navigation
{
    /// <summary>
    /// 可视化网格 通过读取.map格式的数据来生成地图网格 并将其可视化
    /// </summary>
    public class GraphGrid : Graph
    {
        public GameObject obstaclePrefab;
        public string mapName = "arena_test.map"; // .map的文件名
        public bool get8Vicinity = false;
        public float cellSize = 1f;

        [Range(0, Mathf.Infinity)]
        public float defaultCost = 1f;
        [Range(0, Mathf.Infinity)]
        public float maximumCost = Mathf.Infinity;

        string mapsDir = "Maps";

        int numRows;    // 行/高
        int numCols;    // 列/宽

        GameObject[] vertexObjs;

        bool[,] mapVertices;

        private int Grid2Id(int x, int y)
        {
            return Mathf.Max(numRows, numCols) * y + x; // TODO 这里是怎么想的？使得高一定要大于宽 否则逻辑出错
        }

        private Vector2 Id2Grid(int id)
        {
            Vector2 location = Vector2.zero;
            location.y = Mathf.Floor(id / numCols);
            location.x = Mathf.Floor(id % numCols);

            return location;
        }

        /// <summary>
        /// 读取.map地图数据
        /// </summary>
        /// <param name="filename"></param>
        private void LoadMap(string filename)
        {
            string path = Application.dataPath + "/" + mapsDir + "/" + filename;

            try
            {
                StreamReader strmRdr = new StreamReader(path);
                using (strmRdr)
                { // 只要离开了范围就调用strmRdr的Dispose函数
                    int j = 0, i = 0, id = 0;
                    string line;
                    Vector3 position = Vector3.zero;
                    Vector3 scale = Vector3.zero;

                    // 读取.map的头部消息
                    line = strmRdr.ReadLine();  // 不重要的一行
                    line = strmRdr.ReadLine();  // 读取height属性
                    numRows = int.Parse(line.Split(' ')[1]);
                    line = strmRdr.ReadLine();  // 读取width属性
                    numCols = int.Parse(line.Split(' ')[1]);

                    line = strmRdr.ReadLine();  // map 这个词

                    // 初始化成员变量，同时申请内存空间
                    vertices = new List<Vertex>(numRows * numCols);
                    neighbours = new List<List<Vertex>>(numRows * numCols);
                    costs = new List<List<float>>(numRows * numCols);
                    vertexObjs = new GameObject[numCols * numRows];
                    mapVertices = new bool[numRows, numCols];
                    
                    // 开始读取地图数据
                    for(i = 0; i < numRows; ++i)
                    {
                        line = strmRdr.ReadLine();
                        for (j = 0; j < numCols; ++j)
                        {
                            mapVertices[i, j] = line[j] == '.';

                            position.x = j * cellSize;
                            position.z = i * cellSize;
                            id = Grid2Id(i, j);
                            
                            GameObject tmpGo = mapVertices[i, j] ? vertexPrefab : obstaclePrefab;
                            // 根据类型实例化预设
                            vertexObjs[id] = Instantiate(tmpGo, position, Quaternion.identity) as GameObject;
                            // 生成名称
                            vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());
                            // 挂载组件
                            Vertex v = vertexObjs[id].AddComponent<Vertex>();
                            v.id = id;
                            vertices.Add(v);
                            neighbours.Add(new List<Vertex>());
                            costs.Add(new List<float>());
                            // 调整预设体的缩放
                            float y = vertexObjs[id].transform.localScale.y;
                            scale = new Vector3(cellSize, y, cellSize);
                            vertexObjs[id].transform.localScale = scale;
                            vertexObjs[id].transform.parent = gameObject.transform;
                        }
                    }
                    // 用于设置每个顶点的邻居数据
                    for (i = 0; i < numRows; ++i)
                    {
                        for (j = 0; j < numCols; ++j)
                        {
                            SetNeighbours(j, i);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

        }

        protected void SetNeighbours(int x, int y, bool get8 = false)
        {
            int col = x;
            int row = y;
            int i, j, vertexId = Grid2Id(y, x);

            neighbours[vertexId] = new List<Vertex>();
            costs[vertexId] = new List<float>();
            Vector2[] pos = new Vector2[0];

            // 获取四周八格还是相邻四格
            if (get8)
            {
                pos = new Vector2[8];
                int c = 0;
                for(i = row - 1; i <= row; ++i)
                {
                    for(j = col - 1; j <= col; j++)
                    {
                        pos[c] = new Vector2(j, i);
                        c++;
                    }
                }
            }
            else
            {
                pos = new Vector2[4];
                pos[0] = new Vector2(col, row - 1);
                pos[1] = new Vector2(col - 1, row);
                pos[2] = new Vector2(col + 1, row);
                pos[3] = new Vector2(col, row + 1);
            }

            for(int idx = 0; idx < pos.Length; ++idx)
            {
                i = (int)pos[idx].y;
                j = (int)pos[idx].x;

                if (i < 0 || j < 0)
                    continue;

                if (i >= numRows || j >= numCols)
                    continue;

                if (i == row && j == col)
                    continue;

                if ( !mapVertices[i, j])
                    continue;

                int id = Grid2Id(j, i);
                neighbours[vertexId].Add(vertices[id]);
                costs[vertexId].Add(defaultCost);
            }

        }

        public override void Load()
        {
            LoadMap(mapName);
        }

        /// <summary>
        /// 基于广度优先的搜索算法 加上价值体系 就等于A*算法了？
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Vertex GetNearestVertex(Vector3 position)
        {
            int col = (int)(position.x / cellSize);
            int row = (int)(position.z / cellSize);
            
            Vector2 p = new Vector2(col, row); // 得到position所在的网格编号

            List<Vector2> explored = new List<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(p);

            // 可能mapVertices 存储着是否是可导航格子的数据 当获得可用的格子时直接返回，
            // 否则获取该位置四周一格范围内的其他格子，再检测一遍，如此递增
            // explored 用于标记这个格子是否已经检测过
            do
            {
                p = queue.Dequeue();
                col = (int)p.x;
                row = (int)p.y;
                int id = Grid2Id(col, row);

                if (mapVertices[row, col])
                    return vertices[id];

                if (!explored.Contains(p))
                {
                    explored.Add(p);
                    int i, j;
                    for(i = row - 1; i <= (row + 1); ++i)
                    {
                        for(j = col - 1; j <= (col + 1); ++j)
                        {
                            if (i < 0 || j < 0)
                                continue;
                            if (j >= numCols || i >= numRows)
                                continue;
                            if (i == row && j == col)
                                continue;
                            queue.Enqueue(new Vector2(j, i));
                        }
                    }
                }
            }
            while (queue.Count != 0);

            return null;
        }

    }
}