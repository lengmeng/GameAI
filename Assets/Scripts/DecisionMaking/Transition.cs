using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FSM
{
    /// <summary>
    /// 过度联系类，用于维护当前状态的下一个状态及进入下一状态需要满足的条件
    /// </summary>
    public class Transition
    {
        public Condition condition; // 状态转换条件
        public State target;        // 下一个状态
    }
}