using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleHitSon : DoubleHit
{

    private void Start()
    {
        player = transform.parent.GetComponent<DoubleHit>().player;
    }

    public GameObject TackleBlast;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            Instantiate(TackleBlast, target.transform.position, Quaternion.identity);
            HitAndKo(target);
        }
    }
}
