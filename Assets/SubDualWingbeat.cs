using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDualWingbeat : SubSkill
{
    // Start is called before the first frame update
    void Start()
    {
        if (MainSkill.Damage != 0 || MainSkill.SpDamage != 0)
        {
            player.RemoveASubSkill(subskill);
            MainSkill.KOPoint *= 2;
        }
        Destroy(gameObject);
    }

}
