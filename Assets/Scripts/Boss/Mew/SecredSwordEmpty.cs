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
    private Transform target;
    private Vector3 target2;
    private Vector3 initialDirection;
    public void Initialize(float Angle, float Radius, GameObject Target)
    {
        angle = Angle;
        radius = Radius;
        target = Target.transform;
    }
    void Start()
    {
        position = target.position + Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius;
        Destroy(gameObject, 3f);
        Invoke("RecordPosition", 1.7f);

        initialDirection = Quaternion.Euler(0f, 0f, angle) * Vector2.right * -1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer < 1.7f)
        {
            position = target.position + Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius;
            transform.position = position;
            transform.right = initialDirection;
        }
        else
        {
            float moveSpeed = 16f; // ÒÆ¶¯ËÙ¶È
            transform.position = Vector3.MoveTowards(transform.position, target2, moveSpeed * Time.deltaTime);
        }
    }
    void RecordPosition()
    {
        target2 = target.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, Type.TypeEnum.Fighting);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }

        }
    }
}
