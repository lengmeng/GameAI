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

        /// <summary>
        /// 强制改变position，所以一定会掉到水平下
        /// </summary>
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
        /// 获取落地时间 使用了重力势能与动能的转换公式
        /// 动能公式 Ek = mv²/2 重力势能 E = mgh  初始动能 = 最终动能 + 变换的重力势能
        /// </summary>
        /// <param name="height">地面高度</param>
        /// <returns>落地时间</returns>
        public float GetLandingTime(float height = 0.0f)
        {
            Vector3 position = transform.position;
            float time = 0.0f;
            // direction归一化 因此y值为sinθ 得到在垂直方向上的速度 
            float valueInt = (direction.y * direction.y) * (speed * speed);
            // 初始动能 - 重力势能 = 最终动能   
            valueInt = valueInt - (Physics.gravity.y * 2 * (position.y - height));
            valueInt = Mathf.Sqrt(valueInt); // 开方得到落地时速度
            // 根据速度变化来计算落地时间 
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

        /// <summary>
        /// 瞄准发射 实在看不懂如何解
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static Vector3 GetFetFireDirection(Vector3 startPos, Vector3 endPos, float speed)
        {
            Vector3 direction = Vector3.zero;
            Vector3 delta = endPos - startPos;
            
            // 二元一次方程求解方程：(-b±根号(b² - 4ac))/2a 
            float a = Vector3.Dot(Physics.gravity, Physics.gravity);
            float b = -4 * (Vector3.Dot(Physics.gravity, delta) + speed * speed);
            float c = 4 * Vector3.Dot(delta, delta);

            if (4 * a * c > b * b) // 无解
                return direction;

            // 算出两个时间
            float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b * b - 4 * a * c)));
            float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b * b - 4 * a * c)));
            // 这里有两个解 需要根据具体需求进行再次限制 以获得正确的结果
            float time;
            if(time0 < 0.0f)
            {
                if (time1 < 0)
                    return direction;
                time = time1;
            }
            else
            {
                if (time1 < 0)
                    time = time0;
                else
                    time = Mathf.Min(time0, time1);
            }

            direction = 2 * delta - Physics.gravity * (time * time);
            direction = direction / (2 * speed * time);

            return direction;
        }

        /// <summary>
        /// TODO 停止抛射运动
        /// </summary>
        public void StopProjectile()
        {
            set = false;
            timeElapsed = 0;
        }
    }
}
