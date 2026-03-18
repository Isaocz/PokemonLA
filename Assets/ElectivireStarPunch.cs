using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivireStarPunch : Projectile
{
    /// <summary>
    /// 父电击魔兽
    /// </summary>
    public Electivire ParentElectivire;


    /// <summary>
    /// 拳特效
    /// </summary>
    public ElectivirePunchEffect punch;


    public Vector2 direction;
    public float MaxRange; // 最大移动距离
    private Animator animator;
    bool isCanNotMove;
    Vector3 StartPostion;

    bool isDestory;

    //开始不触发计时器
    float StartTimer = 0.0f;


    public EmptyTrace trace;

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
        if (StartTimer < 0.2f)
        {
            StartTimer += Time.deltaTime;
        }
        

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
            //Debug.Log("des");
            transform.GetComponent<Collider2D>().enabled = false;
            punch.PunchOver();
            isCanNotMove = true;
            if (trace != null ) { trace.isCanNotMove = true; }
            _mTool.RemoveAllPSChild(gameObject);
            if (empty != null) { empty.GetComponent<Electivire>().LunchThuder(2,transform.position); }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || (collision.tag == "Room" && (transform.position - BornPosition).magnitude >= 1.0f))
        {
            BallBreak();
            if (empty != null)
            {
                if ( collision.tag == "Player")
                {
                    PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                    Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, 80, 0, PokemonType.TypeEnum.Electric);
                    if (playerControler != null)
                    {
                        playerControler.KnockOutPoint = 10;
                        playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                        playerControler.ParalysisFloatPlus(0.12f);
                    }
                }
            }
        }
    }
}
