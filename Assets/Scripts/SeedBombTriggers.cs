using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBombTriggers : MonoBehaviour
{

    public SeedBomb ParentSB;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            ParentSB.HitAndKo(target);
        }
    }
}
