using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FSM
{
    public class Condition
    {
        public virtual bool Test()
        {
            return false;
        }
    }
}