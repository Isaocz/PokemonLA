using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronDefense : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        player.playerData.DefBounsJustOneRoom += 2;
        if (SkillFrom == 2) { player.playerData.SpDBounsJustOneRoom += 1; }
        player.ReFreshAbllityPoint();
    }

}
