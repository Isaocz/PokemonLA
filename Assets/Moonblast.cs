using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonblast : Skill
{
    public float initialSpeed;
    public float maskDuration;
    public GameObject mask;
    private float moveSpeed;
    private float alltime;
    private bool ishit;
    private bool timeChange;
    private bool canBFborn;
    void Start()
    {
        GameObject Mask = Instantiate(mask, player.transform);
        Destroy(Mask, maskDuration);
        alltime = ExistenceTime;
        timeChange = false;
        ishit = false;
        canBFborn = false;
    }

    void Update()
    {
        StartExistenceTimer();
        if (!ishit)
        {
            moveSpeed = initialSpeed * ExistenceTime / alltime;
            transform.Translate(moveSpeed * Vector3.right * Time.deltaTime);
        }
        else
        {
            if (!timeChange)
            {
                Destroy(gameObject, 0.3f);
                timeChange = true;
            }
            if (canBFborn)
            {
                switch (Random.Range(0, 2))
                {
                    case 0: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型); break;
                    case 1: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.蓝色增加特攻型); break;
                }
                canBFborn = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy != null)
            {
                HitAndKo(enemy);
                if(Random.Range(0f,1f) + player.LuckPoint/ 30f > 0.8f)
                {
                    enemy.SpDChange(-1, 3f);
                }
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                ishit = true;
                timeChange = false;
                if(SkillFrom == 2)
                {
                    canBFborn = true;
                }
            }
        }
        if(collision.CompareTag("Enviroment") || collision.CompareTag("Room"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            ishit = true;
            timeChange = false;
        }
    }
}
