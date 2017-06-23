using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.BehaviorTree
{
    /// <summary>
    /// 任务节点，用于执行行为
    /// </summary>
    public class Task : MonoBehaviour
    {
        public List<Task> children;
        protected bool result = false;
        protected bool isFinished = false;

        /// <summary>
        /// 完成时调用
        /// </summary>
        /// <param name="r">结果</param>
        public virtual void SetResult(bool r)
        {
            
            result = r;
            isFinished = true;
        }

        /// <summary>
        /// 创建行为
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Run()
        {
            SetResult(true);
            yield break;
        }

        /// <summary>
        /// 开始行为的一般方法
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator RunTask()
        {
            yield return StartCoroutine(Run());
        }
    }
}