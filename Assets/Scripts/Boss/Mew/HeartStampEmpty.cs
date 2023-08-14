using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeartStampEmpty : Projectile
{
    private Vector3 position;
    private Vector3 moveDirection;
    public float moveSpeed = 5f;
    // Start is called before the first frame update
    public void SetTarget(Vector3 target)
    {
        position = target;
        moveDirection = (position - transform.position).normalized;
    }
    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (position != null)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, Dmage, 0, 0, Type.TypeEnum.Psychic);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
