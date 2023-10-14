using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaltsConfusion : Projectile
{
    // Start is called before the first frame update
    bool isDestory;
    float ExitTime;
    SpriteRenderer CircleS;


    private void Awake()
    {
        AwakeProjectile();
        ExitTime = 3.5f;
    }

    private void Start()
    {
        CircleS = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (ExitTime >= 0)
        {
            ExitTime -= Time.deltaTime;
            if (ExitTime > 3.0f) { CircleS.color = CircleS.color + new Color(0, 0, 0, 2f * Time.deltaTime);}
        }
        else
        {
            if ((transform.position - BornPosition).magnitude >= 3)
            {
                if (spriteRenderer.material.color.a >= 0)
                {
                    spriteRenderer.material.color = spriteRenderer.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
                    CircleS.color = CircleS.color - new Color(0, 0, 0, 3f * Time.deltaTime);
                }
                if (spriteRenderer.material.color.a <= 0.1f)
                {
                    Destroy(gameObject);
                }

            }
        }
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (isDestory)
        {
            CollisionDestory();
            CircleS.color = CircleS.color - new Color(0, 0, 0, 3f * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")) || other.tag == ("Enviroment"))
        {
            isDestory = true;
            Destroy(rigidbody2D);

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);

            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                if (empty != null) { Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, Type.TypeEnum.Psychic); }
                else { Pokemon.PokemonHpChange(null, other.gameObject, 0, SpDmage, 0, Type.TypeEnum.Psychic); }
                if (playerControler != null)
                {
                    //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / 250), 14);
                    playerControler.KnockOutPoint = 5;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                if (empty != null) { Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, 0, SpDmage, 0, Type.TypeEnum.Psychic); }
                else { Pokemon.PokemonHpChange(null, e.gameObject, 0, SpDmage, 0, Type.TypeEnum.Psychic); }
                //e.EmptyHpChange(0, (SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / (250 * e.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((e.EmptyType01 == Type.TypeEnum.Ice || e.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2), 14);
            }
        }
    }
}
