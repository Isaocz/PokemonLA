using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesis : Skill
{
    float RecoveryAmount;
    // Start is called before the first frame update
    void Start()
    {
        if (!player.playerData.IsPassiveGetList[118]  ) {
            if (Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus)
            {
                RecoveryAmount = 2 * player.maxHp / 3;
            }
            else if (Weather.GlobalWeather.isSandstorm || Weather.GlobalWeather.isSandstormPlus || Weather.GlobalWeather.isRain || Weather.GlobalWeather.isRainPlus || Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus)
            {
                RecoveryAmount = player.maxHp / 4;
            }
            else { RecoveryAmount = player.maxHp / 2; }
        }
        else
        {
            RecoveryAmount = player.maxHp / 2;
        }

        if ( SkillFrom == 2 && player.InGressCount.Count != 0 )
        {
            float NowFrozenPoint = Mathf.Clamp(player.PlayerFrozenPointFloat, 0.0f, 1.0f);
            float NowToxicPoint = Mathf.Clamp(player.ToxicPointFloat, 0.0f, 1.0f);
            float NowParalysisPoint = Mathf.Clamp(player.ParalysisPointFloat, 0.0f, 1.0f);
            float NowSleepPoint = Mathf.Clamp(player.SleepPointFloat, 0.0f, 1.0f);
            float NowBurnPoint = Mathf.Clamp(player.BurnPointFloat, 0.0f, 1.0f);
            player.PlayerFrozenRemove();
            player.ToxicRemove();
            player.ParalysisRemove();
            player.SleepRemove();
            player.BurnRemove();


            if (!player.playerData.IsPassiveGetList[118])
            {
                if (Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus) { }
                else if (Weather.GlobalWeather.isSandstorm || Weather.GlobalWeather.isSandstormPlus || Weather.GlobalWeather.isRain || Weather.GlobalWeather.isRainPlus || Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus)
                {
                    if (NowFrozenPoint > 0.2f) { player.PlayerFrozenFloatPlus(NowFrozenPoint - 0.2f); }
                    if (NowToxicPoint > 0.2f) { player.ToxicFloatPlus(NowToxicPoint - 0.2f); }
                    if (NowParalysisPoint > 0.2f) { player.ParalysisFloatPlus(NowParalysisPoint - 0.2f); }
                    if (NowSleepPoint > 0.2f) { player.SleepFloatPlus(NowSleepPoint - 0.2f); }
                    if (NowBurnPoint > 0.2f) { player.BurnFloatPlus(NowBurnPoint - 0.2f); }
                }
                else
                {
                    if (NowFrozenPoint > 0.5f) { player.PlayerFrozenFloatPlus(NowFrozenPoint - 0.5f); }
                    if (NowToxicPoint > 0.5f) { player.ToxicFloatPlus(NowToxicPoint - 0.5f); }
                    if (NowParalysisPoint > 0.5f) { player.ParalysisFloatPlus(NowParalysisPoint - 0.5f); }
                    if (NowSleepPoint > 0.5f) { player.SleepFloatPlus(NowSleepPoint - 0.5f); }
                    if (NowBurnPoint > 0.5f) { player.BurnFloatPlus(NowBurnPoint - 0.5f); }
                }
            }
            else
            {
                if (NowFrozenPoint > 0.5f) { player.PlayerFrozenFloatPlus(NowFrozenPoint - 0.5f); }
                if (NowToxicPoint > 0.5f) { player.ToxicFloatPlus(NowToxicPoint - 0.5f); }
                if (NowParalysisPoint > 0.5f) { player.ParalysisFloatPlus(NowParalysisPoint - 0.5f); }
                if (NowSleepPoint > 0.5f) { player.SleepFloatPlus(NowSleepPoint - 0.5f); }
                if (NowBurnPoint > 0.5f) { player.BurnFloatPlus(NowBurnPoint - 0.5f); }
            }
        }


        Pokemon.PokemonHpChange( null , player.gameObject , 0 , 0 , (int)RecoveryAmount , Type.TypeEnum.IgnoreType );
        player.isCanNotMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime <= 1 && player.isCanNotMove)
        {
            player.isCanNotMove = false;
        }
    }
}
