using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPanelSwitch : UISwitch
{
    //母界面
    public MilkBarPanel ParentPanel;

    /*
    //所有的界面
    public List<TownDevelopmentProject.ProjectLocation> ProjectLocationList;

    
    //当前的界面
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
