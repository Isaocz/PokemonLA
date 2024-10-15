using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartStampEmpty : Projectile
{
    private Transform target;
    private Vector3 direction;
    private float angle;
    private bool isRotate;
    public float moveSpeed;
    public float time;
    public int phrase;
    private float timer;
    private float currentSpeed;

    private void OnEnable()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject, 8f);
        isRotate = false;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer <= 0.5f)
        {
            if(phrase == 3)
            {
                currentSpeed = 10f;
            }
            else
            {
                currentSpeed = 5f;
            }
        }
        else if (timer < time && timer > 0.5f)
        {
            currentSpeed = 0f;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
        if (!isRotate && timer > 1f)
        {
            target = GameObject.FindWithTag("Player").transform;
            direction = target.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            isRotate = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Psychic);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
