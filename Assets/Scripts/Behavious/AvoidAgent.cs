using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Behaviours
{
    public class AvoidAgent : AgentBehaviour
    {
        public float collisionRadius = 0.4f;
        GameObject[] targets;
        Agent[] agents;

        void Start()
        {
            // Tag为Agent的物体都视为需要规避的物体
            targets = GameObject.FindGameObjectsWithTag("Agent");
            agents = new Agent[targets.Length];
            for (int i = 0; i < targets.Length; ++i)
            {
                agents[i] = targets[i].GetComponent<Agent>();
            }
        }

        public override Steering GetSteering()
        {
            // 用于计算周围所有Agent的距离和速度
            float shortestTime = Mathf.Infinity;
            GameObject firstTarget = null;
            float firstMinSeparation = 0.0f;
            float firstDistance = 0.0f;
            Vector3 firstRelativePos = Vector3.zero;
            Vector3 firstRelativeVel = Vector3.zero;

            for(int i = 0; i < targets.Length; ++i)
            {
                Agent targetAgent = agents[i];
                Vector3 relativePos = targets[i].transform.position - transform.position;
                Vector3 relativeVel = targetAgent.velocity - agent.velocity;
                float relativeSpeed = relativeVel.magnitude;
                // 计算碰撞时间 
                float timeToCollision = Vector3.Dot(relativePos, relativeVel); // 点积得到 距离矢量 在相对方向上的投影的标量 * 相对方向的标量
                // 处于第一个相对方向的标量 得到距离矢量在相对方向上的投影值，也就是相对方向上的移动距离
                // 距离再除以一次相对方向的标量(这里应该理解为速度)，得到相对方向上移动的时间，也就得到碰撞的时间
                timeToCollision /= relativeSpeed * relativeSpeed * -1;
                // 
                float distance = relativePos.magnitude;
                float minSeparation = distance - relativeSpeed * timeToCollision;
                if (minSeparation > 2 * collisionRadius)
                    continue;
                if(timeToCollision > 0.0f && timeToCollision < shortestTime)
                {
                    shortestTime = timeToCollision;
                    firstTarget = targets[i];
                    firstRelativePos = relativePos;
                    firstRelativeVel = relativeVel;
                }

                if (firstTarget == null)
                    return null;

                Steering steering = new Steering();
                if (firstMinSeparation <= 0.0f || firstDistance < 2 * collisionRadius)
                    firstRelativePos = firstTarget.transform.position;
                else
                    firstRelativePos += firstRelativeVel * shortestTime;

                firstRelativePos.Normalize();
                steering.linear = -firstRelativePos * agent.maxAccel;
                return steering;
            }
            return base.GetSteering();
        }
    }
}