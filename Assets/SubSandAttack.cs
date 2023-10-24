using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSandAttack : SubSkill
{

    // Start is called before the first frame update
    void Start()
    {
        if (MainSkill.SkillType != 5 || MainSkill.SkillIndex == 117 || MainSkill.SkillIndex == 118) { Destroy(gameObject); }
        else { player.RemoveASubSkill(subskill); }
    }

    // Update is called once per frame
    void Update()
    {

        StartExistenceTimer();
    }
}
