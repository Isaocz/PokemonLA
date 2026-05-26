using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÂêáðÀ­»Ø¸´·´ÊÉ±¬Õ¨
/// </summary>
public class WeavileHealExplosion : Projectile
{
    public float KOPoint = 10.0f;

    bool isDestory;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 5.0f, () => { Destroy(gameObject); });
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Empty")
        {
            if (empty != null)
            {
                if (collision.tag == "Empty")
                {
                    Pokemon.PokemonHpChange(null, collision.gameObject, 40, 0, 0, PokemonType.TypeEnum.Normal);
                    Empty e = collision.GetComponent<Empty>();
                    if (e != null && e != empty)
                    {
                        e.EmptyKnockOut(KOPoint);
                    }
                }
            }
        }
    }
}
