using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supersonic : Skill
{
    List<Empty> SupersonicEmptyList = new List<Empty> { };

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }


    public void SupersonicEmpty(Empty target)
    {
        if (!SupersonicEmptyList.Contains(target))
        {
            SupersonicEmptyList.Add(target);
            target.EmptyConfusion(5.0f,1.0f);
            if (SkillFrom == 2) { target.AtkChange(-1, 5.0f); }
        }
    }
}
