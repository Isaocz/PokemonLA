using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBeamEmpty : Projectile
{
    private void OnEnable()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject,1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Ice);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                playerControler.Frozen(0.5f, 1f, 0.6f);
                playerControler.Cold(1f);
            }
        }
    }
}
