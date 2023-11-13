using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SecredFireEmpty : Projectile
{
    private Rigidbody2D rb;
    private Vector3 moveDirection;
    private float speed;
    private float acceleration;

    public void Initialize(Vector3 centerPosition, float radius)
    {
        Vector3 directionToCenter = centerPosition - transform.position;
        Vector3 perpendicularDirection = new Vector3(-directionToCenter.y, directionToCenter.x).normalized * radius;
        Vector3 targetPosition = centerPosition + perpendicularDirection;
        moveDirection = (targetPosition - transform.position).normalized;
        speed = 5f;
        acceleration = 2f;
        StartCoroutine(StartMovingAfterDelay(3f));
    }

    private IEnumerator StartMovingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = moveDirection * speed;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 8f);
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
