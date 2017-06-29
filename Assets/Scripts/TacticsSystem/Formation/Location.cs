using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem.Formation
{
    /// <summary>
    /// 数据类型 与Steering类似，通过给定编队的锚点和旋转来设置目标的位置和旋转
    /// </summary>
    public class Location
    {
        public Vector3 position;
        public Quaternion rotation;

        public Location()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
        }

        public Location(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

    }
}