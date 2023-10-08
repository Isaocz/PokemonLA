using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillOWispEmpty : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveDirection;
    public GameObject mew;

    public int SpAPower;

    private void Start()
    {
        Destroy(gameObject,4f);
    }
    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(mew, collision.gameObject, 0, SpAPower, 0, Type.TypeEnum.Fire);
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
    public void Initialize(float moveSpeed, Vector3 direction)
    {
        this.moveSpeed = moveSpeed;
        this.moveDirection = direction;
    }
}
