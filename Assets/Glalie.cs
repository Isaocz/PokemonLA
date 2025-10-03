using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glalie : Empty
{
    //冰鬼护状态机
    enum State
    {
        Idle, //原地发呆
        Bite, //啃咬
    }
    State NowState;

    float BiteRushTimer;

    float RUSHTIME;

    public bool isBiteMove;

    Vector2 Director = new Vector2(1,1);//敌人朝向

    //冲刺的次数
    int BiteRushCount;
    //最大冲刺次数
    int MAX_RUSHCOUNT = 3;


    /// <summary>
    /// 死亡后放出冰砾的预制件
    /// </summary>
    public GlalieIceShard iceShard;

    /// <summary>
    /// 咬碎的动画预制件
    /// </summary>
    public GameObject Crunch;
    /// <summary>
    /// 咬碎预制件
    /// </summary>
    public List<GameObject> CrunchOBJ = new List<GameObject> { };
    /// <summary>
    /// 发呆计时器
    /// </summary>
    public float IdleTimer;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ice;//敌人第一属性
        EmptyType02 = PokemonType.TypeEnum.No;//敌人第二属性
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        Emptylevel = SetLevel(player.Level, MaxLevel);//设定敌人等级
        EmptyHpForLevel(Emptylevel);//设定血量
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//设定攻击力
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//设定特攻
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //设置移动方向
        Director = new Vector2(((transform.localPosition.x) <= 0 ? 1 : -1), ((transform.localPosition.y) <= 0 ? 1 : -1));
        //设置动画机朝向
        animator.SetFloat("LookX" , Director.x);
        animator.SetFloat("LookY" , Director.y);

        IdleState(0.3f);

        DestoryEvent += DieBlest;



        StartOverEvent();
    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();//如果玩家组件丢失，重新获取
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            EmptyDie();//判定是否执行死亡
            UpdateEmptyChangeHP();//判定生命值是否变化
            StateMaterialChange();//判定是否更换状态材质
            if (!isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone)
            {
                switch (NowState)
                {
                    case State.Bite:
                        if (isBiteMove)
                        {
                            //开启残影
                            if (ShadowCoroutine == null)
                            {
                                RUSHTIME = setrushtime();
                                StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                                CrunchOBJ.Add(Instantiate(Crunch, transform.position + Vector3.up * 0.6f, Quaternion.identity, transform));
                            }
                            //冲刺
                            rigidbody2D.position = new Vector2(
                                Mathf.Clamp(rigidbody2D.position.x
                                    + (float)Director.x * Time.deltaTime * speed * 5.5f,                    //方向*速度
                                ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x, //最小值
                                ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x),//最大值
                                Mathf.Clamp(rigidbody2D.position.y
                                    + (float)Director.y * Time.deltaTime * speed * 5.5f,                     //方向*速度 
                                ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y,  //最小值
                                ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y));//最大值

                            //计时器
                            BiteRushTimer += Time.deltaTime;
                            if (BiteRushTimer >= ((isEmptyConfusionDone)?RUSHTIME : 0.15f))
                            {
                                SetRushDir();
                                if (ShadowCoroutine != null)
                                {
                                    StopShadowCoroutine();
                                }
                                BiteRushTimer = 0;
                                BiteRushCount++;
                                isBiteMove = false;
                                if (BiteRushCount >= MAX_RUSHCOUNT)
                                {
                                    animator.SetInteger("isRushOver" , 2);
                                    //StartCoroutine(IdleForSecoends(9.0f));
                                }
                                else
                                {
                                    animator.SetInteger("isRushOver", 1);
                                }
                            }

                        }
                        else
                        {
                            //关闭残影
                            if (ShadowCoroutine != null)
                            {
                                StopShadowCoroutine();
                            }
                            
                        }
                        break;
                    case State.Idle:
                        IdleTimer -= Time.deltaTime;
                        if (IdleTimer <= 0.0f)
                        {
                            StartRush();
                            IdleTimer = 0.0f;
                        }
                        break;
                }
            }
            if ((isSleepDone || isFearDone) && NowState != State.Idle)
            {
                IdleState(3.0f);
                animator.SetInteger("isRushOver", 2);
                animator.SetTrigger("BiteOver");
                //关闭残影
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
            }

            //设置动画机朝向
            animator.SetFloat("LookX", Director.x);
            animator.SetFloat("LookY", Director.y);
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//如果玩家组件丢失，重新获取
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//如果被魅惑，计算魅惑时间
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            EmptyBeKnock();//判定是否被击退
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//未被魅惑 且与玩家碰撞时
        {
            if (isBiteMove)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 80, 0, 0, PokemonType.TypeEnum.Dark);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 7.0f;
                    playerControler.KnockOutDirection = (new Vector2(Director.y, Director.x)).normalized;
                }
            }
            else
            {
                EmptyTouchHit(other.gameObject);//触发触碰伤害
            }
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//被魅惑 且与其他敌人碰撞时
        {
            if (isBiteMove)
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, 80, 0, 0, PokemonType.TypeEnum.Dark);
            }
            else
            {
                InfatuationEmptyTouchHit(other.gameObject);//触发魅惑后触碰伤害
            }
        }
    }

    float setrushtime()
    {
        if (isEmptyConfusionDone) { return Random.Range(0.04f, 0.24f); }
        else                      { return 0.15f; }
    }

    /// <summary>
    /// 进入发呆状态
    /// </summary>
    public void IdleState(float Idletime)
    {
        NowState = State.Idle;
        isBiteMove = false;
        BiteRushCount = 0;
        animator.ResetTrigger("Bite");
        IdleTimer = Idletime;
    }
    
    
    public void StartRush()
    {
        NowState = State.Bite;
        animator.SetTrigger("Bite");
        RUSHTIME = setrushtime();
        //设置移动方向
        Director = new Vector2(((transform.localPosition.x) <= 0 ? 1 : -1), ((transform.localPosition.y) <= 0 ? 1 : -1));
        //设置动画机朝向
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);
    }

    /*
    /// <summary>
    /// 发呆冷却
    /// </summary>
    /// <param name="idleTime">发呆时间</param>
    /// <returns></returns>
    IEnumerator IdleForSecoends(float idleTime)
    {
        NowState = State.Idle;
        isBiteMove = false;
        BiteRushCount = 0;
        animator.ResetTrigger("Bite");
        yield return new WaitForSeconds(idleTime);
        NowState = State.Bite;
        animator.SetTrigger("Bite");
        //设置移动方向
        Director = new Vector2(((transform.localPosition.x) <= 0 ? 1 : -1), ((transform.localPosition.y) <= 0 ? 1 : -1));
        //设置动画机朝向
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);
    }
    */

    public void BiteShakeStart()
    {
        NowState = State.Bite;
        isBiteMove = false;
        
    }

    public void BiteRushStart()
    {
        NowState = State.Bite;
        animator.SetInteger("isRushOver", 0);
        isBiteMove = true;
        
    }





    /// <summary>
    /// 设置冲刺方向
    /// </summary>
    void SetRushDir()
    {
        Vector2 dir1 = Quaternion.AngleAxis( 90, Vector3.forward) * Director;
        Vector2 dir2 = Quaternion.AngleAxis(-90, Vector3.forward) * Director;
        RaycastHit2D raycastHit2D1 = Physics2D.Raycast(transform.position, dir1);
        RaycastHit2D raycastHit2D2 = Physics2D.Raycast(transform.position, dir2);
        if (raycastHit2D1.distance >= raycastHit2D2.distance)
        {
            Director = dir1;
            Debug.Log(dir1);
        }
        else
        {
            Director = dir2;
            Debug.Log(dir2);
        }

        

        //设置动画机朝向
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);

        
        
    }



    /// <summary>
    /// 冰鬼护死亡后爆炸
    /// </summary>
    void DieBlest()
    {
        if (IsDeadrattle) {
            GlalieIceShard i1 = Instantiate(iceShard, transform.position, Quaternion.Euler(0, 0, 90), ParentPokemonRoom.transform);
            i1.SetNewIceShard(0, this);
            GlalieIceShard i2 = Instantiate(iceShard, transform.position, Quaternion.Euler(0, 0, 210), ParentPokemonRoom.transform);
            i2.SetNewIceShard(0, this);
            GlalieIceShard i3 = Instantiate(iceShard, transform.position, Quaternion.Euler(0, 0, 330), ParentPokemonRoom.transform);
            i3.SetNewIceShard(0, this);
        }
    }

}
