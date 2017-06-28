using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem
{
    /// <summary>
    /// 目标角色
    /// </summary>
    public class Character : MonoBehaviour
    {
        public Location location;
        public void SetTarget(Location location)
        {
            this.location = location;
        }
    }
}