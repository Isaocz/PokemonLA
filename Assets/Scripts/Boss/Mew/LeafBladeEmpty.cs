using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBladeEmpty : Projectile
{
    public float moveSpeed = 10f;
    private Transform target; //Ä¿±ê

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        Vector3 direction = target.position - transform.position;
        Destroy(gameObject, 5f);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Update is called once per frame
    void Update()
    {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 90, 0, 0, Type.TypeEnum.Grass);
        }
    }
}
