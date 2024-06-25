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
        inWoodenHouse,    //玩家在铁骨建筑公司木屋内
        inSkillMaker,    //玩家在图图技能艺术廊内
        inDayCareF1,    //玩家在破壳宝育园一层
        inDayCareF2,    //玩家在破壳宝育园二层
        inItemShop,    //玩家在道具商店
        inBossClub,    //玩家在头目俱乐部

    }
    public TownPlayerState State;
    public TownPlayerState StartState;

    private void Awake()
    {
        townMap = this;
        State = StartState;

        FindObjectOfType<CameraAdapt>().HideCameraMasks();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<TownPlayer>() != null) { Player = FindObjectOfType<TownPlayer>(); Player.transform.position = InstancePlayerPosition() ;  }
        if (FindObjectOfType<Camera>() != null) { MainCamera = FindObjectOfType<Camera>(); MainCamera.transform.position = InstanceCameraPosition() ;  }
    }

    public Vector3 InstancePlayerPosition()
    {
        Vector3 OutPut = Vector3.zero;
        switch (State)
        {
            case TownPlayerState.inTown:
                break;
            case TownPlayerState.inMilkBar:
                OutPut = new Vector3 (1016.27f, 1.98f, 0);
                break;
            case TownPlayerState.inWoodenHouse:
                OutPut = new Vector3(202.45f, -1.51f, 0);
                break;
            case TownPlayerState.inSkillMaker:
                OutPut = new Vector3(400.0f, -0.14f, 0);
                break;
            case TownPlayerState.inDayCareF1:
                OutPut = new Vector3(606.96f, -5.41f, 0);
                break;
            case TownPlayerState.inDayCareF2:
                OutPut = new Vector3(794.278f, 3.53f, 0);
                break;
            case TownPlayerState.inItemShop:
                OutPut = new Vector3(200.0f, 201.6f, 0);
                break;
            case TownPlayerState.inBossClub:
                OutPut = new Vector3(387.04f, 197.85f, 0);
                break;
        }
        return OutPut;
    }


    public Vector3 InstanceCameraPosition()
    {
        Vector3 OutPut = new Vector3(0, 0.7f, -11);
        switch (townMap.State)
        {
            case TownPlayerState.inTown:
                OutPut = new Vector3(0, 0.7f, -11);
                break;
            case TownPlayerState.inMilkBar:
                OutPut = new Vector3(1010.871f, 0.6414994f, -11);
                break;
            case TownPlayerState.inWoodenHouse:
                OutPut = new Vector3(200.0f, 0.6414994f, -11);
                break;
            case TownPlayerState.inSkillMaker:
                OutPut = new Vector3(400.0f, 0.6414994f, -11);
                break;
            case TownPlayerState.inDayCareF1:
                OutPut = new Vector3(600.0f, 0.6414994f, -11);
                break;
            case TownPlayerState.inDayCareF2:
                OutPut = new Vector3(800.0f, 0.6414994f, -11);
                break;
            case TownPlayerState.inItemShop:
                OutPut = new Vector3(200.0f, 200.12f, -11);
                break;
            case TownPlayerState.inBossClub:
                OutPut = new Vector3(396.44f, 199.98f, -11);
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
                    new Vector2(36.0f, 32.2f),
                    new Vector2(-26.0f, 32.2f),
                    new Vector2(-26.0f, -2.0f),
                    new Vector2(36.0f, -2.0f)
                };
                break;
            case TownPlayerState.inMilkBar:
                OutPut = new Vector2[] {
                    new Vector2(1021.2f + 00, 11.4f + 00),
                    new Vector2(978.8f -00, 11.4f + 00),
                    new Vector2(978.8f -00, -10.1f - 00),
                    new Vector2(1021.2f + 00, -10.1f - 00)
                };
                break;
            case TownPlayerState.inWoodenHouse:
                OutPut = new Vector2[] {
                    new Vector2(218.6062f, 10.74151f),
                    new Vector2(181.3938f, 10.74151f),
                    new Vector2(181.3938f, -10.74151f),
                    new Vector2(218.6062f, -10.74151f)
                };
                break;
            case TownPlayerState.inSkillMaker:
                OutPut = new Vector2[] {
                    new Vector2(414.323f, 10.74151f),
                    new Vector2(385.677f, 10.74151f),
                    new Vector2(385.677f, -10.74151f),
                    new Vector2(414.323f, -10.74151f)
                };
                break;
            case TownPlayerState.inDayCareF1:
                OutPut = new Vector2[] {
                    new Vector2(618.6063f, 10.74151f),
                    new Vector2(581.3937f, 10.74151f),
                    new Vector2(581.3937f, -10.74151f),
                    new Vector2(618.6063f, -10.74151f)
                };
                break;
            case TownPlayerState.inDayCareF2:
                OutPut = new Vector2[] {
                    new Vector2(818.6063f, 10.74151f+4.598501f),
                    new Vector2(781.3937f, 10.74151f+4.598501f),
                    new Vector2(781.3937f, -10.74151f+4.698501f),
                    new Vector2(818.6063f, -10.74151f+4.698501f)
                };
                break;
            case TownPlayerState.inItemShop:
                OutPut = new Vector2[] {
                    new Vector2(211.6625f, 207.875f),
                    new Vector2(188.3375f, 207.875f),
                    new Vector2(188.3375f, 192.125f),
                    new Vector2(211.6625f, 192.125f)
                };
                break;
            case TownPlayerState.inBossClub:
                OutPut = new Vector2[] {
                    new Vector2(417.8835f, 210.723f),
                    new Vector2(382.1165f, 210.723f),
                    new Vector2(382.1165f, 189.2377f),
                    new Vector2(417.8835f, 189.2377f)
                };
                break;
        }
        return OutPut;
    }

}
