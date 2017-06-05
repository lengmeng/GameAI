using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Behaviours
{
    /// <summary>
    /// 对齐行为
    /// </summary>
    public class Align : AgentBehaviour
    {
        public float targetRadius;  // 对齐的容差值
        public float slowRadius;    // 旋转加速度的缓冲范围
        public float timeToTarget = 0.1f;

        private Agent targetAgent;
        void Start()
        {
            targetAgent = target.GetComponent<Agent>();
        }

        public override Steering GetSteering()
        {
            float targetOrientation = targetAgent.orientation;
            float rotation = targetOrientation - agent.orientation;
            rotation = MapToRange(rotation);            // 修正方向
            float rotationSize = Mathf.Abs(rotation);   // 方向差

            if (rotationSize < targetRadius) return null;

            Steering steering = new Steering();

            float targetRotation = rotationSize > slowRadius ? agent.maxRotation : agent.maxRotation * rotationSize/slowRadius;
            // 获得旋转加速度
            targetRotation *= rotation / rotationSize;      // 旋转方向
            steering.angular = targetRotation - agent.rotation;
            steering.angular /= timeToTarget;

            float angularAccel = Mathf.Abs(steering.angular);

            if(angularAccel > agent.maxAngularAccel)
            {
                steering.angular /= angularAccel;
                steering.angular *= agent.maxAngularAccel;
            }

            return steering;
        }
    }
}