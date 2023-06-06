using System.Collections;
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



    // Start is called before the first frame update
    void Start()
    {
        //获取玩家
        targer = GameObject.FindGameObjectWithTag("Player");
        StartPosition = gameObject.transform.position;
        Direction = ( new Vector3 (Random.Range(-1.0f, 1.0f) , Random.Range(-1.0f,1.0f) , 0.0f)).normalized;
    }

    public void LunchItem()
    {
        gameObject.transform.position += new Vector3(LunchSpeed * Direction.x, LunchSpeed * Direction.y,0);
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