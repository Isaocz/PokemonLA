using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCheckSwitchChildLight : MonoBehaviour
{
    public bool IsOn
    {
        get { return isON; }
        set { isON = value; }
    } 
    bool isON = false;

    Animator animator;


    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }


    public void On()
    {
        isON = true;
        if (animator != null) { animator.SetBool("ON", true); }
    }

    public void OFF()
    {
        isON = false;
        if (animator != null) { animator.SetBool("ON", false); }
    }


}
