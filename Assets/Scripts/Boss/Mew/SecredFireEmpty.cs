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

    public void Initialize(Vector3 centerPosition, float radius)
    {
        Vector3 directionToCenter = centerPosition - transform.position;
        Vector3 perpendicularDirection = new Vector3(-directionToCenter.y, directionToCenter.x).normalized * radius;
        Vector3 targetPosition = centerPosition + perpendicularDirection;
        moveDirection = (targetPosition - transform.position).normalized;
        float Distance = Vector3.Distance(transform.position, targetPosition);
        speed = Distance / 1f;
        StartCoroutine(StartMovingAfterDelay(3f));
    }

    private IEnumerator StartMovingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = moveDirection * speed;
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        ObjectPoolManager.ReturnObjectToPool(gameObject, 8f);
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
