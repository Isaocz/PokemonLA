using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleAxel : Skill
{

    public int KickCount;
    public bool isKickDone;

    public SubTripleAxel sub2;
    public SubTripleAxel sub3;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                HitAndKo(e);
                isKickDone = true;
                switch (KickCount) 
                {
                    case 0:
                        e.Frozen(7.5f , 1.0f , 0.05f + ((float)player.LuckPoint/30));
                        break;
                    case 2:
                        e.Frozen(7.5f, 1.0f, 0.15f + ((float)player.LuckPoint / 30));
                        break;
                    case 3:
                        e.Frozen(10f, 1.2f, 0.3f + ((float)player.LuckPoint / 30));
                        break;
                }

            }
        }
    }

    private void OnDestroy()
    {
        if (isKickDone)
        {
            switch (KickCount) 
            {
                case 0:
                    player.AddASubSkill(sub2);
                    if (player.Skill01 != null && (player.Skill01.SkillIndex == 273 || player.Skill01.SkillIndex == 274))
                    {
                        player.MinusSkillCDTime(1, player.Skill01.ColdDown, false);
                    }
                    if (player.Skill02 != null && (player.Skill02.SkillIndex == 273 || player.Skill02.SkillIndex == 274))
                    {
                        player.MinusSkillCDTime(2, player.Skill02.ColdDown, false);
                    }
                    if (player.Skill03 != null && (player.Skill03.SkillIndex == 273 || player.Skill03.SkillIndex == 274))
                    {
                        player.MinusSkillCDTime(3, player.Skill03.ColdDown, false);
                    }
                    if (player.Skill04 != null && (player.Skill04.SkillIndex == 273 || player.Skill04.SkillIndex == 274))
                    {
                        player.MinusSkillCDTime(4, player.Skill04.ColdDown, false);
                    }
                    break;
                case 2:
                    player.AddASubSkill(sub3);
                    if (player.Skill01 != null && (player.Skill01.SkillIndex == 273 || player.Skill01.SkillIndex == 274))
                    {
                        player.MinusSkillCDTime(1, player.Skill01.ColdDown, false);
                    }
                    if (player.Skill02 != null && (player.Skill02.SkillIndex == 273 || player.Skill02.SkillIndex == 274))
                    {
                        player.MinusSkillCDTime(2, player.Skill02.ColdDown, false);
                    }
                    if (player.Skill03 != null && (player.Skill03.SkillIndex == 273 || player.Skill03.SkillIndex == 274))
                    {
                        player.MinusSkillCDTime(3, player.Skill03.ColdDown, false);
                    }
                    if (player.Skill04 != null && (player.Skill04.SkillIndex == 273 || player.Skill04.SkillIndex == 274))
                    {
                        player.MinusSkillCDTime(4, player.Skill04.ColdDown, false);
                    }
                    break;
            }


        }
    }
}
