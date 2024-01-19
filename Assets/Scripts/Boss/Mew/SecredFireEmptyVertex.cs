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
    public float acceleration;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        direction = (player.position - transform.position).normalized;
        StartCoroutine(StartMovingAfterDelay(1.5f));
        ObjectPoolManager.ReturnObjectToPool(gameObject, 6.5f);
    }

    private IEnumerator StartMovingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject ef = Instantiate(Effects, transform.position, Quaternion.identity);
        Destroy(ef, 1.4f);
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
