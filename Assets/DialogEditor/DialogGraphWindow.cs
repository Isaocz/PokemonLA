#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DialogueSystem;

namespace LFramework.AI.Kit.DialogueSystem
{
    public class DialogGraphWindow : EditorWindow
    {
        // 移除原先的打开方式
        // [MenuItem("Window/UI Toolkit/DialogueView")]
        // public static void ShowExample()
        // {
        //
        //     DialogueView wnd = GetWindow<DialogueView>();
        //     wnd.titleContent = new GUIContent("DialogueView");
        // }

        private DialogGraphView _graphView = null;

        // 打开DialogTree资源时触发
        [OnOpenAsset(1)]
        public static bool OnOpenAsssets(int id, int line)
        {
            if (EditorUtility.InstanceIDToObject(id) is DialogTree tree)
            {
                //打开不同DialogTree文件
                if (DialogGraphView.treeData != tree)
                {
                    //Debug.Log(DialogGraphView.treeData);
                    DialogGraphView.treeData = tree;

                    //判断窗口是否打开
                    if (HasOpenInstances<DialogGraphWindow>())
                    {
                        CloseEditorWindow();
                    }


                    //大大大大大坑！新版本unity不自动在磁盘上应用资源更新，必须先给目标物体打上Dirty标记
                    EditorUtility.SetDirty(tree);
                }

                DialogGraphWindow wnd = GetWindow<DialogGraphWindow>();
                wnd.titleContent = new GUIContent("DialogueView");

                return true;
            }
            return false;
        }

        public static void CloseEditorWindow()
        {
            DialogGraphWindow wnd = GetWindow<DialogGraphWindow>();
            if (wnd != null)
            {
                wnd.Close();
            }
            
        }

        public void CreateGUI()
        {
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets/DialogEditor/DialogGraphWindow.uxml");
            visualTree.CloneTree(rootVisualElement);

            
            _graphView = rootVisualElement.Q<DialogGraphView>("DialogGraphView");

            //_inspectorView = rootVisualElement.Q<InspectorView>("InspectorView");

            var saveButton = rootVisualElement.Q<ToolbarButton>("SaveButton");

            saveButton.clicked += OnSaveButtonClicked;

            //初始化节点图
            DialogGraphView.Instance.ResetNodeView();
        }

        //保存资源文件
        private void OnSaveButtonClicked()
        {
            EditorUtility.SetDirty(DialogGraphView.treeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Save");
        }

        private void OnDestroy()
        {
            //对象销毁之前记得保存一下，保险
            AssetDatabase.SaveAssets();
        }
    }


}
#endif
