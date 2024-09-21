using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 选择某个初始技能
/// </summary>
public class InitalSkillPickUp : InitalSkillBar
{
    public void PickUp()
    {
        if (ParentInitalSkillPanel.InitalSkill.Count < 4)
        {
            ParentInitalSkillPanel.PickUpaSkill(skill, Role);
            Destroy(gameObject);
        }
    }

}
