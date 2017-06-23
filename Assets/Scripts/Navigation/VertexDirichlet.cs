using GameAI.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.NavigationWithDirichlet
{
    /// <summary>
    /// 基于Dirichlet的Vertex 需要具有Vertex标记和collider组件
    /// 检测碰撞，若与Agent或Player标记的对象发生碰撞，则在本物体及父物体中调用指定函数
    /// </summary>
    public class VertexDirichlet : Vertex
    {

        public void OnTriggerEnter(Collider other)
        {
            string objName = other.gameObject.name;

            if (objName.Equals("Agent") || objName.Equals("Player"))
            {
                VertexReport report = new VertexReport(id, other.gameObject);

                SendMessageUpwards("AddLocation", report);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            string objName = other.gameObject.name;

            if (objName.Equals("Agent") || objName.Equals("Player"))
            {
                VertexReport report = new VertexReport(id, other.gameObject);

                SendMessageUpwards("RemoveLocation", report);
            }
        }
    }
}