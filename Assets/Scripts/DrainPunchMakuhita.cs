using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainPunchMakuhita : Projectile
{
    void Start()
    {
        Invoke("DestroySelf", 0.5f);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!empty.isEmptyInfatuationDone && collision.tag == "Player")
        {
            // ���������˺�

            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            int BeforeHP = -1;
            if (playerControler != null)
            {
                BeforeHP = playerControler.Hp;
                playerControler.KnockOutPoint = 9f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Fighting);
            if (BeforeHP != -1)
            {
                Pokemon.PokemonHpChange(null, empty.gameObject, 0, 0, Mathf.Clamp((BeforeHP - playerControler.Hp) / 2, 1, 10000), PokemonType.TypeEnum.IgnoreType);
            }

        }
        else if (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
        {
            
            Empty e = collision.GetComponent<Empty>();
            int BeforeHP = e.EmptyHp;
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Fighting);
            Pokemon.PokemonHpChange(null, empty.gameObject, 0, 0, Mathf.Clamp((BeforeHP - e.EmptyHp) / 2 , 1 , 10000), PokemonType.TypeEnum.IgnoreType);
        }
    }


    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
