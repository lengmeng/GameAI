using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Behaviours
{

    public class VelocityMatch: AgentBehaviour
    {
        public float timeToTarget = 0.1f;
        protected Agent m_agent;

        void Start()
        {
            //m_agent = target.GetComponent<Agent>();
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            steering.linear = m_agent.velocity - agent.velocity;
            steering.linear /= timeToTarget;
            if(steering.linear.magnitude > agent.maxAccel)
            {
                steering.linear = steering.linear.normalized * agent.maxAccel;
            }
            steering.angular = 0.0f;
            return steering;
        }
    }
}