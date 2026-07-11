using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailButton : EnviromentButton
{


    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("HailButton");


        Weather.GlobalWeather.ChangeWeatherHail(60, false);

    }

}
