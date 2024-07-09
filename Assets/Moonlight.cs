using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonlight : Skill
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

        player.ChangeHp(recover, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
