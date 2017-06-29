using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.TacticsSystem.Formation
{
    /// <summary>
    /// 阵容管理者 与 FormationPattern为同一个对象的组件成员
    /// </summary>
    public class FormationManager : MonoBehaviour
    {
        public FormationPattern Pattern; // 这里是没有赋值的，想法是根据阵容修改
        private List<SlotAssignment> slotAssignments;
        private Location driftOffset;

        private void Awake()
        {
            slotAssignments = new List<SlotAssignment>();
        }

        #region 外部接口

        /// <summary>
        /// 刷新阵容信息
        /// </summary>
        public void UpdateSlotAssignments()
        {
            for (int i = 0; i < slotAssignments.Count; i++)
            {
                slotAssignments[i].slotIndex = i;
            }

            driftOffset = Pattern.GetDriftOffset(slotAssignments);
        }

        /// <summary>
        /// 增加指定的对象到阵容
        /// </summary>
        public bool AddCharacter(GameObject character)
        {
            int occupiedSlots = slotAssignments.Count;

            if (!Pattern.SupportsSlots(occupiedSlots + 1))
                return false;

            SlotAssignment sa = new SlotAssignment();
            sa.character = character;
            slotAssignments.Add(sa);

            UpdateSlotAssignments();
            return true;
        }

        /// <summary>
        /// 从阵容中剔除指定的成员
        /// </summary>
        /// <param name="agent">成员对象</param>
        public void RemoveCharacter(GameObject agent)
        {
            int index = slotAssignments.FindIndex(x => x.character.Equals(agent));

            slotAssignments.RemoveAt(index);

            UpdateSlotAssignments();
        }

        /// <summary>
        /// 根据leader的位置更新成员的坐标位置，使得队伍保持阵形
        /// </summary>
        public void UpdateSlots()
        {
            GameObject leader = Pattern.leader;
            Vector3 anchor = leader.transform.position;
            Quaternion rotation = leader.transform.rotation;
            Vector3 slotPos;
            
            foreach (SlotAssignment sa in slotAssignments)
            {
                Vector3 relPos = anchor;
                slotPos = Pattern.GetSlotLocation(sa.slotIndex);
                // TransformDirection()获得自身坐标下的slotPos变化在世界坐标下的结果
                // 如自身坐标Z轴与世界坐标X轴同向时，(0,0,1)得到的是(1,0,0)
                // leader的世界坐标 + 相对位移 = 成员位置所在的世界坐标
                relPos += leader.transform.TransformDirection(slotPos); 

                Location charDrift = new Location(relPos, rotation);
                Character character = sa.character.GetComponent<Character>();
                character.SetTarget(charDrift);
            }

        }

        #endregion
    }
}