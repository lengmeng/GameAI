using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Navigation
{
    /// <summary>
    /// 代表每一个导航点
    /// </summary>
    public class Vertex : MonoBehaviour
    {
        public int id;
        public List<Edge> neighbours;
        [HideInInspector]
        public Vertex prev;
    }
}