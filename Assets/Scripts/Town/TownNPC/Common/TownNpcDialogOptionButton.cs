using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownNpcDialogOptionButton : MonoBehaviour
{

    /// <summary>
    /// ��ť��Ӧ��ѡ��
    /// </summary>
    public DialogOption ButtonOption;

    public TextMeshProUGUI ButtonText;

    /// <summary>
    /// ��ť��Ӧ�Ľ���
    /// </summary>
    public TownNPCTalkPanel ParentPanel;

    public int ButtonIndex;
    


    public void SetButton(int Index , string s)
    {
        ButtonIndex = Index;
        ButtonText.text = s;
    }

    public void SelectOptionButton()
    {
        if (ParentPanel != null )
        {
            ParentPanel.SelectOption(ButtonIndex);
        }
    }
    
}
