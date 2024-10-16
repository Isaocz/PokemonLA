﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IteamPickUp : Item
{
    
    //声明一个布尔型变量，表示物品是否被触发
    public bool isLunch = false;
    protected bool CanBePickUp = false;
    protected Vector3 StartPosition;
    protected Vector3 Direction;
    protected float LunchSpeed = 0.08f;



    //声明一个变量,代表物品移动的目标
    public GameObject targer;


    public bool BanLunchUp;
    public bool BanLunchDown;
    public bool BanLunchRight;
    public bool BanLunchLeft;
    int BanCount;

    public MiniMapBlock.MiniMapBlockMarkType MiniMapBlockMark;


    // Start is called before the first frame update
    void Start()
    {
        //获取玩家
        targer = GameObject.FindObjectOfType<PlayerControler>().gameObject;
        StartPosition = gameObject.transform.position;
        int r = Random.Range(0,360);
        while ((BanLunchUp && r > 45 && r <= 135) || (BanLunchLeft && r > 135 && r <= 225) || (BanLunchDown && r > 225 && r <= 315) || (BanLunchRight &&( r > 315 || r <= 45)))
        {
            r = Random.Range(0, 360);
            BanCount++;
            if (BanCount >= 20)
            {
                break;
            }
        }
        Direction = ( Quaternion.AngleAxis(r,Vector3.forward) * Vector3.right ).normalized;
    }

    void Update()
    {
        if (targer == null) {
            if (GameObject.FindObjectOfType<PlayerControler>() == null)
            {
                Destroy(gameObject);
            }
            else
            {
                targer = GameObject.FindObjectOfType<PlayerControler>().gameObject;
            }
        }
    }

    public void LunchItem()
    {
        //Debug.Log((transform.position.x));
        //Debug.Log((transform.position.x + 15.0f));
        //Debug.Log(((transform.position.x + 15.0f) / 30));
        //Debug.Log((((transform.position.x + 15.0f) / 30) * 30.0f));
        //gameObject.transform.position = new Vector3(  Mathf.Clamp(gameObject.transform.position.x + LunchSpeed * Direction.x , (((StartPosition.x+15.0f) / (int)30 )*30.0f) - 12.0f, (((StartPosition.x + 15.0f) / (int)30)*30.0f) + 12.0f), Mathf.Clamp(gameObject.transform.position.y + LunchSpeed * Direction.y, (((StartPosition.y + 12.0f) / (int)24)*24.0f) - 8.5f, (((StartPosition.y + 12.0f) / (int)24)*24.0f) + 8.5f) ,0);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + LunchSpeed * Direction.x, gameObject.transform.position.y + LunchSpeed * Direction.y, 0);
        LunchSpeed -= 0.0012f;
        if(LunchSpeed <= 0.002f)
        {
            CanBePickUp = true;
            LunchSpeed = 0.08f;
        }
    } 

    protected void DoNotLunch()
    {
        LunchSpeed -= 0.0012f;
        if (LunchSpeed <= 0.002f)
        {
            CanBePickUp = true;
            LunchSpeed = 0.08f;
        }
    }

}