using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubHelpingHand : SubSkill
{

    public bool isPlus;

    // Start is called before the first frame update
    void Start()
    {
        if (!isPlus)
        {
            if (MainSkill.Damage != 0) { MainSkill.Damage *= 1.3f; player.RemoveASubSkill(subskill); Destroy(gameObject); }
            if (MainSkill.SpDamage != 0) { MainSkill.SpDamage += 1.3f; player.RemoveASubSkill(subskill); Destroy(gameObject); }
        }
        else
        {
            if (MainSkill.Damage != 0) { MainSkill.Damage *= 1.6f; player.RemoveASubSkill(subskill); Destroy(gameObject); }
            if (MainSkill.SpDamage != 0) { MainSkill.SpDamage += 1.6f; player.RemoveASubSkill(subskill); Destroy(gameObject); }
        }
    }

}