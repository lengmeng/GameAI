using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameAI.DecisionMaking.MarkovState
{
    /// <summary>
    /// 用于存储Transitions的类
    /// </summary>
    public class MarkovTransition : MonoBehaviour
    {
        public Matrix4x4 matrix;
        public MonoBehaviour action;

        public virtual bool IsTriggered()
        {
            // 实现细节
            return false;
        }


    }
}