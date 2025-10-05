using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusEnergy : Skill
{
    public SubFocusEnergy sub;
    public SubFocusEnergy subPlus;
    // Start is called before the first frame update
    void Start()
    {
        if (SkillFrom == 2)
        {
            player.AddASubSkill(subPlus);
        }
        else
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
