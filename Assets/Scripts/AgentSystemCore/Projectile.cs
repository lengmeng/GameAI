using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.AgentCore
{
    /// <summary>
    /// 弹射 挂载于被弹射出去的物体
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        private bool set = false;
        private Vector3 firePos;
        private Vector3 direction;
        private float speed;
        private float timeElapsed;

        void Update()
        {
            if (!set) return;

            timeElapsed += Time.deltaTime;
            transform.position = firePos + direction * speed * timeElapsed;
            transform.position += Physics.gravity * (timeElapsed * timeElapsed) / 2.0f; // 重力

            // 清除低于水平线的物品
            if (transform.position.y < -1.0f)
            {
                set = false;
                gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 自身发射出去
        /// </summary>
        /// <param name="firePos">发射点</param>
        /// <param name="direction">发射方向</param>
        /// <param name="speed">发射速度</param>
        public void Set(Vector3 firePos, Vector3 direction, float speed)
        {
            this.firePos = firePos;
            this.direction = direction;
            this.speed = speed;
            transform.position = firePos;
            set = true;
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 获取落地时间
        /// </summary>
        /// <param name="height">地面高度</param>
        /// <returns>落地时间</returns>
        public float GetLandingTime(float height = 0.0f)
        {
            Vector3 position = transform.position;
            float time = 0.0f;
            float valueInt = (direction.y * direction.y) * (speed * speed);
            valueInt = valueInt - (Physics.gravity.y * 2 * (position.y - height));
            valueInt = Mathf.Sqrt(valueInt);
            float valueAdd = (-direction.y) * speed;
            float valueSub = (-direction.y) * speed;
            valueAdd = (valueAdd + valueInt) / Physics.gravity.y;
            valueSub = (valueSub - valueInt) / Physics.gravity.y;

            if (float.IsNaN(valueAdd) && !float.IsNaN(valueSub))
                return valueSub;
            else if (!float.IsNaN(valueAdd) && float.IsNaN(valueSub))
                return valueAdd;
            else if (float.IsNaN(valueAdd) && float.IsNaN(valueSub))
                return -1.0f;

            time = Mathf.Max(valueAdd, valueSub);

            return time;
        }

        /// <summary>
        /// 预测落地地点
        /// </summary>
        /// <param name="height">地面高度</param>
        /// <returns>落地地点</returns>
        public Vector3 GetLandingPos(float height = 0.0f)
        {
            Vector3 landingPos = Vector3.zero;
            float time = GetLandingTime(height);
            if(time < 0.0f)
            {
                return landingPos;
            }
            landingPos.y = height;
            landingPos.x = firePos.x + direction.x * speed * time;
            landingPos.z = firePos.z + direction.z * speed * time;

            return landingPos;
        }
    }
}
