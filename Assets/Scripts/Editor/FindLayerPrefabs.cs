using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class FindLayerPrefabs
{
    [MenuItem("Tools/FindPrefabs")]
    public static void FindPrefabs()
    {
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        List<string> results = new List<string>();

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null) continue;

            // 检查所有子物体是否有 Button 组件
            var buttons = prefab.GetComponentsInChildren<Button>(true);
            if (buttons != null && buttons.Length > 0)
            {
                results.Add(path);
            }
        }

        Debug.Log($"找到 {results.Count} 个包含 Button 的预制件：");
        foreach (var r in results)
        {
            Debug.Log(r);
        }
    }
}
