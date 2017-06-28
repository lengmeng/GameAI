using GameAI.DecisionMaking.FSMTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FSM
{
    public class State : MonoBehaviour
    {
        public List<Transition> transitions;

        public virtual void Awake()
        {
            transitions = new List<Transition>();
            // TODO
        }

        /// <summary>
        /// 初始化状态
        /// </summary>
        public virtual void OnEnable()
        {
            // TODO
        }

        /// <summary>
        /// 完成化状态
        /// </summary>
        public virtual void OnDisable()
        {
            // TODO
        }

        /// <summary>
        /// 状态的行为发展
        /// </summary>
        public virtual void Update()
        {
            // TODO
        }

        /// <summary>
        /// 每帧检测与当前状态相关联的过度条件，找到满足条件的则激活
        /// 过度条件对应的状态，关闭当前状态（自动进行状态跳转）
        /// </summary>
        public void LateUpdate()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].condition.Test())
                {
                    State target;
                    if (transitions[i].GetType().Equals(typeof(TransitionDecision)))
                    {
                        // 如果是合并类型的状态 则通过GetState获取下一个状态
                        TransitionDecision td = transitions[i] as TransitionDecision;
                        target = td.GetState();
                    }
                    else
                        target = transitions[i].target;

                    target.enabled = true;
                    this.enabled = false;
                    return;
                }
            }
        }
    }
}