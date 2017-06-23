using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FuzzyLogic
{
    /// <summary>
    /// 亲近度函数
    /// </summary>
    public class MembershipFunction : MonoBehaviour
    {
        public int stateId;
        public virtual float GetDOM(object input)
        {
            return 0f;
        }
    }
}
