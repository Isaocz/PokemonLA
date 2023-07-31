using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : Skill
{
    GameObject BabyFile;
    int ChildCount;
    CircleCollider2D circleCollider;
    ParticleSystem PS1;
    ParticleSystem PS2;

    // Start is called before the first frame update
    void Start()
    {
        if (player != null && transform.parent.GetComponent<PlayerControler>() != null) {
            BabyFile = player.transform.GetChild(5).gameObject;
            foreach (Transform ChildFile in BabyFile.transform)
            {
                if (ChildFile.childCount > 0)
                {
                    foreach (Transform Baby in ChildFile.transform)
                    {
                        Round r = this;
                        r.player = null;
                        r.baby = Baby.GetComponent<Baby>();
                        Instantiate(r, Baby.transform.position, Quaternion.identity, Baby.transform) ;
                        ChildCount++;
                    }
                }
            }
            if (SkillFrom == 2)
            {
                circleCollider = GetComponent<CircleCollider2D>();
                PS1 = transform.GetChild(0).GetComponent<ParticleSystem>();
                PS2 = transform.GetChild(1).GetComponent<ParticleSystem>();

                var SofL = PS1.sizeOverLifetime;
                SofL.sizeMultiplier += ChildCount;//1.6 4 2 10

                var PS2Main = PS2.main;
                PS2Main.startSpeed = 3 + ChildCount * 0.5f;

                var PS2E = PS2.emission;
                PS2E.rateOverTime = 10 + 2.5f * ChildCount;

                circleCollider.radius += 0.4f * ChildCount;

                SpDamage += 10 * ChildCount;

            }
        }
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
            if (target != null) {
                HitAndKo(target);
            }
        }
    }

}
