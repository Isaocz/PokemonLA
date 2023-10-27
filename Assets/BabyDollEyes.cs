using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDollEyes : Skill
{
    public GameObject bdeEffect;
    public Vector3 Excursion;
    List<Empty> enemies = new List<Empty>();
    GameObject bdeblink;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 SpawnPosition = transform.position + Excursion;
        bdeblink = Instantiate(bdeEffect, SpawnPosition, Quaternion.identity);
        Destroy(bdeblink, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        bdeblink.transform.position = transform.position + Excursion;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy && !enemies.Contains(enemy))
            {
                enemy.LevelChange(-1, "Atk");
                enemy.AtkDown(2f);
                enemies.Add(enemy);
            }
        }
    }
}
