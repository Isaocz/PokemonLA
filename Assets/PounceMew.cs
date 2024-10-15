using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PounceMew : Projectile
{
    public Vector3 offset = new Vector3(0, 0.5f, 0);
    private float timer;
    private float Hittimer;
    void Start()
    {
        timer = 0f;
        Hittimer = 0f;
        Destroy(gameObject, 5.1f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        Hittimer += Time.deltaTime;
        transform.position = empty.transform.position + offset;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(timer < 4.6f)
        {
            if(Hittimer > 0.5f && collision.CompareTag("Player"))
            {
                PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Bug);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 2.5f;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
                Hittimer = 0f;
            }
        }
    }
}
