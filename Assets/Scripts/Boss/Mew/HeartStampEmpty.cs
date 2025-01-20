using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartStampEmpty : Projectile
{
    private Transform target;
    private Vector3 direction;
    private float rotateAngle;
    private bool isRotate;
    public float moveSpeed;
    public float time;
    public int phrase;
    private float timer;
    private float currentSpeed;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void OnEnable()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject, 8f);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        isRotate = false;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Rotate(0, 0, rotateAngle);
        if (timer <= 0.5f)
        {
            if(phrase == 3)
            {
                currentSpeed = 10f;
                rotateAngle = 20f;
            }
            else
            {
                currentSpeed = 5f;
                rotateAngle = 20f;
            }
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, timer * 2f);
        }
        else if (timer < time && timer > 0.5f)
        {
            currentSpeed = 0f;
            rotateAngle = 120f;
        }
        else
        {
            currentSpeed = moveSpeed;
            rotateAngle = 120f;
        }
        //transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
        if (!isRotate && timer > 1f)
        {
            currentSpeed = moveSpeed + (timer - 1f) * 2;
            target = GameObject.FindWithTag("Player").transform;
            direction = (target.position - transform.position).normalized;
            //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            rb.velocity = currentSpeed * direction;
            isRotate = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timer > 0.5f && collision.CompareTag("Player"))
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
