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
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Ice);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                if (Random.Range(0.0f, 1.0f) >= 0.6f) { playerControler.PlayerFrozenFloatPlus(0.5f , 1.2f); }
            }
        }
    }
}
