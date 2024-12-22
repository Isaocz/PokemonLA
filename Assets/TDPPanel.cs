using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TDPPanel : MonoBehaviour
{


    /// <summary>
    /// 按钮预制体
    /// </summary>
    public TownDevelopmentProjectButton Button;

    /// <summary>
    /// 按钮的父对象
    /// </summary>
    public GameObject ButtonParent;

    /// <summary>
    /// 显示当前AP余额
    /// </summary>
    public Text NowAP;


    /// <summary>
    /// 切换当前项目页的切换键
    /// </summary>
    public TDPPanelSwitch tdpPanelSwitch;


    public UIDescribe uiDescribe;


    WoodenHouseConkeldurrTalkPanel ParentPanel;



    //所有项目列表
    List<List<TownDevelopmentProject>> ProjectList = new List<List<TownDevelopmentProject>>
    {
        new List<TownDevelopmentProject>{ },//小镇
        new List<TownDevelopmentProject>{ },//修建木屋
        new List<TownDevelopmentProject>{ },//冒险者工会
        new List<TownDevelopmentProject>{ },//奶吧
        new List<TownDevelopmentProject>{ },//技能画廊
        new List<TownDevelopmentProject>{ },//保育圆
        new List<TownDevelopmentProject>{ },//保育圆二楼
        new List<TownDevelopmentProject>{ },//道具商店
        new List<TownDevelopmentProject>{ },//头目俱乐部
        new List<TownDevelopmentProject>{ },//推石俱乐部
    };








    private void OnEnable()
    {
        ParentPanel = transform.parent.GetChild(0).GetComponent<WoodenHouseConkeldurrTalkPanel>();
        tdpPanelSwitch.RemoveAllSwitch();
        tdpPanelSwitch.ParentPanel = this;
        SetGroupPanel();
        
    }

    void ResetList()
    {
        ProjectList = new List<List<TownDevelopmentProject>>
        {
            new List<TownDevelopmentProject>{ },//小镇
            new List<TownDevelopmentProject>{ },//修建木屋
            new List<TownDevelopmentProject>{ },//冒险者工会
            new List<TownDevelopmentProject>{ },//奶吧
            new List<TownDevelopmentProject>{ },//技能画廊
            new List<TownDevelopmentProject>{ },//保育圆
            new List<TownDevelopmentProject>{ },//保育圆二楼
            new List<TownDevelopmentProject>{ },//道具商店
            new List<TownDevelopmentProject>{ },//头目俱乐部
            new List<TownDevelopmentProject>{ },//推石俱乐部
        };
    }


    public void SetGroupPanel()
    {
        if (SaveLoader.saveLoader != null)
        {
            TownLoader.CheckforUnlock();

            _mTool.RemoveAllChild(ButtonParent);

            NowAP.text = SaveLoader.saveLoader.saveData.AP.ToString();
            List <TownDevelopmentProject> l = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;
            ResetList();


            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].ProjectProgress == TownDevelopmentProject.ProjectStatus.NotStarted || l[i].ProjectProgress == TownDevelopmentProject.ProjectStatus.NotSelected)
                {
                    ProjectList[(int)l[i].ProjectLoca].Add(l[i]);
                    //Instantiate(Button, ButtonParent.transform).SetTDPButton(l[i], uiDescribe , ParentPanel , this);
                }
                
            }
            for (int i = 0; i < ProjectList.Count; i ++)
            {
                if (ProjectList[i].Count != 0) 
                {
                    tdpPanelSwitch.AddTDPSwitch(ProjectList[i][0].ProjectLoca);
                }
            }

            SetTDPButton((int)tdpPanelSwitch.ProjectLocationList[tdpPanelSwitch.NowProjectLocationIndex]);
        }
    }


    public void SetTDPButton(int Index)
    {
        List<TownDevelopmentProject> l = ProjectList[Index];
        _mTool.RemoveAllChild(ButtonParent);
        Debug.Log("Remove");
        for (int i = 0; i < l.Count; i++)
        {
            Instantiate(Button, ButtonParent.transform).SetTDPButton(l[i], uiDescribe, ParentPanel, this);
        }
        RefreshPanel();
    }





    /// <summary>
    /// 购买项目后刷新界面
    /// </summary>
    public void RefreshPanel()
    {
        NowAP.text = SaveLoader.saveLoader.saveData.AP.ToString();
        for (int i = 0; i < ButtonParent.transform.childCount; i++)
        {
            ButtonParent.transform.GetChild(i).GetComponent<TownDevelopmentProjectButton>().SetButtonState();
        }
    }

}
