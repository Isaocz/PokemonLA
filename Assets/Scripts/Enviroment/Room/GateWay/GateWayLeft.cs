using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateWayLeft : GateWay
{

    
    //声明一个布尔型变量，摄像机是否移动。一个计时器
    bool isCameraMove = false;
    float timer = 0;

    public float MoveDistance;


    //每帧检测一次，当发生碰撞后摄像机开始移动，110代表速度，当移动到大位置后不允许摄像机移动，并修正一次摄像机位置
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

    //当碰撞时且碰撞对象是玩家时,摄像机开始移动
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
