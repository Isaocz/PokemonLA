using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextFloorGateWay : GateWay
{
    //����һ�������ͱ�����������Ƿ��ƶ���һ����ʱ��

    
    private void Start()
    {
        if (FloorNum.GlobalFloorNum != null) {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = FloorNum.GlobalFloorNum.NextFloorText[FloorNum.GlobalFloorNum.FloorNumber+1];
        }
    }




    //ÿ֡���һ�Σ���������ײ���������ʼ�ƶ���110�����ٶȣ����ƶ�����λ�ú�����������ƶ���������һ�������λ��
    private void Update()
    {
        if (transform.parent.GetComponent<Room>() == null)
        {
            if (transform.parent.parent.GetComponent<Room>().isClear <= 0)
            {
                if (FloorNum.GlobalFloorNum.MaxFloor > FloorNum.GlobalFloorNum.FloorNumber)
                {
                    GetComponent<BoxCollider2D>().isTrigger = true;
                    animator.SetTrigger("Enable");
                }

            }
        }
        else if (transform.parent.GetComponent<Room>().isClear <= 0 && GetComponent<BoxCollider2D>().isTrigger == false)
        {
            if (FloorNum.GlobalFloorNum.MaxFloor > FloorNum.GlobalFloorNum.FloorNumber+1)
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
                animator.SetTrigger("Enable");
            }

        }

    }



    //����ײʱ����ײ���������ʱ,�������ʼ�ƶ�
    private void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.tag == ("Player") && Player.GetComponent<PlayerControler>() != null)
        {
            if (FloorNum.GlobalFloorNum != null && FindObjectOfType<MapCreater>() != null)
            {
                FloorNum.GlobalFloorNum.FloorNumber += 1;
            }
            SceneLoadManger.sceneLoadManger.LoadGame();
        }
    }
}
