using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillOWispEmpty : Projectile
{
    public float moveSpeed;
    private Vector3 moveDirection;

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Fire);
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
