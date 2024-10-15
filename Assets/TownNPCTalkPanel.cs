using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem;
using TMPro;
using System.Reflection;
using System;

public class TownNPCTalkPanel : NPCTalkPanel
{
    public PlayerPokemon player;

    /// <summary>
    /// 初始对话节点
    /// </summary>
    protected DialogNode InitialNode;
    /// <summary>
    /// 当前对话节点
    /// </summary>
    protected DialogNode CurrentNode;
    /// <summary>
    /// 当前NPC对话状态
    /// </summary>
    protected NPCDialogState DialogState;


    /// <summary>
    /// 和玩家交互的界面
    /// </summary>
    public GameObject PlayerInterfacePanel;


    /// <summary>
    /// NPC对话树
    /// </summary>
    public DialogTree NPCDialogTree;

    /// <summary>
    /// 当前的对话节点
    /// </summary>
    public DialogNodeDataBase CurrentDialogNode;

    /// <summary>
    /// 当前在对话队列里的节点
    /// </summary>
    public List<DialogNodeDataBase> InQueueDialogNode = new List<DialogNodeDataBase> { };

    /// <summary>
    /// 对话的队列
    /// </summary>
    public Queue<DialogString> DialogQueue = new Queue<DialogString> { };


    //是否打开玩家互动界面
    bool isInterfacePanelOpen = false;

    //六个按钮
    TownNpcDialogOptionButton[] ButtonList = new TownNpcDialogOptionButton[6] {null, null, null, null, null, null};

    /// <summary>
    /// 玩家离开时重置对话框
    /// </summary>
    public override void PlayerExit()
    {
        if (TalkInformation != null)
        {
            Debug.Log(DialogQueue.Count);
            //初始化六个按钮
            CurrentNode = InitialNode;
            for (int i = 0; i < 6; i++)
            {
                ButtonList[i].ParentPanel = this;
                ButtonList[i].gameObject.SetActive(false);
            }

            InitDialogTreeData();
            //SetText();

            //设对话面板和对话暂停为false
            isTalkPuse = false;
            gameObject.SetActive(false);
            
            if (PlayerInterfacePanel) { PlayerInterfacePanel.SetActive(false); }
        }

    }


    /// <summary>
    /// 初始化对话树数据
    /// </summary>
    private void InitDialogTreeData()
    {
        if (NPCDialogTree.StartNodeData == null)
        {
            Debug.LogError("The Start node does not exist in the DialogTree file");
            return;
        }

        Debug.Log("isInterfacePanelOpen" + isInterfacePanelOpen);
        if (!isInterfacePanelOpen) {
            //StartNode只有一个接口
            CurrentDialogNode = NPCDialogTree.StartNodeData.ChildNode[0];
            if (!InQueueDialogNode.Contains(NPCDialogTree.StartNodeData)) { InQueueDialogNode.Add(NPCDialogTree.StartNodeData); }

            Debug.Log("Clear");
            DialogQueue.Clear();
            InQueueDialogNode.Clear();
        }
        else
        {
            isInterfacePanelOpen = false;
        }
    }



    protected void NPCTPAwake()
    {
        ParentTownNPC = transform.parent.parent.GetComponent<TownNPC>();
        TalkInformation = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        HeadIconImage = transform.GetChild(5).GetChild(0).GetComponent<Image>();
        

        //初始化六个按钮
        CurrentNode = InitialNode;
        if (ButtonList[0] == null) { ButtonList[0] = transform.GetChild(4).GetChild(0).GetComponent<TownNpcDialogOptionButton>(); }
        if (ButtonList[1] == null){  ButtonList[1] = transform.GetChild(4).GetChild(1).GetComponent<TownNpcDialogOptionButton>();}
        if (ButtonList[2] == null){    ButtonList[2] = transform.GetChild(4).GetChild(2).GetComponent<TownNpcDialogOptionButton>();}
        if (ButtonList[3] == null){     ButtonList[3] = transform.GetChild(4).GetChild(3).GetComponent<TownNpcDialogOptionButton>(); }
        if (ButtonList[4] == null){    ButtonList[4] = transform.GetChild(4).GetChild(4).GetComponent<TownNpcDialogOptionButton>();}
        if (ButtonList[5] == null){    ButtonList[5] = transform.GetChild(4).GetChild(5).GetComponent<TownNpcDialogOptionButton>(); }
        for (int i = 0; i < 6; i++)
        {
            ButtonList[i].ParentPanel = this;
            ButtonList[i].gameObject.SetActive(false);
        }

        InitDialogTreeData();
        SetText();
    }


    public void NPCTPContinue()
    {
        if (!isTalkPuse)
        {


            if (DialogQueue.Count == 0 && CurrentDialogNode.isLastNode())
            {
                PlayerExit();
            }
            else
            {
                SetText();
            }

        }
    }



