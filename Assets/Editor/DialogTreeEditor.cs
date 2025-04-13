using UnityEngine;
using UnityEditor;
using DialogueSystem;

[InitializeOnLoad]
public class DialogTreeEditor
{
    static DialogTreeEditor()
    {
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }

    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (path.EndsWith(".asset"))
        {
            var asset = AssetDatabase.LoadAssetAtPath<DialogTree>(path);
            if (asset != null)
            {
                // 加载新图标
                Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/DialogTree Icon.png");
                if (icon != null)
                {
                    // 绘制新图标，确保覆盖旧图标
                    GUI.DrawTexture(selectionRect, icon, ScaleMode.ScaleToFit, true);
                }
            }
        }
    }


}