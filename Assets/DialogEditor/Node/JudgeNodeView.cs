#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



namespace DialogueSystem
{
    public class JudgeNodeView : NodeViewBase
    {
        public JudgeNodeView(DialogNodeDataBase dialogNodeData) : base(dialogNodeData)
        {
            title = "ConditionalNode";

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
            Port trueOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            trueOutput.portName = "True";
            trueOutput.portColor = Color.green;
            trueOutput.name = "0";
            outputContainer.Add(trueOutput);

            Port falseOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            falseOutput.portName = "False";
            falseOutput.portColor = Color.red;
            falseOutput.name = "1";
            outputContainer.Add(falseOutput);

            if (DialogNodeData.ChildNode.Count == 0)
            {
                DialogNodeData.ChildNode.Add(null);
                DialogNodeData.ChildNode.Add(null);
            }

            RefreshExpandedState();
        }

    }
}
#endif