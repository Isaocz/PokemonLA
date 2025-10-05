using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaunterSludgeBombWaveCollider : Projectile
{
    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!empty.isEmptyInfatuationDone && collision.tag == "Player")
        {
            // 对玩家造成伤害
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Psychic);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                playerControler.ToxicFloatPlus(0.5f);
            }

        }
        else if (empty != null && empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
        {
            Empty e = collision.GetComponent<Empty>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Psychic);
            e.EmptyToxicDone(1f, 5.0f, 0.5f);
        }
    }
}
