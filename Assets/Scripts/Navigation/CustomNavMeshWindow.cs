using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameAI.NavigationMesh
{
    /// <summary>
    /// 一个编辑器窗口，使得对导航网格的处理在编辑状态下自动处理 
    /// 不会加载图形组件的Start的负担，从而影响场景启动
    /// </summary>
    public class CustomNavMeshWindow : EditorWindow
    {
        static bool isEnabled = false;
        static GameObject graphObj;
        static CustomNavMesh graph;
        static CustomNavMeshWindow window;
        static GameObject graphVertex;

        /// <summary>
        /// 使得存在一个CustomNavMesh对象，挂载CustomNavMesh组件
        /// 并且在界面修改时调用OnScene
        /// </summary>
        [MenuItem("Tools/CustomNavMeshWindow")]
        static void Init()
        {
            window = EditorWindow.GetWindow<CustomNavMeshWindow>();
            window.title = "CustomNavMeshWindow";
            SceneView.onSceneGUIDelegate += OnScene;

            graphObj = GameObject.Find("CustomNavMesh");
            if(graphObj == null)
            {
                graphObj = new GameObject("CustomNavMesh");
                graphObj.AddComponent<CustomNavMesh>();
                graph = graphObj.GetComponent<CustomNavMesh>();
            }
            else
            {
                graph = graphObj.GetComponent<CustomNavMesh>();
                if (graph == null)
                    graphObj.AddComponent<CustomNavMesh>();
                graph = graphObj.GetComponent<CustomNavMesh>();
            }
        }

        void OnDestroy()
        {
            SceneView.onSceneGUIDelegate -= OnScene;
        }

        void OnGUI()
        {
            isEnabled = EditorGUILayout.Toggle("允许网格解析", isEnabled);
            if (GUILayout.Button("创建邻边"))
            {
                if (graph != null)
                    graph.Load();
            }
        }

        /// <summary>
        /// 处理在场景窗口上鼠标左键点击
        /// </summary>
        /// <param name="sceneView"></param>
        private static void OnScene(SceneView sceneView)
        {
            if (!isEnabled)
                return;

            if(Event.current.type == EventType.MouseDown)
            {
                graphVertex = graph.vertexPrefab;
                if(graphVertex == null)
                {
                    Debug.LogError("没有设置顶点预设体");
                    return;
                }
                // 根据用户在场景窗口上的点击生成射线，并检测碰撞
                Event e = Event.current;
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                RaycastHit hit;
                GameObject newV;
                
                if(Physics.Raycast(ray, out hit))
                {
                    GameObject obj = hit.collider.gameObject;
                    Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                    Vector3 pos;

                    // 开始解析三角形网格 在每个三角形的质点生成导航点预设体
                    for (int i = 0; i < mesh.triangles.Length; i += 3)
                    {
                        // mesh.triangles存放三角形顶点序号（每三个对应一个三角形）
                        int t0 = mesh.triangles[i];
                        int t1 = mesh.triangles[i+1];
                        int t2 = mesh.triangles[i+2];
                        // 求三角形的质点
                        pos = mesh.vertices[t0];
                        pos += mesh.vertices[t1];
                        pos += mesh.vertices[t2];
                        pos /= 3;
                        newV = (GameObject)Instantiate(graphVertex, pos, Quaternion.identity);
                        newV.transform.Translate(obj.transform.position); // 平移
                        newV.transform.parent = graphObj.transform;
                        graphObj.transform.parent = obj.transform;
                    }
                }
            }
        }
    }
}