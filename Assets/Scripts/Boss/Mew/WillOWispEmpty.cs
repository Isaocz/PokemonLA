using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillOWispEmpty : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveDirection;
    public GameObject mew;

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(mew, playerControler.gameObject, 0, 60, 0, Type.TypeEnum.Fire);
            playerControler.BurnFloatPlus(0.4f);
            Destroy(gameObject); // ����WillOWisp����
        }
    }
    public void Initialize(float moveSpeed, Vector3 direction)
    {
        this.moveSpeed = moveSpeed;
        this.moveDirection = direction;
    }
}