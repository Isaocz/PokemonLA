using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safeguard : Skill
{

    // Start is called before the first frame update
    void Start()
    {
        player.isSafeguard = true;
        if (SkillFrom == 2)
        {
            float NowFrozenPoint = Mathf.Clamp(player.PlayerFrozenPointFloat, 0.0f, 1.0f);
            float NowToxicPoint = Mathf.Clamp(player.ToxicPointFloat, 0.0f, 1.0f);
            float NowParalysisPoint = Mathf.Clamp(player.ParalysisPointFloat, 0.0f, 1.0f);
            float NowSleepPoint = Mathf.Clamp(player.SleepPointFloat, 0.0f, 1.0f);
            float NowBurnPoint = Mathf.Clamp(player.BurnPointFloat, 0.0f, 1.0f);
                        
            if (NowFrozenPoint < 1.0f) {
                player.PlayerFrozenRemove();
                if (NowFrozenPoint > 0.2f) { player.PlayerFrozenFloatPlus(NowFrozenPoint - 0.2f , 0); }
            }
            if (NowToxicPoint < 1.0f) {
                player.ToxicRemove();
                if (NowToxicPoint > 0.2f) { player.ToxicFloatPlus(NowToxicPoint - 0.2f); }
            }
            if (NowParalysisPoint < 1.0f) {
                player.ParalysisRemove();
                if (NowParalysisPoint > 0.2f) { player.ParalysisFloatPlus(NowParalysisPoint - 0.2f); }
            }
            if (NowSleepPoint < 1.0f) {
                player.SleepRemove();
                if (NowSleepPoint > 0.2f) { player.SleepFloatPlus(NowSleepPoint - 0.2f); }
            }
            if (NowBurnPoint < 1.0f) {
                player.BurnRemove();
                if (NowBurnPoint > 0.2f) { player.BurnFloatPlus(NowBurnPoint - 0.2f); }
            }
        }
    }

    private void OnDestroy()
    {
        player.isSafeguard = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
