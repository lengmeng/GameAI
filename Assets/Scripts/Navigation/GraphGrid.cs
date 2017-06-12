using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Navigation
{
    /// <summary>
    /// 可视化网格 通过读取.map格式的数据来生成地图网格 并将其可视化
    /// </summary>
    public class GraphGrid : Graph
    {
        public GameObject obstaclePrefab;
        public string mapName = "arena.map"; // .map的文件名
        public bool get8Vicinity = false;
        public float cellSize = 1f;

        [Range(0, Mathf.Infinity)]
        public float defaultCost = 1f;
        [Range(0, Mathf.Infinity)]
        public float maximumCost = Mathf.Infinity;

        string mapsDir = "Maps";

        int numCols;
        int numRows;

        GameObject[] vertexObjs;

        bool[,] mapVertices;

        private int Grid2Id(int x, int y)
        {
            return Mathf.Max(numRows, numCols) * y + x;
        }

        private Vector2 Id2Grid(int id)
        {
            Vector2 location = Vector2.zero;
            location.x = Mathf.Floor(id / numCols);
            location.y = Mathf.Floor(id % numCols);

            return location;
        }

        private void LoadMap(string filename)
        {
            //TODO 
            // 读取.map数据

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