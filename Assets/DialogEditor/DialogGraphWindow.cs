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
        // �Ƴ�ԭ�ȵĴ򿪷�ʽ
        // [MenuItem("Window/UI Toolkit/DialogueView")]
        // public static void ShowExample()
        // {
        //
        //     DialogueView wnd = GetWindow<DialogueView>();
        //     wnd.titleContent = new GUIContent("DialogueView");
        // }

        private DialogGraphView _graphView = null;

        // ��DialogTree��Դʱ����
        [OnOpenAsset(1)]
        public static bool OnOpenAsssets(int id, int line)
        {
            if (EditorUtility.InstanceIDToObject(id) is DialogTree tree)
            {
                //�򿪲�ͬDialogTree�ļ�
                if (DialogGraphView.treeData != tree)
                {
                    //Debug.Log(DialogGraphView.treeData);
                    DialogGraphView.treeData = tree;

                    //�жϴ����Ƿ��
                    if (HasOpenInstances<DialogGraphWindow>())
                    {
                        CloseEditorWindow();
                    }


                    //������ӣ��°汾unity���Զ��ڴ�����Ӧ����Դ���£������ȸ�Ŀ���������Dirty���
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

            //��ʼ���ڵ�ͼ
            DialogGraphView.Instance.ResetNodeView();
        }

        //������Դ�ļ�
        private void OnSaveButtonClicked()
        {
            EditorUtility.SetDirty(DialogGraphView.treeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Save");
        }

        private void OnDestroy()
        {
            //��������֮ǰ�ǵñ���һ�£�����
            AssetDatabase.SaveAssets();
        }
    }


}
#endif
