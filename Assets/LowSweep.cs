using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowSweep : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
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
                if (SkillFrom == 2 && e.isSpeedChange)
                {
                    CTDamage++;
                }
                e.SpeedChange();
                e.SpeedRemove01(5);
            }
        }
    }
}
