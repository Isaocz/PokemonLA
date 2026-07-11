using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearButton : EnviromentButton
{

    //public Room ParentRoom;
    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();
    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("FearButton");
        ParentRoom = FindParentRoom();
        if (ParentRoom != null)
        {
            Transform emptyFile = ParentRoom.EmptyFile();
            ParentRoom.CameraShake(1.0f, 1.5f, true);
            for (int i = 0; i < emptyFile.transform.childCount; i++)
            {
                Empty e = emptyFile.transform.GetChild(i).GetComponent<Empty>();
                if (e != null) { e.Fear(7.5f , 1); }
            }
        }
    }
}
