using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixBodyPressCollider : Projectile
{
    void Start()
    {
        Invoke("DetachSelf", 0.7f);
        Invoke("DestroySelf", 3f);
    }



    void DetachSelf()
    {
        transform.parent = null;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (empty != null)
        {
            if (!empty.isEmptyInfatuationDone && collision.tag == "Player")
            {
                // 对玩家造成伤害
                PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, Type.TypeEnum.Electric,Pokemon.SpecialAttackTypes.BodyPress);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 9f;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }

            }
        }
    }
}

