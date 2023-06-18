using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudSlup : Skill
{
    public SubMudSlupPlus SubMud;

    private void Start()
    {
        if(SkillFrom == 2 && SubMud != null)
        {
            player.AddASubSkill(SubMud);
        }
    }

    // Start is called before the first frame update

    void Update()
    {
        
        StartExistenceTimer();
    }

}