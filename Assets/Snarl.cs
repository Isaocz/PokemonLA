using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snarl : Skill
{
    public GameObject snarleffect;

    float SpAdowntime = 4.0f;

    private void Start()
    {
        if (SkillFrom == 2)
        {
            Pokemon.PokemonHpChange(null, player.gameObject, player.maxHp / 16.0f, 0.0f, 0, Type.TypeEnum.IgnoreType);
            player.KnockOutDirection = Vector2.zero;
            player.KnockOutPoint = 0;

            var PSRadius = transform.GetChild(2).GetComponent<ParticleSystem>().shape.radius;
            var PSEmision = transform.GetChild(2).GetComponent<ParticleSystem>().emission.rateOverTimeMultiplier;


            float HPPer = (float)player.Hp / (float)player.maxHp;
            if ( HPPer >= 0.0f && HPPer < 0.1f )
            {
                transform.localScale = new Vector3(2.4f, 2.4f, 1.0f);
                PSRadius = 2.4f;
                PSEmision = 34;
                SpAdowntime = 12.0f;
            }
            else if(HPPer >= 0.1f && HPPer < 0.3f)
            {
                transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
                PSRadius = 2.0f;
                PSEmision = 30;
                SpAdowntime = 10.0f;
            }
            else if (HPPer >= 0.3f && HPPer < 0.65f)
            {
                transform.localScale = new Vector3(1.6f, 1.6f, 1.0f);
                PSRadius = 1.6f;
                PSEmision = 26;
                SpAdowntime = 8.0f;
            }
            else if (HPPer >= 0.65f && HPPer < 0.85f)
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1.0f);
                PSRadius = 1.3f;
                PSEmision = 21;
                SpAdowntime = 6.0f;
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                PSRadius = 1.0f;
                PSEmision = 17;
                SpAdowntime = 4.0f;
            }
        }
    }


    private void Update()
    {
        StartExistenceTimer();


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                target.SpAChange(-1, SpAdowntime);
                GameObject se = Instantiate(snarleffect, target.transform.position, Quaternion.identity);
                Destroy(se, 0.5f);
            }
        }
    }
}
