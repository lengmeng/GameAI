using GameAI.AgentCore;
using GameAI.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Actions
{
    public class Jump : VelocityMatch
    {
        public JumpPoint jumpPoint;
        bool canAchieve = false;
        public float maxYVelocity;
        public Vector3 gravity = new Vector3(0, -9.8f, 0);

        private Projectile projectile;


    }
}
