using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaniluxeIcicleCrash : Projectile
{

    /// <summary>
    /// ±ùÖù
    /// </summary>
    public GameObject IcicleCrash;

    private void Start()
    {
        
        AwakeProjectile();
        Timer.Start(this, 0.15f, () => { Instantiate(IcicleCrash, transform.position + 0.25f * Vector3.up, Quaternion.identity); });
        Timer.Start(this, 5.0f, () => { Destroy(gameObject);  });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (empty != null && other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);

            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 1;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    playerControler.PlayerFrozenFloatPlus(0.3f, 1.2f);
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
            }
        }
    }

}
