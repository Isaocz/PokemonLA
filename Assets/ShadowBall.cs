using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBall : Skill
{
    public float moveSpeed;
    // Start is called before the first frame update

    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    GameObject OverPS1;
    bool isSpAUp;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        OverPS1 = transform.GetChild(3).gameObject;
    }


    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * moveSpeed * Time.deltaTime;
            postion.y += direction.y * moveSpeed * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                BallBreak();

            }
        }
    }


    void BallBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Over");
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
                if (target != null) {
                    HitAndKo(target);
                    if (Random.Range(0f, 1f) + (float)player.LuckPoint / 30 > 0.8f)
                    {
                        target.SpAChange(-1,0.0f);
                    }
                }
                BallBreak();
            }
            else if (other.tag == "Room")
            {
                BallBreak();
            }
            else if (other.tag == "Enviroment")
            {
                if (SkillFrom != 2)
                {
                    BallBreak();
                }
                else
                {
                    if (!isSpAUp) { SpDamage += 20; moveSpeed += 2.5f; isSpAUp = true; }
                }
            }
        }
    }
}
