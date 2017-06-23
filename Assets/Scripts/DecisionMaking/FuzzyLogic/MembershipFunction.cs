using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FuzzyLogic
{
    /// <summary>
    /// 相性函数，相性值决定了角色会更加趋向于去做某些事情
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
