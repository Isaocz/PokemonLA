using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// 表示人任务的按钮
/// </summary>
public class AchievementButton : Button
{
    /// <summary>
    /// 母界面
    /// </summary>
    public AchievementPanel ParentPanel;

    /// <summary>
    /// 按钮对应的成就
    /// </summary>
    public PlayerAchievement Achievement;

    /// <summary>
    /// 显示成就的名字
    /// </summary>
    public TextMeshProUGUI AchievementName;

    /// <summary>
    /// 显示成就的进度
    /// </summary>
    public MissionScrollBar AchievementBar;


    public void SetAchievement( PlayerAchievement achievement )
    {
        Achievement = achievement;
        AchievementName.text = ((achievement.achievement.AchiName.Length <= 7) ? _mTool.AddSpaceInString(achievement.achievement.AchiName) : achievement.achievement.AchiName);
        AchievementBar.SetBar(achievement);
    }

    public void SwitchInfoPanel()
    {
        ParentPanel.InfoPanel.SetPanel(Achievement);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SwitchInfoPanel();
    }
}
