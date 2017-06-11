using GameAI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.AgentCore
{
    /// <summary>
    /// 跳跃系统的起跳点——需rigidbody和collider
    /// 和自己所想差不多，一个固定在某处的跳跃点，使用触发器触发，当AI来到时触发Jump组件，实现跳跃
    /// </summary>
    public class JumpLocation : MonoBehaviour
    {
        public LandingLocation landingLoaction;

        public void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Agent")) return;

            Agent agent = other.GetComponent<Agent>();
            Jump jump = other.GetComponent<Jump>();

            if (agent == null || jump == null) return;

            Vector3 originPos = transform.position;
            Vector3 targetPos = landingLoaction.transform.position;
            jump.Isolate(true);
            jump.jumpPoint = new JumpPoint(originPos, targetPos);
            jump.DoJump();
        }
    }

}