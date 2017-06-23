using GameAI.DecisionMaking.DecisionTree;
using GameAI.DecisionMaking.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FSMTree
{
    /// <summary>
    /// 用于存放决策树的关联类
    /// </summary>
    public class TransitionDecision : Transition
    {
        public DecisionTreeNode root;

        public State GetState()
        {
            ActionState action;
            action = root.MakeDecision() as ActionState;
            return action.state;
        }
    }
}