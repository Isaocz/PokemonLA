using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecredFireEmptyVertex : Projectile
{
    public GameObject Effects;

    private Transform player;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float speed;
    private bool isEffect;
    public float acceleration;
    public bool startMoving;
    public GameObject trail;

    private void OnEnable()
    {
        startMoving = false;
        isEffect = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        direction = (player.position - transform.position).normalized;
        speed = 8f;
        ObjectPoolManager.ReturnObjectToPool(gameObject, 6.5f);
    }

    private void FixedUpdate()
    {
        if (startMoving)
        {
            speed += acceleration * Time.fixedDeltaTime;
            if (!isEffect)
            {
                GameObject ef = Instantiate(Effects, transform.position, Quaternion.identity);
                GameObject trails = Instantiate(trail, transform.position, Quaternion.LookRotation(direction));
                Destroy(ef, 1.4f);
                Destroy(trails, 1f);
                isEffect = true;
            }
            rb.velocity = direction * speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Fire);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
