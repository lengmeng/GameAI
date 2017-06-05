using GameAI.AgentCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.SteeringPipeline
{
    public class Constraint : MonoBehaviour
    {
        public virtual bool WillViolate(Path path)
        {
            return true;
        }

        public virtual Goal Suggest(Path path)
        {
            return new Goal();
        }
    }
}