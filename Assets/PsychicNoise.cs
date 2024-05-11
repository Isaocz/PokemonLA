using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicNoise : Skill
{

    List<Empty> EmptyFearList = new List<Empty> { };

    public SubZenHeadbutt SubZH;

    private void Start()
    {
        if (SkillFrom == 2) { player.AddASubSkill(SubZH); }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Empty target = collision.GetComponent<Empty>();
        if (target != null)
        {
            HitAndKo(target);
            if (!EmptyFearList.Contains(target))
            {
                EmptyFearList.Add(target);
                if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 15.0f) > 0.7f)
                {
                    target.Fear(4.0f, 1);
                }
            }
        }
    }
}
