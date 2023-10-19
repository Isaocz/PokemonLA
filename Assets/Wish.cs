using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish : Skill
{
    private float rotationSpeed;
    private float timer;
    void Start()
    {
        Invoke("Heal", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        StartExistenceTimer();
        if (timer < 5f)
        {//ÎåÃëºóÍ£Ö¹Ðý×ª
            rotationSpeed = 80f + Mathf.Pow(Mathf.Exp(1.5f) * timer, 2);
            transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    void Heal()
    {
        if (SkillFrom != 2)
        {
            player.ChangeHp(player.maxHp / 4, 0, 0);
        }
        else
        {
            player.ChangeHp(player.maxHp * 2 / 5, 0, 0);
        }
        transform.Rotate(0, 0, 180f);
        for (int i = 0; i < 4; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
