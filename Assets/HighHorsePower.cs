using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighHorsePower : Skill
{
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    public GameObject HighHorsePowerEffect;
    public SoftMud softMud;

    bool isMoveOver;
    Vector2 LastNowPosition1;
    Vector2 LastNowPosition2;

    float HHPSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = (transform.rotation * Vector2.right).normalized;
        LastNowPosition2 = PlayerRigibody.position;
    }

    // Update is called once per frame
    void Update()
    {


        //StartExistenceTimer();
        if (!isMoveOver)
        {
            HHPSpeed = Mathf.Clamp(HHPSpeed + Time.deltaTime * 0.5f, 0, 2.5f);
            LastNowPosition1 = PlayerRigibody.position;
            if (!Mathf.Approximately(LastNowPosition1.x, LastNowPosition2.x) || !Mathf.Approximately(LastNowPosition1.y, LastNowPosition2.y))
            {
                Direction = (LastNowPosition1 - LastNowPosition2).normalized;
                //transform.rotation = Quaternion.LookRotation(Direction);
                //transform.RotateAround(transform.parent.position, Vector3.forward, );
                int Angle1 = (int)transform.rotation.eulerAngles.z;
                int Angle2 = (int)_mTool.Angle_360(Direction, Vector2.up);
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Angle2 + 90));
                transform.localPosition = new Vector3(0 + Direction.x, 0.4f + Direction.y, 0);
            }
            LastNowPosition2 = PlayerRigibody.position;

            Vector2 postion = PlayerRigibody.position;
            postion.x += Direction.x * HHPSpeed * player.speed * Time.deltaTime;
            postion.y += Direction.y * HHPSpeed * player.speed * Time.deltaTime;
            PlayerRigibody.position = postion;
        }
        else
        {
            StartExistenceTimer();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            Instantiate(HighHorsePowerEffect , other.transform.position , Quaternion.identity);
            HitAndKo(target);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Room" || other.gameObject.tag == "Enviroment")
        {
            player.isInvincibleAlways = true;
            isMoveOver = true;
            GetComponent<Animator>().SetTrigger("Over");
            transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
            if(SkillFrom == 2 && (other.gameObject.tag == "Room" || other.gameObject.tag == "Enviroment"))
            {
                Instantiate(softMud, player.transform.position, Quaternion.identity);
            }
            if (other.gameObject.tag == "Empty")
            {
                Empty target = other.gameObject.GetComponent<Empty>();
                Instantiate(HighHorsePowerEffect, other.transform.position, Quaternion.identity);
                HitAndKo(target);
            }
        }
    }

    private void OnDestroy()
    {

        player.isInvincibleAlways = false;

    }
}
