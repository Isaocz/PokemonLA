using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorNum : MonoBehaviour
{
    public static FloorNum GlobalFloorNum;
    public int FloorNumber;
    public int MaxFloor;
    public int[] MapSize = new int[] { 5, 8, 12, 15, 15 };

    public float[] FloorBossHPBonus = new float[] { 1.2f , 1.8f , 2.8f , 4.0f , 5.5f , 7.5f , 10.0f  , 12.5f , 15.0f , 17.5f ,  20.0f  };

    public int[] EmptyLevelAlpha = new int[] { -5 , -3 , -2 , 0 , 0 , 0 , 0 , 2 , 2 , 4 , 4 };

    public string[] NextFloorText = new string[]
    {
        "前方为呼呼森林，请注意不要迷路！",
        "前方为呼呼森林的深处，请注意不要迷路！",
        "前方为隆隆山洞，请注意落石！",
        "前方为隆隆山洞的深处，请注意落石！",
        "前方为隆隆山洞的深处，请注意落石！",
        "前方为萧萧雪山，请注意保暖！(前方区域尚未开放，感谢您试玩当前版本)",

    };

    public bool isBabyCenterBeCreated;
    public bool isMewRoomBeCreated;
    public bool isMewBeTalk;
    public bool isMintRoomBeCreated;

    public List<bool> isBossRoomBeCreated = new List<bool> { };

    public void InstanceFloorNum()
    {
        FloorNumber = 0;
        isBabyCenterBeCreated = false;
        isMewRoomBeCreated = false;
        isMewBeTalk = false;
        isMintRoomBeCreated = false;
        for (int i = 0; i < isBossRoomBeCreated.Count; i++  )
        {
            isBossRoomBeCreated[i] = false;
        }
    }

    private void Awake()
    {
        GlobalFloorNum = this;
        DontDestroyOnLoad(this);
        FloorNumber = 0;
    }
}
