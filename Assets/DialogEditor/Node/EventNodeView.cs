#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public class EventNodeView : NodeViewBase
    {
        private int nextIndex = 0;
        public EventNodeView(DialogNodeDataBase dialogNodeData) : base(dialogNodeData)
        {
            title = "EventDialogNode";

            Port input = GetPortForNode(this, Direction.Input, Port.Capacity.Multi);
            Port output = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            input.portName = "Input";
            input.portColor = new Color(0.5990566f , 0.7373128f , 1);
            output.portName = "Output";
            output.portColor = new Color(0.5990566f, 0.7373128f, 1);
            output.name = "0";

            inputContainer.Add(input);
            outputContainer.Add(output);

            //工具条
            Toolbar toolbar = new Toolbar();
            ToolbarButton addButton = new ToolbarButton(AddTextField)
            {
                text = "Add"
            };
            ToolbarButton delButton = new ToolbarButton(DeleteTextField)
            {
                text = "Del"
            };
            toolbar.Add(addButton);
            toolbar.Add(delButton);

            toolbar.style.flexDirection = FlexDirection.RowReverse;
            contentContainer.Add(toolbar);

            while (nextIndex < DialogNodeData.OutputItems.Count)
            {
                AddTextField();
            }

            if (DialogNodeData.ChildNode.Count < 1)
            {
                DialogNodeData.ChildNode.Add(null);
            }
        }

        public void AddTextField()
        {
            if (DialogNodeData.OutputItems.Count < nextIndex + 1)
            {
                DialogNodeData.OutputItems.Add(new DialogString());
            }




            var horizontalContainer = new VisualElement();
            horizontalContainer.style.flexDirection = FlexDirection.Row;

            TextField textField01 = new TextField();
            textField01.name = nextIndex.ToString();
            textField01.style.minWidth = 120;
            textField01.style.maxWidth = 120;
            //初始化
            textField01.SetValueWithoutNotify(DialogNodeData.OutputItems[nextIndex].DialogueString);

            textField01.RegisterValueChangedCallback(evt =>
            {
                if (int.TryParse(textField01.name, out int index))
                {
                    DialogNodeData.OutputItems[index].DialogueString = evt.newValue;
                }
                else
                {
                    Debug.LogError("textField.name(string) to int fail");
                }
            });
            horizontalContainer.Add(textField01);


            TextField textField02 = new TextField();
            textField02.name = nextIndex.ToString();
            textField02.style.minWidth = 80;
            textField02.style.maxWidth = 80;
            //初始化
            textField02.SetValueWithoutNotify(DialogNodeData.OutputItems[nextIndex].Value);

            textField02.RegisterValueChangedCallback(evt =>
            {
                if (int.TryParse(textField02.name, out int index))
                {
                    DialogNodeData.OutputItems[index].Value = evt.newValue;
                }
                else
                {
                    Debug.LogError("textField.name(string) to int fail");
                }
            });
            horizontalContainer.Add(textField02);


            extensionContainer.Add(horizontalContainer);
            RefreshExpandedState();

            nextIndex++;







        }

        public void DeleteTextField()
        {
            if (nextIndex > 0)
            {
                nextIndex--;

                DialogNodeData.OutputItems.RemoveAt(DialogNodeData.OutputItems.Count - 1);
                extensionContainer.Remove(extensionContainer[nextIndex]);
            }
        }
    }
}
#endif
