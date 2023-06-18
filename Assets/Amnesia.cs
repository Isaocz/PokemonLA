using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amnesia : Skill
{
    private void Start()
    {
        ExistenceTime = (player.isParalysisDone ? 1.8f : 1.0f) * ColdDown * (1 - (player.SpeedAbilityPoint / 500));
        if (SkillFrom == 2) { player.playerData.SpDBounsAlways += 3; player.playerData.DefBounsAlways += 1; }
        else { player.playerData.SpDBounsAlways += 2; }
        player.ReFreshAbllityPoint();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        if (SkillFrom == 2) { player.playerData.SpDBounsAlways -= 3; player.playerData.DefBounsAlways -= 1; }
        else { player.playerData.SpDBounsAlways -= 2; }
        player.ReFreshAbllityPoint();
    }
}
