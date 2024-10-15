using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillOWispEmpty : Projectile
{
    private float moveSpeed;
    private Vector3 moveDirection;
    private float timer;
    private new CircleCollider2D collider2D;
    public bool isStage;

    private void OnEnable()
    {
        timer = 0f;
        collider2D = GetComponent<CircleCollider2D>();
        collider2D.radius = 0.2019496f;
        transform.GetChild(0).localScale = new Vector3(1f, 1f);
    }
    private void Update()
    {
        float currentSpeed = 0f;
        timer += Time.deltaTime;
        if (timer > 3.5f && timer < 4f)
        {
            float t = (timer - 3.5f) / 0.5f;
            transform.GetChild(0).localScale = Vector3.Lerp(new Vector3(1f, 1f), new Vector3(0, 0), t);
            collider2D.radius = Mathf.Lerp(0.2019496f, 0, t);
            currentSpeed = Mathf.Lerp(moveSpeed + 3.5f, 0, t);
        }
        else if (timer > 4f)
        {
            Destroy(gameObject);
        }
        else
        {
            currentSpeed = moveSpeed + timer;
        }
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isStage)
        {
            if (collision.tag == "Player")
            {
                PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Fire);
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
    }
    public void Initialize(float moveSpeed, Vector3 direction, bool isstage = false)
    {
        this.moveSpeed = moveSpeed;
        this.moveDirection = direction;
        isStage = isstage;
    }
}
