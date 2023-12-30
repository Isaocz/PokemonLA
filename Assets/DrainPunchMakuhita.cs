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
            // 对玩家造成伤害

            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            int BeforeHP = playerControler.Hp;
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, Type.TypeEnum.Fighting);
            Pokemon.PokemonHpChange(null, empty.gameObject, 0, 0, Mathf.Clamp((BeforeHP - playerControler.Hp) / 2, 1, 10000), Type.TypeEnum.IgnoreType);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 9f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }

        }
        else if (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
        {
            
            Empty e = collision.GetComponent<Empty>();
            int BeforeHP = e.EmptyHp;
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, Type.TypeEnum.Fighting);
            Pokemon.PokemonHpChange(null, empty.gameObject, 0, 0, Mathf.Clamp((BeforeHP - e.EmptyHp) / 2 , 1 , 10000), Type.TypeEnum.IgnoreType);
        }
    }


    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
