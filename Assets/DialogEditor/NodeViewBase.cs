#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem
{
    public abstract class NodeViewBase : Node
    {
        public Action<NodeViewBase> OnNodeSelected;

        public string GUID;

        //�Ի�����
        public DialogNodeDataBase DialogNodeData = null;

        public NodeViewBase(DialogNodeDataBase dialogNodeData) : base()
        {
            GUID = Guid.NewGuid().ToString();
            DialogNodeData = dialogNodeData;
            //��ӣ��°汾unity���Զ��ڴ�����Ӧ����Դ���£������ȸ�Ŀ���������Dirty���
            EditorUtility.SetDirty(DialogNodeData);
        }

        public Port GetPortForNode(NodeViewBase node, Direction portDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(bool));
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}

#endif
