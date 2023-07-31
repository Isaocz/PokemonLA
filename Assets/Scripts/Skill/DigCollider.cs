using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigCollider : MonoBehaviour
{
    Skill ParentDig;

    private void Start()
    {
        ParentDig = transform.parent.parent.GetComponent<Skill>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                ParentDig.HitAndKo(e);
            }
        }
    }
}
