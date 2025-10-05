using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixDigAnimatorPause : MonoBehaviour
{

    public Empty empty;
    Animator animator;
    bool isOver;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {


        if ((empty.isDie || empty == null) ) {
            if (!isOver) {
                Debug.Log("onixdig"); animator.speed = 1; animator.SetTrigger("Over"); isOver = true;
            }
        }
        else
        {
            if (!empty.isEmptyFrozenDone && !empty.isSleepDone && !empty.isCanNotMoveWhenParalysis && !empty.isSilence)
            {
                if (animator.speed == 0) { animator.speed = 1; }
            }
            else
            {
                if (animator.speed != 0) { animator.speed = 0; }
            }
        }
    }
}
