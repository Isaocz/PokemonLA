using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillButton : EnviromentButton
{

    //public Room ParentRoom;
    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();
    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("KillButton");
        ParentRoom = FindParentRoom();
        if (ParentRoom != null)
        {
            Transform emptyFile = ParentRoom.EmptyFile();
            ParentRoom.CameraShake(2.0f , 2.5f , true);
            for (int i = 0; i < emptyFile.transform.childCount; i++)
            {
                Empty e = emptyFile.transform.GetChild(i).GetComponent<Empty>();
                if (e != null) { Pokemon.PokemonHpChange(null, e.gameObject, e.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType); }
            }
        }
    }
}
