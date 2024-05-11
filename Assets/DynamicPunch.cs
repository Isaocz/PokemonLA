using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPunch : Skill
{
    Collider2D c;
    SkillColliderRangeChangeByTime s;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                HitAndKo(e);
                e.EmptyConfusion(5.0f, 1);
            }
        }
    }
}
