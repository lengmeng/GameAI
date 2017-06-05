using GameAI.AgentCore;
using GameAI.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Actions
{
    /// <summary>
    /// 路径跟随 曾加速度能够避免出现冲出头的现象 （存在各种问题，如成环时，路程结束时）
    /// </summary>
    public class PathFollower : Seek
    {
        public Path path;
        public float pathOffset = 0.0f; // 与路线的偏差度

        float currentParam;

        public override void Awake()
        {
            base.Awake();
            target = new GameObject(); // 临时位置 用于使用Seek进行移动
            currentParam = 0f;
        }

        public override Steering GetSteering()
        {
            currentParam = path.GetParam(transform.position, currentParam);
            float targetParam = currentParam + pathOffset;
            target.transform.position = path.GetPosition(targetParam);
            return base.GetSteering();
        }

    }
}