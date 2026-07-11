using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandstomeButton : EnviromentButton
{


    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("SandstomeButton");
        Weather.GlobalWeather.ChangeWeatherSandStorm(60, false);

    }

}
