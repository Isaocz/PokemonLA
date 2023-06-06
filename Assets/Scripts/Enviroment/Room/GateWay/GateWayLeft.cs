using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateWayLeft : GateWay
{

    
    //����һ�������ͱ�����������Ƿ��ƶ���һ����ʱ��
    bool isCameraMove = false;
    float timer = 0;

    public float MoveDistance;


    //ÿ֡���һ�Σ���������ײ���������ʼ�ƶ���110�����ٶȣ����ƶ�����λ�ú�����������ƶ���������һ�������λ��
    private void Update()
    {
        if (isCameraMove)
        {
            Vector3 position = Maincamera.transform.position;
            position.x = position.x - 150 * Time.deltaTime;
            timer += 150 * Time.deltaTime;
            Maincamera.transform.position = position;
            if (timer >= 30.0f)
            {
                isCameraMove = false;
                Maincamera.transform.position += new Vector3(timer - 30.0f, 0,  0);
                timer = 0.0f;
            }
        }
  
        if (transform.parent.GetComponent<Room>() == null) { 
            if(transform.parent.parent.GetComponent<Room>().isClear == 0)
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
        else if(transform.parent.GetComponent<Room>().isClear == 0 && GetComponent<BoxCollider2D>().isTrigger == false)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            animator.SetTrigger("Enable");

        }

    }

    //����ײʱ����ײ���������ʱ,�������ʼ�ƶ�
    private void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.tag == ("Player") )
        {
            isCameraMove = true;
            Player.transform.position += new Vector3(-MoveDistance, 0, 0);
            Player.GetComponent<PlayerControler>().NowRoom = Player.GetComponent<PlayerControler>().NowRoom + Vector3Int.left;
            Player.GetComponent<PlayerControler>().InANewRoom = true;
            Player.GetComponent<PlayerControler>().NewRoomTimer = 0f;
            UiMiniMap.Instance.SeeMapOver();
            UiMiniMap.Instance.MiniMapMove(Vector3.right);
        }
    }
}
