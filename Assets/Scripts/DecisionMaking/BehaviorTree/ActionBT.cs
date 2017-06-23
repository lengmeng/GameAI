using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI.DecisionMaking.BehaviorTree
{
    /// <summary>
    /// 行为节点
    /// </summary>
    public class ActionBT : Task
    {
        public override IEnumerator Run()
        {
            isFinished = false;
            // 实现行为
            return base.Run();
        }
    }
}