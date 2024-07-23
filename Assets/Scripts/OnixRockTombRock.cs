using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixRockTombRock : MonoBehaviour
{
    public bool isBreak;
    public bool isStealthRock;
    public CrystalOnixStealthRock StealthRock;

    private void Start()
    {
        if (!isStealthRock) { Invoke("RockBreak", 30); }
        else { Invoke("RockBreak", 4); }
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
            Debug.Log("XSXS");
            if (isStealthRock) { Instantiate(StealthRock , transform.position , Quaternion.identity);}
        }
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
        
    }
}
