using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePunch : Skill
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
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                HitAndKo(e);
                if (SkillFrom != 2)
                {
                    e.Frozen(7.5f, 1, 0.1f + (float)(player.LuckPoint / 30));
                }
                else
                {

                    e.Frozen(7.5f, 1, 0.2f + (float)(player.LuckPoint / 20));
                }

            }
        }
    }
}
