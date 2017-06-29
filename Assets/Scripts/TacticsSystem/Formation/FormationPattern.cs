using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem.Formation
{
    /// <summary>
    /// 编队模式基类（通过重写GetSlotLocation可以实现不同的阵容布局）
    /// 领导者如果是玩家那没问题，如果是NPC如何保证该NPC不死？，好像可以通过GetDriftOffset进行实时替换leader
    /// 把Manager和Pattern都放在一个空对象下，管理该对象下所有挂载的士兵成员？
    /// </summary>
    public class FormationPattern : MonoBehaviour
    {
        public int numOfSlots;
        public GameObject leader;   

        void Start()
        {
            if (leader == null)
                leader = transform.gameObject;
        }

        /// <summary>
        /// 通过给定的索引 获得该处于该索引的对象的location信息
        /// 阵容中每一槽位的相对位置
        /// </summary>
        public virtual Vector3 GetSlotLocation(int slotIndex)
        {
            return Vector3.zero;
        }

        /// <summary>
        /// 返回该阵容是否支持指定的数量
        /// </summary>
        public bool SupportsSlots(int slotCount)
        {
            return slotCount <= numOfSlots;
        }

        /// <summary>
        /// 获得该队伍中领导者的数据（未完整）
        /// </summary>
        /// <param name="slotAssignments"></param>
        /// <returns></returns>
        public virtual Location GetDriftOffset(List<SlotAssignment> slotAssignments)
        {
            Location location = new Location();
            location.position = leader.transform.position;
            location.rotation = leader.transform.rotation;

            return location;
        }


    }
}