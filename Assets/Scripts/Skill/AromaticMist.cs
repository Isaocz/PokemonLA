using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AromaticMist : Skill
{
    bool isPlusDone;
    List<FairyButterfly> BFList = new List<FairyButterfly> { };

    // Start is called before the first frame update
    void Start()
    {
        if (player.playerData.SpDBounsJustOneRoom <= 8)
        {
            player.playerData.SpDBounsJustOneRoom += 1;
            isPlusDone = true;
            if (SkillFrom == 2)
            {
                for (int i = 0; i < player.ButterflyManger.transform.childCount; i++)
                {
                    FairyButterfly bf = player.ButterflyManger.transform.GetChild(i).GetComponent<FairyButterfly>();
                    if (!bf.isAttack && !bf.isCanNotAttack)
                    {
                        bf.isCanNotAttack = true;
                        BFList.Add(bf);
                    }
                } 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        if (isPlusDone) {
            player.playerData.SpDBounsJustOneRoom -= 1;
        }
        if (SkillFrom == 2)
        {
            for (int i = 0; i < BFList.Count; i++)
            {
                BFList[i].isCanNotAttack = false;
            }
        }
    }
}
