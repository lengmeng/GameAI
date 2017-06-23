using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FSM
{
    /// <summary>
    /// 高级的状态，有一个stateInitial状态用于执行内部的有限状态机
    /// </summary>
    public class StateHighLevel : State
    {
        // 将所有内部state维护到列表中，方便在大状态关闭时
        // 关闭所有内部状态
        public List<State> states;
        public State stateInitial;
        protected State stateCurrent;

        public override void OnEnable()
        {
            if (stateCurrent == null)
            {
                stateCurrent = stateInitial;
                stateCurrent.enabled = true;
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            stateCurrent.enabled = false;
            foreach (var s in states)
            {
                s.enabled = false;
            }
        }
    }
}