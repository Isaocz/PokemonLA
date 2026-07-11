using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicTerrainButton : EnviromentButton
{


    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("PsychicTerrainButton");
        Instantiate(PassiveItemGameObjList.ObjList.List[30], transform.position, Quaternion.identity);
    }

}
