using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneClaws : Skill
{

    bool isSpeImprove;
    bool isAtkImprove;
    int PlayerBeforeHP;

    // Start is called before the first frame update
    void Start()
    {
        if (player.playerData.MoveSpeBounsJustOneRoom <= 8) {
            player.playerData.MoveSpeBounsJustOneRoom++;
            isSpeImprove = true;
        }
        if (player.playerData.AtkBounsJustOneRoom <= 8) {
            player.playerData.AtkBounsJustOneRoom++;
            isAtkImprove = true;
        }
        player.ReFreshAbllityPoint();
        PlayerBeforeHP = player.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (SkillFrom == 2)
        {
            if (player.Hp < PlayerBeforeHP)
            {
                player.MinusSkillCDTime(gameObject.GetComponent<Skill>() , 1 , false);
            }
            PlayerBeforeHP = player.Hp;
        }

    }

    private void OnDestroy()
    {
        if (isSpeImprove)
        {
            player.playerData.MoveSpeBounsJustOneRoom--;
        }
        if (isAtkImprove)
        {
            player.playerData.AtkBounsJustOneRoom--;
        }
        player.ReFreshAbllityPoint();
    }
}
