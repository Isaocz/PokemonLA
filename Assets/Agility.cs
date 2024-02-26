using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agility : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        player.playerData.MoveSpwBounsAlways += 1;
        player.playerData.SpeBounsAlways += 2;
        player.ReFreshAbllityPoint();
        if (SkillFrom == 2)
        {
            if (player.Skill01 != null && player.Skill01.SkillType == 14 && player.Skill01.SkillIndex != 353 && player.Skill01.SkillIndex != 354 )
            {
                player.MinusSkillCDTime(1,0.5f,false);
            }
            if (player.Skill02 != null && player.Skill02.SkillType == 14 && player.Skill02.SkillIndex != 353 && player.Skill02.SkillIndex != 354)
            {
                player.MinusSkillCDTime(2, 0.5f, false);
            }
            if (player.Skill03 != null && player.Skill03.SkillType == 14 && player.Skill03.SkillIndex != 353 && player.Skill03.SkillIndex != 354)
            {
                player.MinusSkillCDTime(3, 0.5f, false);
            }
            if (player.Skill04 != null && player.Skill04.SkillType == 14 && player.Skill04.SkillIndex != 353 && player.Skill04.SkillIndex != 354)
            {
                player.MinusSkillCDTime(4, 0.5f, false);
            }
        }
    }

    private void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        player.playerData.MoveSpwBounsAlways -= 1;
        player.playerData.SpeBounsAlways -= 2;
        player.ReFreshAbllityPoint();
    }

}
