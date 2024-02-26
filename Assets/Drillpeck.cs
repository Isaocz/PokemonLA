using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drillpeck : Skill
{
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    bool MoveStop;

    Vector2 PlayerLookAt;
    Vector2 d;


    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        animator = transform.GetComponent<Animator>();
        player.isCanNotMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();


        if (ExistenceTime <= 0.5f && MoveStop == false) { MoveStopF(); }
        if (!MoveStop)
        {
            Vector2 postion = PlayerRigibody.position;
            postion.x += Direction.x * 2.5f * player.speed * Time.deltaTime;
            postion.y += Direction.y * 2.5f * player.speed * Time.deltaTime;
            PlayerRigibody.position = postion;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Room" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Water")
        {
            MoveStopF();
            if (other.gameObject.tag == "Empty")
            {
                Empty target = other.gameObject.GetComponent<Empty>();
                if (target != null)
                {
                    HitAndKo(target);
                }
            }
        }
    }



    public void MoveStopF()
    {
        player.isCanNotMove = false;
        transform.GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Blast");
        MoveStop = true;
        ExistenceTime = 0.5f;
    }

}
