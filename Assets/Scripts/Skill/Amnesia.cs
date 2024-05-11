using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amnesia : Skill
{
    private void Start()
    {
        ExistenceTime = player.GetSkillCD(this); 
        if (SkillFrom == 2) { player.playerData.SpDBounsJustOneRoom += 3; player.playerData.DefBounsJustOneRoom += 1; }
        else { player.playerData.SpDBounsJustOneRoom += 2; }
        player.ReFreshAbllityPoint();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        if (SkillFrom == 2) { player.playerData.SpDBounsJustOneRoom -= 3; player.playerData.DefBounsJustOneRoom -= 1; }
        else { player.playerData.SpDBounsJustOneRoom -= 2; }
        player.ReFreshAbllityPoint();
    }
}
