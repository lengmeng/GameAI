using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.AgentCore
{
    /// <summary>
    /// 路径系统 目前仅支持一条路径
    /// </summary>
    public class Path : MonoBehaviour
    {
        public List<GameObject> nodes = new List<GameObject>();
        List<PathSegment> segments;

        void Start()
        {
            segments = GetSegments();
        }

        /// <summary>
        /// 为每个寻路点两两之间生成PathSegment
        /// </summary>
        /// <returns></returns>
        private List<PathSegment> GetSegments()
        {
            List<PathSegment> segments = new List<PathSegment>();
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Vector3 src = nodes[i].transform.position;
                Vector3 dst = nodes[i + 1].transform.position;
                PathSegment segment = new PathSegment(src, dst);
                segments.Add(segment);
            }

            return segments;
        }

        /// <summary>
        /// 获取位于lastParam路程之后的，当前position位置所处的整个路径段的位置
        /// </summary>
        /// <param name="position">指定的位置</param>
        /// <param name="lastParam">已走过的路程，用于确定position所在的路段</param>
        /// <returns>position位置在整个路程上的位置</returns>
        public float GetParam(Vector3 position, float lastParam)
        {
            float param = 0f;
            PathSegment currentSegment = null;
            float tempParam = 0f;
            foreach (PathSegment ps in segments)
            {
                tempParam += Vector3.Distance(ps.a, ps.b);
                if (lastParam <= tempParam)
                {
                    currentSegment = ps;
                    break;
                }
            }
            if (currentSegment == null)
            {
                return 0f;
            }

            Vector3 currPos = position - currentSegment.a;
            Vector3 segmentDirection = currentSegment.b - currentSegment.a;
            segmentDirection.Normalize();

            Vector3 pointInSegment = Vector3.Project(currPos, segmentDirection); // 向量currPos在direction方向上的投影

            param = tempParam - Vector3.Distance(currentSegment.a, currentSegment.b);
            param += pointInSegment.magnitude;
            return param;
        }

        /// <summary>
        /// 获取指定路程的点的位置
        /// </summary>
        /// <param name="param">路程</param>
        /// <returns></returns>
        public Vector3 GetPosition(float param)
        {
            Vector3 position = Vector3.zero;
            PathSegment currentSegment = null;
            float tempParam = 0f;
            foreach (PathSegment ps in segments)
            {
                tempParam += Vector3.Distance(ps.a, ps.b);
                if (param <= tempParam)
                {
                    currentSegment = ps;
                    break;
                }
            }
            if (currentSegment == null)
                return Vector3.zero;

            Vector3 segmentDirection = currentSegment.b - currentSegment.a;
            segmentDirection.Normalize();
            tempParam -= Vector3.Distance(currentSegment.a, currentSegment.b);
            tempParam = param - tempParam;
            position = currentSegment.a + segmentDirection * tempParam;
            return position;
        }

        #region 编辑器功能
        void OnDrawGizmos()
        {
            Vector3 direction;
            Color tmp = Gizmos.color;
            Gizmos.color = Color.magenta;
            for(int i = 0; i < nodes.Count-1; ++i)
            {
                Vector3 src = nodes[i].transform.position;
                Vector3 dst = nodes[i + 1].transform.position;
                direction = dst - src;
                Gizmos.DrawRay(src, direction);
            }
            Gizmos.color = tmp;
        }

        [ContextMenu("GeneratePaths")]
        private void GeneratePathsData()
        {
            nodes.Clear();
            int count = this.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                Transform childT = this.transform.GetChild(i);
                nodes.Add(childT.gameObject);
            }
        }
        #endregion
    }
}