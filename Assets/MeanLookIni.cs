using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookIni : Skill
{
    public GameObject meanlook;
    public GameObject meanlookAnimation;
    List<Empty> enemies = new List<Empty>();
    bool isMeanLookOver;

    int PlayerLastHP;

    private void Start()
    {
        PlayerLastHP = player.Hp;
    }

    void Update()
    {
        StartExistenceTimer();

        if (!isMeanLookOver && ExistenceTime <= 4.15f)
        {
            isMeanLookOver = true;
            transform.GetComponent<Collider2D>().enabled = false;
            if (transform.childCount > 0) { transform.GetChild(0).gameObject.SetActive(false); }
        }
    }

    private void FixedUpdate()
    {
        if (SkillFrom == 2 && enemies.Count != 0)
        {
            int PlayerDmage = player.Hp - PlayerLastHP;
            if (PlayerDmage < 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    Pokemon.PokemonHpChange(null , enemies[i].gameObject , -PlayerDmage , 0 , 0 , PokemonType.TypeEnum.IgnoreType);
                }
            }
            PlayerLastHP = player.Hp;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy && !enemy.isBoos && !enemies.Contains(enemy))
            {
                if (!collision.GetComponent<SubEmptyBody>())
                {
                    enemies.Add(enemy);
                    GameObject meanLook = Instantiate(meanlook, enemy.transform.position, Quaternion.identity);
                    meanLook.GetComponent<MeanLook>().target = enemy;
                    GameObject meanlookani = Instantiate(meanlookAnimation, enemy.gameObject.transform.GetChild(2).position + new Vector3(0, 0.5f), Quaternion.identity);
                    meanlookani.GetComponent<MeanLookEyes>().target = enemy;
                }
                else
                {
                    enemy = enemy.GetComponent<SubEmptyBody>().ParentEmpty;
                    enemies.Add(enemy);
                    GameObject meanLook = Instantiate(meanlook, enemy.transform.position, Quaternion.identity);
                    meanLook.GetComponent<MeanLook>().target = enemy;
                    GameObject meanlookani = Instantiate(meanlookAnimation, enemy.gameObject.transform.GetChild(2).position + new Vector3(0, 0.5f), Quaternion.identity);
                    meanlookani.GetComponent<MeanLookEyes>().target = enemy;
                }
            }
        }
    }
}
