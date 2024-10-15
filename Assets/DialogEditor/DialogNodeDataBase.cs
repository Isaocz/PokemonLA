using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DialogueSystem
{
    public abstract class DialogNodeDataBase : ScriptableObject
    {
        /// <summary>
        /// �ڵ�����
        /// </summary>
        [HideInInspector] public Vector2 Position = Vector2.zero;

        [HideInInspector] public string Path;

        /// <summary>
        /// �ڵ�����
        /// </summary>
        public abstract NodeType NodeType { get; }

        protected DialogNodeDataBase()
        {
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }

        public List<DialogString> OutputItems = new List<DialogString>();

        public List<DialogNodeDataBase> ChildNode = new List<DialogNodeDataBase>();


        /// <summary>
        /// ��ǰ�ڵ��Ƿ������սڵ�
        /// </summary>
        /// <returns></returns>
        public bool isLastNode()
        {
            if (ChildNode == null || ChildNode.Count == 0)
            {
                return true;
            }
            else
            {
                bool b = false;
                for (int i = 0; i < ChildNode.Count; i++) { if (ChildNode[i].NodeType == NodeType.End) { b = true; } }
                return b;
            }
        }

    }
}


