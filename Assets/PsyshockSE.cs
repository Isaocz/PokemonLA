using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsyshockSE : Skill
{
    public float boomTime;
    void Start()
    {
        Destroy(gameObject, 1.2f);
        Invoke("HitEnemy", boomTime);
    }

    void HitEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.3f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Empty"))
            {
                Empty enemy = collider.GetComponent<Empty>();
                if (enemy != null)
                {
                    HitAndKo(enemy);
                }
            }
        }
    }
}
