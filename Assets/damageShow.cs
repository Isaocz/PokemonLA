using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageShow : MonoBehaviour
{
    public float DestoryTime;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, 1f);
    }
}
