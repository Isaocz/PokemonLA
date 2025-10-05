using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonClawDruddigon : Projectile
{
    float Timer;

    private void Update()
    {
        Timer += Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Timer <= 0.4f)
        {
            if (!empty.isEmptyInfatuationDone && collision.tag == "Player")
            {
                // 对玩家造成伤害
                PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Dragon);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 5f;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }

            }
            else if (empty != null && empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
            {
                Empty e = collision.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Dragon);

            }
            if (empty.isEmptyConfusionDone && collision.gameObject == empty.gameObject)
            {
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Dragon);
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
