using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.AgentCore
{
    public class Projectile : MonoBehaviour
    {
        private bool set = false;
        private Vector3 firePos;
        private Vector3 direction;
        private float speed;
        private float timeElapsed;

        void Update()
        {
            if (!set) return;
        }
    }
}
