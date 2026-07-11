using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTerrainButton : EnviromentButton
{


    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("ElectricTerrainButton");
        Instantiate(PassiveItemGameObjList.ObjList.List[27], transform.position, Quaternion.identity);
    }

}
