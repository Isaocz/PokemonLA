using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class PrefabCreator
{

    //六边形矩阵的角度向量列表
    static List<Vector3> vList = new List<Vector3> { };

    /// <summary>
    /// 蜂窝六边形之间的间距角度
    /// </summary>
    public static float intervalAngle = 18.0f;

    /// <summary>
    /// 生成的球面的半径
    /// </summary>
    public static float Radius = 3.0f;

    /// <summary>
    /// 以vector(0,0,1)为起始，生成的六边形次数
    /// </summary>
    public static int COUNT = 5;


    /// <summary>
    /// 六边形
    /// </summary>
    public GameObject Hex;


    [MenuItem("Tools/Create Prefab From Selected")]
    static void CreatePrefab()
    {
        Vector3 StartVector = new Vector3(0.0f, 0.0f, 1.0f);
        StartVector = StartVector.normalized;
        //getVectorByHex(StartVector , 0);
        GameObject parent = new GameObject("HexSphere");
        GenerateHexVectors(StartVector , parent.transform);



        /**
        for (int i = 0; i < vList.Count; i++)
        {
            //Instantiate(Hex, transform.position + vList[i] * Radius, Quaternion.LookRotation(vList[i]), transform);
            // 获取当前选中的 GameObject
            GameObject Hex = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Empty/__Common/Hex.prefab");
            GameObject instance = PrefabUtility.InstantiatePrefab(Hex) as GameObject;
            instance.transform.position = vList[i] * Radius;
            instance.transform.rotation = Quaternion.LookRotation(vList[i]);
            instance.transform.parent = parent.transform;
        }
        **/
    }


    static void GenerateHexVectors(Vector3 start , Transform parent)
    {
        Queue<(Vector3, int)> queue = new Queue<(Vector3, int)>();
        //queue.Enqueue((start.normalized, 0));
        TryAdd(queue, start.normalized , 0, parent);
        //AddVector(start.normalized);

        while (queue.Count > 0)
        {
            var (main, count) = queue.Dequeue();
            if (count >= COUNT) continue;

            // 获取旋转轴
            //获取起始向量在zx平面的投影
            Vector3 shadowZX = new Vector3(main.x, 0, main.z);
            //获取起始向量和zx平面的夹角
            float angleZX = Vector3.SignedAngle(Vector3.forward, shadowZX, Vector3.up);

            //获取起始向量在zy平面的投影
            Vector3 shadowZY = new Vector3(0, main.y, main.z);
            //获取起始向量和zy平面的夹角
            float angleZY = Vector3.SignedAngle(Vector3.forward, shadowZY, Vector3.right);

            //旋转后的x轴
            Vector3 axisX = (Quaternion.AngleAxis(angleZX, Vector3.up) * Vector3.right).normalized;
            //旋转后的y轴
            Vector3 axisY = (Vector3.Cross(axisX, main)).normalized;
            Debug.Log(axisX + "+" + main);
            Debug.Log(Vector3.Cross(axisX, main).normalized);
            Debug.Log(Vector3.Cross(new Vector3(0.71f, 0.00f, -0.71f), new Vector3(-0.58f, 0.58f, 0.58f)).normalized);

            //另外两个方向轴
            Vector3 HexAxis1 = Quaternion.AngleAxis(30, main) * axisY;
            Vector3 HexAxis2 = Quaternion.AngleAxis(-30, main) * axisY;
            Debug.Log(angleZX + "+" + angleZY + "+" + axisX + "+" + axisY + "+" + HexAxis1 + "+" + HexAxis2);

            // 六个方向
            TryAdd(queue, Quaternion.AngleAxis(intervalAngle, axisX) * main, count, parent);
            TryAdd(queue, Quaternion.AngleAxis(-intervalAngle, HexAxis1) * main, count, parent);
            TryAdd(queue, Quaternion.AngleAxis(intervalAngle, HexAxis2) * main, count, parent);
            TryAdd(queue, Quaternion.AngleAxis(-intervalAngle, axisX) * main, count, parent);
            TryAdd(queue, Quaternion.AngleAxis(intervalAngle, HexAxis1) * main, count, parent);
            TryAdd(queue, Quaternion.AngleAxis(-intervalAngle, HexAxis2) * main, count, parent);
        }
    }

    static void TryAdd(Queue<(Vector3, int)> queue, Vector3 vec, int count , Transform parent)
    {
        vec = vec.normalized;
        if (AddVector(vec))
        {
            queue.Enqueue((vec, count + 1));
            GameObject Hex = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Empty/__Common/Hex.prefab");
            GameObject instance = PrefabUtility.InstantiatePrefab(Hex) as GameObject;
            instance.transform.position = vec * Radius;
            instance.transform.rotation = Quaternion.LookRotation(vec);

            float c = Mathf.Clamp(count , 1.0f , 4.0f);
            instance.GetComponent<ShieldHex>().count = c;
            instance.transform.parent = parent;
        }
    }

    static bool AddVector(Vector3 vec)
    {
        for (int i = 0; i < vList.Count; i++)
        {
            if (Mathf.Abs(Vector3.Angle(vList[i], vec)) <= intervalAngle / 2.0f) return false; // 近似去重
        }
        vList.Add(vec);
        //Debug.Log("Added: " + vec);
        return true;
    }



}