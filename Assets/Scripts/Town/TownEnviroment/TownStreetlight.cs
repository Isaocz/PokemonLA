using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownStreetlight : MonoBehaviour
{

    //·���Ƿ����
    public bool isOn;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        LighrOff();
    }

    /// <summary>
    /// ����·��
    /// </summary>
    public void LighrOn()
    {
        isOn = true;
        animator.SetBool("On" , true);
    }

    /// <summary>
    /// Ϩ��·��
    /// </summary>
    public void LighrOff()
    {
        isOn = false;
        animator.SetBool("On", false);
    }

}
