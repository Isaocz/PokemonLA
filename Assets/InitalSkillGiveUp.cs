using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ȡ��ѡ��ĳ����ʼ����
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
