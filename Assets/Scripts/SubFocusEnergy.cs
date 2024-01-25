using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFocusEnergy : SubSkill
{
    public SubFocusEnergy sub01;

    // Start is called before the first frame update
    void Start()
    {

       
        if (MainSkill.Damage != 0 || MainSkill.SpDamage != 0)
        {
            MainSkill.CTLevel += 2;
            player.RemoveASubSkill(subskill);
            if (isPlusSkill)
            {
                player.AddASubSkill(sub01);
            }
        }
        Destroy(gameObject);
    }
}
