using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistyExplosion : Skill
{

    public MistyExplosionMist Mist;
    float Timer;


    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        Timer += Time.deltaTime;
        if (Timer >= 0.5f)
        {
            Timer = 0;
            MistyExplosionMist m = Instantiate(Mist , transform.position , Quaternion.identity );
            m.SpDamage = SpDamage;
            m.player = player;
            m.SkillFrom = SkillFrom;
            m.CTDamage = CTDamage;
            m.CTLevel = CTLevel;
        }
    }

}
