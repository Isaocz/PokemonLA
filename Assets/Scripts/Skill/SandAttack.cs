using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandAttack : Skill
{
    public SubSandAttack sub;

    // Start is called before the first frame update
    void Start()
    {
        if (SkillFrom == 2) { player.AddASubSkill(sub); }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

}
