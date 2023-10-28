using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperVoice : Skill
{

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
            HitAndKo(target);
            if (SkillFrom == 2) { target.EmptyConfusion(5,1); }
        }
    }
}
