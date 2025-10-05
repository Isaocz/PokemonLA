using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLashOut : SubSkill
{

    public int BeforeHp;

    // Start is called before the first frame update
    void Start()
    {
        if (MainSkill.SkillIndex == 397 || MainSkill.SkillIndex == 398)
        {
            player.RemoveASubSkill(subskill);
            if (BeforeHp > MainSkill.player.Hp)
            {
                MainSkill.Damage += 75;
            }
    
        }
        Destroy(gameObject);
    }
}
