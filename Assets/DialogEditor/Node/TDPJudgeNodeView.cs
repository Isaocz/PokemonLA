

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



namespace DialogueSystem
{
    public class TDPJudgeNodeView : NodeViewBase
    {
        public TDPJudgeNodeView(DialogNodeDataBase dialogNodeData) : base(dialogNodeData)
        {
            title = "TDPJudgeNodeView";

            // 创建输入端口
            Port input = GetPortForNode(this, Direction.Input, Port.Capacity.Multi);
            input.portName = "Input";
            input.portColor = Color.cyan;
            inputContainer.Add(input);
            if (DialogNodeData.OutputItems.Count == 0)
            {
                DialogNodeData.OutputItems.Add(new DialogString());
            }












            Button background = new Button();

            TextField textField = new TextField();
            textField.name = "0";
            textField.style.minWidth = 160;
            textField.style.maxWidth = 160;
            //初始化
            textField.SetValueWithoutNotify(DialogNodeData.OutputItems[0].DialogueString);

            textField.RegisterValueChangedCallback(evt =>
            {
                DialogNodeData.OutputItems[0].DialogueString = evt.newValue;
            });

            background.Add(textField);
            extensionContainer.Add(background);
            RefreshExpandedState();






            // 创建输出端口
            Port LockedOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            LockedOutput.portName = "Locked";
            LockedOutput.portColor = Color.white;
            LockedOutput.name = "0";
            outputContainer.Add(LockedOutput);

            Port NotStartedOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            NotStartedOutput.portName = "NotStarted";
            NotStartedOutput.portColor = Color.white;
            NotStartedOutput.name = "1";
            outputContainer.Add(NotStartedOutput);

            Port InProgressOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            InProgressOutput.portName = "InProgress";
            InProgressOutput.portColor = Color.white;
            InProgressOutput.name = "2";
            outputContainer.Add(InProgressOutput);

            Port CompletedOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            CompletedOutput.portName = "Completed";
            CompletedOutput.portColor = Color.white;
            CompletedOutput.name = "3";
            outputContainer.Add(CompletedOutput);

            Port NotSelectedOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            NotSelectedOutput.portName = "NotSelected";
            NotSelectedOutput.portColor = Color.white;
            NotSelectedOutput.name = "4";
            outputContainer.Add(NotSelectedOutput);

            if (DialogNodeData.ChildNode.Count == 0)
            {
                DialogNodeData.ChildNode.Add(null);
                DialogNodeData.ChildNode.Add(null);
                DialogNodeData.ChildNode.Add(null);
                DialogNodeData.ChildNode.Add(null);
                DialogNodeData.ChildNode.Add(null);
            }

            RefreshExpandedState();
        }

    }
}
#endif