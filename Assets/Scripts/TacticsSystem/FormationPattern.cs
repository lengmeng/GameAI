using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem
{
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
        /// 在需要的时候可以为所有的location设置一个偏移量
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