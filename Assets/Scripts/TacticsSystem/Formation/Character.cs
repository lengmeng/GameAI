using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem.Formation
{
    /// <summary>
    /// 成员 可以放在代表成员的Agent之类的代码中
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