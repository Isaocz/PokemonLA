using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningSun : Skill
{
    void Start()
    {
        int recover;

        //Î´¼ì²é»Ö¸´·âËø
        if (Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus)
        {
            recover = player.maxHp * 2 / 3;
        }
        else if (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isRain || Weather.GlobalWeather.isRainPlus || Weather.GlobalWeather.isHailPlus || Weather.GlobalWeather.isSandstorm || Weather.GlobalWeather.isSandstormPlus)
        {
            recover = player.maxHp / 4;
        }
        else
        {
            recover = player.maxHp / 2;
        }

        //player.ChangeHp(recover, 0, 0);
        int BeforeHP = player.Hp;
        Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, recover, Type.TypeEnum.IgnoreType);
        int HealHP = player.Hp - BeforeHP;
        if (SkillFrom == 2)
        {
            if (player.BurnPointFloat < 1 && player.isBurnStart && !player.isBurnDone) { player.BurnRemove(); }
            if (player.ToxicPointFloat < 1 && player.isToxicStart && !player.isToxicDone) { player.ToxicRemove(); }
            if (player.ParalysisPointFloat < 1 && player.isParalysisStart && !player.isParalysisDone) { player.ParalysisRemove(); }
            if (player.SleepPointFloat < 1 && player.isSleepStart && !player.isSleepDone) { player.SleepRemove(); }
            if (player.PlayerFrozenPointFloat < 1 && player.isPlayerFrozenStart && !player.isPlayerFrozenDone) { player.PlayerFrozenRemove(); }
        }

    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
