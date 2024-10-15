using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistyExplosionMist : Skill
{

    public bool isBoom;
    GameObject MistOb01;
    GameObject MistOb02;
    GameObject MistOb03;
    GameObject ExplosionOb;
    CircleCollider2D CircleCollider;

    // Start is called before the first frame update
    void Start()
    {
        CircleCollider = GetComponent<CircleCollider2D>();
        MistOb01 = transform.GetChild(0).gameObject;
        MistOb02 = transform.GetChild(1).gameObject;
        MistOb03 = transform.GetChild(2).gameObject;
        ExplosionOb = transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();

        if (ExistenceTime < 2.0f && !isBoom)
        {
            Boom();
        }
        if (isBoom)
        {
            if (ExistenceTime < 2.0f && ExistenceTime >= 1.7f)
            {
                CircleCollider.enabled = false;
            }
            else if(ExistenceTime < 1.7f && ExistenceTime >= 0.5f)
            {
                CircleCollider.enabled = true;
                CircleCollider.radius = Mathf.Clamp(CircleCollider.radius + Time.deltaTime * 2, 0.0f, 2f);
            }
            else if (ExistenceTime < 0.5f && ExistenceTime >= 0.0f)
            {
                CircleCollider.enabled = true;
                CircleCollider.radius = Mathf.Clamp(CircleCollider.radius - Time.deltaTime * 4, 0.0f, 2f);
            }
        }
    }

    public void Boom()
    {
        ExistenceTime = 2.0f;
        CircleCollider.radius = 0;
        isBoom = true;
        Destroy(MistOb01.gameObject , 0.5f);
        Destroy(MistOb02.gameObject , 0.5f);
        Destroy(MistOb03.gameObject , 0.5f);
        ExplosionOb.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            if (isBoom)
            {
                GameObject p = null;
                if (player != null) { p = player.gameObject; }
                if (other.tag == "Empty") {
                    HitAndKo(other.GetComponent<Empty>()); 
                    if (SkillFrom == 2 && player != null) {
                        if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.5f)
                        {
                            player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅黄色回血型);
                        }
                        else
                        {
                            player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型);
                        }
                    } 
                }
                else if (other.tag == "Player") {
                    Pokemon.PokemonHpChange(p, other.gameObject, 0, SpDamage, 0, PokemonType.TypeEnum.Fairy);
                    other.GetComponent<PlayerControler>().KnockOutPoint = KOPoint;
                    other.GetComponent<PlayerControler>().KnockOutDirection = (other.GetComponent<PlayerControler>().transform.position - transform.position).normalized;
                    if (SkillFrom == 2 && player != null)
                    {
                        if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.5f)
                        {
                            player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅黄色回血型);
                        }
                        else
                        {
                            player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型);
                        }
                    }
                }
            }
            else
            {
                if (other.tag == "Empty")
                {
                    Boom();
                }
            }
        }
        if (other.GetComponent<MistyExplosionMist>() != null)
        {
            other.GetComponent<MistyExplosionMist>().Boom();
        }
    }
}
