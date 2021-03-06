﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI.AgentCore
{
    /// <summary>
    /// 最主要的组件类 它利用各种行为实现智能移动 
    /// 代码根据steering的值对物体进行旋转、移动
    /// </summary>
    public class Agent : MonoBehaviour
    {

        public float maxSpeed;      // 最大速度
        public float maxAccel;      // 最大加速度
        public float maxRotation;   // 最大旋转速度
        public float maxAngularAccel;// 最大旋转加速度

        public float orientation;   // 方向 以Y轴为旋转角度
        public float rotation;
        public Vector3 velocity;    // 速度

        public float priorityThreshold; // 优先级阈值 当对steering的修改超过它时 则先执行这部分行为

        protected Steering steering = null;

        private Dictionary<int, List<Steering>> groups;

        void Start()
        {
            velocity = Vector3.zero;
            groups = new Dictionary<int, List<Steering>>();
        }

        /// <summary>
        /// 每帧中根据的参数数值计算移动
        /// </summary>
        public virtual void Update()
        {
            Vector3 displacement = velocity * Time.deltaTime;
            orientation += rotation * Time.deltaTime;
            // 限制方向值在0~360
            if (orientation < 0)
                orientation += 360.0f;
            else if (orientation > 360.0f)
                orientation -= 360.0f;

            transform.Translate(displacement, Space.World); // 平移
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, orientation);      // 以up方向为轴 旋转orientation度
        }

        /// <summary>
        /// LateUpdate在所有Update之后执行
        /// </summary>
        public virtual void LateUpdate()
        {
            // 通过优先级机制获取混合后的行为
            steering = GetPrioritySteering();
            groups.Clear();

            if (steering == null) return;

            velocity += steering.linear * Time.deltaTime;   // 加速度 * 时间 = 速度
                                                            // 速度值是个矢量 包含了方向 当矢量值大于最大速度时 
                                                            // 归一化得到矢量值等于1，再乘上最大速度值 得到最大移动速度
            if (velocity.magnitude > maxSpeed)
            {
                velocity.Normalize();
                velocity = velocity * maxSpeed;
            }
            // sqrMagnitude是矢量长度的平方，少了开方的消耗
            if (steering.linear.sqrMagnitude == 0.0f)
            {
                velocity = Vector3.zero;
            }

            rotation += steering.angular * Time.deltaTime;  // 旋转角度程度
            if (steering.angular == 0.0f)
            {
                rotation = 0.0f;
            }

            steering = null;
        }

        /// <summary>
        /// 设置Steering数据
        /// </summary>
        /// <param name="steering"></param>
        /// <param name="priority"></param>
        public void SetSteering(Steering steering, int priority)
        {
            if (steering != null)
            {
                if (!groups.ContainsKey(priority))
                {
                    groups.Add(priority, new List<Steering>());
                }

                groups[priority].Add(steering);
            }
        }

        private Steering GetPrioritySteering()
        {
            Steering steering = null;
            float sqrThreshold = priorityThreshold * priorityThreshold;
            foreach(var group in groups.Values) // TODO 优化foreach
            {
                steering = new Steering();
                foreach(Steering singleSteering in group)
                {
                    steering.linear += singleSteering.linear;
                    steering.angular += singleSteering.angular;
                }
                if(steering.linear.sqrMagnitude > sqrThreshold || Mathf.Abs(steering.angular) > priorityThreshold)
                {
                    return steering;
                }
            }
            return null;
        }
    }
}