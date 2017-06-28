using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.MarkovState
{
    /// <summary>
    /// 作为成员功能的父类
    /// </summary>
    public class GoalGOB
    {
        public string name;
        public float value;
        public float change;

        /// <summary>
        /// 获取不满度，根据各个不同的模式进行重写
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public virtual float GetDiscontentment(float newValue)
        {
            return newValue * newValue;
        }

        public virtual float GetChange()
        {
            return 0f;
        }
    }
}