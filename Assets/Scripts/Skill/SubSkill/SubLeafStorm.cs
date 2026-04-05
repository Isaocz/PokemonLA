using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLeafStorm : Skill
{
    // Start is called before the first frame update
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            HitAndKo(collision.gameObject);
        }
    }
}
