using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBoorAnimation : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == ("Player") && transform.parent.parent.GetComponent<Room>().isClear == 0)
        {
            animator.SetTrigger("Door");
        }
    }



    public void DestoryDoor()
    {
        transform.parent.GetChild(4).gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
