using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.DecisionTree
{
    /// <summary>
    /// 行为节点
    /// </summary>
    public class ActionNode: DecisionTreeNode
    {
        public bool Activated = false;

        public override DecisionTreeNode MakeDecision()
        {
            return this;
        }

        public virtual void LateUpdate()
        {
            if (!Activated)
                return;
            // 行为实现
        }


    }
}