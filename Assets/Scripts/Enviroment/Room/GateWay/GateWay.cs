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

    public void DoorEnable()
    {
        animator.speed = 0;
    }
}
