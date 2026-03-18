using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrodeExplosion : Projectile
{
    public float KOPoint = 12.5f;

    bool isDestory;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 3.5f, () => { if (!isDestory) { isDestory = true; } });
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Empty")
        {
            if (empty != null)
            {
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Normal);
                if (collision.tag == "Player")
                {
                    PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                    if (playerControler != null)
                    {
                        playerControler.KnockOutPoint = KOPoint;
                        playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    }
                }
                if (collision.tag == "Empty")
                {
                    Empty e = collision.GetComponent<Empty>();
                    if (e != empty)
                    {
                        e.EmptyKnockOut(KOPoint);
                    }
                }
            }
        }
    }
}
