using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzorExtrasensory : Projectile
{
    // Start is called before the first frame update
    bool isDestory;
    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 3.0f, () => { if (!isDestory) { isDestory = true; } });
    }




    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        DestoryByRange(20);
        if (isDestory)
        {
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            isDestory = true;
            Destroy(rigidbody2D);

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);

            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, Type.TypeEnum.Psychic);
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
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, 0, SpDmage, 0, Type.TypeEnum.Psychic);
                //e.EmptyHpChange(0, (SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / (250 * e.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((e.EmptyType01 == Type.TypeEnum.Ice || e.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2), 14);
            }
        }
    }
}
