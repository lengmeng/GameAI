using UnityEngine;
using System.Collections;


namespace GameAI.AgentCore
{
    /// <summary>
    /// 用于存储运动和旋转的中介
    /// </summary>
    public class Steering
    {
        public float angular;
        public Vector3 linear;
        public Steering()
        {
            angular = 0.0f;
            linear = new Vector3();
        }
    }
}