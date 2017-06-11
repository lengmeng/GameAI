using GameAI.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI.AgentCore;

namespace GameAI.Actions
{
    /// <summary>
    /// 避开墙壁
    /// </summary>
    public class AvoidWall : Seek
    {
        public float avoidDistance; // 规避距离
        public float lookAhead;     // 视线距离

        public override void Awake()
        {
            base.Awake();
            target = new GameObject();
        }

        public override Steering GetSteering()
        {
            // avoidDistance 规避距离 lookAhead 视线检测距离
            // 使用实现检测前方 如检测到碰撞则将target放至碰撞点 之后实现规避该点
            Vector3 position = transform.position;
            Vector3 rayVector = agent.velocity.normalized * lookAhead;
            Vector3 direction = rayVector;
            RaycastHit hit;

            if(Physics.Raycast(position, direction, out hit, lookAhead))
            {
                position = hit.point + hit.normal * avoidDistance;
                target.transform.position = position;
                return base.GetSteering();
            }
            return null;
        }
    }
}
