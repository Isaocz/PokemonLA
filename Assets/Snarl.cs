using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snarl : Skill
{
    public GameObject snarleffect;

    private void Update()
    {
        StartExistenceTimer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                target.SpAChange(-1, 4f);
                GameObject se = Instantiate(snarleffect, target.transform.position, Quaternion.identity);
                Destroy(se, 0.5f);
            }
        }
    }
}
