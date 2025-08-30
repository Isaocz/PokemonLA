using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//冰宝
//有冰岩怪 跳上冰岩怪的背---在背上发呆---从背上跳下---发呆---走路接近冰岩怪---跳上冰岩怪的背
//无冰岩怪 陷入害怕状态 逃往房间角落


public class Bergmite : Empty
{
    /// <summary>
    /// 亲冰岩怪
    /// </summary>
    public Avalugg ParentAvalugg;

    /// <summary>
    /// 冰宝状态机
    /// </summary>
    public enum BergmiteState
    {
        Idle,//发呆
        Run,//走路
        Jump,//跳上冰岩怪
        Drop,//跳下冰岩怪
        IdleInParent,//在冰岩怪身上发呆
    };
    public BergmiteState NowState;

    /// <summary>
    /// 冰宝处于冰岩怪身上的位置
    /// </summary>
    public Vector2 ParentPosition;

    /// <summary>
    /// 冰砾
    /// </summary>
    public BergmiteIceShard IceShard;

    Vector2 Director;//冰宝朝向
    /// <summary>
    /// 目标位置
    /// </summary>
    Vector2 TargetPosition;

    Vector3 LastPosition;//计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行

    //冰雾例子效果1，2
    ParticleSystem FrozenMistPS1;
    ParticleSystem FrozenMistPS2;

    //害怕时的逃跑目标
    Vector2 FearTarget = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Normal;//敌人第一属性
        EmptyType02 = PokemonType.TypeEnum.Normal;//敌人第二属性
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        Emptylevel = SetLevel(player.Level, MaxLevel);//设定敌人等级
        EmptyHpForLevel(Emptylevel);//设定血量
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//设定攻击力
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//设定特攻
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.2f ;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.2f ;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        FrozenMistPS1 = transform.GetChild(4).GetChild(0).GetComponent<ParticleSystem>();
        FrozenMistPS2 = transform.GetChild(4).GetChild(1).GetComponent<ParticleSystem>();
        //启动计算方向携程
        StartCoroutine(CheckLook());
        //设置朝向
        SetDirector(Vector2.down);
        //苏醒后发呆
        IdleStart(IDLE_OF_AWAKE);
        StopFrozenMistPS();


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

