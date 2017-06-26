using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.DecisionMaking.FuzzyLogic
{
    /// <summary>
    /// 模糊决策者
    /// </summary>
    public class FuzzyDecisionMaker : MonoBehaviour
    {
        /// <summary>
        /// 进行决策，
        /// </summary>
        /// <param name="inputs">输入</param>
        /// <param name="mfList">相性数据</param>
        /// <param name="rules">规则数据</param>
        /// <returns></returns>
        public Dictionary<int, float> MakeDecision(object[] inputs, MembershipFunction[][] mfList, FuzzyRule[] rules)
        {
            Dictionary<int, float> inputDOM = new Dictionary<int, float>();
            Dictionary<int, float> outputDom = new Dictionary<int, float>();
            MembershipFunction memberFunc;

            // 遍历输入，初始化每个状态的相性值
            foreach (object input in inputs)
            {
                for (int i = 0; i < mfList.Length; i++)
                {
                    for (int j = 0; j < mfList[i].Length; j++)
                    {
                        memberFunc = mfList[i][j];
                        int mfId = memberFunc.stateId;
                        float dom = memberFunc.GetDOM(input);
                        if (!inputDOM.ContainsKey(mfId))
                        {
                            inputDOM.Add(mfId, dom);
                            outputDom.Add(mfId, 0f);
                        }
                        else
                            inputDOM[mfId] = dom;
                    }
                }
            }

            // 递归设置输出的相性值
            foreach (FuzzyRule rule in rules)
            {
                int outputId = rule.conclusionStateId;
                float best = outputDom[outputId];
                float min = 1f;
                foreach (int state in rule.stateIds)
                {
                    float dom = inputDOM[state];
                    if (dom < best) continue;
                    if (dom < min) min = dom;
                }

                outputDom[outputId] = min;
            }
            return outputDom;
        }
    }
}
