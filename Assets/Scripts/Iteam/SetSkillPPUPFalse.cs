using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkillPPUPFalse : MonoBehaviour
{
    public List<Skill> SkillList;
    private void Awake()
    {
        for(int i = 0; i < SkillList.Count; i++)
        {
            SkillList[i].isPPUP = false;
            if (SkillList[i].SkillIndex == 309 || SkillList[i].SkillIndex == 310) { SkillList[i].SkillType = 1; SkillList[i].SpDamage = 50; }
        }
    }
}
