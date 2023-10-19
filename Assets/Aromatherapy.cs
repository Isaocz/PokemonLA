using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aromatherapy : GrassSkill
{

    public SubAromatherapy sub;

    private void Start()
    {
        player.PlayerFrozenRemove();
        player.ToxicRemove();
        player.ParalysisRemove();
        player.SleepRemove();
        player.BurnRemove();
        if (player.InGressCount.Count != 0)
        {
            player.AddASubSkill(sub);
        }
    }


    // Update is called once per frame
    void Update()
    {

        StartExistenceTimer();
    }

}
