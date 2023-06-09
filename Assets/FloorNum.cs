using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorNum : MonoBehaviour
{
    public static FloorNum GlobalFloorNum;
    public int FloorNumber;
    public int MaxFloor;
    public int[] MapSize = new int[] { 8, 15, 20 };

    private void Awake()
    {
        GlobalFloorNum = this;
        DontDestroyOnLoad(this);
        FloorNumber = 0;
    }
}
