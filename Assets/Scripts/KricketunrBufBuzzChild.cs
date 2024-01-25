using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KricketunrBufBuzzChild : Projectile
{
    // Start is called before the first frame update
    bool isDestory;

    SpriteRenderer s1;
    private void Awake()
    {
        AwakeProjectile();
        s1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if ((transform.position - BornPosition).magnitude >= 20)
        {
            if (spriteRenderer.material.color.a >= 0)
            {
                spriteRenderer.material.color = spriteRenderer.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
                s1.material.color = s1.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
            }
            if ((transform.position - BornPosition).magnitude >= 10 + 3)
            {
                Destroy(gameObject);
            }
        }
        if (isDestory)
        {
            s1.material.color = s1.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player"))
        {
            isDestory = true;
            Destroy(rigidbody2D);

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);

            if (other.tag == ("Player"))
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, Type.TypeEnum.Bug);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 5;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
        }
    }
}
