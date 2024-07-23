using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateWay : MonoBehaviour
{
    //�����������
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
    /// ����������Ѳ�ͬ������ʱ������λ������ͬ
    /// </summary>
    /// <returns></returns>
    public float MoveDisByPlayerBody( PlayerControler Player )
    {
        float OutPut = 0;
        switch (Player.PlayerBodySize)
        {
            case 0: //С����
                OutPut = 0;
                break;
            case 1: //������
                OutPut = 0.28f;
                break;
            case 2: //������
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
