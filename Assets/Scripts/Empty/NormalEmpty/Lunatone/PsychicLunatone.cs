using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicLunatone : Projectile
{

    float Timer;

    private void Update()
    {
        Timer += Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Timer <= 2.0f)
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
                }

            }
            else if (empty != null && empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
            {
                Empty e = collision.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Psychic);

            }
            if (empty.isEmptyConfusionDone && collision.gameObject == empty.gameObject)
            {
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Psychic);
            }
        }
    }
}
