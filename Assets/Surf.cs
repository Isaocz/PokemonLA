using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surf : Skill
{
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
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                if (SkillFrom == 2)
                {
                    target.SpeedChange();
                    target.SpeedRemove01(3.0f * target.OtherStateResistance);
                }
                HitAndKo(target);
            }


        }
    }
}
