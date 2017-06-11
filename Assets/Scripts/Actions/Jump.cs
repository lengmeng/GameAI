using GameAI.AgentCore;
using GameAI.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameAI.Actions
{
    /// <summary>
    /// 跳跃AI 
    /// 由外部给与一个跳跃点信息JumpPoint，根据该信息在跳跃点出创建target对象。
    /// 计算target跳跃的速度方向，但物体到达此处并且具有与target相同的速度方向时进行跳跃。
    /// </summary>
    public class Jump : VelocityMatch
    {
        public JumpPoint jumpPoint; // 本次跳跃的信息 包含起点与终点
        bool canAchieve = false;    // 是否可以跳跃
        public float maxYVelocity;  // 跳跃速度时在Y轴上的速度 决定物体的跳跃高度
        public Vector3 gravity = new Vector3(0, -9.8f, 0);

        private Projectile projectile;
        private List<AgentBehaviour> behaviours;

        public override void Awake()
        {
            base.Awake();
            this.enabled = false;
            projectile = gameObject.AddComponent<Projectile>();
            behaviours = new List<AgentBehaviour>();
            AgentBehaviour[] abs;
            abs = gameObject.GetComponents<AgentBehaviour>();
            foreach(AgentBehaviour b in abs)
            {
                if (b == this)
                    continue;
                behaviours.Add(b);
            }
        }

        public override Steering GetSteering()
        {
            // 检查是否有一个跳跃配置，如若无则创建
            if(jumpPoint != null && target == null)
            {
                CalculteTarget();
            }

            // 如果计算不通过 说明该跳跃点无法使用 则返回空
            if (!canAchieve)
            {
                return null;
            }

            // Approximately 约等于 ≈ 
            // 达到跳跃点 并且速度方向正确时 进行跳跃
            if (Mathf.Approximately((transform.position - target.transform.position).magnitude, 0f) &&
                Mathf.Approximately((agent.velocity - target.GetComponent<Agent>().velocity).magnitude, 0f))
            {
                DoJump();
                return null;
            }

            // 将两者速度调至一致
            return base.GetSteering();
        }

        /// <summary>
        /// 禁止其他的Agent
        /// </summary>
        /// <param name="state"></param>
        public void Isolate(bool state)
        {
            foreach (AgentBehaviour b in behaviours)
                b.enabled = false;
            this.enabled = state;
        }

        /// <summary>
        /// 停止跳跃欲动
        /// </summary>
        public void StopJump()
        {
            projectile.StopProjectile();
        }

        /// <summary>
        /// 开始跳跃 计算出抛射方向 然后抛射出去
        /// </summary>
        public void DoJump()
        {
            projectile.enabled = true;
            Vector3 direction = Vector3.zero;
            direction = Projectile.GetFetFireDirection(jumpPoint.jumpLocation,
               jumpPoint.landingLocation, agent.maxSpeed);
            projectile.Set(jumpPoint.jumpLocation, direction, agent.maxSpeed);
        }

        /// <summary>
        /// 创建跳跃点对象 target 并且计算在该点跳跃时的速度方向
        /// </summary>
        public void CalculteTarget()
        {
            target = new GameObject();
            target.AddComponent<Agent>();
            m_agent = target.GetComponent<Agent>();

            // 计算第一次跳跃的时间 
            float sqrtTerm = Mathf.Sqrt(2f * gravity.y * jumpPoint.deltaPosition.y + maxYVelocity * 
                agent.maxSpeed); // 初始重力势能 + 动能 = 最终动能
            float time = (maxYVelocity - sqrtTerm) / gravity.y; // （最终速度 - 初始速度） / 加速度 = 时间
            if (!CheckJumpTime(time))
            {
                time = (maxYVelocity + sqrtTerm) / gravity.y;
            }
        }

        /// <summary>
        /// 检查时间的计算是否正确
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private bool CheckJumpTime(float time)
        {
            float vx = jumpPoint.deltaPosition.x / time;
            float vz = jumpPoint.deltaPosition.z / time;
            float speedSq = vx * vx + vz * vz;

            if(speedSq < agent.maxSpeed * agent.maxSpeed)
            {
                target.GetComponent<Agent>().velocity = new Vector3(vx, 0f, vz); // 弹道对象的速度方向
                canAchieve = true;
                return true;
            }
            return false;
        }

    }
}