            if (ParentAvalugg != null)
            {
                if (!isEmptyFrozenDone && !isSleepDone && !isSilence) {
                    switch (NowState)
                    {
                        case BergmiteState.Idle:
                            //发呆
                            if (!isCanNotMoveWhenParalysis)
                            {
                                IdleTimer += Time.deltaTime;
                                if (IdleTimer >= TimeOfIdle)
                                {
                                    IdleOver();
                                    RunStart();
                                }
                            }
                            break;
                        case BergmiteState.Run:
                            if (!isCanNotMoveWhenParalysis)
                            {
                                if (!isFearDone)
                                {
                                    if (!isEmptyInfatuationDone)
                                    {
                                        //向亲冰岩怪移动
                                        rigidbody2D.position += ((Vector2)(ParentAvalugg.transform.position - transform.position).normalized * Time.deltaTime * speed);
                                        //设置朝向
                                        Director = _mTool.MainVector2((ParentAvalugg.transform.position - transform.position).normalized);
                                        SetDirector(Director);
                                        //当距离小于JumpInDistence时 跳上背
                                        if (Vector3.Distance(ParentAvalugg.transform.position, transform.position) <= JumpInDistence)
                                        {
                                            //移动结束 跳上背上距离自己最近的点
                                            RunOver();
                                            ParentPosition = ParentAvalugg.SetAPositionHaveBergmite(this);
                                            JumpStart(ParentPosition + (Vector2)ParentAvalugg.transform.position);
                                        }
                                        if (FearTarget != Vector2.zero) { FearTarget = Vector2.zero; }
                                    }
                                    else
                                    {
                                        if (Vector2.Distance( (Vector2)player.transform.position , (Vector2)transform.position) >= 1.6f) {
                                            //向玩家移动
                                            rigidbody2D.position += ((Vector2)(player.transform.position - transform.position).normalized * Time.deltaTime * speed);
                                            //设置朝向
                                            Director = _mTool.MainVector2((player.transform.position - transform.position).normalized);
                                            SetDirector(Director);
                                        }
                                    }
                                }
                                else//害怕时向角落移动
                                {
                                    if (FearTarget == Vector2.zero)
                                    {
                                        FearTarget = new Vector2(((Random.Range(0.0f, 1.0f) > 0.5f) ? (ParentPokemonRoom.RoomSize[2] + 1.0f) : (ParentPokemonRoom.RoomSize[3] - 1.0f)), ((Random.Range(0.0f, 1.0f) > 0.5f) ? (ParentPokemonRoom.RoomSize[1] + 1.0f) : (ParentPokemonRoom.RoomSize[0] - 1.0f)));
                                        FearTarget += (Vector2)ParentPokemonRoom.transform.position;
                                        //Debug.Log(FearTarget);
                                    }
                                    if (Vector3.Distance((Vector3)FearTarget, transform.position) >= 1.2f)
                                    {
                                        rigidbody2D.position += ((Vector2)((Vector3)FearTarget - transform.position).normalized * Time.deltaTime * speed * 2.0f);
                                        //设置朝向
                                        Director = _mTool.MainVector2(((Vector3)FearTarget - transform.position).normalized);
                                    }
                                    SetDirector(Director);
                                }
                            }
                            break;
                        case BergmiteState.Jump:
                            //跳上冰岩怪的背
                            JumpTimer += Time.deltaTime;
                            if (JumpTimer >= 0.13333f && JumpTimer < 0.63333f)
                            {
                                rigidbody2D.position += (JumpDirector * Time.deltaTime * JumpDistence);
                            }
                            else if (JumpTimer >= 0.63333f && JumpTimer < 1.13333f)
                            {
                                rigidbody2D.position += (JumpDirector * Time.deltaTime * JumpDistence) + (Vector2.up * Time.deltaTime * 4.0f);
                            }
                            else if (JumpTimer >= 1.13333f && (Vector2)transform.localPosition != ParentPosition + Vector2.up * 2.0f)
                            {
                                transform.localPosition = ParentPosition + Vector2.up * 2.0f;
                            }
                            break;
                        case BergmiteState.IdleInParent:
                            //在冰岩怪的背上发呆
                            /**
                            IdleInParentTimer += Time.deltaTime;
                            if (IdleInParentTimer >= 3.0)
                            {
                                Drop(new Vector2(10.0f, -3.0f));
                            }
                            **/
                            break;
                        case BergmiteState.Drop:
                            //跳下冰岩怪的背
                            DropTimer += Time.deltaTime;
                            if (DropTimer >= 0.13333f && DropTimer < 0.3f)
                            {
                                rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f) - (Vector2.up * Time.deltaTime * 3.0f);
                            }
                            else if (DropTimer >= 0.3f && DropTimer < 0.46666f)
                            {
                                rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f) - (Vector2.up * Time.deltaTime * 3.0f);
                            }
                            else if (DropTimer >= 0.46666f && DropTimer < 0.63333f)
                            {
                                rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f);
                            }
                            else if (DropTimer >= 0.63333f && rigidbody2D.position != DropTarget)
                            {
                                rigidbody2D.position = DropTarget;
                            }
                            break;
                    }
                } 
            }
            else
            {
                if (!isFearDone)
                {
                    Fear(1000.0f, 100.0f);
                }
                switch (NowState)
                {
                    case BergmiteState.Idle:
                        //发呆
                        IdleOver();
                        RunStart();
                        break;
                    case BergmiteState.Run:
                        if (!isCanNotMoveWhenParalysis)
                        {
                            if (FearTarget == Vector2.zero)
                            {
                                FearTarget = new Vector2(((Random.Range(0.0f, 1.0f) > 0.5f) ? (ParentPokemonRoom.RoomSize[2] + 1.0f) : (ParentPokemonRoom.RoomSize[3] - 1.0f)), ((Random.Range(0.0f, 1.0f) > 0.5f) ? (ParentPokemonRoom.RoomSize[1] + 1.0f) : (ParentPokemonRoom.RoomSize[0] - 1.0f)));
                                FearTarget += (Vector2)ParentPokemonRoom.transform.position;
                                //Debug.Log(FearTarget);
                            }
                            if (Vector3.Distance((Vector3)FearTarget, transform.position) >= 1.2f)
                            {
                                rigidbody2D.position += ((Vector2)((Vector3)FearTarget - transform.position).normalized * Time.deltaTime * speed * 2.0f);
                                //设置朝向
                                Director = _mTool.MainVector2(((Vector3)FearTarget - transform.position).normalized);
                            }
                        }
                        SetDirector(Director);
                        break;
                    case BergmiteState.Jump:
                        //跳上冰岩怪的背
                        JumpTimer += Time.deltaTime;
                        if (JumpTimer >= 0.13333f && JumpTimer < 0.63333f)
                        {
                            rigidbody2D.position += (JumpDirector * Time.deltaTime * JumpDistence);
                        }
                        else if (JumpTimer >= 0.63333f && JumpTimer < 1.13333f)
                        {
                            rigidbody2D.position += (JumpDirector * Time.deltaTime * JumpDistence) + (Vector2.up * Time.deltaTime * 4.0f);
                        }
                        else if (JumpTimer >= 1.13333f && (Vector2)transform.localPosition != ParentPosition + Vector2.up * 2.0f)
                        {
                            transform.localPosition = ParentPosition + Vector2.up * 2.0f;
                        }
                        break;
                    case BergmiteState.IdleInParent:
                        //在冰岩怪的背上发呆
                        /**
                        IdleInParentTimer += Time.deltaTime;
                        if (IdleInParentTimer >= 3.0)
                        {
                            Drop(new Vector2(10.0f, -3.0f));
                        }
                        **/
                        break;
                    case BergmiteState.Drop:
                        //跳下冰岩怪的背
                        DropTimer += Time.deltaTime;
                        if (DropTimer >= 0.13333f && DropTimer < 0.3f)
                        {
                            rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f) - (Vector2.up * Time.deltaTime * 3.0f);
                        }
                        else if (DropTimer >= 0.3f && DropTimer < 0.46666f)
                        {
                            rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f) - (Vector2.up * Time.deltaTime * 3.0f);
                        }
                        else if (DropTimer >= 0.46666f && DropTimer < 0.63333f)
                        {
                            rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f);
                        }
                        else if (DropTimer >= 0.63333f && rigidbody2D.position != DropTarget)
                        {
                            rigidbody2D.position = DropTarget;
                        }
                        break;
                }
            }
        }
    }

    public override void DieEvent()
    {
        base.DieEvent();
        Debug.Log("Die" + this.name);
        if (ParentAvalugg != null) { ParentAvalugg.ChildBergmiteDie(this); }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//如果玩家组件丢失，重新获取
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//如果被魅惑，计算魅惑时间
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            if (NowState != BergmiteState.Jump && NowState != BergmiteState.IdleInParent && NowState != BergmiteState.Drop ) { EmptyBeKnock(); } //判定是否被击退
            //根据魅惑情况确实目标位置
            if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (NowState == BergmiteState.Drop)
        {
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 40, 0, 0, PokemonType.TypeEnum.Ice);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 3;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, 40, 0, 0, PokemonType.TypeEnum.Ice);
            }
        }
        else
        {
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//未被魅惑 且与玩家碰撞时
            {
                EmptyTouchHit(other.gameObject);//触发触碰伤害

            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//被魅惑 且与其他敌人碰撞时
            {
                InfatuationEmptyTouchHit(other.gameObject);//触发魅惑后触碰伤害
            }
        }

    }





    //============================共通=================================

    /// <summary>
    /// 设置冰宝的动画机方向
    /// </summary>
    void SetDirector( Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX" , director.x);
        animator.SetFloat("LookY" , director.y);
    }

    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && NowState == BergmiteState.Run )
            {
                //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                SetDirector(_mTool.MainVector2((transform.position - LastPosition)));
                //重置位置
                LastPosition = transform.position;
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    //============================共通=================================








    //============================发呆=================================

    /// <summary>
    /// 苏醒后发呆的时间
    /// </summary>
    static float IDLE_OF_AWAKE = 0.4f;

    /// <summary>
    /// 从背上跳下来后发呆的时间
    /// </summary>
    static float IDLE_OF_DROP = 8.8f;

    /// <summary>
    /// 发呆计时器
    /// </summary>
    float IdleTimer = 0;

    /// <summary>
    /// 发呆的时间
    /// </summary>
    float TimeOfIdle;

    /// <summary>
    /// 发呆开始
    /// </summary>
    public void IdleStart(float IdleTime)
    {
        NowState = BergmiteState.Idle;
        TimeOfIdle = IdleTime;
    }

    /// <summary>
    /// 发呆结束
    /// </summary>
    public void IdleOver()
    {
        IdleTimer = 0.0f;
        TimeOfIdle = 0.0f;
    }

    //============================发呆=================================











    //============================走路=================================

    /// <summary>
    /// 跳跃开始
    /// </summary>
    public void RunStart()
    {
        NowState = BergmiteState.Run;
        //StartFrozenMistPS();
    }


    /// <summary>
    /// 启动冰雾粒子效果
    /// </summary>
    void StartFrozenMistPS()
    {
        FrozenMistPS1.Play();
        FrozenMistPS2.Play();
    }

    /// <summary>
    /// 暂停冰雾粒子效果
    /// </summary>
    void StopFrozenMistPS()
    {
        FrozenMistPS1.Stop();
        FrozenMistPS2.Stop();
    }

    /// <summary>
    /// 跳跃结束
    /// </summary>
    public void RunOver()
    {
        //StopFrozenMistPS();
    }

    //============================走路=================================



















    //============================冰宝跳上冰岩怪的背=================================

    //冰宝距离冰岩怪距离为JumpInDistence时 ， 跳上背
    public static float JumpInDistence = 3.0f;

    /// <summary>
    /// 跳上的目标位置
    /// </summary>
    Vector2 JumpTarget;

    /// <summary>
    /// 跳上的角度
    /// </summary>
    Vector2 JumpDirector;
    /// <summary>
    /// 跳上的距离
    /// </summary>
    float JumpDistence;

    /// <summary>
    /// 跳上计时器
    /// </summary>
    float JumpTimer = 0;

    /// <summary>
    /// 跳上开始
    /// </summary>
    public void JumpStart(Vector2 Target)
    {
        //动画机开始跳跃
        animator.SetTrigger("Jump");
        //状态机跳跃
        NowState = BergmiteState.Jump;
        //跳上位置，角度和距离
        JumpTarget = Target;
        JumpDirector = (JumpTarget - (Vector2)transform.position).normalized;
        JumpDistence = Vector2.Distance(JumpTarget, (Vector2)transform.position);
        if (ParentAvalugg != null) {
            //冰宝的父对象变为冰岩怪
            transform.parent = ParentAvalugg.ChildBergmiteHome.transform;
            //忽略碰撞
            ParentAvalugg.IgnoreOneChildCollision(this);
            //设置冰雾
            ParentAvalugg.SetFrozenMistByChild();
        }
    }

    /// <summary>
    /// 跳上结束
    /// </summary>
    public void JumpOver()
    {
        JumpTimer = 0.0f;
        IdleInParentStart();
    }

    //============================冰宝跳上冰岩怪的背=================================






















    //============================在冰岩怪背上发呆=================================

    /// <summary>
    /// 在冰岩怪背上发呆计时器
    /// </summary>
    float IdleInParentTimer = 0;

    /// <summary>
    /// 在冰岩怪背上发呆开始
    /// </summary>
    public void IdleInParentStart()
    {
        NowState = BergmiteState.IdleInParent;
        SetDirector(Vector2.down);
    }

    /// <summary>
    /// 在冰岩怪背上发呆结束 跳下冰岩怪的背
    /// </summary>
    public void IdleInParentOver()
    {
        IdleInParentTimer = 0.0f;
    }

    //============================在冰岩怪背上发呆=================================















    //============================冰宝跳下冰岩怪的背=================================

    /// <summary>
    /// 跳下的目标位置
    /// </summary>
    Vector2 DropTarget ;

    /// <summary>
    /// 跳下的角度
    /// </summary>
    Vector2 DropDirector;

    /// <summary>
    /// 跳下的距离
    /// </summary>
    float DropDistence;

    /// <summary>
    /// 跳下计时器
    /// </summary>
    float DropTimer = 0;

    /// <summary>
    /// LunchCount 发射模式
    /// </summary>
    public int LunchCount;

    /// <summary>
    /// 跳下开始
    /// </summary>
    public void DropStart(Vector2 Target , int lunchCount)
    {

        //动画机开始跳下
        animator.SetTrigger("Drop");
        //状态机跳下
        NowState = BergmiteState.Drop;
        //跳下位置，角度和距离
        DropTarget = Target;
        DropDirector = (DropTarget - JumpTarget).normalized;
        DropDistence = Vector2.Distance(DropTarget, JumpTarget);
        //冰宝的父对象变为冰岩怪
        if (ParentAvalugg != null) {
            transform.parent = ParentAvalugg.transform.parent;
        }
        //设置冰雾
        //ParentAvalugg.SetFrozenMistByChild();
        LunchCount = lunchCount;
    }


    /// <summary>
    /// 冰宝发射
    /// </summary>
    /// <param name="Target">发射的目标位置</param>
    public void Drop(Vector2 TargetPos, int lunchCount)
    {
        //TargetPos = new Vector2(10.0f, 4.0f);
        IdleInParentOver();
        SetDirector((TargetPos - (Vector2)transform.position).normalized);
        DropStart(TargetPos, lunchCount);
        if (ParentAvalugg != null) { ParentAvalugg.SetAPositionDontHaveBergmite(ParentPosition); }
    }

    /// <summary>
    /// 摔倒地面
    /// </summary>
    public void DropGround()
    {
        LunchIceShard();
        
    }

    /// <summary>
    /// 发射冰砾
    /// </summary>
    public void LunchIceShard()
    {
        CameraShake(0.3f, 2.5f, true);
        if (!isFearDone && !isSilence && !isSleepDone)
        {
            Debug.Log(Director);
            //Debug.Log(isEmptyConfusionDone);
            float alpha = 0;
            if (LunchCount % 2 == 1) { alpha = 90; }
            
            if (!isEmptyConfusionDone)
            {
                for (int i = 0; i < 6; i++)
                {
                    
                    BergmiteIceShard e1 = Instantiate(IceShard, transform.position + Quaternion.AngleAxis(alpha + i * 60 , Vector3.forward) * Vector3.right * 0.75f, Quaternion.Euler(0, 0, alpha + i * 60));
                    e1.empty = this;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    BergmiteIceShard e1 = Instantiate(IceShard, transform.position + Quaternion.AngleAxis(alpha + i * 90, Vector3.forward) * Vector3.right * 0.75f, Quaternion.Euler(0, 0, alpha + i * 90));
                    e1.empty = this;
                }
            }

        }
    }

    /// <summary>
    /// 跳下结束
    /// </summary>
    public void DropOver()
    {
        DropTimer = 0.0f;
        IdleStart(IDLE_OF_DROP);
        //忽略碰撞
        if (ParentAvalugg != null) {
            ParentAvalugg.ResetOneChildCollision(this);
        }
        LunchCount = 0;
    }

    //============================冰宝跳下冰岩怪的背=================================



}
