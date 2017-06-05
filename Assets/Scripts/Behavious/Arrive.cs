using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Behaviours
{
    /// <summary>
    /// 到达某个点
    /// </summary>
    public class Arrive : AgentBehaviour
    {
        public float targetRadius;          // 进入目标指定范围
        public float slowRadius;            // 减速范围
        public float timeToTarget = 0.1f;   // 到目的地的时间

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            Vector3 direction = target.transform.position - transform.position;
            float distance = direction.magnitude;
            // 如果已经达到指定点的指定范围内 则直接返回
            if (distance < targetRadius)
                return steering;

            float targetSpeed = distance > slowRadius ? agent.maxSpeed : agent.maxSpeed * distance / slowRadius;
            // 由当前方向计算得到前往目的地的合力方向，并计算出加速
            Vector3 desiredVelocity = direction;
            desiredVelocity.Normalize();
            desiredVelocity *= targetSpeed;
            steering.linear = desiredVelocity - agent.velocity;
            steering.linear /= timeToTarget; // 速度/时间 = 加速度
            if(steering.linear.magnitude > agent.maxAccel)
            {
                steering.linear.Normalize();
                steering.linear *= agent.maxAccel;
            }
            return steering;
        }
    }
}