using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkillPPUPFalse : MonoBehaviour
{

    public static SetSkillPPUPFalse Instence;

    public List<Skill> SkillList;


    public List<Skill> TestSkillList;

    public List<Skill> KillAllSkillList;


    private void Awake()
    {
        Instence = this;
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i] != null) {
                SkillList[i].isPPUP = false;
                if (SkillList[i].SkillIndex == 309 || SkillList[i].SkillIndex == 310) { SkillList[i].SkillType = 1; SkillList[i].SpDamage = 50; } //天气求
                if (SkillList[i].SkillIndex == 31 || SkillList[i].SkillIndex == 32) { SkillList[i].SkillType = 1;  } //太晶爆发
            }
        }
    }
}
