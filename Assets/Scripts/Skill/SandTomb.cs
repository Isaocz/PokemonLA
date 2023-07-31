using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandTomb : Skill
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
                if (!SlienceEmptyList.Contains(target))
                {
                    SlienceEmptyList.Add(target);
                    target.Blind(2.5f, 1);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SoftMud softMud = other.GetComponent<SoftMud>();
        if (SkillFrom == 2 && softMud != null && !softMud.isBeUsed)
        {
            Instantiate(this, other.transform.position, Quaternion.identity).ExistenceTime = MaxTime;
            softMud.isBeUsed = true;
        }
    }

}
