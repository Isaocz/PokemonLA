using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//迷你冰-》无多多冰 --》细雪-》冲刺-》发呆-》
//      -》有多多冰
//      -》有双倍多多冰

public class Vanillite : Empty
{

    //迷你冰的父辈的状态
    public enum VanilaFamilyState
    {
        Single,       //没有父辈
        Father,       //以多多冰为父辈
        Grandfather,  //以双双多多冰为父辈
    }
    public VanilaFamilyState FamilyState;

    //迷你冰没有父辈时的状态机
    public enum SingleState
    {
        Idle,     //发呆
        Move,     //移动
        Blow,     //吹兵器
        Rush,     //冲刺
    }
    public SingleState singleState;

    //迷你冰以多多冰为父辈时的状态机
    public enum FatherState
    {
        Idle,     //发呆
        Lunch,    //被发射
        Back,     //返回母体
        Surround, //环绕
    }
    public FatherState fatherState;

    //迷你冰以双倍多多冰为父辈时的状态机
    public enum GrandfatherState
    {
    }
    public GrandfatherState grandfatherState;


    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;

    /// <summary>
    /// 敌人的目标的坐标
    /// </summary>
    Vector2 TargetPosition;

    Vector3 LastPosition;//计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行

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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint)/* * 1.2f */;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint)/* * 1.2f */;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //启动计算方向携程
        StartCoroutine(CheckLook());


        //初始化状态机（无父辈）
        ChildLeaveHome();//保持单身
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Idle;
        IdleStart_Single(TIME_START_SINGLE);

        //初始化方向
        Director = new Vector2(-1,-1);
        SetDirector(Director);
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

            //特性雪隐 移速增加
            float WeatherSpeedAlpha = (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
            switch (FamilyState)
            {
                case VanilaFamilyState.Single:
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        animator.ResetTrigger("Sleep");
                        switch (singleState)
                        {
                            case SingleState.Idle:
                                IdleTimer_Single -= Time.deltaTime;
                                //发呆结束后或恐惧时转进移动状态
                                if (IdleTimer_Single <= 0 || isFearDone)
                                {
                                    IdleOver_Single();
                                    MoveStart_Single();
                                }
                                break;
                            case SingleState.Move:
                                //设置方向
                                Vector2 MoveDirector = (TargetPosition - (Vector2)transform.position).normalized;
                                //恐惧时移动
                                if (isFearDone)
                                {
                                    //设置方向
                                    MoveDirector = -MoveDirector;
                                    //移动
                                    rigidbody2D.position = new Vector2(
                                        Mathf.Clamp(rigidbody2D.position.x
                                            + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha * 2.0f,       //方向*速度
                                        ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //最小值
                                        ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//最大值
                                        Mathf.Clamp(rigidbody2D.position.y
                                            + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha * 2.0f,        //方向*速度 
                                        ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //最小值
                                        ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//最大值
                                    Director = _mTool.TiltMainVector2(MoveDirector);
                                    SetDirector(Director);
                                }
                                else
                                {
                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE)
                                    {
                                        //移动
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha,       //方向*速度
                                            ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //最小值
                                            ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//最大值
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha,        //方向*速度 
                                            ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //最小值
                                            ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//最大值
                                        Director = _mTool.TiltMainVector2(MoveDirector);
                                        SetDirector(Director);
                                    }
                                    else
                                    {
                                        MoveOver_Single();
                                        RushStart_Single(RUSH_TIME);
                                    }
                                }
                                break;
                            case SingleState.Rush:
                                RushTimer_Single -= Time.deltaTime;
                                if (RushTimer_Single <= 0 || isFearDone)
                                {
                                    RushOver_Single();
                                    BlowStart_Single();
                                }
                                else
                                {
                                    rigidbody2D.position = new Vector2(
                                        Mathf.Clamp(rigidbody2D.position.x
                                            + (float)RushDirector.x * Time.deltaTime * speed * 6.5f * WeatherSpeedAlpha,       //方向*速度
                                        ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //最小值
                                        ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//最大值
                                        Mathf.Clamp(rigidbody2D.position.y
                                            + (float)RushDirector.y * Time.deltaTime * speed * 6.5f * WeatherSpeedAlpha,        //方向*速度 
                                        ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //最小值
                                        ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//最大值
                                }
                                break;
                            case SingleState.Blow:
                                break;
                        }
                    }
                    if ((isSleepDone || isSilence) && singleState != SingleState.Idle)
                    {
                        animator.SetTrigger("Sleep");
                        Debug.Log("Sleep");
                        switch (singleState)
                        {
                            case SingleState.Move:
                                MoveOver_Single();
                                IdleStart_Single(0.2f);
                                break;
                            case SingleState.Blow:
                                BlowOver_Single();
                                IdleStart_Single(0.2f);
                                break;
                            case SingleState.Rush:
                                RushOver_Single();
                                IdleStart_Single(0.2f);
                                break;
                        }
                    }
                    break;
                case VanilaFamilyState.Father:
                    if (ParentEmptyByChild != null) {
                        //父辈的位置
                        Vector2 ParentPosition = ((Vector2)ParentEmptyByChild.transform.position);
                        switch (fatherState)
                        {
                            case FatherState.Idle:
                                IdleTimer_Father -= Time.deltaTime;
                                //发呆结束后或恐惧时转进移动状态
                                if (IdleTimer_Father <= 0)
                                {
                                    IdleOver_Father();
                                    BackStart_Father(0);
                                }
                                break;
                            case FatherState.Back:
                                //距离父辈的距离大于一定时接近父辈
                                if (Vector2.Distance(ParentPosition, (Vector2)transform.position) > SURROUND_DISTENCE) {
                                    //设置方向
                                    Vector2 MoveDirector = (ParentPosition - (Vector2)transform.position).normalized;
                                    //不恐惧时移动
                                    if (!isFearDone)
                                    {
                                        //移动
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha * 1.3f,       //方向*速度
                                            ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //最小值
                                            ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//最大值
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha * 1.3f,        //方向*速度 
                                            ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //最小值
                                            ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//最大值
                                        Director = _mTool.TiltMainVector2(MoveDirector);
                                        SetDirector(Director);
                                    }
                                }
                                //小于等于时进入迅游状态
                                else
                                {
                                    BackOver_Father();
                                    SurroundStart_Father(0.0f);
                                }
                                break;
                            case FatherState.Surround:
                                //环绕
                                Surround(surroundRotationSpeed);
                                break;
                            case FatherState.Lunch:
                                break;
                        }
                    }
                    break;
                case VanilaFamilyState.Grandfather:
                    switch (grandfatherState)
                    {
                        default: break;
                    }
                    break;
            }
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//如果玩家组件丢失，重新获取
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//如果被魅惑，计算魅惑时间
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            //Debug.Log(NeedSearchParent);
            //寻找父辈
            if (NeedSearchParent)
            {
                NeedSearchParent = false;
                SearchVanilliteParent();
            }

            //仅当单人巡逻状态时 判定是否被击退 
            if (FamilyState == VanilaFamilyState.Single) { EmptyBeKnock(); }
            
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
        //无父辈状态 冲刺
        if (FamilyState == VanilaFamilyState.Single && singleState == SingleState.Rush)
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











    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■
    /// <summary>
    /// 设置多多冰的动画机方向
    /// </summary>
    void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && 
                (FamilyState == VanilaFamilyState.Single && singleState == SingleState.Move)//TODO
             )
            {
                //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                //SetDirector(_mTool.MainVector2((transform.position - LastPosition)));
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

    /// <summary>
    /// 是否需要寻找父辈
    /// </summary>
    bool NeedSearchParent = false;

    /// <summary>
    /// 寻找父亲
    /// </summary>
    public void SearchVanilliteParent()
    {
        Vaniluxe grandfather = SearchParentByDistence<Vaniluxe>();
        //有祖父
        if (grandfather != null)
        {
            Debug.Log("GrandFather");
            ChildBackHome(grandfather);//回祖父家
            FamilyState = VanilaFamilyState.Grandfather;
        }
        //无祖父
        else
        {
            Vanillish father = SearchParentByDistence<Vanillish>();
            //有父
            if (father != null)
            {
                Debug.Log("Father");
                ChildBackHome(father);//回父亲家
                FamilyState = VanilaFamilyState.Single;
                fatherState = FatherState.Idle;
                IdleStart_Father(TIME_START_FATHER);
            }
            //无父
            else
            {
                Debug.Log("Single");
                ChildLeaveHome();//保持单身
                FamilyState = VanilaFamilyState.Single;
                singleState = SingleState.Idle;
                IdleStart_Single(TIME_START_SINGLE);
            }
        }
    }


    
    //复写出家
    public override void ChildLeaveHome()
    {
        base.ChildLeaveHome();
        //恢复碰撞体
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.ResetOneChildCollision(this);
        }
    }

    //复写回家
    public override void ChildBackHome(Empty parent)
    {
        base.ChildBackHome(parent);
        //忽略碰撞体
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.IgnoreOneChildCollision(this);
        }
    }


    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■






























    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■无父辈■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■





    //========================发呆=================================

    //开始后的冷却时间
    static float TIME_START_SINGLE = 0.5f;
    //冲刺后的冷却时间
    static float TIME_AFTER_BLOW_SINGLE = 4.3f;

    /// <summary>
    /// 发呆计时器
    /// </summary>
    float IdleTimer_Single = 0;

    /// <summary>
    /// 发呆开始
    /// </summary>
    public void IdleStart_Single(float Timer)
    {
        IdleTimer_Single = Timer;
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Idle;
    }

    /// <summary>
    /// 发呆结束
    /// </summary>
    public void IdleOver_Single()
    {
        IdleTimer_Single = 0.0f;
    }
    //========================发呆=================================










    //========================移动=================================

    //停止移动的距离
    static float MOVE_STOP_DISTENCE = 3.8f;

    /// <summary>
    /// 移动计时器
    /// </summary>
    float MoveTimer_Single = 0;

    /// <summary>
    /// 移动开始
    /// </summary>
    public void MoveStart_Single()
    {
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Move;
    }

    /// <summary>
    /// 移动结束
    /// </summary>
    public void MoveOver_Single()
    {
        MoveTimer_Single = 0.0f;
    }
    //========================移动=================================







    //========================冲刺=================================

    //冲刺的时间
    static float RUSH_TIME = 0.18f;

    /// <summary>
    /// 冲刺计时器
    /// </summary>
    float RushTimer_Single = 0;

    Vector2 RushDirector = Vector2.zero;

    /// <summary>
    /// 冲刺开始
    /// </summary>
    public void RushStart_Single(float time)
    {
        RushTimer_Single = time;
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Rush;
        StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        RushDirector = (TargetPosition - (Vector2)transform.position).normalized;
        SetDirector(_mTool.TiltMainVector2(RushDirector));
    }

    /// <summary>
    /// 冲刺结束
    /// </summary>
    public void RushOver_Single()
    {
        RushTimer_Single = 0.0f;
        StopShadowCoroutine();
        RushDirector = Vector2.zero;
    }
    //========================冲刺=================================





    //========================吹雪=================================

    /// <summary>
    /// 细雪
    /// </summary>
    public VanillitePodesSnow PodesSnow;

    /// <summary>
    /// 吹雪计时器
    /// </summary>
    float BlowTimer_Single = 0;

    /// <summary>
    /// 吹雪开始
    /// </summary>
    public void BlowStart_Single()
    {
        animator.SetTrigger("Blow");
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Blow;
    }

    //发射细雪
    public void LunchPodesSnow()
    {
        if (!isFearDone) {
            Vector2 LunchDir = (TargetPosition - (Vector2)transform.position).normalized;
            if (isEmptyConfusionDone)
            {
                LunchDir = Quaternion.AngleAxis( ((Random.Range(0,1)>0.5f)? -60.0f : 60.0f ), Vector3.forward) * LunchDir;
            }
            VanillitePodesSnow ps = Instantiate(PodesSnow, transform.position + (Vector3)LunchDir * 0.7f, Quaternion.Euler(0, 0, _mTool.Angle_360Y((Vector3)LunchDir, Vector3.right)));
            SetDirector(_mTool.TiltMainVector2(LunchDir));
            ps.empty = this;
        }
    }

    /// <summary>
    /// 吹雪结束
    /// </summary>
    public void BlowOver_Single()
    {
        BlowTimer_Single = 0.0f;
        IdleStart_Single(TIME_AFTER_BLOW_SINGLE);
    }
    //========================吹雪=================================


    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■无父辈■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■























    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■以多多冰为父辈■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■



    //========================发呆=================================

    //开始后的冷却时间
    static float TIME_START_FATHER = 0.0f;

    /// <summary>
    /// 发呆计时器
    /// </summary>
    float IdleTimer_Father = 0;

    /// <summary>
    /// 发呆开始
    /// </summary>
    public void IdleStart_Father(float Timer)
    {
        IdleTimer_Father = Timer;
        FamilyState = VanilaFamilyState.Father;
        fatherState = FatherState.Idle;
    }

    /// <summary>
    /// 发呆结束
    /// </summary>
    public void IdleOver_Father()
    {
        IdleTimer_Father = 0.0f;
    }
    //========================发呆=================================











    //========================返回母体=================================

    /// <summary>
    /// 返回母体计时器
    /// </summary>
    float BackTimer_Father = 0;

    /// <summary>
    /// 返回母体开始
    /// </summary>
    public void BackStart_Father(float Timer)
    {
        BackTimer_Father = Timer;
        FamilyState = VanilaFamilyState.Father;
        fatherState = FatherState.Back;
    }

    /// <summary>
    /// 返回母体结束
    /// </summary>
    public void BackOver_Father()
    {
        BackTimer_Father = 0.0f;
    }
    //========================返回母体=================================











    //========================环绕=================================

    //环绕距离
    static float SURROUND_DISTENCE = 2.0f;

 
    public float SurroundRotationSpeed
    {
        get { return surroundRotationSpeed; }
        set { surroundRotationSpeed = value; }
    }
    float surroundRotationSpeed = 35.0f;

    /// <summary>
    /// 环绕计时器
    /// </summary>
    float SurroundTimer_Father = 0;

    /// <summary>
    /// 环绕开始
    /// </summary>
    public void SurroundStart_Father(float Timer)
    {
        SurroundTimer_Father = Timer;
        FamilyState = VanilaFamilyState.Father;
        fatherState = FatherState.Surround;
        //静止刚体
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Surround( float RotationSpeed )
    {
        //父辈的位置
        Vector2 ParentPosition = ((Vector2)ParentEmptyByChild.transform.position);
        //环绕移动
        //当前角度
        float NowAngle = _mTool.Angle_360Y(((Vector2)transform.position - ParentPosition).normalized, Vector2.right);
        //Debug.Log("Vector2" + ((Vector2)transform.position - ParentPosition).normalized);
        //Debug.Log("NowAngle" + NowAngle);
        float PlusAngle = (NowAngle + RotationSpeed * Time.deltaTime) % (360.0f);
        //Debug.Log("PlusAngle" + PlusAngle);
        //Debug.Log("Xcos" + Mathf.Cos(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE);
        //Debug.Log("Ysin" + Mathf.Sin(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE);
        transform.position = new Vector2(
                   Mathf.Clamp(ParentPosition.x
                       + Mathf.Cos(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE,       //方向*速度
                   ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //最小值
                   ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//最大值
                   Mathf.Clamp(ParentPosition.y
                       + Mathf.Sin(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE,        //方向*速度 
                   ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //最小值
                   ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//最大值
        //设置方向
        SetDirector(_mTool.TiltMainVector2((Vector2)transform.position - ParentPosition));
    }

    /// <summary>
    /// 环绕结束
    /// </summary>
    public void SurroundOver_Father()
    {
        SurroundTimer_Father = 0.0f;
        //恢复刚体
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
    //========================环绕=================================











    //========================被发射=================================

    /// <summary>
    /// 被发射计时器
    /// </summary>
    float LunchTimer_Father = 0;

    /// <summary>
    /// 被发射开始
    /// </summary>
    public void LunchStart_Father(float Timer)
    {
        LunchTimer_Father = Timer;
        FamilyState = VanilaFamilyState.Father;
        fatherState = FatherState.Lunch;
    }

    /// <summary>
    /// 被发射结束
    /// </summary>
    public void LunchOver_Father()
    {
        LunchTimer_Father = 0.0f;
    }
    //========================被发射=================================






    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■以多多冰为父辈■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■






















    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■以双倍多多冰为父辈■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■





    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■以双倍多多冰为父辈■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

}
