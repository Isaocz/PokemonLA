using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySlamCollider : MonoBehaviour
{
    Skill ParentHeavySlam;

    private void Start()
    {
        ParentHeavySlam = transform.parent.GetComponent<Skill>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                ParentHeavySlam.HitAndKo(e);
            }
        }
    }
}
