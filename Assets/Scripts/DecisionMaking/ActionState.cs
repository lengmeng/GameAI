using GameAI.DecisionMaking.DecisionTree;
using GameAI.DecisionMaking.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FSMTree
{
    /// <summary>
    /// 用于合并FSM和决策树而使用的新的节点
    /// </summary>
    public class ActionState : DecisionTreeNode
    {
        public State state;

        public override DecisionTreeNode MakeDecision()
        {
            return this;
        }
    }
}