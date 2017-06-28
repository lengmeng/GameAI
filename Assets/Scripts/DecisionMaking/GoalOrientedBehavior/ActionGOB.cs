using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.MarkovState
{
    /// <summary>
    /// 模版行为的基类
    /// </summary>
    public class ActionGOB : MonoBehaviour
    {
        public virtual float GetGoalChange(GoalGOB goal)
        {
            return 0f;
        }

        public virtual float GetDuration()
        {
            return 0f;
        }
    }
}