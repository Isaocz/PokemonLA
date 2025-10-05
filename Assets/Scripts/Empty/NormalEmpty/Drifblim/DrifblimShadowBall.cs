using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrifblimShadowBall : Projectile
{
    public float moveSpeed = 5f;
    public Vector2 direction;
    public float MaxRange = 20f; // ×î´óÒÆ¶¯¾àÀë
    private Animator animator;
    bool isCanNotMove;
    Vector3 StartPostion;
    void Start()
    {
        animator = GetComponent<Animator>();
        isCanNotMove = false;
        StartPostion = transform.position;
    }

    void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * moveSpeed * Time.deltaTime;
            postion.y += direction.y * moveSpeed * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                BallBreak();
            }
        }
    }

    void BallBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Over");
            isCanNotMove = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player"||collision.tag == "Room" || collision.tag == "Enviroment"|| (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject ))
        {
            BallBreak();
            if (collision.tag == "Player")
            {
                PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, 80, 0, PokemonType.TypeEnum.Ghost);
                if(playerControler != null)
                {
                    playerControler.KnockOutPoint = 5;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            else if (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
            {
                Empty e = collision.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, 0, 80, 0, PokemonType.TypeEnum.Ghost);
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
