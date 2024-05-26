using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMap : MonoBehaviour
{

    public static TownMap townMap;

    public TownPlayer Player;
    public Camera MainCamera;

    //表示玩家在小镇中所处的位置
    public enum TownPlayerState
    {
        inMilkBar, //玩家在酒吧中
        inTown,    //玩家在镇上
    }
    public TownPlayerState State;

    private void Awake()
    {
        townMap = this;
        State = TownPlayerState.inTown;

        FindObjectOfType<CameraAdapt>().HideCameraMasks();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<TownPlayer>() != null) { Player = FindObjectOfType<TownPlayer>(); Player.transform.position = InstancePlayerPosition() ;  }
        if (FindObjectOfType<Camera>() != null) { MainCamera = FindObjectOfType<Camera>(); MainCamera.transform.position = InstanceCameraPosition() ;  }
    }

    Vector3 InstancePlayerPosition()
    {
        Vector3 OutPut = Vector3.zero;
        switch (State)
        {
            case TownPlayerState.inTown:
                break;
            case TownPlayerState.inMilkBar:
                OutPut = new Vector3 ( 1021.69f , 3.2f , 0);
                break;
        }
        return OutPut;
    }


    Vector3 InstanceCameraPosition()
    {
        Vector3 OutPut = new Vector3(0, 0.7f, -11);
        switch (townMap.State)
        {
            case TownPlayerState.inTown:
                OutPut = new Vector3(0, 0.7f, -11);
                break;
            case TownPlayerState.inMilkBar:
                Debug.Log(townMap.State);
                OutPut = new Vector3(1014.43f, 0.7f, -11);
                break;
        }

        return OutPut;
    }

    public Vector2[] CameraBoard()
    {
        Vector2[] OutPut = new Vector2[] {
                    new Vector2(30000.0f, 30000.0f),
                    new Vector2(-30000.0f, 30000.0f),
                    new Vector2(-30000.0f, -30000.0f),
                    new Vector2(30000.0f, -30000.0f)
                };
        switch (townMap.State)
        {
            case TownPlayerState.inTown:
                OutPut = new Vector2[] {
                    new Vector2(34.0f, 31.6f),
                    new Vector2(-26.0f, 31.6f),
                    new Vector2(-26.0f, -2.0f),
                    new Vector2(34.0f, -2.0f)
                };
                break;
            case TownPlayerState.inMilkBar:
                Debug.Log(townMap.State);
                OutPut = new Vector2[] {
                    new Vector2(1029.2f, 11.4f),
                    new Vector2(970.8f, 11.4f),
                    new Vector2(970.8f, -10.1f),
                    new Vector2(1029.2f, -10.1f)
                };
                break;
        }
        return OutPut;
    }

}
