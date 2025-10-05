using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGress : MonoBehaviour
{


    Animator animator;
    public int Count;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "NPC")
        {
            if (Count == 0) { animator.SetTrigger("PlayerIn"); }
            Count++;
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "NPC")
        {
            Count--;
            if (Count == 0) { animator.SetTrigger("PlayerOut"); }
        }
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
