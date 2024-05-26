using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownStreetlight : MonoBehaviour
{

    //路灯是否点亮
    public bool isOn;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        LighrOff();
    }

    /// <summary>
    /// 点亮路灯
    /// </summary>
    public void LighrOn()
    {
        isOn = true;
        animator.SetBool("On" , true);
    }

    /// <summary>
    /// 熄灭路灯
    /// </summary>
    public void LighrOff()
    {
        isOn = false;
        animator.SetBool("On", false);
    }

}
