using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ѡ��ĳ����ʼ����
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
