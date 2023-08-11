using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MakeItRainEmpty : Projectile
{
    private Vector3 direction;
    private float movespeed = 4f;
    // Start is called before the first frame update
    public void MIRrotate(Vector3 Direction)
    {
        direction = Direction;
    }
    void Start()
    {
        Destroy(gameObject, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * movespeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            int MoneyIncreseddamage = playerControler.Money/2 + 70;
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, MoneyIncreseddamage < 200 ? MoneyIncreseddamage : 200, 0, Type.TypeEnum.Steel);
            Destroy(gameObject);
            playerControler.ChangeMoney(1);
            
        }
    }
}
