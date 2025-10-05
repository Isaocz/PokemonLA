using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 取消选择某个初始技能
/// </summary>
public class InitalSkillGiveUp : InitalSkillBar
{
    public void GiveUp()
    {
        if (ParentInitalSkillPanel.InitalSkill.Count > 1) {
            ParentInitalSkillPanel.GiveUpaSkill(skill, Role);
            Destroy(gameObject);
        }
    }
}
