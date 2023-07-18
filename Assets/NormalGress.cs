using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGress : MonoBehaviour
{

    Animator animator;
    PlayerControler player;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            if (other.GetComponent<PlayerControler>() != null)
            {
                animator.SetTrigger("PlayerIn");
            }
        }
        if (other.transform.tag == "NPC")
        {
            if (other.GetComponent<MewTail>() != null)
            {
                animator.SetTrigger("PlayerIn");
            }
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            animator.SetTrigger("PlayerOut");
        }
        if (other.transform.tag == "NPC")
        {
            if (other.GetComponent<MewTail>() != null)
            {
                animator.SetTrigger("PlayerOut");
            }
        }
    }
}
