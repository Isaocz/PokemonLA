using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassyTerrainButton : EnviromentButton
{


    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("GrassyTerrainButton");
        Instantiate(PassiveItemGameObjList.ObjList.List[28], transform.position, Quaternion.identity);
    }

}
