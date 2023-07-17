using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSlideSkill : Skill
{

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (SkillFrom == 2)
        {
            if (other.tag == "Enviroment")
            {
                SkillBreakabelStone stone = other.GetComponent<SkillBreakabelStone>();

                if (stone != null)
                {
                    stone.RockBreak();
                }
            }
        }
    }
}
