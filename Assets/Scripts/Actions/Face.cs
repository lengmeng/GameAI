using GameAI.AgentCore;
using GameAI.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Actions
{
    // 面向指定目标
    public class Face : Align
    {
        protected GameObject targetAux;

        private Agent newAgent;
        public override void Awake()
        {
            base.Awake();
            targetAux = target;
            // 为了使用对齐行为，创建一个临时对象，该对象存储着面向目标需要的旋转，
            // 然后通过对齐功能使得本对象与临时对象对齐，实现面向目标的旋转。
            target = new GameObject();
            newAgent = target.AddComponent<Agent>();
        }

        public override Steering GetSteering()
        {
            Vector3 direction = targetAux.transform.position - transform.position;
            if(direction.magnitude > 0.0f)
            {
                // 与z轴的夹角 返回值单位为弧度(z轴是正方向)
                float targetOrientation = Mathf.Atan2(direction.x, direction.z);
                targetOrientation *= Mathf.Rad2Deg; // 弧度转角度
                newAgent.orientation = targetOrientation;
            }
            return base.GetSteering();
        }

        void OnDestroy()
        {
            Destroy(target);
        }
    }
}