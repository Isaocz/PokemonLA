using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItRainEmpty : Projectile
{
    private Vector3 direction;
    private float movespeed;
    // Start is called before the first frame update
    public void MIRrotate(Vector3 Direction)
    {
        direction = Direction;
    }
    void OnEnable()
    {
        movespeed = 5f;
        ObjectPoolManager.ReturnObjectToPool(gameObject, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * movespeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            int MoneyIncreseddamage = playerControler.Money/2 + 50;
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, MoneyIncreseddamage < 100 ? MoneyIncreseddamage : 100, 0, Type.TypeEnum.Steel);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 1f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject);
            playerControler.ChangeMoney(1);
            
        }
    }
}
