using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amnesia : Skill
{
    private void Start()
    {
        ExistenceTime = (player.isParalysisDone ? 1.8f : 1.0f) * (ColdDown * (isPPUP?0.625f:1)) * (1 - ((float)player.SpeedAbilityPoint / 500));
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
