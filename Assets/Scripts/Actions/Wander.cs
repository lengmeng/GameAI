using System.Collections;
using System.Collections.Generic;
using GameAI.AgentCore;
using UnityEngine;

namespace GameAI.Actions
{
    /// <summary>
    /// 漫步 
    /// 在正前方offset的位置的radius范围圈上 随机选取左右rate角度的一个点作为下一个漫步的目标点 (每帧都去取点，导致问题)
    /// </summary>
    public class Wander : Face
    {
        public float offset;    // 移动距离
        public float radius;    // 目标点选取范围
        public float rate;      // 角度

        public override void Awake()
        {
            target = new GameObject();
            target.transform.position = transform.position;
            base.Awake();
        }

        public override Steering GetSteering()
        {
            // 让对象以当前方向为基础 随机看向左右某个点 rate是左右的最大旋转度
            float wanderOrientation = Random.Range(-1.0f, 1.0f) * rate;
            float targetOrientation = wanderOrientation + agent.orientation;
            // 获得目标点
            Vector3 orientationVec = GetOriAsVec(agent.orientation);
            Vector3 targetPosition = (offset * orientationVec) + transform.position;
            targetPosition += (GetOriAsVec(targetOrientation) * radius);
            targetAux.transform.position = targetPosition;  // 将目标设置到下一个漫步点（使得Face能够正常面向）
            
            Steering steering = base.GetSteering();         // 得到面向目标点的旋转
            steering = steering == null ? new Steering() : steering;
            // 前进
            steering.linear = targetAux.transform.position - transform.position;
            steering.linear.Normalize();
            steering.linear *= agent.maxAccel;
            return steering;
        }
    }
}
