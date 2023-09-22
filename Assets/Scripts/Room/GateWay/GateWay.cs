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

    public void DoorEnable()
    {
        animator.speed = 0;
    }
}
