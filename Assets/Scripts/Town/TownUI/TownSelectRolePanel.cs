using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 小镇的角色选择界面
/// </summary>
public class TownSelectRolePanel : MonoBehaviour
{
    /// <summary>
    /// 角色选择界面的滚动界面
    /// </summary>
    public ScrollRect ScrollPanel;
    /// <summary>
    /// 角色选择界面的滚动界面
    /// </summary>
    public Button GameStartButton;
    /// <summary>
    /// 切换为左边角色
    /// </summary>
    public Button LeftRoleButton;
    /// <summary>
    /// 切换为右边角色
    /// </summary>
    public Button RightRoleButton;

    /// <summary>
    /// 说明框右
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// 说明框左
    /// </summary>
    public UIDescribe uIDescribeL;


    /// <summary>
    /// 角色信息界面
    /// </summary>
    public TSRPanelRoleInfo RoleInfoPanel;
    /// <summary>
    /// 天赋值雷达界面
    /// </summary>
    public TSRPanelTelentRader TelentRaderPanel;
    /// <summary>
    /// 特性选择界面
    /// </summary>
    public TRSPanelAbilityPanel AbilityPanel;
    /// <summary>
    /// 技能选择界面
    /// </summary>
    public TRSPanelInitalSkill InitalSkillPanel;
    /// <summary>
    /// 技能一次性道具界面
    /// </summary>
    public TRSPanelInitalItem InitalSpaceItemPanel;
    /// <summary>
    /// 冒险密语设置界面
    /// </summary>
    public TRSPanelSeedPanel SeedPanel;
    /// <summary>
    /// 当前选择的角色
    /// </summary>
    public int RoleIndex = 0;

    List<RoleInfo> UnlockRole = new List<RoleInfo> { };

    bool SetPanelHight;

    SaveData save;

    /// <summary>
    /// 界面唤出时设置界面
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
    /// 设置角色选择界面
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
    /// 向左切换角色
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
    /// 向右切换角色
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
    /// 设置楼层数为0
    /// </summary>
    public void AdventureStart()
    {
        if (FloorNum.GlobalFloorNum != null) { FloorNum.GlobalFloorNum.FloorNumber = 0; }
    }


}
