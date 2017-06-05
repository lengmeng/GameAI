using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.SteeringPipeline
{
    /// <summary>
    ///  使用基于steering管道的行为合并机制
    /// </summary>
    public class Targeter : MonoBehaviour
    {
        public virtual Goal GetGoal()
        {
            return new Goal();
        }
    }
}