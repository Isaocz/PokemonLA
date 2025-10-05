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
            if (ExistenceTime <= 0.15f && !isMoveOver)
            {
                isMoveOver = true;
                GetComponent<Animator>().SetTrigger("Over");
                var e1 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
                e1.loop = false;
                var e2 = transform.GetChild(2).GetComponent<ParticleSystem>().main;
                e2.loop = false;
                GameObject g1 = transform.GetChild(1).gameObject; g1.transform.parent = null; g1.transform.localScale = new Vector3(1, 1, 1);
                GameObject g2 = transform.GetChild(1).gameObject; g2.transform.parent = null; g2.transform.localScale = new Vector3(1, 1, 1);

            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Room" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Water")
        {
            if (!isMoveOver)
            {
                isMoveOver = true;
                ExistenceTime = 0.1f;
                GetComponent<Animator>().SetTrigger("Over");
                var e1 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
                e1.loop = false;
                var e2 = transform.GetChild(2).GetComponent<ParticleSystem>().main;
                e2.loop = false;
                GameObject g1 = transform.GetChild(1).gameObject; g1.transform.parent = null; g1.transform.localScale = new Vector3(1, 1, 1);
                GameObject g2 = transform.GetChild(1).gameObject; g2.transform.parent = null; g2.transform.localScale = new Vector3(1, 1, 1);
            }
            if (other.gameObject.tag == "Empty")
            {
                Empty target = other.gameObject.GetComponent<Empty>();
                if (target != null)
                {
                    if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30.0f) >= 0.8f)
                    {
                        target.DefChange(-1, 0);
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
