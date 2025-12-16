using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHex : MonoBehaviour
{

    public float count;

    private void Start()
    {
        Animator animator = transform.GetComponent<Animator>();
        animator.SetFloat("Count", count);
    }
}
