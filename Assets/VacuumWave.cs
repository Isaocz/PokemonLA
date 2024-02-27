using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumWave : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    bool isOver;

    public GameObject TackleBlast;

    SpriteRenderer Sprite;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        StartExistenceTimer();

    }

    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 9f * Time.deltaTime;
            postion.y += direction.y * 9f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                PunchBreak();

            }

        }
        if (isCanNotMove && !isOver)
        {
            isOver = true;
            animator.SetTrigger("Over");
        }
    }

    void PunchBreak()
    {
        if (!isCanNotMove)
        {
            var e = transform.GetChild(1).GetComponent<ParticleSystem>().main;
            e.loop = false;
            transform.GetChild(1).parent = null;
            transform.GetComponent<Collider2D>().enabled = false;


            isCanNotMove = true;


        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                PunchBreak();
                if (target != null)
                {
                    Instantiate(TackleBlast , other.transform.position , Quaternion.identity);
                    HitAndKo(target);

                }
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                PunchBreak();
            }
        }
        if (other.tag == "Projectel")
        {
            Destroy(other.gameObject);
            if (SkillFrom == 2)
            {
                CTLevel++;
                CTDamage++;
            }
        }
    }
}
