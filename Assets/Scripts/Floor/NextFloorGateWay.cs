using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextFloorGateWay : GateWay
{
    //声明一个布尔型变量，摄像机是否移动。一个计时器


    Room ParentRoom;

    
    private void Start()
    {
        if (FloorNum.GlobalFloorNum != null) {
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = FloorNum.GlobalFloorNum.NextFloorText[FloorNum.GlobalFloorNum.FloorNumber+1];
        }
        ParentRoom = transform.parent.GetComponent<Room>();
    }

    public GameObject GameOverPanel;




    //每帧检测一次，当发生碰撞后摄像机开始移动，110代表速度，当移动到大位置后不允许摄像机移动，并修正一次摄像机位置
    private void Update()
    {

        if (ParentRoom == null) { ParentRoom = transform.parent.GetComponent<Room>(); }

        //无房间时（不明使用场景）
        if (ParentRoom == null)
        {
            //清除完所有敌人
            if (ParentRoom.isClear <= 0)
            {
                // 最大层数(7) > 当前层数(012345) + 1
                // 可以到达下一层
                if (FloorNum.GlobalFloorNum.MaxFloor > FloorNum.GlobalFloorNum.FloorNumber + 1)
                {
                    GetComponent<BoxCollider2D>().isTrigger = true;
                    animator.SetTrigger("Enable");
                }

            }
        }
        //有房间时（地图冒险）
        else {
            if (ParentRoom.isClear <= 0 && GetComponent<BoxCollider2D>().isTrigger == false)
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
                animator.SetTrigger("Enable");
            }
        }
    }



    //当碰撞时且碰撞对象是玩家时,摄像机开始移动
    private void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.tag == ("Player") && Player.GetComponent<PlayerControler>() != null)
        {
            // 最大层数(7) > 当前层数(012345) + 1
            // 进入下一层
            if (FloorNum.GlobalFloorNum.MaxFloor > FloorNum.GlobalFloorNum.FloorNumber + 1)
            {

                MapCreater m = FindObjectOfType<MapCreater>();
                if (FloorNum.GlobalFloorNum != null && m != null)
                {
                    if (ScoreCounter.Instance != null)
                    {
                        ScoreCounter.Instance.FloorBounsAP += APBounsPoint.FloorBouns(FloorNum.GlobalFloorNum.FloorNumber);
                        ScoreCounter.Instance.CandyBouns += APBounsPoint.FloorCandyBouns(FloorNum.GlobalFloorNum.FloorNumber);
                        ScoreCounter.Instance.TimePunishAP += APBounsPoint.TimePunish(m.MapTime, FloorNum.GlobalFloorNum.FloorNumber);
                    }
                    FloorNum.GlobalFloorNum.FloorNumber += 1;
                }
                SceneLoadManger.sceneLoadManger.LoadGame();
            }
            // 最大层数(7) <= 当前层数(6) + 1
            // 呼出游戏结束界面
            else
            {
                if (TPMask.In != null )
                {
                    TPMask.In.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }


    // FloorNumber = 0 第一层
    // FloorNumber = 1 第二层
    // MaxFloor = 7 (最大层数为第七层)
    private void OnTriggerExit2D(Collider2D Player)
    {
        if (Player.tag == ("Player") && Player.GetComponent<PlayerControler>() != null)
        {
            // 最大层数(7) > 当前层数(012345) + 1
            // 无反应
            if (FloorNum.GlobalFloorNum.MaxFloor > FloorNum.GlobalFloorNum.FloorNumber + 1)
            {

            }
            // 最大层数(7) <= 当前层数(6) + 1
            // 关闭游戏结束界面
            else
            {
                if (TPMask.In != null && TPMask.In.transform.GetChild(1).gameObject.activeInHierarchy)
                {
                    TPMask.In.transform.GetChild(1).gameObject.SetActive(false);
                }
                
            }

        }
    }


}
