using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanIceSpinner : MonoBehaviour
{
    //¸¸ºÆ´ó¾¨
    public Cetitan ParentCetitan;

    //Á£×ÓÁÐ±í
    public Transform PSList;

    //¼¼ÄÜÅö×²È¦
    public SkillColliderRangeChangeByTimeManual SkillCollider;





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





    /// <summary>
    /// ±ùÐý½áÊø
    /// </summary>
    public void SpinnerOver()
    {
        SkillCollider.SkillCircleOver();
        for (int i = 0; i < PSList.transform.childCount; i++)
        {
            ParticleSystem ps = PSList.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var m = ps.main;
                m.loop = false;
            }
        }
        Destroy(this.gameObject, 2.0f);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        ParentCetitan.ReboundCheck(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ParentCetitan.CollisionEnter2DEvent_IceSpinner(collision.gameObject);
        }
        //Ïû³ý³¡µØ
        if (collision.gameObject.GetComponent<GrassyTerrain>())
        {
            if (collision.gameObject.transform.childCount != 0)
            {
                for (int i = 0; i < collision.gameObject.transform.childCount; i++)
                {
                    if (collision.gameObject.transform.GetChild(i).GetComponent<SkillRangeCircle>())
                    {
                        collision.gameObject.transform.GetChild(i).GetComponent<SkillRangeCircle>().DestroyCircle();
                    }
                    if (collision.gameObject.transform.GetChild(i).GetComponent<ParticleSystem>())
                    {
                        var e = collision.gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().emission;
                        e.enabled = false;
                    }
                }
                collision.gameObject.transform.DetachChildren();
            }
        }
        //»÷Ëé±ùÖù
        //CetitanIcicleCrashOBJ icOBJ = collision.gameObject.GetComponent<CetitanIcicleCrashOBJ>();
        //if (icOBJ != null)
        //{
        //    icOBJ.BreakByCetitan();
        //}
    }


    private void OnDestroy()
    {

    }
}
