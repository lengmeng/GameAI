using GameAI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.AgentCore
{
    /// <summary>
    /// 跳跃系统落地点——需rigidbody和collider
    /// 落地之后删除JumpPoint数据 恢复其他组件
    /// </summary>
    public class LandingLocation : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Agent")) return;

            Agent agent = other.GetComponent<Agent>();
            Jump jump = other.GetComponent<Jump>();
            if (agent == null || jump == null) return;

            jump.Isolate(false);
            jump.jumpPoint = null;
            jump.StopJump();
        }
    }
}