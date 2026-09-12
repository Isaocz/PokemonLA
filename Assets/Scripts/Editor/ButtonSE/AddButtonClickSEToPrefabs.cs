using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Events;

public class AddButtonClickSEToPrefabs
{
    [MenuItem("Tools/ButtonClickSE/Add ButtonClickSE")]
    public static void ProcessPrefabs()
    {
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        int processedCount = 0;

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // 只处理 Assets/ 下的预制件
            if (!path.StartsWith("Assets/"))
                continue;

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            bool modified = false;

            // 找到所有 Button（包括子对象、孙对象）
            var buttons = prefab.GetComponentsInChildren<Button>(true);

            foreach (var btn in buttons)
            {
                // 检查是否已有 ButtonClickSE 组件
                ButtonClickSE se = btn.GetComponent<ButtonClickSE>();
                if (se == null)
                {
                    se = Undo.AddComponent<ButtonClickSE>(btn.gameObject);
                    modified = true;
                }

                // 检查是否已有 CallClickSE 持久化监听器
                bool hasListener = false;
                int count = btn.onClick.GetPersistentEventCount();
                for (int i = 0; i < count; i++)
                {
                    var target = btn.onClick.GetPersistentTarget(i);
                    var method = btn.onClick.GetPersistentMethodName(i);

                    if (target == se && method == "CallClickSE")
                    {
                        hasListener = true;
                        break;
                    }
                }

                //使用 UnityEventTools 添加持久化监听器
                if (!hasListener)
                {
                    Undo.RecordObject(btn, "Add ButtonClickSE Listener");
                    UnityEventTools.AddPersistentListener(btn.onClick, se.CallClickSE);
                    modified = true;
                }
            }

            // 如果预制件被修改，则保存
            if (modified)
            {
                EditorUtility.SetDirty(prefab);
                processedCount++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"处理完成，共修改 {processedCount} 个预制件。");
    }
}
