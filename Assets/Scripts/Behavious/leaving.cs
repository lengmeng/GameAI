using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI.Behaviours
{
    /// <summary>
    /// 离开某个点
    /// </summary>
    public class Leaving : AgentBehaviour
    {
        public float escapeRadius;          // 开始逃跑范围(过近不逃跑)
        public float dangerRadius;          // 脱离范围
        public float timeToTarget = 0.1f;   // 达到目标的时间

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            Vector3 direction = transform.position - target.transform.position;
            float distance = direction.magnitude;

            if (distance > dangerRadius)
            {
                return steering;
            }

            float reduce = distance < escapeRadius ? 0.0f : distance / dangerRadius * agent.maxSpeed;
            // 由当前方向计算得到前往目的地的合力方向，并计算出加速
            Vector3 desiredVelocity = direction;
            desiredVelocity.Normalize();
            desiredVelocity *= reduce;
            steering.linear = desiredVelocity - agent.velocity;
            steering.linear /= timeToTarget; // 速度/时间 = 加速度
            if (steering.linear.magnitude > agent.maxAccel)
            {
                steering.linear.Normalize();
                steering.linear *= agent.maxAccel;
            }
            return steering;
        }
    }

}