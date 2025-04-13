using System;
using System.Collections.Generic;
//using LFramework.AI.Kit.DialogueSystem;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Create new DialogTreeData", fileName = "DialogTreeData")]
    public class DialogTree : ScriptableObject
    {
        public DialogNodeDataBase StartNodeData = null;

        public List<DialogNodeDataBase> ChildNodeDataList = new List<DialogNodeDataBase>();

        // 用来储存节点视图信息
        [Serializable]
        public class ViewData
        {
            public Vector3 Position;
            public Vector3 Scale = new Vector3(1, 1, 1);
        }




        public ViewData GraphViewData = new ViewData();
    }
}

