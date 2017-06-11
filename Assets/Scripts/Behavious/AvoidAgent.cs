using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Behaviours
{
    /// <summary>
    /// 规避tag为Agent的对象
    /// </summary>
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
            float shortestTime = Mathf.Infinity; // 保存最小碰撞时间
            GameObject firstTarget = null;
            float firstMinSeparation = 0.0f;
            float firstDistance = 0.0f;
            Vector3 firstRelativePos = Vector3.zero;    // 与最先碰撞对象的相对位置
            Vector3 firstRelativeVel = Vector3.zero;    // 与最先碰撞对象的相对速度

            for(int i = 0; i < targets.Length; ++i)
            {
                Agent targetAgent = agents[i];
                Vector3 relativePos = targets[i].transform.position - transform.position;
                Vector3 relativeVel = targetAgent.velocity - agent.velocity;
                float relativeSpeed = relativeVel.magnitude;
                // 相对距离与相对方向的点积，得到相对方向上的移动距离 * 相对速度的值
                // 因此需要除以两次相对速度的值得到碰撞时间
                float timeToCollision = Vector3.Dot(relativePos, relativeVel);
                timeToCollision /= relativeSpeed * relativeSpeed * -1;

                // 相对移动的结果 使得两者之间的距离小于2 * collisionRadius时 才继续
                float distance = relativePos.magnitude;
                float minSeparation = distance - relativeSpeed * timeToCollision;
                if (minSeparation > 2 * collisionRadius)
                    continue;

                // 更新数据
                if(timeToCollision > 0.0f && timeToCollision < shortestTime)
                {
                    shortestTime = timeToCollision;
                    firstTarget = targets[i];
                    firstMinSeparation = minSeparation;
                    firstDistance = distance;
                    firstRelativePos = relativePos;
                    firstRelativeVel = relativeVel;
                }

                if (firstTarget == null)
                    return null;

                Steering steering = new Steering();

                if (firstMinSeparation <= 0.0f || firstDistance < 2 * collisionRadius)
                    firstRelativePos = firstTarget.transform.position;      // 说明对方在相对移动的方向上，那么位移方向就是对方的方向
                else
                    firstRelativePos += firstRelativeVel * shortestTime;    // 得到自身碰撞需要的位移方向
                firstRelativePos.Normalize();

                steering.linear = -firstRelativePos * agent.maxAccel;       // 往反方向移动 避开碰撞
                return steering;
            }
            return base.GetSteering();
        }
    }
}