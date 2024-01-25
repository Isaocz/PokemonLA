using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubZenHeadbutt : SubSkill
{
    List<TraceEffect> TE = new List<TraceEffect>{};
    bool isDamagePlus;

    // Start is called before the first frame update
    void Start()
    {

        Invoke("DamgaePlus" , 0.2f);
    }

    void DamgaePlus() {


        bool isTraceSkill = false;
        for (int i = 0; i < SkillTag.Length; i++)
        {
            if (SkillTag[i] == Skill.SkillTagEnum.带有跟踪效果或类似效果的技能) { isTraceSkill = true; }
        }
        if (isTraceSkill)
        {
            for (int i = 0; i < MainSkill.transform.childCount; i++)
            {
                Debug.Log(MainSkill.transform.childCount);
                if (MainSkill.transform.GetChild(i).GetComponent<TraceEffect>())
                {
                    TE.Add(MainSkill.transform.GetChild(i).GetComponent<TraceEffect>());
                }
            }
            if (MainSkill.GetComponent<TraceEffect>() != null) { TE.Add(MainSkill.GetComponent<TraceEffect>()); }
            for (int i = 0; i < TE.Count; i++)
            {
                TE[i].distance += 1.3f;
                if (i == TE.Count - 1)
                {
                    player.RemoveASubSkill(subskill);
                }
            }
            if (!isDamagePlus)
            {
                isDamagePlus = true;
                if (MainSkill.Damage != 0) { MainSkill.Damage += 20f; }
                if (MainSkill.SpDamage != 0) { MainSkill.SpDamage += 20f; }
            }
        }
        
        Destroy(gameObject);
    }
}
