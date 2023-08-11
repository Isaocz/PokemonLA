using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAirSlash : Projectile
{
    public float SplitAirSlashSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,4f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * SplitAirSlashSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, 75, 0, Type.TypeEnum.Flying);
        }
    }
}
