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
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "ǰ���Ǽ����̵꣡";
        }
        else if (RoomPosition == map.MewRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "��С�ģ�\nǰ���в����ı������ڲݴ��д��֣�";
        }
        else if (RoomPosition == map.BabyCenterRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "ǰ����ʵ���������䣡";
        }
        else if (RoomPosition == map.MintRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "ǰ�������𱡺��";
        }
        else if (RoomPosition == map.BerryTreeRoomPoint)
        {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "ǰ��������Сɭ�֣�";
        }
    }

}
