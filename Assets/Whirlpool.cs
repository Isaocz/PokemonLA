using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlpool : Skill
{


    List<Empty> SlienceEmptyList = new List<Empty> { };
    float MaxTime;

    // Start is called before the first frame update
    void Start()
    {
        MaxTime = ExistenceTime;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if (!target.isSpeedChange) {
                    target.SpeedChange();
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null && target.isSpeedChange)
            {
                target.SpeedRemove01(0.2f);
            }

        }
    }
}
