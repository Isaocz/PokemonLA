using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDetect : SubSkill
{
    // Start is called before the first frame update
    void Start()
    {
        if(MainSkill.Damage != 0 || MainSkill.SpDamage != 0)
        {
            MainSkill.CTLevel += 2;
            player.RemoveASubSkill(subskill);
        }
        Destroy(gameObject);
    }

}
