using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBlade : GrassSkill
{

    //获取起始特效
    GameObject StartVFX;
    GameObject OverVFX;
    GameObject EndureVFX;



    bool isEndureOver;
    bool isDamageDown;

    int CTPlus;





    // Start is called before the first frame update
    void Start()
    {

        StartVFX = transform.GetChild(1).gameObject;
        OverVFX = transform.GetChild(3).gameObject;

        OverVFX.transform.parent = transform.parent;
        OverVFX.transform.localPosition = Vector3.zero;
        OverVFX.transform.rotation = Quaternion.Euler(0, 0, 0);

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        GetComponent<Animator>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 120);
        transform.position = player.transform.position + (Quaternion.AngleAxis(120, Vector3.forward) * (transform.position - player.transform.position));



        if (Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus)
        {
            ExistenceTime = 1.05f;
            Destroy(OverVFX.gameObject);
        }

        if (Weather.GlobalWeather.isRain || Weather.GlobalWeather.isRainPlus || Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus || Weather.GlobalWeather.isSandstorm || Weather.GlobalWeather.isSandstormPlus)
        {
            if (!isDamageDown)
            {
                isDamageDown = true;
                Damage /= 2;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();

        if (SkillFrom == 2 && ExistenceTime > 1f)
        {
            for (int i = 0; i < player.InGressCount.Count; i++)
            {
                NormalGress n = player.InGressCount[i].GetComponent<NormalGress>();
                GressPlayerINOUT g = player.InGressCount[i].GetComponent<GressPlayerINOUT>();
                if (player.InGressCount[i].gameObject.tag == "Grass")
                {
                    if (n != null && !n.isDie) { 
                        n.GrassDie();
                        Damage += 15;
                        if (CTPlus < 3)
                        {
                            CTPlus++;
                            CTLevel++;
                        }
                    }
                    if (g != null && !g.isDie) { 
                        g.GrassDie();
                        Damage += 15;
                        if (CTPlus < 3)
                        {
                            CTPlus++;
                            CTLevel++;
                        }
                    }
                }
            }
        }


        if (ExistenceTime < 1f && ExistenceTime >= 0.5f)
        {
            if (!isEndureOver)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                GetComponent<Animator>().enabled = true;
                GetComponent<Collider2D>().enabled = true;
                isEndureOver = true;
            }
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - (Time.deltaTime/0.5f)*240);
            transform.position = player.transform.position + (Quaternion.AngleAxis((-(Time.deltaTime / 0.5f) * 220), Vector3.forward) * (transform.position - player.transform.position));
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                if (Weather.GlobalWeather.isRain || Weather.GlobalWeather.isRainPlus || Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus || Weather.GlobalWeather.isSandstorm || Weather.GlobalWeather.isSandstormPlus)
                {
                    if (!isDamageDown)
                    {
                        isDamageDown = true;
                        Damage /= 2;
                    }
                }
                HitAndKo(target);
            }
        }
    }


}
