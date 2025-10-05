using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidSpaceItemButton : MonoBehaviour
{
    PlayerControler Player;

    private void Start()
    {
        Player = transform.parent.parent.parent.GetComponent<PlayerControler>();
    }

    public void SpaceItemButtonDown()
    {


        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {
            Player.IsSpaceItemButtonDown = true;
        }
    }
}
