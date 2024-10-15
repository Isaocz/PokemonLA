using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownNpcDialogOptionButton : MonoBehaviour
{

    /// <summary>
    /// 按钮对应的选项
    /// </summary>
    public DialogOption ButtonOption;

    public TextMeshProUGUI ButtonText;

    /// <summary>
    /// 按钮对应的界面
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
