using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPanel : MonoBehaviour
{

    /// <summary>
    /// �����������
    /// </summary>
    public AchievementInfoPanel InfoPanel;

    /// <summary>
    /// ����İ�ťԤ�Ƽ�
    /// </summary>
    public AchievementButton Button;

    /// <summary>
    /// ��ť�ĸ�����
    /// </summary>
    public Transform ButtonParent;

    /// <summary>
    /// �л�
    /// </summary>
    public AchievementPanelSwitch Switch;

    /// <summary>
    /// ����ɵĳɾ�
    /// </summary>
    List<PlayerAchievement> CompleteAchi = new List<PlayerAchievement> { };

    /// <summary>
    /// δ��ɵĳɾ�
    /// </summary>
    List<PlayerAchievement> ProgressAchi = new List<PlayerAchievement> { };

    /// <summary>
    /// ��ǰ�浵�ĳɾ��б�
    /// </summary>
    List<PlayerAchievement> SaveAchiList;

    /// <summary>
    /// ��鵱ǰ���񣬲��ҷ���
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
    /// �����б�
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
        if (Switch.SwitchIndex == 0)        //��ʾ���ڽ��еĳɾ�
        {
            NowList = ProgressAchi;
        }
        else if (Switch.SwitchIndex == 1)        //��ʾ����ɵĳɾ�
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
