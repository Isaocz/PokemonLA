using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoneEdgeEmpty : Projectile
{
    public float Stonespeed = 13f;
    float timer = 0f;

    void Start()
    {
        Destroy(gameObject, 7f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2f)
            transform.Translate(Vector3.down * Stonespeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, Type.TypeEnum.Rock);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
