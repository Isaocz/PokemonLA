using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using System.IO;
using UnityEditor.Events;

public class ScanButtonsNeedingSEInAllScenes
{
    [MenuItem("Tools/ButtonClickSE/Scan All Scenes Buttons Needing SE")]
    public static void ScanAndFixAllScenes()
    {
        string scenesFolder = "Assets/Scenes/";
        string[] scenePaths = Directory.GetFiles(scenesFolder, "*.unity", SearchOption.AllDirectories);

        int totalFixedButtons = 0;
        int totalScenesModified = 0;

        foreach (string scenePath in scenePaths)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            int fixedInScene = ScanAndFixSceneButtons(scenePath);

            if (fixedInScene > 0)
            {
                EditorSceneManager.SaveScene(scene);
                totalScenesModified++;
            }

            totalFixedButtons += fixedInScene;
        }

        Debug.Log($"处理完成：共修改 {totalScenesModified} 个场景，修复 {totalFixedButtons} 个按钮。");
    }

    private static int ScanAndFixSceneButtons(string scenePath)
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>(true);
        int fixedCount = 0;

        foreach (var btn in buttons)
        {
            // 跳过 Prefab Asset（只处理场景实例）
            if (PrefabUtility.IsPartOfPrefabAsset(btn.gameObject))
                continue;

            bool needComponent = false;
            bool needEvent = false;

            // 是否有 ButtonClickSE 组件
            ButtonClickSE se = btn.GetComponent<ButtonClickSE>();
            if (se == null)
            {
                needComponent = true;
            }
            else
            {
                // 有组件，但是否有 CallClickSE 持久化事件？
                bool hasListener = false;
                int eventCount = btn.onClick.GetPersistentEventCount();

                for (int i = 0; i < eventCount; i++)
                {
                    var target = btn.onClick.GetPersistentTarget(i);
                    var method = btn.onClick.GetPersistentMethodName(i);

                    if (target == se && method == "CallClickSE")
                    {
                        hasListener = true;
                        break;
                    }
                }

                if (!hasListener)
                    needEvent = true;
            }

            // 如果不需要任何修复，跳过
            if (!needComponent && !needEvent)
                continue;

            fixedCount++;

            // 找到按钮所在的最外层物体（根节点）
            Transform root = btn.transform;
            while (root.parent != null)
                root = root.parent;

            // 输出扫描结果
            Debug.Log(
                $"[需要修复]\n" +
                $"场景: {scenePath}\n" +
                $"按钮: {btn.name}\n" +
                $"最外层物体: {root.name}\n" +
                $"完整路径: {GetFullPath(btn.transform)}\n" +
                $"需要添加组件: {needComponent}\n" +
                $"需要添加事件: {needEvent}\n"
            );

            //执行修复

            // 添加组件
            if (needComponent)
            {
                se = Undo.AddComponent<ButtonClickSE>(btn.gameObject);
                Debug.Log($"→ 已为按钮添加组件 ButtonClickSE");
            }

            // 添加事件
            if (needEvent)
            {
                Undo.RecordObject(btn, "Add ButtonClickSE Listener");
                UnityEventTools.AddPersistentListener(btn.onClick, se.CallClickSE);
                Debug.Log($"→ 已为按钮添加 onClick 事件 CallClickSE()");
            }
        }

        return fixedCount;
    }

    private static string GetFullPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
