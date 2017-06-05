using GameAI.AgentCore;
using GameAI.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Actions
{
    public class Evade : Flee
    {
        public float maxPrediction; // 最大预判时间值
        private GameObject targetAux;
        private Agent targetAgent;

        #region 系统事件

        public override void Awake()
        {
            base.Awake();
            // 获取target目标对象的Agent组件，目前并没有获取target的方法
            targetAgent = target.GetComponent<Agent>();
            targetAux = target;
            target = new GameObject();
        }

        void OnDestroy()
        {
            Destroy(targetAux);
        }

        public override Steering GetSteering()
        {
            // 预判对象的走位
            Vector3 direction = targetAux.transform.position - transform.position;
            float distance = direction.magnitude;
            float speed = agent.velocity.magnitude;
            float prediction = speed <= distance / maxPrediction ? maxPrediction : distance / speed;

            target.transform.position = targetAux.transform.position;
            // 将目标的位置增至预判的位置
            target.transform.position += targetAgent.velocity * prediction;

            // 修改了算法参数，然后直接使用父类实现的算法
            return base.GetSteering();
        }
        #endregion
    }
}