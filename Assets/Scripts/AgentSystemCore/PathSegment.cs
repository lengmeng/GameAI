using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.AgentCore
{
    public class PathSegment
    {
        public Vector3 a;
        public Vector3 b;

        public PathSegment() : this(Vector3.zero, Vector3.zero) { }
        public PathSegment(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }
    }
}