using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquidation : Skill
{
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    public GameObject TackleBlast;
    bool isMoveOver;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        player.isCanNotMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //StartExistenceTimer();
        if (!isMoveOver)
        {
            Vector2 postion = PlayerRigibody.position;
            postion.x += Direction.x * 2.5f * player.speed * Time.deltaTime;
            postion.y += Direction.y * 2.5f * player.speed * Time.deltaTime;
            PlayerRigibody.position = postion;
        }
        else
        {
            StartExistenceTimer();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Room" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Water")
        {
            isMoveOver = true;
            ExistenceTime = 0.01f;
            GetComponent<Animator>().SetTrigger("Over");
            transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            transform.GetChild(2).GetComponent<ParticleSystem>().Stop();

            if (other.gameObject.tag == "Empty")
            {
                Empty target = other.gameObject.GetComponent<Empty>();
                if (target != null) {
                    if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30.0f) >= 0.8f)
                    {
                        target.DefChange(-1,0);
                    }
                    if (SkillFrom == 2 && target.isSpeedChange) { Damage *= 1.5f; }
                    Instantiate(TackleBlast, target.transform.position, Quaternion.identity).GetComponent<DestoryState>().RemoveChild();
                    HitAndKo(target);
                }
            }
        }
    }


    private void OnDestroy()
    {
        player.isCanNotMove = false;
    }
}
