using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisarmingVoice : Skill
{

    bool isSkillFrom02Done;

    public void SkillFrom02()
    {
        if (!isSkillFrom02Done && SkillFrom == 2)
        {
            isSkillFrom02Done = true;
            player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.Ç³·ÛÉ«ÆÕÍ¨ÐÍ);
            for (int i = 0; i < player.ButterflyManger.transform.childCount; i++)
            {
                FairyButterfly bf = player.ButterflyManger.transform.GetChild(i).GetComponent<FairyButterfly>();
                if (bf != null)
                {
                    bf.isInfatuation = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