    /// <summary>
    /// 选择相应的分支
    /// </summary>
    public void SelectOption(int index)
    {
        CurrentDialogNode = CurrentDialogNode.ChildNode[0].ChildNode[index];
        if (InQueueDialogNode.Contains(CurrentDialogNode))
        {
            InQueueDialogNode.Remove(CurrentDialogNode);
        }
        SetText();
    }
    

    


    /// <summary>
    /// 设置文本内容和选项按钮
    /// </summary>
    public void SetText()
    {
        Debug.Log(CurrentDialogNode.name);

        //节点判断
        if (DialogQueue.Count == 0)
        {
            //当前队列为空时，如果当前节点已被读取，进行下一个节点
            if (InQueueDialogNode.Contains(CurrentDialogNode))
            {
                if (CurrentDialogNode.ChildNode != null && CurrentDialogNode.ChildNode.Count != 0)
                {
                    switch (CurrentDialogNode.ChildNode[0].NodeType)
                    {
                        case NodeType.Start:
                            CurrentDialogNode = CurrentDialogNode.ChildNode[0];
                            break;
                        case NodeType.SequentialDialogNode:
                            
                            CurrentDialogNode = CurrentDialogNode.ChildNode[0];
                            if (InQueueDialogNode.Contains(CurrentDialogNode)) {
                                InQueueDialogNode.Remove(CurrentDialogNode);
                            }
                            break;
                        case NodeType.RandomDialogNode:
                            break;
                        case NodeType.BranchingDialogNode:
                            return;
                        case NodeType.JudgeNode:
                            if (CurrentDialogNode.ChildNode != null && CurrentDialogNode.ChildNode[0].OutputItems != null && CurrentDialogNode.ChildNode[0].OutputItems.Count != 0)
                            {
                                JudgeNodeNext();
                            }
                            return;
                        case NodeType.End:
                            PlayerExit();
                            return;
                        case NodeType.EventNode:
                            if (CurrentDialogNode.ChildNode != null && CurrentDialogNode.ChildNode[0].OutputItems != null && CurrentDialogNode.ChildNode[0].OutputItems.Count != 0)
                            {
                                EventNodeNext();
                            }
                            return;
                        default:
                            PlayerExit();
                            return;
                    }
                }
            }
            //当前队列为空时，如果当前节点未被读取，读取当前节点
            if (!InQueueDialogNode.Contains(CurrentDialogNode))
            {
                InQueueDialogNode.Add(CurrentDialogNode);
                if (CurrentDialogNode.OutputItems != null && CurrentDialogNode.OutputItems.Count != 0)
                {
                    switch (CurrentDialogNode.NodeType)
                    {
                        case NodeType.Start:
                            CurrentDialogNode = CurrentDialogNode.ChildNode[0];
                            break;
                        case NodeType.SequentialDialogNode:
                            for (int i = 0; i < CurrentDialogNode.OutputItems.Count; i++)
                            {
                                DialogQueue.Enqueue(CurrentDialogNode.OutputItems[i]);
                            }
                            break;
                        case NodeType.BranchingDialogNode:
                            return;
                        case NodeType.RandomDialogNode:
                            break;
                        case NodeType.JudgeNode:
                            if (CurrentDialogNode.ChildNode != null && CurrentDialogNode.ChildNode[0].OutputItems != null && CurrentDialogNode.ChildNode[0].OutputItems.Count != 0)
                            {
                                JudgeNodeNext();
                            }
                            return;
                        case NodeType.EventNode:
                            if (CurrentDialogNode.ChildNode != null && CurrentDialogNode.ChildNode[0].OutputItems != null && CurrentDialogNode.ChildNode[0].OutputItems.Count != 0)
                            {
                                EventNodeNext();
                            }
                            return;
                        case NodeType.End:
                            PlayerExit();
                            return;
                        default:
                            PlayerExit();
                            return;
                    }
                }
            }
            
        }
        //重置按钮
        for (int i = 0; i < 6; i++)
        {
            ButtonList[i].gameObject.SetActive(false);
        }

        //输出文本和表情
        if (DialogQueue.Count != 0)
        {
            DialogString d = DialogQueue.Dequeue();
            TalkInformation.text = d.DialogueString;
            TalkInformation.text = TalkInformation.text.Replace("\\n", "\n");
            HeadIconImage.sprite = ParentTownNPC.NPCFace(d.DialogueFace);
            if (d.DialogueString == "_打开面板") { 
                isInterfacePanelOpen = true;
            }

            if (DialogQueue.Count == 0 )
            {
                if (CurrentDialogNode.ChildNode != null && CurrentDialogNode.ChildNode.Count != 0) {
                    
                    switch (CurrentDialogNode.ChildNode[0].NodeType)
                    {
                        case NodeType.BranchingDialogNode:
                            //设置分支选项
                            if ((CurrentDialogNode.ChildNode[0].ChildNode != null && CurrentDialogNode.ChildNode[0].ChildNode.Count != 0))
                            {
                                for (int i = 0; i < CurrentDialogNode.ChildNode[0].ChildNode.Count; i++)
                                {
                                    ButtonList[i].gameObject.SetActive(true);
                                    ButtonList[i].SetButton(i, CurrentDialogNode.ChildNode[0].OutputItems[i].DialogueString);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /*
        //如果有互动界面 切换至此时打开互动界面
        
        */
        if (PlayerInterfacePanel != null && !PlayerInterfacePanel.gameObject.activeInHierarchy && isInterfacePanelOpen)
        {
            isTalkPuse = true;
            gameObject.SetActive(false);
            PlayerInterfacePanel.SetActive(true);
        }




    }


    /// <summary>
    /// 事件节点
    /// </summary>
    public void EventNodeNext()
    {
        if (SaveLoader.saveLoader != null)
        {
            Type type = typeof(NPCDialogState);
            // 获取静态实例
            NPCDialogState DialogState = SaveLoader.saveLoader.saveData.TownNPCDialogState;


            for (int i = 0; i < CurrentDialogNode.OutputItems.Count; i++) {
                // 获取实例字段信息
                FieldInfo instanceField = type.GetField(CurrentDialogNode.OutputItems[i].DialogueString, BindingFlags.Public | BindingFlags.Instance);

                if (instanceField != null)
                {
                    object Value = CurrentDialogNode.OutputItems[i].Value;

                    switch (instanceField.FieldType)
                    {
                        case Type t when t == typeof(bool):
                           
                            bool resultb;
                            if (Boolean.TryParse((string)Value, out resultb)) {
                                instanceField.SetValue(DialogState, resultb);
                            }
                            break;
                        case Type t when t == typeof(int):
                            int resultint;
                            if (Int32.TryParse((string)Value, out resultint))
                            {
                                instanceField.SetValue(DialogState, resultint);
                            }
                            
                            break;
                        case Type t when t == typeof(string):
                            instanceField.SetValue(DialogState, Value); ;
                            break;
                        default:
                            Debug.Log("ErrorType");
                            break;
                    }

                }
            }

            CurrentDialogNode = CurrentDialogNode.ChildNode[0];

        }


        SetText();
    }


    /// <summary>
    /// 判断节点
    /// </summary>
    public void JudgeNodeNext()
    {
        Debug.Log(CurrentDialogNode.ChildNode[0].NodeType + "+" + CurrentDialogNode.NodeType);
        //判断节点正确连接，有两个选项
        if (CurrentDialogNode.NodeType == NodeType.JudgeNode && CurrentDialogNode.ChildNode.Count == 2 && SaveLoader.saveLoader != null)
        {
            Type type = typeof(NPCDialogState);
            // 获取静态实例
            NPCDialogState DialogState = SaveLoader.saveLoader.saveData.TownNPCDialogState;

            // 获取实例字段信息
            FieldInfo instanceField = type.GetField(CurrentDialogNode.OutputItems[0].DialogueString, BindingFlags.Public | BindingFlags.Instance);

            if (instanceField != null)
            {
                if ((bool)instanceField.GetValue(DialogState))
                {
                    CurrentDialogNode = CurrentDialogNode.ChildNode[0];
                    Debug.Log(instanceField.Name + (bool)instanceField.GetValue(DialogState) + CurrentDialogNode.name);
                }
                else
                {
                    CurrentDialogNode = CurrentDialogNode.ChildNode[1];
                    Debug.Log(instanceField.Name + (bool)instanceField.GetValue(DialogState) + CurrentDialogNode.name);
                }
            }
            else
            {
                CurrentDialogNode = CurrentDialogNode.ChildNode[0];
                Debug.Log("instanceFieldNull");
            }

        }
        //判断节点错误链接,仅有一个选项
        else
        {
            CurrentDialogNode = CurrentDialogNode.ChildNode[0];
            Debug.Log("FailNodeContect");
        }
        Debug.Log("SetText");
        SetText();
    }


    /// <summary>
    /// 关闭玩家互动界面
    /// </summary>
    public void ClosePlayerInterfacePanel()
    {
        CurrentDialogNode = CurrentDialogNode.ChildNode[0];
        PlayerInterfacePanel.SetActive(false);
        isTalkPuse = false;
        gameObject.SetActive(true);
        
    }

    
}


