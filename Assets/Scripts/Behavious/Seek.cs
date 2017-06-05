using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Behaviours
{
    /// <summary>
    /// 前往目标
    /// </summary>
    public class Seek : AgentBehaviour
    {
        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            steering.linear = target.transform.position - transform.position;
            steering.linear.Normalize();
            steering.linear = steering.linear * agent.maxAccel;
            // 一个往目标的最大加速度
            return steering;
        }

    }
}