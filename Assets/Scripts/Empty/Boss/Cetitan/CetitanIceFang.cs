using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanIceFang : MonoBehaviour
{
    public Cetitan ParentCetitan;


    /// <summary>
    /// Åö×²Ïä
    /// </summary>
    CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = transform.GetComponent<CircleCollider2D>();
    }


    private void Update()
    {
        //Ïú»Ù±ùÖù
        if (ParentCetitan != null && circleCollider != null)
        {
            ParentCetitan.Break_IcleCrash_Circle(((Vector2)transform.position + circleCollider.offset), circleCollider.radius);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ParentCetitan.CollisionEnter2DEvent_IceFang(collision.gameObject);
        }
        
        //ÆôÓÃ »÷Ëé±ùÖù
        //CetitanIcicleCrashOBJ icOBJ = collision.gameObject.GetComponent<CetitanIcicleCrashOBJ>();
        //if (icOBJ != null)
        //{
        //    icOBJ.BreakByCetitan();
        //}
    }

}
