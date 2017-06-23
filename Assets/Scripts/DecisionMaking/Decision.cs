using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.DecisionTree
{
    /// <summary>
    /// 决策节点
    /// </summary>
    public class Decision : DecisionTreeNode
    {
        public ActionNode NodeTrue;
        public ActionNode NodeFalse;

        public virtual ActionNode GetBranch()
        {
            return null;
        }
    }
}