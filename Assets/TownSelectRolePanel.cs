using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// С��Ľ�ɫѡ�����
/// </summary>
public class TownSelectRolePanel : MonoBehaviour
{
    /// <summary>
    /// ��ɫѡ�����Ĺ�������
    /// </summary>
    public ScrollRect ScrollPanel;
    /// <summary>
    /// ��ɫѡ�����Ĺ�������
    /// </summary>
    public Button GameStartButton;
    /// <summary>
    /// �л�Ϊ��߽�ɫ
    /// </summary>
    public Button LeftRoleButton;
    /// <summary>
    /// �л�Ϊ�ұ߽�ɫ
    /// </summary>
    public Button RightRoleButton;

    /// <summary>
    /// ˵������
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// ˵������
    /// </summary>
    public UIDescribe uIDescribeL;


    /// <summary>
    /// ��ɫ��Ϣ����
    /// </summary>
    public TSRPanelRoleInfo RoleInfoPanel;
    /// <summary>
    /// �츳ֵ�״����
    /// </summary>
    public TSRPanelTelentRader TelentRaderPanel;
    /// <summary>
    /// ����ѡ�����
    /// </summary>
    public TRSPanelAbilityPanel AbilityPanel;
    /// <summary>
    /// ����ѡ�����
    /// </summary>
    public TRSPanelInitalSkill InitalSkillPanel;
    /// <summary>
    /// ����һ���Ե��߽���
    /// </summary>
    public TRSPanelInitalItem InitalSpaceItemPanel;
    /// <summary>
    /// ð���������ý���
    /// </summary>
    public TRSPanelSeedPanel SeedPanel;
    /// <summary>
    /// ��ǰѡ��Ľ�ɫ
    /// </summary>
    public int RoleIndex = 0;

    List<RoleInfo> UnlockRole = new List<RoleInfo> { };

    bool SetPanelHight;

    SaveData save;

    /// <summary>
    /// ���滽��ʱ���ý���
    /// </summary>
    private void OnEnable()
    {
        save = SaveLoader.saveLoader.saveData;
        UnlockRole.Clear();
        UnlockRole = new List<RoleInfo> { };
        for (int i = 0; i < save.RoleList.Count; i++)
        {
            if (save.RoleList[i].isUnlock) { UnlockRole.Add(save.RoleList[i]); }
        }
        
        if (StartPanelPlayerData.PlayerData.Player != UnlockRole[RoleIndex].Role) { RoleIndex = 0; }

        StartPanelPlayerData.PlayerData.Player = UnlockRole[RoleIndex].Role;
        SetSelectRolePanel(UnlockRole[RoleIndex]);
    }


    /// <summary>
    /// ���ý�ɫѡ�����
    /// </summary>
    public void SetSelectRolePanel( RoleInfo role )
    {




        if (RoleInfoPanel.uIDescribeL == null) { RoleInfoPanel.uIDescribeL = uIDescribeL; }
        if (RoleInfoPanel.uIDescribeR == null) { RoleInfoPanel.uIDescribeR = uIDescribeR; }
        RoleInfoPanel.SetRoleInfoPanel(role);

        if (TelentRaderPanel.uIDescribeL == null) { TelentRaderPanel.uIDescribeL = uIDescribeL; }
        if (TelentRaderPanel.uIDescribeR == null) { TelentRaderPanel.uIDescribeR = uIDescribeR; }
        TelentRaderPanel.SetTalentRaderPanel(role);

        if (AbilityPanel.uIDescribeL == null) { AbilityPanel.uIDescribeL = uIDescribeL; }
        if (AbilityPanel.uIDescribeR == null) { AbilityPanel.uIDescribeR = uIDescribeR; }
        if (AbilityPanel.GamestartButton == null) { AbilityPanel.GamestartButton = GameStartButton; }
        AbilityPanel.SetAbilityToggle(role);

        if (InitalSkillPanel.uIDescribeL == null) { InitalSkillPanel.uIDescribeL = uIDescribeL; }
        if (InitalSkillPanel.uIDescribeR == null) { InitalSkillPanel.uIDescribeR = uIDescribeR; }
        InitalSkillPanel.SetInitalSkillPanel(role);

        if (InitalSpaceItemPanel.uIDescribeL == null) { InitalSpaceItemPanel.uIDescribeL = uIDescribeL; }
        if (InitalSpaceItemPanel.uIDescribeR == null) { InitalSpaceItemPanel.uIDescribeR = uIDescribeR; }
        InitalSpaceItemPanel.SetInitalItemPanel(save);

        if (SeedPanel.uICallDescribe.DescribeUI == null) { SeedPanel.uICallDescribe.DescribeUI = uIDescribeR; }
        SeedPanel.ParentTRSPanel = this;
        SeedPanel.SeedToggle.isOn = false;
        SeedPanel.SeedToggle.image.sprite = SeedPanel.SeedToggle.SpriteNormal;
        SeedPanel.ChangeToggle();



        SetPanelHight = true;

        Invoke("SetScrollPanelHeight", 0.05f);
    }

    private void LateUpdate()
    {
        if (SetPanelHight)
        {
            SetScrollPanelHeight();
            SetPanelHight = false;
        }

        
    }

    private void FixedUpdate()
    {
        SetScrollPanelHeightOnly();
    }

    public void SetScrollPanelHeightOnly()
    {
        ScrollPanel.content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetHigh() + 110.0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(SeedPanel.transform.parent.GetComponent<RectTransform>());
    }

    public void SetScrollPanelHeight()
    {

        SetScrollPanelHeightOnly();
        ScrollPanel.verticalNormalizedPosition = 1f;
    }


    float GetHigh()
    {
        float Output = 0;

        Output = TelentRaderPanel.GetComponent<RectTransform>().rect.height + InitalSkillPanel.GetComponent<RectTransform>().rect.height + InitalSpaceItemPanel.GetComponent<RectTransform>().rect.height + SeedPanel.GetComponent<RectTransform>().rect.height;
        return Output;
    }



    /// <summary>
    /// �����л���ɫ
    /// </summary>
    public void LeftRoleSwirch()
    {
        if (UnlockRole.Count > 1)
        {
            RoleIndex -= 1;
            if (RoleIndex < 0) { RoleIndex = UnlockRole.Count - 1; }
            StartPanelPlayerData.PlayerData.Player = UnlockRole[RoleIndex].Role;
            SetSelectRolePanel(UnlockRole[RoleIndex]);
        }
    }

    /// <summary>
    /// �����л���ɫ
    /// </summary>
    public void RightRoleSwirch()
    {
        if (UnlockRole.Count > 1) {
            RoleIndex += 1;
            if (RoleIndex >= UnlockRole.Count) { RoleIndex = 0; }
            StartPanelPlayerData.PlayerData.Player = UnlockRole[RoleIndex].Role;
            SetSelectRolePanel(UnlockRole[RoleIndex]);
        }
    }

    /// <summary>
    /// ����¥����Ϊ0
    /// </summary>
    public void AdventureStart()
    {
        if (FloorNum.GlobalFloorNum != null) { FloorNum.GlobalFloorNum.FloorNumber = 0; }
    }


}
