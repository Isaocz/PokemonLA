using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCastform : Projectile
{
    void Start()
    {
        Invoke("SetPSActive", 0.3f);
        Invoke("DsetorySelf", 1.3f);
    }

    void SetPSActive()
    {

        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 对玩家造成伤害
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            if (empty != null) { Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Electric); }
            else { Pokemon.PokemonHpChange(null, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Electric); }
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                if (Random.Range(0.0f, 1.0f) > 0.9f) { playerControler.ParalysisFloatPlus(0.4f); }
            }

        }
    }

    void DsetorySelf()
    {
        Destroy(gameObject);
    }
}
