using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem
{
    /// <summary>
    /// 数据类型 存储有列表索引与目标对象
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