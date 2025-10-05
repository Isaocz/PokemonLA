using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownDevelopmentProjectButton : MonoBehaviour
{


    public Text Price;

    public Text ProjectType;

    public Image Mark;
    public Sprite APMark;
    public Sprite NotSelectMark;

    public Text ProjectName;

    public UICallDescribe Describe;

    int TDPIndex;

    WoodenHouseConkeldurrTalkPanel TalkPanel;

    TDPPanel ParentPanel;

    Button b;


    public void SetTDPButton(TownDevelopmentProject p , UIDescribe ui , WoodenHouseConkeldurrTalkPanel panel , TDPPanel parent )
    {
        b = transform.GetComponent<Button>();
        TalkPanel = panel;
        ParentPanel = parent;
        ProjectType.text = p.ProjectTypeChinese();
        if (p.ProjectAPPrice > 0 && p.ProjectProgress != TownDevelopmentProject.ProjectStatus.NotSelected) 
        { Price.text = p.ProjectAPPrice.ToString(); Mark.sprite = APMark; }
        else  
        { Price.text = ""; Mark.sprite = NotSelectMark; }
        ProjectName.text = p.ProjectName;
        Describe.DescribeText = p.ProjectDescribe;
        Describe.DescribeUI = ui;
        TDPIndex = p.ProjectIndex;

        SetButtonState();
    }



    public void SelectTDPButton()
    {

        if (b != null && 
            ( SaveLoader.saveLoader.saveData.AP >= SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[TDPIndex].ProjectAPPrice ||
              SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[TDPIndex].ProjectProgress == TownDevelopmentProject.ProjectStatus.NotSelected))
        {

            if (SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[TDPIndex].ProjectProgress != TownDevelopmentProject.ProjectStatus.NotSelected) 
            {
                SaveLoader.saveLoader.saveData.AP -= SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[TDPIndex].ProjectAPPrice;
            }

            TalkPanel.StartProject();
            Debug.Log(SaveLoader.saveLoader.saveData.TownNPCDialogState.isStartAProject);
            TalkPanel.gameObject.SetActive(true);
            ParentPanel.gameObject.SetActive(false);
            ParentPanel.RefreshPanel();
            this.gameObject.SetActive(false);

            
            TownLoader.StartProgressProject(TDPIndex);
        }
    }

    /// <summary>
    /// 根据余额设定按钮状态
    /// </summary>
    public void SetButtonState()
    {
        if (b != null && SaveLoader.saveLoader != null)
        {
            if ((SaveLoader.saveLoader.saveData.AP >= SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[TDPIndex].ProjectAPPrice ||
              SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[TDPIndex].ProjectProgress == TownDevelopmentProject.ProjectStatus.NotSelected))
            {
                Price.color = Color.white;
                Mark.color = Color.white;
                ProjectName.color = Color.white;
                ProjectType.color = Color.white;
                b.interactable = true;
            }
            else
            {
                Price.color = new Color(1, 0.1650943f, 0.1650943f, 0.6f);
                Mark.color = new Color(1, 1, 1, 0.6f);
                ProjectName.color = new Color(1, 1, 1, 0.6f);
                ProjectType.color = new Color(1, 1, 1, 0.6f);
                b.interactable = false;
            }
        }
    }

}
