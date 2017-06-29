using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem.Formation
{
    /// <summary>
    /// 数据类型 用于阵容列表 存储成员的索引与目标对象 代表一个成员
    /// </summary>
    public class SlotAssignment
    {
        public int slotIndex;
        public GameObject character;

        public SlotAssignment()
        {
            slotIndex = -1;
            character = null;
        }
    }
}