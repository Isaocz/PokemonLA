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
            case 0: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.ǳ��ɫ��ͨ��); break;
            case 1: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.ǳ��ɫ��ͨ��); break;
            case 2: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.ǳ��ɫ��ͨ��); break;
            case 3: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.��ɫ���ӹ�����); break;
            case 4: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.ǳ��ɫ��Ѫ��); break;
            case 5: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.��ɫ���ٹ�����); break;
            case 6: player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.��ɫ�����ع���); break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy && !enemies.Contains(enemy))
            {
                //֮ǰ
                //enemy.LevelChange(-1, "Atk");
                //enemy.AtkDown(2f);
                //����
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
