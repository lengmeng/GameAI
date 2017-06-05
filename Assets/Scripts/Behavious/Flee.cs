using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI.Behaviours
{
    public class Flee : AgentBehaviour
    {
        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            steering.linear = transform.position - target.transform.position;
            steering.linear.Normalize();
            steering.linear = steering.linear * agent.maxAccel;
            // 往目标相反方向 施加最大加速度
            return steering;
        }
    }
}
