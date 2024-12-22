#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public class BranchingDialogNodeView : NodeViewBase
    {
        private int optionIndex = 0;

        public BranchingDialogNodeView(DialogNodeDataBase dialogNodeData) : base(dialogNodeData)
        {
            title = "BranchingDialogNode";

            Port input = GetPortForNode(this, Direction.Input, Port.Capacity.Multi);
            input.portName = "Input";
            input.portColor = Color.cyan;
            inputContainer.Add(input);

            // 工具条
            Toolbar toolbar = new Toolbar();
            ToolbarButton addButton = new ToolbarButton(AddOption)
            {
                text = "Add Option"
            };
            ToolbarButton delButton = new ToolbarButton(DeleteOption)
            {
                text = "Delete Option"
            };
            toolbar.Add(addButton);
            toolbar.Add(delButton);

            toolbar.style.flexDirection = FlexDirection.RowReverse;
            contentContainer.Add(toolbar);

            // 初始化现有选项
            while (optionIndex < DialogNodeData.OutputItems.Count)
            {
                
                AddOption();
            }

            // 确保至少有一个默认输出端口
            if (optionIndex == 0)
            {
                AddOption();
            }

            if (DialogNodeData.ChildNode.Count < 1)
            {
                DialogNodeData.ChildNode.Add(null);
            }
        }

        public void AddOption()
        {
            if (optionIndex < 6) {
                
                if (DialogNodeData.OutputItems.Count < optionIndex + 1)
                {
                    DialogNodeData.OutputItems.Add(new DialogString());
                }
                while (DialogNodeData.ChildNode.Count < DialogNodeData.OutputItems.Count)
                {
                    DialogNodeData.ChildNode.Add(null);
                }

                // 创建文本字段
                TextField textField = new TextField();
                textField.name = optionIndex.ToString();
                textField.style.minWidth = 160;
                textField.style.maxWidth = 160;
                textField.SetValueWithoutNotify(DialogNodeData.OutputItems[optionIndex].DialogueString);

                textField.RegisterValueChangedCallback(evt =>
                {
                    if (int.TryParse(textField.name, out int index))
                    {
                        DialogNodeData.OutputItems[index].DialogueString = evt.newValue;
                    }
                    else
                    {
                        Debug.LogError("textField.name(string) to int fail");
                    }
                });

                // 创建输出端口
                Port output = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
                output.portName = $"Option {optionIndex}";
                output.portColor = Color.cyan;
                output.name = optionIndex.ToString();

                // 添加到节点
                extensionContainer.Add(textField);
                outputContainer.Add(output);
                RefreshExpandedState();

                optionIndex++;
            }
        }

        public void DeleteOption()
        {
            if (optionIndex > 1)
            {
                optionIndex--;
                DialogNodeData.OutputItems.RemoveAt(DialogNodeData.OutputItems.Count - 1);
                while (DialogNodeData.ChildNode.Count > DialogNodeData.OutputItems.Count)
                {
                    DialogNodeData.ChildNode.RemoveAt(DialogNodeData.ChildNode.Count - 1);
                }
                extensionContainer.Remove(extensionContainer[optionIndex]);
                outputContainer.Remove(outputContainer[optionIndex]);
            }
        }
    }
}
#endif