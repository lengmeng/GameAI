using GameAI.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI.TacticsSystem.Formation
{
    /// <summary>
    /// 用于保存路径
    /// </summary>
    public class Lurker : MonoBehaviour
    {
        [HideInInspector]
        public List<int> pathIds;
        [HideInInspector]
        public List<GameObject> pathObjs;

        public List<Vertex> path;

        private void Awake()
        {
            if (ReferenceEquals(pathIds, null))
                pathIds = new List<int>();
            if (ReferenceEquals(pathObjs, null))
                pathObjs = new List<GameObject>();
        }
    }
}