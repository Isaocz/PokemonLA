using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 初始技能选择界面
/// </summary>
public class TRSPanelInitalSkill : MonoBehaviour
{
    /// <summary>
    /// 取消选择技能的按钮
    /// </summary>
    public InitalSkillGiveUp GiveUpPrefabs;
    /// <summary>
    /// 选择技能的按钮
    /// </summary>
    public InitalSkillPickUp PickUpPrefabs;

    /// <summary>
    /// 取消选择技能界面
    /// </summary>
    public Transform GiveUpPanel;
    /// <summary>
    /// 选择技能界面
    /// </summary>
    public Transform PickUpPanel;

    /// <summary>
    /// 说明框右
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// 说明框左
    /// </summary>
    public UIDescribe uIDescribeL;


    public List<Skill> InitalSkill = new List<Skill> { };
    public List<Skill> CandidateSkillList = new List<Skill> { };

    /// <summary>
    /// 设置初始技能界面
    /// </summary>
    /// <param name="role"></param>
    public void SetInitalSkillPanel(RoleInfo role)
    {
        _mTool.RemoveAllChild(GiveUpPanel.gameObject);
        _mTool.RemoveAllChild(PickUpPanel.gameObject);
        InitalSkill.Clear();
        CandidateSkillList.Clear();
        InitalSkill = new List<Skill> { };
        CandidateSkillList = new List<Skill> { };

        //初始化已选择名单
        InitalSkill = new List<Skill> { };
        if (role.Role.InitialSkill01 != null)
        {
            InitalSkill.Add(role.Role.InitialSkill01);
        }
        if (role.Role.InitialSkill02 != null)
        {
            InitalSkill.Add(role.Role.InitialSkill02);
        }
        if (role.Role.InitialSkill03 != null)
        {
            InitalSkill.Add(role.Role.InitialSkill03);
        }
        if (role.Role.InitialSkill04 != null)
        {
            InitalSkill.Add(role.Role.InitialSkill04);
        }
        InitalSkill.RemoveAll(item => item == null);

        //初始化候补名单
        CandidateSkillList = role.InitialSkills;
        CandidateSkillList = CandidateSkillList.Except(InitalSkill).ToList();
        CandidateSkillList.RemoveAll(item => item == null);

        if (InitalSkill.Count != 0) {
            for (int i = 0; i < InitalSkill.Count; i++)
            {
                InitalSkillGiveUp s = Instantiate(GiveUpPrefabs, GiveUpPanel);
                s.SetSkillBar(InitalSkill[i], uIDescribeR, uIDescribeL);
                s.Role = role;
                s.ParentInitalSkillPanel = this;
            }
        }
        if (CandidateSkillList.Count != 0)
        {
            for (int i = 0; i < CandidateSkillList.Count; i++)
            {
                InitalSkillPickUp s = Instantiate(PickUpPrefabs, PickUpPanel);
                s.SetSkillBar(CandidateSkillList[i], uIDescribeR, uIDescribeL);
                s.Role = role;
                s.ParentInitalSkillPanel = this;
            }
        }

    }

    public void GiveUpaSkill(Skill s, RoleInfo role)
    {
        InitalSkillPickUp p = Instantiate(PickUpPrefabs, PickUpPanel);
        if (InitalSkill.Contains(s)) { InitalSkill.Remove(s); CandidateSkillList.Add(s); }
        p.SetSkillBar(s, uIDescribeR, uIDescribeL);
        p.Role = role;
        p.ParentInitalSkillPanel = this;

        SetRoleInitalSkill(role);
    }

    public void PickUpaSkill(Skill s , RoleInfo role)
    {
        InitalSkillGiveUp g = Instantiate(GiveUpPrefabs, GiveUpPanel);
        if (CandidateSkillList.Contains(s)) { CandidateSkillList.Remove(s); InitalSkill.Add(s); }
        g.SetSkillBar(s, uIDescribeR, uIDescribeL);
        g.Role = role;
        g.ParentInitalSkillPanel = this;

        SetRoleInitalSkill(role);
    }


    /// <summary>
    /// 设置角色的初始技能
    /// </summary>
    /// <param name="role"></param>
    void SetRoleInitalSkill(RoleInfo role)
    {
        SceneLoadManger.sceneLoadManger.SaveGame();

        role.Role.InitialSkill01 = InitalSkill[0];
        role.Role.InitialSkill02 = null;
        role.Role.InitialSkill03 = null;
        role.Role.InitialSkill04 = null;
        if (InitalSkill.Count >= 2) { role.Role.InitialSkill02 = InitalSkill[1]; }
        if (InitalSkill.Count >= 3) { role.Role.InitialSkill03 = InitalSkill[2]; }
        if (InitalSkill.Count >= 4) { role.Role.InitialSkill04 = InitalSkill[3]; }
    }


}
