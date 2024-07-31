using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuseRay : Skill
{
    public float moveSpeed = 5f;
    public GameObject confusionEffect;

    bool isPlus;
    bool isCannotMove;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isCannotMove) { transform.Translate(Vector2.right * moveSpeed * Time.deltaTime); }
        StartExistenceTimer();
    }

    void Blast()
    {
        isCannotMove = true;
        animator.SetTrigger("Blast");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.tag == "Room")
        {
            Blast();
        }

        if (collision.tag == "Enviroment")
        {
            if (SkillFrom != 2) { Blast(); }
            else { isPlus = true; moveSpeed += 1.8f; }
        }

        if (collision.tag == "Empty")
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                target.EmptyConfusion(12.0f, 1.0f);
                if (isPlus)
                {
                    target.SpeedChange();
                    target.SpeedRemove01(3 * target.OtherStateResistance);
                }
                Blast();
                GameObject effect = Instantiate(confusionEffect, target.transform.GetChild(2).position + new Vector3(0,0.5f,0f), Quaternion.identity , target.transform);
                //effect.GetComponent<ConfuseRayEffect>().target = target.transform;
                Destroy(effect, 1f);
            }
        }

    }
}
