using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecredFireEmptyVertex : Projectile
{
    private Transform player;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float speed;
    private float acceleration;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        direction = (player.position - transform.position).normalized;
        speed = 7f;
        acceleration = 3f;
        StartCoroutine(StartMovingAfterDelay(1.5f));
        Destroy(gameObject, 6.5f);
    }

    private IEnumerator StartMovingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = direction * speed;
    }

    private void FixedUpdate()
    {
        speed += acceleration * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Fire);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
