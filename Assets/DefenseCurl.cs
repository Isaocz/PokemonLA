using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseCurl : Skill
{
    
    bool isMSUp;

    // Start is called before the first frame update
    void Start()
    {
        player.playerData.DefBounsJustOneRoom += 1;

        if (SkillFrom == 2 && player.playerData.MoveSpeBounsJustOneRoom <= 8) {
            isMSUp = true;
            player.playerData.MoveSpeBounsJustOneRoom += 1;
        }
        player.ReFreshAbllityPoint();
    }

    private void OnDestroy()
    {
        if (SkillFrom == 2 && isMSUp)
        {
            isMSUp = false;
            player.playerData.MoveSpeBounsJustOneRoom -= 1;
            player.ReFreshAbllityPoint();
        }
    }

    private void Update()
    {
        StartExistenceTimer();
    }
}
