using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameAI.DecisionMaking.BehaviorTree
{
    /// <summary>
    /// 行为选择器 
    /// 当存在一个子任务返回true时返回true，相当于OR逻辑
    /// </summary>
    public class Selector : Task
    {
        public override void SetResult(bool r)
        {
            if (r == true)
                isFinished = true;
        }

        public override IEnumerator RunTask()
        {
            foreach (Task t in children)
            {
                yield return StartCoroutine(t.RunTask());
            }
        }
    }
}