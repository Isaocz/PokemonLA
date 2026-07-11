using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainButton : EnviromentButton
{


    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();
    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("RainButton");
        Weather.GlobalWeather.ChangeWeatherRain(60, false);

    }

}
