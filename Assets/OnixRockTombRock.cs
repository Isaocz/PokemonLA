using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixRockTombRock : MonoBehaviour
{
    public bool isBreak;

    private void Start()
    {
        Invoke("RockBreak", 30);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isBreak && (other.gameObject.tag == "Empty" || other.gameObject.GetComponent<OnixTailDig>() != null ))
        {
            RockBreak();
        }
    }



    public void RockBreak()
    {
        if (gameObject != null && !isBreak)
        {
            isBreak = true;
            GetComponent<Animator>().SetTrigger("Break");
            GetComponent<Collider2D>().enabled = false;
        }
    }



    public void DestorySelf()
    {
        Destroy(gameObject);
    }
}
