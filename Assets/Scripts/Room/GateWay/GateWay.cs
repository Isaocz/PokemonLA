using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateWay : MonoBehaviour
{
    //声明相机对象
    public GameObject Maincamera;

    public Animator animator;
    public void Awake()
    {
        animator = GetComponent<Animator>();
        Maincamera = GameObject.FindWithTag("MainCamera");
    }

    private void Start()
    {
        Transform ParentRoom = transform.parent;
        while (ParentRoom.GetComponent<Room>() == null)
        {
            if (ParentRoom.transform.parent == null)
            {
                break;
            }    
            ParentRoom = ParentRoom.transform.parent;
        }
        if (ParentRoom.GetComponent<Room>() != null) {
            GetComponent<SpriteRenderer>().color = ParentRoom.GetComponent<Room>().GateWayColor;
        }
    }

    /// <summary>
    /// 根据玩家提醒不同，过门时产生的位移量不同
    /// </summary>
    /// <returns></returns>
    public float MoveDisByPlayerBody( PlayerControler Player )
    {
        float OutPut = 0;
        switch (Player.PlayerBodySize)
        {
            case 0: //小体型
                OutPut = 0;
                break;
            case 1: //中体型
                OutPut = 0.28f;
                break;
            case 2: //大体型
                OutPut = 0.55f;
                break;
        }
        return OutPut;
    }

    public void DoorEnable()
    {
        animator.speed = 0;
    }
}
