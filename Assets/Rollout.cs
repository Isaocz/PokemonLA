using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rollout : Skill
{
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    bool MoveStop;

    Vector2 PlayerLookAt;
    Vector2 d;

    BoxCollider2D GryoBallBoxCollider;


    public int RolloutCount;

    public SubRollout sub2;
    public SubRollout sub4;
    public SubRollout sub8;
    public SubRollout sub16;

    float MoveSpeed = 2.5f;

    public GameObject SR;
    
    

    // Start is called before the first frame update
    void Start()
    {

        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        animator = transform.GetComponent<Animator>();
        player.isCanNotMove = true;
        PlayerLookAt = player.look;

        animator.SetFloat("LookX", PlayerLookAt.x);
        animator.SetFloat("LookY", PlayerLookAt.y);

        transform.rotation = Quaternion.Euler(0, 0, 0);

        GryoBallBoxCollider = transform.GetComponent<BoxCollider2D>();
        BoxCollider2D PlayerBoxCollider = player.transform.GetComponent<BoxCollider2D>();
        var s = GryoBallBoxCollider.size;
        s.x = PlayerBoxCollider.size.x + 0.15f;
        s.y = PlayerBoxCollider.size.y + 0.15f;
        GryoBallBoxCollider.offset = PlayerBoxCollider.offset;
        GryoBallBoxCollider.edgeRadius = PlayerBoxCollider.edgeRadius;

        Invoke("DecideDmageAndSpeed" , 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();

        if (ExistenceTime > 1.8f)
        {
            player.transform.GetChild(3).localScale -= new Vector3(5.0f * Time.deltaTime, 5.0f * Time.deltaTime, 0);
        }

        if (ExistenceTime <= 0.5f && MoveStop == false) { MoveStopF(); }
        if (!MoveStop)
        {
            Vector2 postion = PlayerRigibody.position;
            postion.x += Direction.x * MoveSpeed * player.speed * Time.deltaTime;
            postion.y += Direction.y * MoveSpeed * player.speed * Time.deltaTime;
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
                    AddSubRollout();
                    if (SkillFrom == 2) {
                        Instantiate(SR, transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }


    void AddSubRollout()
    {

        switch (RolloutCount)
        {
            case 0:
                player.AddASubSkill(sub2);
                break;
            case 1:
                player.AddASubSkill(sub4);
                break;
            case 2:
                player.AddASubSkill(sub8);
                break;
            case 3:
                player.AddASubSkill(sub16);
                break;
        }
    }

    void DecideDmageAndSpeed()
    {
        switch (RolloutCount)
        {
            case 0:
                MoveSpeed = 2.5f;
                transform.GetChild(0).GetChild(4).localScale = new Vector3(3, 3, 1);
                break;
            case 1:
                MoveSpeed = 2.0f;
                Damage *= 2;
                transform.GetChild(0).GetChild(4).localScale = new Vector3(5, 5, 1);
                break;
            case 2:
                MoveSpeed = 1.5f;
                Damage *= 4;
                transform.GetChild(0).GetChild(4).localScale = new Vector3(7, 7, 1);
                break;
            case 3:
                MoveSpeed = 1.1f;
                Damage *= 8;
                transform.GetChild(0).GetChild(4).localScale = new Vector3(9, 9, 1);
                break;
            case 4:
                MoveSpeed = 0.8f;
                Damage *= 16;
                transform.GetChild(0).GetChild(4).localScale = new Vector3(11, 11, 1);
                break;
        }
    }


    public void MoveStopF()
    {
        player.transform.GetChild(3).localScale = player.PlayerLocalScal;
        player.isCanNotMove = false;
        transform.GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Over");
        MoveStop = true;
        ExistenceTime = 1.0f;
    }


}
