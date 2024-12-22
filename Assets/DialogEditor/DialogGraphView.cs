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
            //增加格子背景
            Insert(0, new GridBackground());

            //增加内容缩放，拖动，拖拽，框选控制器
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            //框选 bug
            //大坑！控制器之间存在优先级
            //这就是为什么框选控制器放在选择拖放节点控制器之前会导致节点无法移动
            //因为框选的优先级更高
            this.AddManipulator(new RectangleSelector());

            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    //此处填你项目对应 uss 文件路径

                    "Assets/DialogEditor/DialogGraphWindow.uss");
            styleSheets.Add(styleSheet);


            if (treeData != null)
            {
                contentViewContainer.transform.position = treeData.GraphViewData.Position;
                contentViewContainer.transform.scale = treeData.GraphViewData.Scale;
            }

            //监听graphView变化事件
            graphViewChanged += OnGraphViewChanged;
            //监听视图Transform变化事件
            viewTransformChanged += OnViewTransformChanged;

            //简单的单例模式
            Instance = this;



        }




        /// <summary>
        /// 菜单点击时鼠标位置
        /// </summary>
        private Vector2 clickPosition;

        /// <summary>
        /// 节点点击事件
        /// </summary>
        public Action<NodeViewBase> OnNodeSelected;

        public static DialogTree treeData = null;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            Debug.Log(evt.mousePosition);

            //将鼠标世界坐标转为视图本地坐标
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




        //节点链接规则
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node &&
                endPort.portType == startPort.portType
            ).ToList();
        }



        /// <summary>
        /// 节点图变化事件
        /// </summary>
        /// <param name="graphviewchange"></param>
        /// <returns></returns>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange)
        {
            if (graphviewchange.elementsToRemove != null)
            {
                graphviewchange.elementsToRemove.ForEach(elem =>
                {
                    //连线删除
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
                //创建连线
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
        /// graphView的Transform发生变化时触发
        /// </summary>
        /// <param name="graphView"></param>
        private void OnViewTransformChanged(GraphView graphView)
        {
            if (treeData != null)
            {
                //保存视图Transform信息
                treeData.GraphViewData.Position = contentViewContainer.transform.position;
                treeData.GraphViewData.Scale = contentViewContainer.transform.scale;
            }
        }


        //确保目录存在
        private void MakeSureTheFolder()
        {
            //TODO：做成可自行设置的对话资源文件部署
            if (!AssetDatabase.IsValidFolder("Assets/DialogueData/NodeData"))
            {
                AssetDatabase.CreateFolder("Assets", "DialogueData");
                AssetDatabase.CreateFolder("Assets/DialogueData", "NodeData");
            }
        }

        /// <summary>
        /// 新建节点
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

            //创建节点的核心，新增的节点需要在这里进行创建方式的添加
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
                        Debug.LogError("未找到该类型的节点");
                        break;
                    }
            }

            //添加节点被选择事件
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.SetPosition(new Rect(position, nodeView.GetPosition().size));


            //对Start节点做个特判
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
        /// 临时字典，用于初始化节点图的，用完记得把内存释放掉
        /// </summary>
        private Dictionary<DialogNodeDataBase, NodeViewBase> NodeDirt;

        /// <summary>
        /// 重置节点图
        /// </summary>
        public void ResetNodeView()
        {
            Debug.Log(treeData);

            if (treeData != null)
            {
                //初始化字典
                NodeDirt = new Dictionary<DialogNodeDataBase, NodeViewBase>();
                var nodeData = treeData.ChildNodeDataList;

                //检查StartNode是否存在
                if (treeData.StartNodeData == null)
                {
                    CreateNode(NodeType.Start);
                }
                else
                {
                    RecoveryNode(treeData.StartNodeData);
                }

                //恢复节点
                foreach (var node in nodeData)
                {
                    RecoveryNode(node);
                }

                //恢复节点边
                RecoveryEdge(treeData.StartNodeData);
                foreach (var node in nodeData)
                {
                    RecoveryEdge(node);
                }

                //清除字典
                NodeDirt.Clear();
            }
        }


        /// <summary>
        /// 恢复节点
        /// </summary>
        /// <param name="DialogNodeData"></param>
        private void RecoveryNode(DialogNodeDataBase DialogNodeData)
        {
            if (DialogNodeData == null)
            {
                return;
            }

            NodeViewBase nodeView = null;
            //恢复节点的核心部分，新增的节点需要在这里进行恢复方式的添加
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
                        Debug.LogError("未找到该类型的节点");
                        break;
                    }
            }

            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.SetPosition(new Rect(DialogNodeData.Position, nodeView.GetPosition().size));
            this.AddElement(nodeView);

            NodeDirt.Add(DialogNodeData, nodeView);
        }


        /// <summary>
        /// 链接两个点
        /// </summary>
        /// <param name="_outputPort">outputPort</param>
        /// <param name="_inputPort">inputPort</param>
        private void AddEdgeByPorts(Port _outputPort, Port _inputPort)
        {
            //虽然是不可能发生，但还是保守一点
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
        /// 恢复节点连线
        /// </summary>
        private void RecoveryEdge(DialogNodeDataBase DialogNodeData)
        {
            if (DialogNodeData == null || DialogNodeData.ChildNode == null)
            {
                return;
            }

            for (int i = 0; i < DialogNodeData.ChildNode.Count; i++)
            {
                //没连就跳过
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
