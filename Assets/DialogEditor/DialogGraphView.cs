#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem
{


    public class DialogGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogGraphView, GraphView.UxmlTraits>
        {
        }

        public static DialogGraphView Instance;


        public DialogGraphView()
        {
            //���Ӹ��ӱ���
            Insert(0, new GridBackground());

            //�����������ţ��϶�����ק����ѡ������
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            //��ѡ bug
            //��ӣ�������֮��������ȼ�
            //�����Ϊʲô��ѡ����������ѡ���ϷŽڵ������֮ǰ�ᵼ�½ڵ��޷��ƶ�
            //��Ϊ��ѡ�����ȼ�����
            this.AddManipulator(new RectangleSelector());

            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    //�˴�������Ŀ��Ӧ uss �ļ�·��

                    "Assets/DialogEditor/DialogGraphWindow.uss");
            styleSheets.Add(styleSheet);


            if (treeData != null)
            {
                contentViewContainer.transform.position = treeData.GraphViewData.Position;
                contentViewContainer.transform.scale = treeData.GraphViewData.Scale;
            }

            //����graphView�仯�¼�
            graphViewChanged += OnGraphViewChanged;
            //������ͼTransform�仯�¼�
            viewTransformChanged += OnViewTransformChanged;

            //�򵥵ĵ���ģʽ
            Instance = this;



        }




        /// <summary>
        /// �˵����ʱ���λ��
        /// </summary>
        private Vector2 clickPosition;

        /// <summary>
        /// �ڵ����¼�
        /// </summary>
        public Action<NodeViewBase> OnNodeSelected;

        public static DialogTree treeData = null;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            Debug.Log(evt.mousePosition);

            //�������������תΪ��ͼ��������
            clickPosition = contentViewContainer.WorldToLocal(evt.mousePosition);
            if (treeData == null || treeData.StartNodeData == null)
            {
                evt.menu.AppendAction("Create StartNode", x => { CreateNode(NodeType.Start, clickPosition); });
            }





            evt.menu.AppendAction("Create RandomDialogNode",
                x => { CreateNode(NodeType.RandomDialogNode, clickPosition); });
            evt.menu.AppendAction("Create SequentialDialogNode",
                x => CreateNode(NodeType.SequentialDialogNode, clickPosition));
            evt.menu.AppendAction("Create BranchingDialogNode",
                x => CreateNode(NodeType.BranchingDialogNode, clickPosition));
            evt.menu.AppendAction("Create JudgeDialogNode",
                x => CreateNode(NodeType.JudgeNode, clickPosition));
            evt.menu.AppendAction("Create EventDialogNode",
                x => CreateNode(NodeType.EventNode, clickPosition));
            evt.menu.AppendAction("Create AchievementJudgeNode",
                x => CreateNode(NodeType.AchievementJudgeNode, clickPosition));
            evt.menu.AppendAction("Create TDPJudgeNode",
                x => CreateNode(NodeType.TDPJudgeNode, clickPosition));
            evt.menu.AppendAction("Create EndNode", x => { CreateNode(NodeType.End, clickPosition); });

        }




        //�ڵ����ӹ���
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node &&
                endPort.portType == startPort.portType
            ).ToList();
        }



        /// <summary>
        /// �ڵ�ͼ�仯�¼�
        /// </summary>
        /// <param name="graphviewchange"></param>
        /// <returns></returns>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange)
        {
            if (graphviewchange.elementsToRemove != null)
            {
                graphviewchange.elementsToRemove.ForEach(elem =>
                {
                    //����ɾ��
                    if (elem is Edge edge)
                    {
                        NodeViewBase parentNodeView = edge.output.node as NodeViewBase;
                        NodeViewBase childNodeView = edge.input.node as NodeViewBase;
                        if (parentNodeView != null && childNodeView != null)
                        {
                            if (int.TryParse(edge.output.name, out int index))
                            {
                                if (index >= 0 && index < parentNodeView.DialogNodeData.ChildNode.Count) { parentNodeView.DialogNodeData.ChildNode[index] = null; }
                                
                            }
                            else
                            {
                                Debug.LogError("Node.name(string) to int fail");
                            }
                        }
                    }
                });
            }

            if (graphviewchange.edgesToCreate != null)
            {
                //��������
                graphviewchange.edgesToCreate.ForEach(edge =>
                {
                    NodeViewBase parentNodeView = edge.output.node as NodeViewBase;
                    NodeViewBase childNodeView = edge.input.node as NodeViewBase;

                    if (parentNodeView != null && childNodeView != null)
                    {
                        if (int.TryParse(edge.output.name, out int index))
                        {
                            Debug.Log(index);
                            if (index >= 0 && index < parentNodeView.DialogNodeData.ChildNode.Count)
                            {
                                parentNodeView.DialogNodeData.ChildNode[index] = childNodeView.DialogNodeData;
                            }
                        }
                        else
                        {
                            Debug.LogError("Node.name(string) to int fail");
                        }
                    }
                });
            }

            nodes.ForEach(node =>
            {
                NodeViewBase nodeView = node as NodeViewBase;
                if (nodeView != null && nodeView.DialogNodeData != null)
                {
                    nodeView.DialogNodeData.Position = nodeView.GetPosition().position;
                }
            });
            return graphviewchange;
        }

        /// <summary>
        /// graphView��Transform�����仯ʱ����
        /// </summary>
        /// <param name="graphView"></param>
        private void OnViewTransformChanged(GraphView graphView)
        {
            if (treeData != null)
            {
                //������ͼTransform��Ϣ
                treeData.GraphViewData.Position = contentViewContainer.transform.position;
                treeData.GraphViewData.Scale = contentViewContainer.transform.scale;
            }
        }


        //ȷ��Ŀ¼����
        private void MakeSureTheFolder()
        {
            //TODO�����ɿ��������õĶԻ���Դ�ļ�����
            if (!AssetDatabase.IsValidFolder("Assets/DialogueData/NodeData"))
            {
                AssetDatabase.CreateFolder("Assets", "DialogueData");
                AssetDatabase.CreateFolder("Assets/DialogueData", "NodeData");
            }
        }

        /// <summary>
        /// �½��ڵ�
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        private void CreateNode(NodeType type, Vector2 position = default)
        {
            if (treeData == null)
            {
                return;
            }
            Debug.Log("xxx");
            MakeSureTheFolder();
            NodeViewBase nodeView = null;

            //�����ڵ�ĺ��ģ������Ľڵ���Ҫ��������д�����ʽ�����
            switch (type)
            {
                
                case NodeType.Start:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<StartNodeData>();
                        dialogNodeData.Path = $"Assets/DialogueData/NodeData/" + treeData.name + $"/StartData[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/"+treeData.name+ $"/StartData[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new StartNodeView(dialogNodeData);
                        break;
                    }
                case NodeType.RandomDialogNode:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<RandomDialogNodeData>();
                        dialogNodeData.Path =
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/RandomDialogData[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/RandomDialogData[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new RandomDialogNodeView(dialogNodeData);
                        break;
                    }
                case NodeType.SequentialDialogNode:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<SequentialDialogNodeData>();
                        dialogNodeData.Path =
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/SequentialDialogData[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/SequentialDialogData[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new SequentialDialogNodeView(dialogNodeData);
                        break;
                    }
                case NodeType.BranchingDialogNode:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<BranchingDialogNodeData>();
                        dialogNodeData.Path =
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/BranchingDialogData[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/BranchingDialogData[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new BranchingDialogNodeView(dialogNodeData);
                        break;
                    }
                case NodeType.JudgeNode:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<JudgeNodeData>();
                        dialogNodeData.Path =
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/JudgeNodeData[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/JudgeNodeData[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new JudgeNodeView(dialogNodeData);
                        break;
                    }
                case NodeType.EventNode:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<EventNodeData>();
                        dialogNodeData.Path =
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/EventNodeData[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/EventNodeData[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new EventNodeView(dialogNodeData);
                        break;
                    }
                case NodeType.AchievementJudgeNode:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<AchievementJudgeNodeData>();
                        dialogNodeData.Path =
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/AchievementJudgeNodeData[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/AchievementJudgeNodeData[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new AchievementJudgeNodeDialogView(dialogNodeData);
                        break;
                    }
                case NodeType.TDPJudgeNode:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<TDPJudgeNodeData>();
                        dialogNodeData.Path =
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/TDPJudgeNode[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/TDPJudgeNode[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new TDPJudgeNodeView(dialogNodeData);
                        break;
                    }
                case NodeType.End:
                    {
                        var dialogNodeData = ScriptableObject.CreateInstance<EndNodeData>();
                        dialogNodeData.Path = $"Assets/DialogueData/NodeData/" + treeData.name + $"/EndData[{dialogNodeData.GetInstanceID()}].asset";
                        EditorUtility.SetDirty(dialogNodeData);

                        AssetDatabase.CreateAsset(dialogNodeData,
                            $"Assets/DialogueData/NodeData/" + treeData.name + $"/EndData[{dialogNodeData.GetInstanceID()}].asset");

                        nodeView = new EndNodeView(dialogNodeData);
                        break;
                    }
                default:
                    {
                        Debug.LogError("δ�ҵ������͵Ľڵ�");
                        break;
                    }
            }

            //��ӽڵ㱻ѡ���¼�
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.SetPosition(new Rect(position, nodeView.GetPosition().size));


            //��Start�ڵ���������
            if (nodeView.DialogNodeData.NodeType == NodeType.Start)
            {
                treeData.StartNodeData = nodeView.DialogNodeData;
            }
            else
            {
                treeData.ChildNodeDataList.Add(nodeView.DialogNodeData);
            }




            this.AddElement(nodeView);
        }






        /// <summary>
        /// ��ʱ�ֵ䣬���ڳ�ʼ���ڵ�ͼ�ģ�����ǵð��ڴ��ͷŵ�
        /// </summary>
        private Dictionary<DialogNodeDataBase, NodeViewBase> NodeDirt;

        /// <summary>
        /// ���ýڵ�ͼ
        /// </summary>
        public void ResetNodeView()
        {
            Debug.Log(treeData);

            if (treeData != null)
            {
                //��ʼ���ֵ�
                NodeDirt = new Dictionary<DialogNodeDataBase, NodeViewBase>();
                var nodeData = treeData.ChildNodeDataList;

                //���StartNode�Ƿ����
                if (treeData.StartNodeData == null)
                {
                    CreateNode(NodeType.Start);
                }
                else
                {
                    RecoveryNode(treeData.StartNodeData);
                }

                //�ָ��ڵ�
                foreach (var node in nodeData)
                {
                    RecoveryNode(node);
                }

                //�ָ��ڵ��
                RecoveryEdge(treeData.StartNodeData);
                foreach (var node in nodeData)
                {
                    RecoveryEdge(node);
                }

                //����ֵ�
                NodeDirt.Clear();
            }
        }


        /// <summary>
        /// �ָ��ڵ�
        /// </summary>
        /// <param name="DialogNodeData"></param>
        private void RecoveryNode(DialogNodeDataBase DialogNodeData)
        {
            if (DialogNodeData == null)
            {
                return;
            }

            NodeViewBase nodeView = null;
            //�ָ��ڵ�ĺ��Ĳ��֣������Ľڵ���Ҫ��������лָ���ʽ�����
            switch (DialogNodeData.NodeType)
            {
                case NodeType.Start:
                    {
                        nodeView = new StartNodeView(DialogNodeData);
                        break;
                    }
                case NodeType.RandomDialogNode:
                    {
                        nodeView = new RandomDialogNodeView(DialogNodeData);
                        break;
                    }
                case NodeType.SequentialDialogNode:
                    {
                        nodeView = new SequentialDialogNodeView(DialogNodeData);
                        break;
                    }
                case NodeType.BranchingDialogNode:
                    {
                        nodeView = new BranchingDialogNodeView(DialogNodeData);
                        break;
                    }
                case NodeType.JudgeNode:
                    {
                        nodeView = new JudgeNodeView(DialogNodeData);
                        break;
                    }
                case NodeType.AchievementJudgeNode:
                    {
                        nodeView = new AchievementJudgeNodeDialogView(DialogNodeData);
                        break;
                    }
                case NodeType.TDPJudgeNode:
                    {
                        nodeView = new TDPJudgeNodeView(DialogNodeData);
                        break;
                    }
                case NodeType.EventNode:
                    {
                        nodeView = new EventNodeView(DialogNodeData);
                        break;
                    }
                case NodeType.End:
                    {
                        nodeView = new EndNodeView(DialogNodeData);
                        break;
                    }
                default:
                    {
                        Debug.LogError("δ�ҵ������͵Ľڵ�");
                        break;
                    }
            }

            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.SetPosition(new Rect(DialogNodeData.Position, nodeView.GetPosition().size));
            this.AddElement(nodeView);

            NodeDirt.Add(DialogNodeData, nodeView);
        }


        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="_outputPort">outputPort</param>
        /// <param name="_inputPort">inputPort</param>
        private void AddEdgeByPorts(Port _outputPort, Port _inputPort)
        {
            //��Ȼ�ǲ����ܷ����������Ǳ���һ��
            if (_outputPort.node == _inputPort.node)
            {
                return;
            }

            Edge tempEdge = new Edge()
            {
                input = _inputPort,
                output = _outputPort
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            Add(tempEdge);
        }

        /// <summary>
        /// �ָ��ڵ�����
        /// </summary>
        private void RecoveryEdge(DialogNodeDataBase DialogNodeData)
        {
            if (DialogNodeData == null || DialogNodeData.ChildNode == null)
            {
                return;
            }

            for (int i = 0; i < DialogNodeData.ChildNode.Count; i++)
            {
                //û��������
                if (DialogNodeData.ChildNode[i] == null)
                {
                    continue;
                }

                Port _output = NodeDirt[DialogNodeData].outputContainer[i].Q<Port>();
                Port _input;
                if (NodeDirt.ContainsKey(DialogNodeData.ChildNode[i]))
                {
                    _input = NodeDirt[DialogNodeData.ChildNode[i]].inputContainer[0].Q<Port>();
                    AddEdgeByPorts(_output, _input);
                }



                
            }
        }


    }
}
#endif
