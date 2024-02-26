using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillOWispEmpty : Projectile
{
    private float moveSpeed;
    private Vector3 moveDirection;
    private float timer;

    private void OnEnable()
    {
        Destroy(gameObject,4f);
    }
    private void Update()
    {
        float currentSpeed;
        timer += Time.deltaTime;
        currentSpeed = moveSpeed + timer;
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Fire);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * WeatherAlpha * (2 * empty.Emptylevel + 10) / 250), 11);
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            playerControler.BurnFloatPlus(0.4f);
            Destroy(gameObject); // Ïú»ÙWillOWisp¶ÔÏó
        }
    }
    public void Initialize(float moveSpeed, Vector3 direction)
    {
        this.moveSpeed = moveSpeed;
        this.moveDirection = direction;
    }
}
