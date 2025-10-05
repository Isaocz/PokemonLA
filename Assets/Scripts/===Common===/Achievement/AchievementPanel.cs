using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPanel : MonoBehaviour
{

    /// <summary>
    /// 任务详情界面
    /// </summary>
    public AchievementInfoPanel InfoPanel;

    /// <summary>
    /// 任务的按钮预制件
    /// </summary>
    public AchievementButton Button;

    /// <summary>
    /// 按钮的父对象
    /// </summary>
    public Transform ButtonParent;

    /// <summary>
    /// 切换
    /// </summary>
    public AchievementPanelSwitch Switch;

    /// <summary>
    /// 已完成的成就
    /// </summary>
    List<PlayerAchievement> CompleteAchi = new List<PlayerAchievement> { };

    /// <summary>
    /// 未完成的成就
    /// </summary>
    List<PlayerAchievement> ProgressAchi = new List<PlayerAchievement> { };

    /// <summary>
    /// 当前存档的成就列表
    /// </summary>
    List<PlayerAchievement> SaveAchiList;

    /// <summary>
    /// 检查当前任务，并且分类
    /// </summary>
    void CheckAchiList()
    {
        ResetAchiList();
        for (int i = 0; i < SaveAchiList.Count; i++)
        {
            if (SaveAchiList[i].State != AchievementList.AchStatus.Locked )
            {
                SaveAchiList[i].isAchievementUnlock();
                if      (SaveAchiList[i].State == AchievementList.AchStatus.Completed)  { CompleteAchi.Add(SaveAchiList[i]); }
                else if (SaveAchiList[i].State == AchievementList.AchStatus.InProgress) { ProgressAchi.Add(SaveAchiList[i]); }
            }
        }
    }

    /// <summary>
    /// 重置列表
    /// </summary>
    void ResetAchiList()
    {
        CompleteAchi.Clear();
        CompleteAchi = new List<PlayerAchievement> { };
        ProgressAchi.Clear();
        ProgressAchi = new List<PlayerAchievement> { };
    }


    public void SetPanel()
    {
        CheckAchiList();
        _mTool.RemoveAllChild(ButtonParent.gameObject);
        List<PlayerAchievement> NowList = new List<PlayerAchievement> { };
        if (Switch.SwitchIndex == 0)        //显示正在进行的成就
        {
            NowList = ProgressAchi;
        }
        else if (Switch.SwitchIndex == 1)        //显示已完成的成就
        {
            NowList = CompleteAchi;
        }

        if (NowList.Count == 0)
        {
            InfoPanel.gameObject.SetActive(false);
        }
        else
        {
            InfoPanel.gameObject.SetActive(true);
            for (int i = 0; i < NowList.Count; i++)
            {
                AchievementButton b = Instantiate(Button, ButtonParent);
                b.SetAchievement(NowList[i]);
                b.ParentPanel = this;
            }
            InfoPanel.SetPanel(NowList[0]);
        }



    }


    private void OnEnable()
    {
        if (SaveLoader.saveLoader != null)
        {
            SaveAchiList = SaveLoader.saveLoader.saveData.PlayerAchievementList;
            SetPanel();
        }
        
    }

}
