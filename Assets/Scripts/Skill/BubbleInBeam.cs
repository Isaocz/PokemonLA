using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleInBeam : Skill
{
    BubbleBeam ParentBB;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        ParentBB = transform.parent.GetComponent<BubbleBeam>();
        player = ParentBB.player;
        SpDamage = ParentBB.SpDamage;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 4.85f)
        {
            timer = 0f;
            animator.SetTrigger("Boom");
            Destroy(gameObject, 0.13f);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty") || (SkillFrom == 2 ? false : collision.CompareTag("Enviroment"))) 
        {
            Empty target = collision.GetComponent<Empty>();
            HitAndKo(target);
            animator.SetTrigger("Boom");
            Destroy(gameObject, 0.13f);
        }
    }
}
