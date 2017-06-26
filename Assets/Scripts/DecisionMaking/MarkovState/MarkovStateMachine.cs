using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.MarkovState
{
    /// <summary>
    /// 基于成员变量的马尔科夫状态机
    /// </summary>
    public class MarkovStateMachine : MonoBehaviour
    {
        public Vector4 state;
        public Matrix4x4 defaultMatrix;
        public float timeReset;
        public float timeCurrent;
        public List<MarkovTransition> transitions;
        private MonoBehaviour action;

        private void Start()
        {
            timeCurrent = timeReset;
        }

        private void Update()
        {
            if (action != null)
                action.enabled = false;

            MarkovTransition triggeredTransition = null;
            // 寻找到一个已激活的状态转换过度线
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].IsTriggered())
                {
                    triggeredTransition = transitions[i];
                    break;
                }
            }

            // 若寻找到则通过其矩阵计算游戏状态
            if(triggeredTransition != null)
            {
                timeCurrent = timeReset;
                Matrix4x4 matrix = triggeredTransition.matrix;
                state = matrix * state;
                action = triggeredTransition.action;
            }
            else // 否则更新倒计时，若达到倒计时则使用默认矩阵重置状态
            {
                timeCurrent -= Time.deltaTime;
                if(timeCurrent <= 0f)
                {
                    state = defaultMatrix * state;
                    timeCurrent = timeReset;
                    action = null;
                }
            }
        }
    }
}