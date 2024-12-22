


#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



namespace DialogueSystem
{
    public class AchievementJudgeNodeDialogView : NodeViewBase
    {
        public AchievementJudgeNodeDialogView(DialogNodeDataBase dialogNodeData) : base(dialogNodeData)
        {
            title = "AchievementJudgeNodeDialogView";

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
            Port CompletedOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            CompletedOutput.portName = "Completed";
            CompletedOutput.portColor = Color.white;
            CompletedOutput.name = "0";
            outputContainer.Add(CompletedOutput);

            Port NotCompletedOutput = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            NotCompletedOutput.portName = "NotCompleted";
            NotCompletedOutput.portColor = Color.white;
            NotCompletedOutput.name = "1";
            outputContainer.Add(NotCompletedOutput);



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
