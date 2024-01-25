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
        animator = GetComponent<Animator>();
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
        if (!ishit && ExistenceTime > 0.3f)
        {
            moveSpeed = initialSpeed * ExistenceTime / alltime;
            transform.Translate(moveSpeed * Vector3.right * Time.deltaTime);
        }
        else
        {
            if (!timeChange)
            {
                animator.SetTrigger("Over");
                timeChange = true;
            }
            if (canBFborn)
            {
                if (SkillFrom == 2)
                {
                    if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 < 0.8f) { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型); }
                    else { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.蓝色增加特攻型); }
                    if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.8f)
                    {
                        if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 < 0.8f) { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型); }
                        else { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.蓝色增加特攻型); }
                    }
                }
                canBFborn = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger) {
            if (collision.CompareTag("Empty"))
            {
                Empty enemy = collision.GetComponent<Empty>();
                if (enemy != null)
                {
                    HitAndKo(enemy);
                    if (Random.Range(0f, 1f) + player.LuckPoint / 30f > 0.8f)
                    {
                        enemy.SpDChange(-1, 0f);
                    }
                    ishit = true;
                    timeChange = false;
                    if (SkillFrom == 2)
                    {
                        canBFborn = true;
                    }
                }
            }
            if (collision.CompareTag("Enviroment") || collision.CompareTag("Room"))
            {
                ishit = true;
                timeChange = false;
            }
        }
    }
}
