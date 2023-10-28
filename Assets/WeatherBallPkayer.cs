using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherBallPkayer : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    public GameObject Rock;


    Color[] WeatherBallTypeColor = {
        new Color (0.9882353f , 0.9921569f , 0.8745098f , 1f ),  //无天气时的球颜色
        new Color (0.9843137f , 1 , 0.8823529f , 0.6431373f ),  //无天气时的粒子颜色
        new Color (0.9803922f , 0.9803922f , 0.9019608f , 0.8f),  //无天气时的光颜色
        new Color (0.9641324f , 1.0f , 0.80660381f ,1 ),  //无天气时的光圈颜色
        

        new Color (0.9150943f , 0.5866283f , 0.3323691f , 1),  //晴天时的球颜色
        new Color (0.9433962f , 0.781642f ,  0.5206479f , 0.6431373f ),  //晴天气时的粒子颜色
        new Color (1 , 0.7450981f , 0.3803922f, 0.8f ),
        new Color (0.9433962f , 0.781642f , 0.5206479f , 0.6431373f),  //晴天时的光圈颜色

        new Color (0.4392157f , 0.7145f , 0.8392157f , 1),  //雨天时的球颜色
        new Color (0.7971698f , 0.9799178f , 1 , 0.6431373f),  //雨天时的粒子颜色
        new Color (0.3803922f , 0.8199072f , 1 , 0.8f),  //雨天时的光颜色
        new Color (0.5348434f , 0.7941553f , 0.9528302f , 1),  //雨天时的光圈颜色

        new Color (0.759434f , 1 , 0.939707f , 1 ),  //冰雹时的球颜色
        new Color (0.7783019f , 1 , 0.9937443f , 0.6431373f),  //冰雹时的粒子颜色
        new Color (0.740566f , 0.9084281f , 1 , 0.8f),  //冰雹时的光颜色
        new Color (0.759434f , 0.9100688f ,1 ,1),  //冰雹时的光圈颜色

        new Color (0.4150943f , 0.4031968f , 0.252581f , 1),  //沙暴时的球颜色
        new Color (0.6142756f , 0.6603774f , 0.3146138f , 0.6431373f),  //沙暴时的粒子颜色
        new Color (0.6431373f , 0.6509434f , 0.4206568f , 0.8f),  //沙暴时的光颜色
        new Color (0.5943396f , 0.555011f , 0.3952919f , 1),  //沙暴时的光圈颜色
    };


    // Start is called before the first frame update
    void Start()
    {
        switch (SkillType)
        {
            case 1://一般
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[0];
                var PSColor01 = transform.GetChild(2).GetComponent<ParticleSystem>().main;
                PSColor01.startColor = WeatherBallTypeColor[1];
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[2];
                transform.GetChild(3).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[3];
                transform.GetChild(4).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(5).gameObject.SetActive(false);
                transform.GetChild(6).gameObject.SetActive(false);
                transform.GetChild(7).gameObject.SetActive(false);
                transform.GetChild(8).gameObject.SetActive(false);
                break;
            case 6://岩石
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[16];
                var PSColor02 = transform.GetChild(2).GetComponent<ParticleSystem>().main;
                PSColor02.startColor = WeatherBallTypeColor[17];
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[18];
                transform.GetChild(3).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[19];
                transform.GetChild(8).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(5).gameObject.SetActive(false);
                transform.GetChild(6).gameObject.SetActive(false);
                transform.GetChild(7).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(false);
                break;
            case 10://火
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[4];
                var PSColor03 = transform.GetChild(2).GetComponent<ParticleSystem>().main;
                PSColor03.startColor = WeatherBallTypeColor[5];
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[6];
                transform.GetChild(3).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[7];
                transform.GetChild(5).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(4).gameObject.SetActive(false);
                transform.GetChild(6).gameObject.SetActive(false);
                transform.GetChild(7).gameObject.SetActive(false);
                transform.GetChild(8).gameObject.SetActive(false);
                break;
            case 11://水
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[8];
                var PSColor04 = transform.GetChild(2).GetComponent<ParticleSystem>().main;
                PSColor04.startColor = WeatherBallTypeColor[9];
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[10];
                transform.GetChild(3).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[11];
                transform.GetChild(6).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(5).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(false);
                transform.GetChild(7).gameObject.SetActive(false);
                transform.GetChild(8).gameObject.SetActive(false);
                break;
            case 15://冰
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[12];
                var PSColor05 = transform.GetChild(2).GetComponent<ParticleSystem>().main;
                PSColor05.startColor = WeatherBallTypeColor[13];
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[14];
                transform.GetChild(3).GetComponent<SpriteRenderer>().color = WeatherBallTypeColor[15];
                transform.GetChild(7).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(5).gameObject.SetActive(false);
                transform.GetChild(6).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(false);
                transform.GetChild(8).gameObject.SetActive(false);
                break;
        }

        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
            postion.x += direction.x * 8f * Time.deltaTime;
            postion.y += direction.y * 8f * Time.deltaTime;
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
            isCanNotMove = true;
            PSDie();
            animator.SetTrigger("Blast");
            if (SkillFrom == 2 && SkillType == 6)
            {
                Instantiate(Rock, transform.position, Quaternion.identity);
            }
        }
    }


    void PSDie()
    {
        var PSSizeDie = transform.GetChild(2).GetComponent<ParticleSystem>().sizeOverLifetime;
        PSSizeDie.enabled = true;
        var PSColorDie = transform.GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime;
        PSColorDie.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                if (target != null)
                {
                    if (SkillFrom == 2) {
                        switch (SkillType)
                        {
                            case 1:
                                break;
                            case 10:
                                target.EmptyBurnDone(1 , 15 , 0.2f + ((float)player.LuckPoint / 30));
                                break;
                            case 11:
                                target.SpeedChange();
                                target.SpeedRemove01(12f * target.OtherStateResistance);
                                break;
                            case 15:
                                target.Frozen(7.5f, 1, 0.2f + ((float)player.LuckPoint / 30));
                                break;
                        }
                    }
                    HitAndKo(target);
                }
                BallBreak();
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                BallBreak();
            }
        }
    }
}
