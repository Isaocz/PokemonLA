using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyButton : EnviromentButton
{


    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("SunnyButton");
        Weather.GlobalWeather.ChangeWeatherSunshine(60, false);

    }

}
