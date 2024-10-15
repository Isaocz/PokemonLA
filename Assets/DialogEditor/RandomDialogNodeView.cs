#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public class RandomDialogNodeView : NodeViewBase
    {
        private int nextIndex = 0;

        public RandomDialogNodeView(DialogNodeDataBase dialogNodeData) : base(dialogNodeData)
        {
            title = "RandomDialogNode";

            Port input = GetPortForNode(this, Direction.Input, Port.Capacity.Multi);
            Port output = GetPortForNode(this, Direction.Output, Port.Capacity.Single);
            input.portName = "Input";
            input.portColor = Color.magenta;
            output.portName = "Output";
            output.portColor = Color.magenta;
            output.name = "0";

            inputContainer.Add(input);
            outputContainer.Add(output);

            //������
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

            //�Ӹ��жϣ���Ȼÿ��һ�δ�һ��
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

            //���˸�û�й��ܵİ��������������������û��ʲôʵ���ԵĹ��ܹ���
            Button background = new Button();

            TextField textField = new TextField();
            textField.name = nextIndex.ToString();
            textField.style.minWidth = 160;
            textField.style.maxWidth = 160;
            //��ʼ��
            textField.SetValueWithoutNotify(DialogNodeData.OutputItems[nextIndex].DialogueString);

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


            // ����ö���ֶ�
            EnumField emotionField = new EnumField("Emotion", DialogNodeData.OutputItems[nextIndex].DialogueFace);
            emotionField.name = nextIndex.ToString();

            emotionField.RegisterValueChangedCallback(evt =>
            {
                Debug.Log(emotionField.name);
                if (int.TryParse(emotionField.name, out int index))
                {
                    DialogNodeData.OutputItems[index].DialogueFace = (DialogString.Face)evt.newValue;
                }
                else
                {
                    Debug.LogError("textField.name(string) to int fail");
                }
            });

            background.Add(textField);
            background.Add(emotionField);
            extensionContainer.Add(background);
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