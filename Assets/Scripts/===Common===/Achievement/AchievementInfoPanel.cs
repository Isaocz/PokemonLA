using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementInfoPanel : MonoBehaviour
{
    public Sprite[] MissionLevelMark;

    public MissionScrollBar Bar;
    public TextMeshProUGUI MissionName;
    public Image MissionClient;
    public TextMeshProUGUI ClientTitle;
    public TextMeshProUGUI ClientName;
    public Image MissionLevel;
    public TextMeshProUGUI ClientMessege;
    public TextMeshProUGUI MissionInfo;
    public TextMeshProUGUI MissionReward;


    public void SetPanel(PlayerAchievement a)
    {
        Bar.SetBar(a);
        MissionName.text = ((a.achievement.AchiName.Length <= 7) ? _mTool.AddSpaceInString(a.achievement.AchiName) : a.achievement.AchiName ) ;

        MissionClient.sprite = MissionNPCList.list.NPCList[a.achievement.AchiClientIndex].NPCIcon;
        ClientTitle.text = MissionNPCList.list.NPCList[a.achievement.AchiClientIndex].NPCChineseTitle;
        ClientName.text = MissionNPCList.list.NPCList[a.achievement.AchiClientIndex].NPCChineseName;
        MissionLevel.sprite = MissionLevelMark[a.achievement.AchLevel];
        ClientMessege.text = a.achievement.AchiClientMessage;
        MissionInfo.text = a.achievement.AchiDescribe;
        MissionReward.text = a.achievement.AchiReward;
    }
}
