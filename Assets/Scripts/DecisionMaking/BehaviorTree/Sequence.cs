using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameAI.DecisionMaking.BehaviorTree
{
    /// <summary>
    /// 顺序执行器
    /// 当所有子任务返回true时返回true，相当于AND逻辑
    /// </summary>
    public class Sequence : Task
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