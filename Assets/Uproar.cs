using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uproar : Skill
{
    List<Empty> UproarEmptyList = new List<Empty> { };

    private void Start()
    {
        player.isCanNotMove = true;
        player.IsInUproarState = true;
        player.SleepRemove();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Empty target = collision.GetComponent<Empty>();
        if (target != null)
        {
            if (SkillFrom == 2) {
                if (target.isSleepDone)
                {
                    target.DefChange(-2, 7.5f);
                    target.SpDChange(-2, 7.5f);
                }
            }
            target.EmptySleepRemove();
            HitAndKo(target);
            UproarEmptyList.Add(target);
            target.IsInUproarState = true;
            //if (SkillFrom == 2) { target.EmptyConfusion(5, 1); }
        }
    }

    private void OnDestroy()
    {
        player.isCanNotMove = false;
        player.IsInUproarState = false;
        for (int i = 0; i < UproarEmptyList.Count; i++ )
        {
            UproarEmptyList[i].IsInUproarState = false;
        }
    }

}
