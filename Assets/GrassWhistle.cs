using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassWhistle : GrassSkill
{

    float Timer;
    bool isCanSleep;

    // Start is called before the first frame update
    void Start()
    {
        player.SpeedChange();
        isCanSleep = true;
        if (SkillFrom == 2) { BornAGrass(transform.position); }
    }

    // Update is called once per frame
    void Update()
    {
        if (SkillFrom == 2)
        {
            if (player.InGressCount.Count == 0 && !player.isSpeedChange) { player.SpeedChange(); }
            if (player.InGressCount.Count != 0 && player.isSpeedChange) { player.SpeedRemove01(0); }
        }

        StartExistenceTimer();
        Timer += Time.deltaTime;
        if (Timer >= 0.49f)
        {
            Timer = 0;
            isCanSleep = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isCanSleep && other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null && !e.isSleepDone)
            {
                e.EmptySleepDone(0.4f , 10 , 1);
                isCanSleep = false;
            }
        }
    }

    private void OnDestroy()
    {
        player.SpeedRemove01(0);
    }
}
