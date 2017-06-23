using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  GameAI.DecisionMaking.DecisionTree
{
    /// <summary>
    /// 决策节点的基类
    /// </summary>
    public class DecisionTreeNode : MonoBehaviour
    {
        public virtual DecisionTreeNode MakeDecision()
        {
            return null;
        }
    }
}

