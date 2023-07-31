using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMudSlupPlus : SubSkill
{

    // Start is called before the first frame update
    void Start()
    {
        if (MainSkill.SkillType != 5) { Destroy(gameObject); }
        else { player.RemoveASubSkill(subskill); }
    }

    // Update is called once per frame
    void Update()
    {

        StartExistenceTimer();
    }
}
