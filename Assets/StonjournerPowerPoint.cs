using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonjournerPowerPoint : MonoBehaviour
{

    public Empty ParentStonjourner;
    List<Empty> BuffList = new List<Empty> { };

    ParticleSystem PS1;
    ParticleSystem PS2;
    SpriteRenderer Sprite;
    CircleCollider2D Collider;
    public bool isSpriteFadeOut;


    private void Start()
    {
        PS1 = GetComponent<ParticleSystem>();
        PS2 = transform.GetChild(1).GetComponent<ParticleSystem>();
        Sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Collider = GetComponent<CircleCollider2D>();

    }

    private void Update()
    {
        if (isSpriteFadeOut)
        {
            Sprite.color -= new Color(0, 0, 0, Time.deltaTime);
            Collider.radius -= 2 * Time.deltaTime;
            if(Collider.radius <= 1)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (ParentStonjourner != null && !BuffList.Contains(e) && e.gameObject != ParentStonjourner.gameObject)
            {
                BuffList.Add(e);
                e.AtkChange(2,0.0f);
                e.SpAChange(2, 0.0f);
            }

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (ParentStonjourner != null && BuffList.Contains(e) && e.gameObject != ParentStonjourner.gameObject)
            {
                BuffList.Remove(e);
                e.AtkChange(-2 , 0);
                e.SpAChange(-2, 0.0f);
            }

        }
    }


    public void EndPS()
    {
        var e1 = PS1.emission;
        e1.enabled = false;
        var e2 = PS2.emission;
        e2.enabled = false;
        isSpriteFadeOut = true;
    }



    public void RemoveBuffEmptyList()
    {
        foreach (Empty e in BuffList)
        {
            
            e.AtkChange(-2, 0);
            e.SpAChange(-2, 0);
        }
        BuffList.Clear();
    }



    private void OnDestroy()
    {
        RemoveBuffEmptyList();
    }


}
