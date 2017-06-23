using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.DecisionTree
{
    /// <summary>
    /// 决策树
    /// </summary>
    public class DecisionTree : DecisionTreeNode
    {
        public DecisionTreeNode Root;
        private ActionNode actionNew;
        private ActionNode actionIOld;

        public override DecisionTreeNode MakeDecision()
        {
            return Root.MakeDecision();
        }


        void Update()
        {
            // 更新节点
            actionNew.Activated = false;
            actionIOld = actionNew;
            actionNew = Root.MakeDecision() as ActionNode;

            if (actionNew == null)
                actionNew = actionIOld;

            actionNew.Activated = true;
        }


    }
}