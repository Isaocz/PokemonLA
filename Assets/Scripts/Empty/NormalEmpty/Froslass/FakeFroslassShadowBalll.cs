using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFroslassShadowBalll : Projectile
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
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            DestoryByRange(MaxRange);
            if (isDestory)
            {
                BallBreak();
            }
            else
            {
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
        if (collision.tag == "Player" || collision.tag == "Room" )
        {
            BallBreak();
        }
    }

}
