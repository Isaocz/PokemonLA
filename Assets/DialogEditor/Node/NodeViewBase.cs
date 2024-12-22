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

        //对话数据
        public DialogNodeDataBase DialogNodeData = null;

        public NodeViewBase(DialogNodeDataBase dialogNodeData) : base()
        {
            GUID = Guid.NewGuid().ToString();
            DialogNodeData = dialogNodeData;
            //大坑，新版本unity不自动在磁盘上应用资源更新，必须先给目标物体打上Dirty标记
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
