using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// ��ʾ������İ�ť
/// </summary>
public class AchievementButton : Button
{
    /// <summary>
    /// ĸ����
    /// </summary>
    public AchievementPanel ParentPanel;

    /// <summary>
    /// ��ť��Ӧ�ĳɾ�
    /// </summary>
    public PlayerAchievement Achievement;

    /// <summary>
    /// ��ʾ�ɾ͵�����
    /// </summary>
    public TextMeshProUGUI AchievementName;

    /// <summary>
    /// ��ʾ�ɾ͵Ľ���
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
