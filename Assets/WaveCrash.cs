using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCrash : Skill
{
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    public GameObject TackleBlast;
    bool isMoveOver;


    Vector2 LastNowPosition1;
    Vector2 LastNowPosition2;

    float HHPSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        LastNowPosition2 = PlayerRigibody.position;
        player.isInvincibleAlways = true;
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
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Room" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Water" || other.tag == "Projectel")
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
                    Instantiate(TackleBlast, target.transform.position, Quaternion.identity).GetComponent<DestoryState>().RemoveChild();
                    int BeforeHP = target.EmptyHp;
                    HitAndKo(target);
                    int DmageHP = Mathf.Clamp( target.EmptyHp - BeforeHP ,  0 , 10000);
                    float Alpha = 3.0f;
                    if (SkillFrom == 2 && target.isSpeedChange)
                    {
                        Alpha = 10.0f;
                    }
                    Pokemon.PokemonHpChange(null , player.gameObject , Damage / Alpha, 0 , 0 , Type.TypeEnum.IgnoreType);

                }
            }
        }
    }


    private void OnDestroy()
    {
        player.isInvincibleAlways = false;
    }
}
