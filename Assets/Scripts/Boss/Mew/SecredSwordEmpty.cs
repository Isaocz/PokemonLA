using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecredSwordEmpty : Projectile
{
    private PlayerControler player;
    private float angle;
    private float radius;
    private Vector3 position;
    private float timer;
    private Vector3 target;
    private Vector3 initialDirection;
    public void Initialize(float Angle, float Radius)
    {
        angle = Angle;
        radius = Radius;
    }
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        position = player.transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius;
        Destroy(gameObject, 5f);
        Invoke("RecordPosition", 1.7f);

        initialDirection = Quaternion.Euler(0f, 0f, angle) * Vector2.right * -1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer < 1.7f)
        {
            position = player.transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius;
            transform.position = position;
            transform.right = initialDirection;
        }
        else
        {
            float moveSpeed = 12f; // ÒÆ¶¯ËÙ¶È
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }
    }
    void RecordPosition()
    {
        target = player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, 90, 0, Type.TypeEnum.Fighting);
        }
    }
}
