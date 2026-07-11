using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSpikeButton : EnviromentButton
{

    public RemoveSpikeButtonSmoke Smoke1;
    public GameObject Smoke2;
    
    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("RemoveSpike");
        ParentRoom = FindParentRoom();
        if (ParentRoom != null)
        {
            Instantiate(Smoke1, ParentRoom.transform.position , Quaternion.identity);
            Instantiate(Smoke2, transform.position , Quaternion.identity);
        }
    }
}
