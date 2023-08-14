using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrossPoisonEmpty : Projectile
{
    private float LaunchSpeed = 8f;
    private float BackSpeed = 4f;
    Vector3 direction;
    Vector3 Backdirection;
    private Rigidbody2D rb;

    private bool canHurt;
    private float timer;

    // Start is called before the first frame update
    public void CProtate(Vector3 Direction)
    {
        direction = Direction;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 15f);
        canHurt = false;
        Backdirection = (-direction);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 3f)
        {
            rb.velocity = direction * LaunchSpeed;
            //发射时不能伤害玩家
            canHurt = false;
        }
        else
        {
            rb.velocity = Backdirection * BackSpeed;
            canHurt = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canHurt == true && collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, Dmage, 0, 0, Type.TypeEnum.Poison);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            playerControler.ToxicFloatPlus(0.3f);
        }
    }
}
