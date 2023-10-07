using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Covet : Skill
{
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
            }
        }
    }
}
