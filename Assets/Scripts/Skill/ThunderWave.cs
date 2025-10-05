using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWave : Skill
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
                WaveBreak();

            }
        }
        else
        {
            StartExistenceTimer();
        }
    }


    void WaveBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;
            if (transform.childCount > 0) {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).parent = null;
            }

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
                    target.EmptyParalysisDone(1,(SkillFrom == 2)? 16.0f : 7.5f,1);
                }
                WaveBreak();
            }
        }
    }
}
