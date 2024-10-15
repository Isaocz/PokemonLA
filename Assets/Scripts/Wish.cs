using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish : Skill
{
    private float rotationSpeed;
    private float timer;
    bool isMove;
    Vector3 StartPosition;

    void Start()
    {
        Invoke("Heal", 5f);
        Invoke("DestroyStar", 2.5f);
        StartPosition = player.transform.position;
        transform.GetChild(5).parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        StartExistenceTimer();
        if (timer < 2f)
        {//ÎåÃëºóÍ£Ö¹Ðý×ª
            rotationSpeed = 80f + Mathf.Pow(Mathf.Exp(1.5f) * timer, 2);
            transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            JudgeisMove();
        }
    }

    //ÅÐ¶ÏÍæ¼ÒÊÇ·ñ´ó·ùÒÆ¶¯
    void JudgeisMove()
    {
        if ((StartPosition - player.transform.position).magnitude >= ((SkillFrom==2 )?1.5f: 1.0f)) { isMove = true; }
    }

    //Ïú»ÙÐÇÐÇ
    void DestroyStar()
    {
        for (int i = 0; i < 4; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void Heal()
    {

        if (!isMove) {

            if (SkillFrom != 2)
            {
                Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, player.maxHp / 4, PokemonType.TypeEnum.IgnoreType);
            }
            else
            {
                Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, player.maxHp * 2 / 5, PokemonType.TypeEnum.IgnoreType);
            }
            transform.Rotate(0, 0, 180f);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
