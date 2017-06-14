using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Navigation
{
    /// <summary>
    /// 基于Dirichlet的Vertex
    /// </summary>
    public class VertexDirichlet : Vertex
    {
        /// <summary>
        /// 发生碰撞时，若是Agent或Player 则调用 AddLocaltion函数
        /// </summary>
        /// <param name="col"></param>
        public void OnTriggerEnter(Collider col)
        {
            string objName = col.gameObject.name;

            if(objName.Equals("Agent") || objName.Equals("Player"))
            {
                VertexReport report = new VertexReport(id, col.gameObject);
                // 向物体和父物体发送消息（存在性能消耗）
                SendMessageUpwards("AddLocaltion", report); 
            }
        }

        /// <summary>
        /// 碰撞结束时，若是Agent或Player 则调用 RemoveLocation函数
        /// </summary>
        /// <param name="col"></param>
        public void OnTriggerExit(Collider col)
        {
            string objName = col.gameObject.name;
            if (objName.Equals("Agent") || objName.Equals("Player"))
            {
                VertexReport report = new VertexReport(id, col.gameObject);
                SendMessageUpwards("RemoveLocation", report);
            }
        }
    }
}