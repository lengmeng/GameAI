using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.AgentCore
{
    /// <summary>
    /// 跳跃机制的数据
    /// </summary>
    public class JumpPoint
    {
        public Vector3 jumpLocation;
        public Vector3 landingLocation;
        public Vector3 deltaPosition;

        public JumpPoint()
            : this(Vector3.zero, Vector3.zero)
        {

        }

        public JumpPoint(Vector3 a, Vector3 b)
        {
            this.jumpLocation = a;
            this.landingLocation = b;
            this.deltaPosition = this.landingLocation - this.jumpLocation;
        }
    }
}