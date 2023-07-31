using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedDoorSetText : MonoBehaviour
{

    public void SetLockedDoorText(MapCreater map , Vector3Int RoomPosition)
    {

        Debug.Log(RoomPosition);
        if(RoomPosition == map.SkillShopRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "前方是技能商店！";
        }
        else if (RoomPosition == map.MewRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "请小心！\n前方有不明的宝可梦在草丛中大闹！";
        }
        else if (RoomPosition == map.BabyCenterRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "前方是实惠培育屋咪！";
        }
        else if (RoomPosition == map.MintRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "前方是香甜薄荷田！";
        }
        else if (RoomPosition == map.BerryTreeRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "前方是树果小森林！";
        }
    }

}
