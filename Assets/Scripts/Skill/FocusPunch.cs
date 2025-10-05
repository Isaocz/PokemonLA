using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPunch : Skill
{

    Collider2D c;
    SkillColliderRangeChangeByTime s;


    int PlayerBeforHP;
    bool isHPDown;


    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<Collider2D>();
        s = GetComponent<SkillColliderRangeChangeByTime>();

        PlayerBeforHP = player.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (ExistenceTime >= 2.0f && !isHPDown && player.Hp < PlayerBeforHP)
        {
            isHPDown = true;
        }
        StartExistenceTimer();
        if (ExistenceTime <= 2.0f && !c.enabled)
        {
            if (!isHPDown)
            {
                c.enabled = true;
                s.enabled = true;
            }
            else
            {
                ExistenceTime = 0;
                Destroy(gameObject);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                HitAndKo(e);

            }
        }
    }
}
