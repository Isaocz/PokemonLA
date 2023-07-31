using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSkill : Skill
{
    public Weather.WeatherEnum WeatherType;

    // Start is called before the first frame update
    void Start()
    {
        if (WeatherType == Weather.WeatherEnum.Rain)
        {
            if (SkillFrom == 2) { Weather.GlobalWeather.ChangeWeatherRain(10, true); }
            else { Weather.GlobalWeather.ChangeWeatherRain(10, false); }
        }
        else if (WeatherType == Weather.WeatherEnum.Sunny)
        {
            if (SkillFrom == 2) { Weather.GlobalWeather.ChangeWeatherSunshine(10, true); }
            else { Weather.GlobalWeather.ChangeWeatherSunshine(10, false); }

        }
        else if (WeatherType == Weather.WeatherEnum.Hail)
        {
            if (SkillFrom == 2) { Weather.GlobalWeather.ChangeWeatherHail(10, true); }
            else { Weather.GlobalWeather.ChangeWeatherHail(10, false); }
        }
        else if (WeatherType == Weather.WeatherEnum.SandStorm)
        {
            if (SkillFrom == 2) { Weather.GlobalWeather.ChangeWeatherSandStorm(10, true); }
            else { Weather.GlobalWeather.ChangeWeatherSandStorm(10, false); }
        }
    }

    private void Update()
    {
        StartExistenceTimer();
    }
}
