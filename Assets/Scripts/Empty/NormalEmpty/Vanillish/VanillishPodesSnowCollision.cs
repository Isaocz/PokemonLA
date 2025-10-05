using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanillishPodesSnowCollision : MonoBehaviour
{
    //父敌人
    public Empty empty;
    //威力
    public int SpDmage;

    //被魅惑时，已经造成伤害的敌人列表
    List<Empty> isAtkedEmpty = new List<Empty> { };

    private void Start()
    {
        var c = transform.GetComponent<ParticleSystem>().collision;
        if (empty.isEmptyInfatuationDone)
        {
            c.collidesWith = LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump");
        }
        else
        {
            c.collidesWith = LayerMask.GetMask("Player", "PlayerFly", "PlayerJump");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (empty != null && other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);

            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Ice);
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
                if (!isAtkedEmpty.Contains(e))
                {
                    isAtkedEmpty.Add(e);
                    Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Ice);
                }
            }
        }
    }
}
