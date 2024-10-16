using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompingTanturmMakuhita : Projectile
{
    void Start()
    {
        Invoke("SetPSActive", 0.4f);
        Invoke("DestroySelf", 2f);
    }

    void SetPSActive()
    {

        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!empty.isEmptyInfatuationDone && collision.tag == "Player")
        {
            // ���������˺�
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ground);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 9f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }

        }
        else if (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
        {
            Empty e = collision.GetComponent<Empty>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ground);
        }
    }


    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
