using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItRainEmpty : Projectile
{
    private Vector3 direction;
    private float movespeed;
    private float timer;
    private SpriteRenderer sr;

    public void MIRrotate(Vector3 Direction)
    {
        direction = Direction;
    }
    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        movespeed = 5f;
        timer = 0f;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(direction * movespeed * Time.deltaTime);
        if(timer > 6.5f && timer < 7f)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (7f - timer) / 0.5f);
        }
        else if (timer > 7f)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
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
