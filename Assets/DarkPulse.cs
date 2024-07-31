using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPulse : Skill
{

    int BeforeHP;

    private void Start()
    {
        BeforeHP = player.Hp;
    }

    private void OnDestroy()
    {
        if (SkillFrom == 2) {
            int DmageHP = player.Hp - BeforeHP;
            if (DmageHP < 0)
            {
                Pokemon.PokemonHpChange(null , player.gameObject , 0 , 0 , -DmageHP , Type.TypeEnum.IgnoreType);
            }
        }
    }

    private void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Empty") {
            Empty e = collision.GetComponent<Empty>();
            if ( e != null)
            {
                HitAndKo(e);
                if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 20.0f) > 0.8f)
                {
                    e.Fear(4.0f, 1);
                }
            }
        }
    }

    /*
    public float diffuseRadius;
    public int particalNumber;
    public float duration;
    public GameObject darkPulsePartical;
    private float initialExistenceTime;
    private bool isRelease;

    void Start()
    {
        initialExistenceTime = ExistenceTime;
        isRelease = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime < (initialExistenceTime - 0.75f) && !isRelease)
        {
            isRelease = true;
            for (int i = 0; i < particalNumber; i++)
            {
                GameObject dpp = Instantiate(darkPulsePartical, this.transform);
                dpp.GetComponent<DarkPulsePar>().Initialize(diffuseRadius, duration, SpDamage);
            }
        }
        else if (ExistenceTime > (initialExistenceTime - 0.75f))
        {
            transform.position = player.transform.position;
        }
    }
    */
}
