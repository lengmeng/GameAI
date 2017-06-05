using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.SteeringPipeline
{
    public class Decomposer : MonoBehaviour
    {
        public virtual Goal Decompose(Goal goal)
        {
            return goal;
        }
    }
}