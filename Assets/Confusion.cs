using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : Skill
{
    public float moveSpeed;
    // Start is called before the first frame update

    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    bool isSpAUp;



    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);
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
            isCanNotMove = true;
            animator.SetTrigger("Over");
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {

            if (other.tag == "Empty" || other.tag == "Room" || other.tag == "Enviroment")
            {

                Empty target = other.GetComponent<Empty>();
                if (target != null)
                {
                    HitAndKo(target);
                    if (Random.Range(0f, 1f) + (float)player.LuckPoint / 30 > 0.9f)
                    {
                        target.EmptyConfusion(5.0f, 1.0f);
                    }
                }
                BallBreak();
            }
        }
    }
}
