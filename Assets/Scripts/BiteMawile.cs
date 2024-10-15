using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteMawile : Projectile
{

    private void Start()
    {
        Timer.Start(this , 0.72f , ()=> { if (gameObject) { Destroy(gameObject); } } );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Dark);
                if (playerControler != null)
                {
                    //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / 250), 14);
                    playerControler.KnockOutPoint = 10;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Dark);
                //e.EmptyHpChange(0, (SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / (250 * e.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((e.EmptyType01 == Type.TypeEnum.Ice || e.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2), 14);
            }
        }
    }
}
