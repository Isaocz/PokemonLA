using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : Skill
{
    // Start is called before the first frame update
    void Start()
    {

        player.ChangeHp(player.maxHp, 0, 0);
        if(SkillFrom == 2)
        {
            player.BurnRemove();
            player.ParalysisRemove();
            player.SleepRemove();
            player.ToxicRemove();
            //player.frozenRemove();
        }
        player.SleepFloatPlus(10);
    }

    private void Update()
    {
        StartExistenceTimer();
    }

}
