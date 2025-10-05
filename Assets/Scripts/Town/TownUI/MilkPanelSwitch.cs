using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPanelSwitch : UISwitch
{
    //ĸ����
    public MilkBarPanel ParentPanel;

    /*
    //���еĽ���
    public List<TownDevelopmentProject.ProjectLocation> ProjectLocationList;

    
    //��ǰ�Ľ���
    public int NowProjectLocationIndex;


    public void AddTDPSwitch(TownDevelopmentProject.ProjectLocation l)
    {
        ProjectLocationList.Add(l);
        AddSwitch((TownDevelopmentProject.ProjectLocationChinese(l).Length <= 7) ? (_mTool.AddSpaceInString(TownDevelopmentProject.ProjectLocationChinese(l))) : (TownDevelopmentProject.ProjectLocationChinese(l)));
        NowProjectLocationIndex = 0;
    }
    */

    public override void LeftSwitch()
    {
        base.LeftSwitch();
        ParentPanel.SetPanel();
        //NowProjectLocationIndex--;
        //if (NowProjectLocationIndex < 0) { NowProjectLocationIndex = ProjectLocationList.Count - 1; }
        //ParentPanel.SetTDPButton((int)ProjectLocationList[NowProjectLocationIndex]);
    }


    public override void RightSwitch()
    {
        base.RightSwitch();
        ParentPanel.SetPanel();
        //NowProjectLocationIndex++;
        //if (NowProjectLocationIndex > ProjectLocationList.Count - 1) { NowProjectLocationIndex = 0; }
        //ParentPanel.SetTDPButton((int)ProjectLocationList[NowProjectLocationIndex]);
    }

}
