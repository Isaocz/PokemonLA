using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleShotEmpty : Projectile
{
    private float moveSpeed;
    private float timer; // ¼ÆÊ±Æ÷

    // Start is called before the first frame update
    void OnEnable()
    {
        timer = 0f;
        ObjectPoolManager.ReturnObjectToPool(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        moveSpeed = 4 * Mathf.Pow((timer - 1), 2) + 1f;
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Dragon);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
