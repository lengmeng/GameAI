using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem.OptimosedWaypoint
{
    public class Waypoint : MonoBehaviour, IComparer
    {
        public float value;
        public List<Waypoint> neighbours;

        /// <summary>
        /// 对比功能
        /// </summary>
        public int Compare(object a, object b)
        {
            Waypoint wa = a as Waypoint;
            Waypoint wb = b as Waypoint;
            if (wa.value == wb.value)
                return 0;

            return wa.value < wb.value ? -1 : 1;
        }

        /// <summary>
        /// 确定两个导航点之间是否可以移动
        /// </summary>
        public static bool CanMove(Waypoint a, Waypoint b)
        {
            // 自定义逻辑 判断是否代理者可以在这两个点之间更好的移动
            return true;
        }

        /// <summary>
        /// 处理导航点
        /// </summary>
        public static void CondenseWaypoints(List<Waypoint> waypoints, float distanceWeight)
        {
            distanceWeight *= distanceWeight;
            waypoints.Sort();
            waypoints.Reverse();
            List<Waypoint> neighbours;


        }
    }
}