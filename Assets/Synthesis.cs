using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesis : Skill
{
    float RecoveryAmount;
    // Start is called before the first frame update
    void Start()
    {
        if(Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus)
        {
            RecoveryAmount = 2 * player.maxHp / 3;
        }
        else if(Weather.GlobalWeather.isSandstorm||Weather.GlobalWeather.isSandstormPlus || Weather.GlobalWeather.isRain || Weather.GlobalWeather.isRainPlus)
        {
            RecoveryAmount = player.maxHp / 4;
        }
        else { RecoveryAmount = player.maxHp / 2; }
        player.ChangeHp(RecoveryAmount, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
