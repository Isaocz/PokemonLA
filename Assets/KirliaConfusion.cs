using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirliaConfusion : Projectile
{

    //极坐标转换用的角度和长度
    public float R;
    public float RSpeed;
    float L;
    //原点
    public Vector2 StartPosition;

    bool isDestory;


    private void Awake()
    {
        AwakeProjectile();
        
    }

    private void Start()
    {
        if (RSpeed == 0) { RSpeed = 210; }
        L = 0;
    }



    private void Update()
    {
        DestoryByRange(7.5f);
        if (spriteRenderer.color.a <= 0.5f)
        {
            Debug.Log("xxx");
        }
        if (isDestory)
        {
            CollisionDestory();
        }
        else
        {
            L += Time.deltaTime * 1.7f;
            R += Time.deltaTime * RSpeed;
            transform.position = new Vector3(Mathf.Cos(R * Mathf.Deg2Rad) * L + StartPosition.x, Mathf.Sin(R * Mathf.Deg2Rad) * L + StartPosition.y, 0);
            //transform.rotation = Quaternion.AngleAxis(R, Vector3.forward) * Quaternion.Euler(0,0,0); 
            transform.rotation = Quaternion.Euler(0, 0, R + 90);
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
