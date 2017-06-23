using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.BehaviorTree
{
    /// <summary>
    /// 条件节点
    /// </summary>
    public class ConditionBT : Task
    {
        public override IEnumerator Run()
        {
            isFinished = false;
            bool r = false;
            
            // 实现行为 定义结果r为true或false

            SetResult(r);
            yield break;
        }
    }
}