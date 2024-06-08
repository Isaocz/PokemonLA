using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : TownChair
{

    private void Start()
    {
        ChairsStart();
        animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        ChairsJumpUp();
        ChairsSetDown();
        ChairsJumpDown();
    }
}
