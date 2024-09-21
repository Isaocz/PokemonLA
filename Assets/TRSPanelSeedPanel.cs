using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRSPanelSeedPanel : MonoBehaviour
{

    /// <summary>
    /// ���ӵ�16����λ��
    /// </summary>
    public int[] Seed = new int[] { 0 , 0, 0, 0, 0, 0, 0, 0 };
    /// <summary>
    /// �������ӵĿ���
    /// </summary>
    public isUseSeedToggle SeedToggle;
    /// <summary>
    /// �������ý���
    /// </summary>
    public GameObject SeedSetPanel;

    public UICallDescribe uICallDescribe;

    public TownSelectRolePanel ParentTRSPanel;



    public void ChangeToggle()
    {
        if (SeedToggle.isOn)
        {
            SeedSetPanel.gameObject.SetActive(true);
            StartPanelPlayerData.PlayerData.isSeedGame = true;
        }
        else
        {
            SeedSetPanel.gameObject.SetActive(false);
            StartPanelPlayerData.PlayerData.isSeedGame = false;
        }
        ParentTRSPanel.SetScrollPanelHeightOnly();
    }


    public void SetSeedByOrder( int Order , int Value)
    {
        Seed[Order] = Value;


        string SeedString02 = Seed[0].ToString("X") + Seed[1].ToString("X") + Seed[2].ToString("X") + Seed[3].ToString("X") + Seed[4].ToString("X") + Seed[5].ToString("X") + Seed[6].ToString("X") + Seed[7].ToString("X");
        Debug.Log(SeedString02);
        long longValue = System.Convert.ToInt32(SeedString02, 16);

        // ��鲢�������ʾ
        if (longValue > 0x7FFFFFFF)
        {
            longValue -= 0x100000000;
        }
        int IntSeed = (int)longValue;
        InitializePlayerSetting.GlobalPlayerSetting.RoundSeed = IntSeed;
        Debug.Log(InitializePlayerSetting.GlobalPlayerSetting.RoundSeed);
        InitializePlayerSetting.GlobalPlayerSetting.DoSeed();
    }


}
