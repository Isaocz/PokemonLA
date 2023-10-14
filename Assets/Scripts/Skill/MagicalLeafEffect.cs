using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalLeafEffect : MonoBehaviour
{
    MagicalLeaf ParentML;

    private void Start()
    {
        ParentML = gameObject.transform.parent.GetComponent<MagicalLeaf>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentML.SkillFrom == 2 && (other.GetComponent<GressPlayerINOUT>() != null || other.GetComponent<NormalGress>() != null))
        {
            ParentML.SpDamage += 15;
            GetComponent<TraceEffect>().distance += 1.3f;
            GetComponent<TraceEffect>().moveSpeed += 1.3f;
        }
        if (other.tag == "Empty"||other.tag == "EmptyFly")
        {
            Empty target = other.gameObject.GetComponent<Empty>();
            ParentML.HitAndKo(target);
            DestroyLeaf();
        }
        if (other.tag == "Room")
        {
            DestroyLeaf();
        }
    }

    void DestroyLeaf()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<TraceEffect>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<TrailRenderer>().time = 0.5f;
        transform.GetChild(0).gameObject.SetActive(true);
        ParentML.StartTimer = true;
    }
}
