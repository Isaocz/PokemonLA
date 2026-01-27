using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MismagiusShadowBalll : Projectile
{
    public Vector2 direction;
    public float MaxRange; // ×î´óÒÆ¶¯¾àÀë
    private Animator animator;
    bool isCanNotMove;
    Vector3 StartPostion;

    bool isDestory;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 10.0f, () => { if (!isDestory) { isDestory = true; } });
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        isCanNotMove = false;
        StartPostion = transform.position;
    }

    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0)
        if (!isCanNotMove)
        {
            DestoryByRange(MaxRange);
            if (isDestory)
            {
                BallBreak();
            }
            else
            {
                //Debug.Log(LunchDirection +"+");
                MoveNotForce();
            }
        }
    }

    public override void DestoryByRange(float ProjectileRange)
    {
        if ((transform.position - BornPosition).magnitude >= ProjectileRange)
        {
            BallBreak();
        }
    }


    void BallBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            Destroy(transform.GetChild(1).gameObject);
            animator.SetTrigger("Over");
            isCanNotMove = true;

            _mTool.RemoveAllPSChild(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Room" || (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject))
        {
            BallBreak();
            if (empty != null)
            {
                if (!empty.isEmptyInfatuationDone && collision.tag == "Player")
                {
                    PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                    Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, 80, 0, PokemonType.TypeEnum.Ghost);
                    if (playerControler != null)
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
    }
}
