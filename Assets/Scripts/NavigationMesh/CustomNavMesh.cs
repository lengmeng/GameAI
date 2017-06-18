using GameAI.Navigation;
using GameAI.NavigationWithVisibility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.NavigationMesh
{
    /// <summary>
    /// 自定义导航网格
    /// </summary>
    public class CustomNavMesh : GraphVisibility
    {
        //private Dictionary<int, int> instIdToId;

        //protected override void Start()
        //{
        //    instIdToId = new Dictionary<int, int>();
        //}

        #region 测试导航系统
        public GameObject Origin;
        public GameObject Target;
        private List<Vertex> path;
        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                path = GetPathAstar(Origin, Target, ManhattanDist);
            }
        }

        void OnDrawGizmos()
        {
            Vector3 direction;
            Color tmp = Gizmos.color;
            Gizmos.color = Color.magenta;
            if (path == null) return;
            for (int i = 0; i < path.Count - 1; ++i)
            {
                Vector3 src = path[i].transform.position;
                Vector3 dst = path[i + 1].transform.position;
                direction = dst - src;
                Gizmos.DrawRay(src, direction);
            }
            Gizmos.color = tmp;
        }
        #endregion

    }
}