using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitalSkillBar : MonoBehaviour
{
    public SelectRoleSkillBar SkillBar;

    public RoleInfo Role;

    public Skill skill;

    public TRSPanelInitalSkill ParentInitalSkillPanel;

    public void SetSkillBar(Skill s ,UIDescribe DescribeL, UIDescribe DescribeR )
    {
        skill = s;
        SkillBar.SetBar(s , DescribeL , DescribeR);
    }
}
