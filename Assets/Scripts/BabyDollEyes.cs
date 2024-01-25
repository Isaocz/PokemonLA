using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDollEyes : Skill
{
    public GameObject bdeEffect;
    public Vector3 Excursion;
    List<Empty> enemies = new List<Empty>();
    GameObject bdeblink;
    bool isBFBorn;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 SpawnPosition = transform.position + Excursion;
        bdeblink = Instantiate(bdeEffect, SpawnPosition, Quaternion.identity,transform);
        bdeblink.transform.position = transform.position + Excursion;
        Destroy(bdeblink, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    void BF()
    {
        switch (Random.Range(0, 7))
        {
            case 0: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型); break;
            case 1: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型); break;
            case 2: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型); break;
            case 3: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.橙色增加攻击型); break;
            case 4: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅黄色回血型); break;
            case 5: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.红色慢速攻击型); break;
            case 6: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.蓝色增加特攻型); break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy && !enemies.Contains(enemy))
            {
                //之前
                //enemy.LevelChange(-1, "Atk");
                //enemy.AtkDown(2f);
                //现在
                enemy.AtkChange(-1,2.0f);
                enemies.Add(enemy);
                if (SkillFrom == 2 && !isBFBorn)
                {
                    isBFBorn = false;
                    BF();
                }
            }
        }
    }
}
