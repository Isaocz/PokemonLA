using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAromatherapy : SubSkill
{
    // Start is called before the first frame update
    void Start()
    {
        if (MainSkill.SkillType == 12)
        {
            if (MainSkill.Damage != 0) { MainSkill.Damage *= 1.3f; player.RemoveASubSkill(subskill); }
            if (MainSkill.SpDamage != 0) { MainSkill.SpDamage += 1.3f; player.RemoveASubSkill(subskill); }
        }
        Destroy(gameObject);
    }

}
