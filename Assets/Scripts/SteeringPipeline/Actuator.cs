using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI.SteeringPipeline
{
    public class Actuator : MonoBehaviour
    {
        public virtual Path GetPath(Goal goal)
        {
            return new Path();
        }

        public virtual Steering GetOutput(Path path, Goal goal)
        {
            return new Steering();
        }
    }
}