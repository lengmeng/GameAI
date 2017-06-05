using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.AgentCore
{
    /// <summary>
    /// 行为的模板类
    /// </summary>
    public class AgentBehaviour : MonoBehaviour
    {
        public GameObject target;   // 目标
        protected Agent agent;      // Agent组件，用于控制对象

        public virtual void Awake()
        {
            agent = gameObject.GetComponent<Agent>();
        }
        public virtual void Update()
        {
            agent.SetSteering(GetSteering());
        }
        public virtual Steering GetSteering()
        {
            return new Steering();
        }

        /// <summary>
        /// 修正角度 值在-180~180之间
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public float MapToRange(float rotation)
        {
            rotation %= 360.0f;
            if(Mathf.Abs(rotation) > 180.0f)
            {
                // 负值归正值
                rotation += (rotation < 0.0f ? 1 : -1) * 360.0f;
            }
            return rotation;
        }
        /// <summary>
        /// 将方向值转为向量
        /// </summary>
        /// <param name="orientation">方向值</param>
        /// <returns>空间向量</returns>
        public Vector3 GetOriAsVec(float orientation)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
            vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;

            return vector.normalized;
        }
    }
}