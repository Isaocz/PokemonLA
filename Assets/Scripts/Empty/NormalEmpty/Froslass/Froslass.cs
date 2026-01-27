using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Froslass : Empty
{
    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;

    /**
    /// <summary>
    /// 计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行
    /// </summary>
    Vector3 LastPosition;
    **/

    /// <summary>
    /// 敌人的目标的坐标
    /// </summary>
    public Vector2 TARGET_POSITION
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }
    Vector2 TargetPosition;


    /// <summary>
    /// 光源的动画状态机
    /// </summary>
    public Animator LightAnimator;

    


    //==============================状态机枚举===================================
    
    /// <summary>
    /// 主状态
    /// </summary>
    enum MainState
    {
        Idle,           //发呆
        UseAurora,      //使用极光慕
        Move,           //瞬间移动
        UseShadowBall,  //释放影子球
    }
    MainState NowState;

    /**
    /// <summary>
    /// 副状态
    /// </summary>
    enum SubState
    {
        //TODO
    }
    SubState TODO;



    /// <summary>
    /// 状态映射关系
    /// </summary>
    private static Dictionary<MainState, SubState[]> StateMap = new()
    {
        { MainState.TODO, new[] { SubState.TODO,... },
        { MainState.TODO, new[] { SubState.TODO,... },
        ....
    };
    **/
    //==============================状态机枚举===================================



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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint)/* * 1.2f */;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint)/* * 1.2f */;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //启动计算方向携程
        //StartCoroutine(CheckLook());

        //开启初始冷却
        IdleStart(TIME_IDLE_START);
        //设置初始方向
        SetDirector(new Vector2(-1,-1));
        LightTurnOFF();

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

            //InsertStateMechineSwitch


            //■■开始判断状态机 当处于冰冻 睡眠 致盲 麻痹状态时状态机停运
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO状态机停运的额外条件 */
            {
                animator.ResetTrigger("Sleep");
                switch (NowState)
                {
                    //发呆状态
                    case MainState.Idle:
                        IdleTimer -= Time.deltaTime;//发呆计时器时间减少
                        if ( IdleTimer <= 0 )         //计时器时间到时间，结束发呆状态
                        {
                            IdleOver();
                            //未使用极光慕
                            if (AuroraObj == null)
                            {
                                UseAuroraStart();
                            }
                            //已经使用极光慕
                            else
                            {
                                MoveStart(TIME_MOVE_WHEN_TARGET_IN_AURORA);
                            }
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                    //使用极光慕状态
                    case MainState.UseAurora:
                        //UseAuroraTimer -= Time.deltaTime;//使用极光慕计时器时间减少
                        //if ( UseAuroraTimer <= 0 )         //计时器时间到时间，结束使用极光慕状态
                        //{
                        //   UseAuroraOver();
                            //TODO添加下一个状态的开始方法
                        //}
                        break;
                    //瞬间移动状态
                    case MainState.Move:
                        MoveTimer -= Time.deltaTime;//瞬间移动计时器时间减少
                        if ( MoveTimer <= 0 )         //计时器时间到时间，结束瞬间移动状态
                        {
                            animator.SetBool("Move", false);
                            LightTurnON();
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                    //释放影子球状态
                    case MainState.UseShadowBall:
                        UseShadowBallTimer -= Time.deltaTime;//释放影子球计时器时间减少
                        if ( UseShadowBallTimer <= 0 )         //计时器时间到时间，结束释放影子球状态
                        {
                            UseShadowBallOver();
                            
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                }
            }

            if ((isSleepDone || isSilence) && NowState != MainState.Idle)
            {
                animator.SetTrigger("Sleep");
                switch (NowState)
                {
                    case MainState.UseAurora:
                        UseAuroraOver();
                        break;
                    case MainState.Move:
                        animator.SetBool("Move", false);
                        LightTurnON();
                        break;
                    case MainState.UseShadowBall:
                        UseShadowBallOver();
                        break;
                }
                animator.SetBool("Move", false);
                IdleStart(TIME_IDLE_START);
            }
            //■■结束判断状态机




        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//如果玩家组件丢失，重新获取
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//如果被魅惑，计算魅惑时间
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            EmptyBeKnock();//判定是否被击退


            //根据魅惑情况确实目标位置
            Transform InfatuationTarget = InfatuationForDistanceEmpty();
            if (!isEmptyInfatuationDone || (ParentPokemonRoom.GetEmptyList().Count + ParentPokemonRoom.GetEmptyCloneList().Count) <= 1 || InfatuationTarget == null)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationTarget.transform.position; }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
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





    
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■
    /// <summary>
    /// 设置敌人的动画机方向
    /// </summary>
    void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    /**
    /// <summary>
    /// 检查是否在移动和朝向
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && 更多条件 )
            {
                //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                Director = _mTool.MainVector2((transform.position - LastPosition));
                animator.SetFloat("LookX", Director.x);
                animator.SetFloat("LookY", Director.y);
                //Debug.Log(Director);
                //重置位置
                LastPosition = transform.position;
            }
            yield return new WaitForSeconds(0.1f);
        }

    }
    **/


    /// <summary>
    /// 刚体敌人在房间限制内移动
    /// </summary>
    /// <param name="dir">移动方向</param>
    /// <param name="Speed">移动速度</param>
    /// <param name="SpeedAlpha">移动速度的加成系数（乘算）</param>
    /// <param name="RoomUpAlpha">房间上边界的限制系数</param>
    /// <param name="RoomDownAlpha">房间下边界的限制系数</param>
    /// <param name="RoomLeftAlpha">房间右边界的限制系数</param>
    /// <param name="RoomRightAlpha">房间左边界的限制系数</param>
    public void MoveBySpeedAndDir(Vector2 dir, float Speed, float SpeedAlpha, float RoomUpAlpha, float RoomDownAlpha, float RoomLeftAlpha, float RoomRightAlpha)
    {

        rigidbody2D.position = new Vector2(
            Mathf.Clamp(rigidbody2D.position.x
                + (float)dir.x * Time.deltaTime * Speed * SpeedAlpha,                    //方向*速度
            ParentPokemonRoom.RoomSize[2] - RoomLeftAlpha + transform.parent.position.x, //最小值
            ParentPokemonRoom.RoomSize[3] + RoomRightAlpha + transform.parent.position.x),//最大值
            Mathf.Clamp(rigidbody2D.position.y
                + (float)dir.y * Time.deltaTime * Speed * SpeedAlpha,                     //方向*速度 
            ParentPokemonRoom.RoomSize[1] - RoomDownAlpha + transform.parent.position.y,  //最小值
            ParentPokemonRoom.RoomSize[0] + RoomUpAlpha + transform.parent.position.y));//最大值
    }


    /// <summary>
    /// 死亡时移除极光慕
    /// </summary>
    public override void DieEvent()
    {
        base.DieEvent();
        RemoveAurora();
        LightTurnOFF();
        foreach (FroslassCloneBody b in CloneBodyList)
        {
            b.animator.SetTrigger("Over");
        }
    }

    private void OnDestroy()
    {
        RemoveAurora();
    }


    /// <summary>
    /// 打开光源
    /// </summary>
    public void LightTurnON()
    {
        LightAnimator.SetBool("ON" , true);
    }


    /// <summary>
    /// 关闭光源
    /// </summary>
    public void LightTurnOFF()
    {
        LightAnimator.SetBool("ON", false);
    }


    //InsertSubStateChange
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■














    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //=========================发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_START = 0.5f; //TODO需修改时间

    //使用极光慕后的冷却时间
    static float TIME_IDLE_USEAURORA = 1.8f; //TODO需修改时间


    //瞬间移动后的冷却时间
    static float TIME_IDLE_MOVE = 1.1f; //TODO需修改时间


    //释放影子球后的冷却时间
    static float TIME_IDLE_USESHADOWBALL = 0.3f; //TODO需修改时间

    //释放多次影子球后的长冷却时间
    static float TIME_IDLE_USESHADOWBALL_LONG_TIME = 5.5f; //TODO需修改时间

    //畏缩时的移动冷却
    static float TIME_IDLE_FEARMOVE = 3.5f; //TODO需修改时间


    /// <summary>
    /// 发呆计时器
    /// <summary>
    float IdleTimer = 0;

    /// <summary>
    /// 发呆开始
    /// <summary>
    public void IdleStart(float Timer)
    {
        IdleTimer = Timer;
        NowState = MainState.Idle;
    }

    /// <summary>
    /// 发呆结束
    /// <summary>
    public void IdleOver()
    {
        IdleTimer = 0;
    }


    //=========================发呆============================






    //=========================使用极光慕============================


    /// <summary>
    /// 极光慕预制体；
    /// </summary>
    public FroslassAurora AuroraPrefabs;

    /// <summary>
    /// 极光慕半径
    /// </summary>
    public static float AuroraRadius = 6.8f;

    /// <summary>
    /// 极光慕实体
    /// </summary>
    FroslassAurora AuroraObj;

    /// <summary>
    /// 极光慕中心点
    /// </summary>
    Vector2 AuroraCenter;




    /// <summary>
    /// 使用极光慕计时器
    /// <summary>
    //float UseAuroraTimer = 0;

    /// <summary>
    /// 使用极光慕开始
    /// <summary>
    public void UseAuroraStart()
    {
        //UseAuroraTimer = Timer;
        NowState = MainState.UseAurora;
        animator.SetTrigger("UseAurora");
    }

    /// <summary>
    /// 使用极光慕结束
    /// <summary>
    public void UseAuroraOver()
    {
        //UseAuroraTimer = 0;
        IdleStart(TIME_IDLE_USEAURORA);
    }

    /// <summary>
    /// 发射极光慕
    /// </summary>
    public void LunchAurora()
    {
        AuroraObj = Instantiate(AuroraPrefabs, transform.position, Quaternion.identity);
        AuroraCenter = transform.position;
        //变为天黑
        NightMaskCotrollor.GlobalNight.ChangeToNight();
        LightTurnON();
    }

    public void RemoveAurora()
    {
        if (AuroraObj != null)
        {
            AuroraObj.AuroraOver();
            AuroraCenter = Vector2.zero;
            //变为天黑
            NightMaskCotrollor.GlobalNight.ChangeToDay();
        }
    }

    


    //=========================使用极光慕============================






    //=========================瞬间移动============================

    /// <summary>
    /// 玩家在极光慕内时的移动时间
    /// </summary>
    public static float TIME_MOVE_WHEN_TARGET_IN_AURORA = 0.75f;

    /// <summary>
    /// 玩家在极光慕外时的移动时间
    /// </summary>
    public static float TIME_MOVE_WHEN_TARGET_NOT_IN_AURORA = 1.25f;

    /// <summary>
    /// 目标是否在极光慕内
    /// </summary>
    public bool isTargetInAurora;

    /// <summary>
    /// 分身
    /// </summary>
    public FroslassCloneBody CloneBody;

    public List<FroslassCloneBody> CloneBodyList = new List<FroslassCloneBody> { };

    /// <summary>
    /// 瞬间移动计时器
    /// <summary>
    float MoveTimer = 0;

    /// <summary>
    /// 瞬间移动开始
    /// <summary>
    public void MoveStart(float Timer)
    {
        MoveTimer = Timer;
        NowState = MainState.Move;
        animator.SetBool("Move" , true);
        LightTurnOFF();
    }

    /// <summary>
    /// 瞬间移动结束
    /// <summary>
    public void MoveOver()
    {
        MoveTimer = 0;
        if (!isFearDone) {
            UseShadowBallStart(TIME_SHADOWBALL_WHEN_TARGET_IN_AURORA);
        }
        else
        {
            IdleStart(TIME_IDLE_FEARMOVE);
        }
        //IdleStart(TIME_IDLE_MOVE);
    }



    /// <summary>
    /// 移动 玩家在极光慕范围内时移动到玩家背后
    ///      玩家在极光慕范围外时移动到极光慕边缘
    /// </summary>
    public void Move()
    {
        if (Vector2.Distance(TargetPosition , AuroraCenter) <= AuroraRadius)
        {
            isTargetInAurora = true;
        }
        else
        {
            isTargetInAurora = false;
        }

        List<Vector2> MoveTarget = new List<Vector2> { };
        Vector2 NowPosition = transform.position;


        float PlayerNotInAuroraDeflectionAngle = 10.0f;

        if (!isFearDone) {
            //玩家在极光慕范围内时移动到玩家背后
            if (isTargetInAurora)
            {
                float alpha = (isEmptyConfusionDone)? 3.6f : 2.6f;
                Vector2 v1 = TargetPosition + (Vector2)(Quaternion.AngleAxis(0.0f, Vector3.forward) * (TargetPosition - NowPosition).normalized * alpha);
                Vector2 v2 = TargetPosition + (Vector2)(Quaternion.AngleAxis(90.0f, Vector3.forward) * (TargetPosition - NowPosition).normalized * alpha);
                Vector2 v3 = TargetPosition + (Vector2)(Quaternion.AngleAxis(180.0f, Vector3.forward) * (TargetPosition - NowPosition).normalized * alpha);
                Vector2 v4 = TargetPosition + (Vector2)(Quaternion.AngleAxis(270.0f, Vector3.forward) * (TargetPosition - NowPosition).normalized * alpha);
                if (!isThisPointInRoom(v1) && !isThisPointInRoom(v2) && !isThisPointInRoom(v3) && !isThisPointInRoom(v4))
                {
                    Vector2 v5 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(30.0f, Vector3.forward) * Vector2.right * 1.5f);
                    Vector2 v6 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(150.0f, Vector3.forward) * Vector2.right * 1.5f);
                    Vector2 v7 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(270.0f, Vector3.forward) * Vector2.right * 1.5f);
                    MoveTarget.Add(v5);
                    MoveTarget.Add(v6);
                    MoveTarget.Add(v7);
                }
                else
                {
                    MoveTarget.Add(v1);
                    MoveTarget.Add(v2);
                    MoveTarget.Add(v3);
                    MoveTarget.Add(v4);
                }
            }
            //玩家在极光慕范围外时移动到极光慕边缘
            else
            {
                if (isEmptyConfusionDone) { PlayerNotInAuroraDeflectionAngle *= 1.4f; }
                Vector2 v1 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(PlayerNotInAuroraDeflectionAngle * 1.0f, Vector3.forward) * (TargetPosition - AuroraCenter).normalized * AuroraRadius);
                Vector2 v2 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(PlayerNotInAuroraDeflectionAngle * -1.0f, Vector3.forward) * (TargetPosition - AuroraCenter).normalized * AuroraRadius);
                Vector2 v3 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(PlayerNotInAuroraDeflectionAngle * 3.0f, Vector3.forward) * (TargetPosition - AuroraCenter).normalized * AuroraRadius);
                Vector2 v4 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(PlayerNotInAuroraDeflectionAngle * -3.0f, Vector3.forward) * (TargetPosition - AuroraCenter).normalized * AuroraRadius);
                if (!isThisPointInRoom(v1) && !isThisPointInRoom(v2) && !isThisPointInRoom(v3) && !isThisPointInRoom(v4))
                {
                    Vector2 v5 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(30.0f, Vector3.forward) * Vector2.right * 1.5f);
                    Vector2 v6 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(150.0f, Vector3.forward) * Vector2.right * 1.5f);
                    Vector2 v7 = AuroraCenter + (Vector2)(Quaternion.AngleAxis(270.0f, Vector3.forward) * Vector2.right * 1.5f);
                    MoveTarget.Add(v5);
                    MoveTarget.Add(v6);
                    MoveTarget.Add(v7);
                }
                else
                {
                    MoveTarget.Add(v1);
                    MoveTarget.Add(v2);
                    MoveTarget.Add(v3);
                    MoveTarget.Add(v4);
                }
            }

            int m = Random.Range(0, MoveTarget.Count);

            //排空
            _mTool.RemoveNullInList<FroslassCloneBody>(CloneBodyList);
            //移动 生成替身
            for (int i = 0; i < MoveTarget.Count; i++)
            {
                MoveTarget[i] = new Vector2(
                        Mathf.Clamp(MoveTarget[i].x, ParentPokemonRoom.EmptyFile().transform.position.x + ParentPokemonRoom.RoomSize[2], ParentPokemonRoom.EmptyFile().transform.position.x + ParentPokemonRoom.RoomSize[3]),
                        Mathf.Clamp(MoveTarget[i].y, ParentPokemonRoom.EmptyFile().transform.position.x + ParentPokemonRoom.RoomSize[1], ParentPokemonRoom.EmptyFile().transform.position.x + ParentPokemonRoom.RoomSize[0])
                    );
                Vector2 dir = _mTool.TiltMainVector2((TargetPosition - MoveTarget[i]).normalized);
                if (i == m)
                {

                    rigidbody2D.position = MoveTarget[i];
                    SetDirector(dir);

                    //LunchDir = (TargetPosition - MoveTarget[i]).normalized;
                    if (isTargetInAurora) { LunchDir = (TargetPosition - MoveTarget[i]).normalized; }
                    else { LunchDir = (MoveTarget[i] - AuroraCenter).normalized; }
                }
                else
                {
                    if (!isSleepDone && !isSilence && !isFearDone) {
                        FroslassCloneBody clone = Instantiate(CloneBody, MoveTarget[i], Quaternion.identity);
                        CloneBodyList.Add(clone);
                        clone.SetCloneBody(this);
                        clone.SetDirector(dir);

                        //clone.LunchDir = (TargetPosition - MoveTarget[i]).normalized;
                        if (isTargetInAurora) { clone.LunchDir = (TargetPosition - MoveTarget[i]).normalized; }
                        else { clone.LunchDir = (MoveTarget[i] - AuroraCenter).normalized; }
                    }
                }
            }

        }
        else
        {
            Vector2 t = AuroraCenter - (Vector2)(TargetPosition - AuroraCenter).normalized * AuroraRadius;
            t = new Vector2(
                 Mathf.Clamp(t.x, ParentPokemonRoom.EmptyFile().transform.position.x + ParentPokemonRoom.RoomSize[2], ParentPokemonRoom.EmptyFile().transform.position.x + ParentPokemonRoom.RoomSize[3]),
                 Mathf.Clamp(t.y, ParentPokemonRoom.EmptyFile().transform.position.x + ParentPokemonRoom.RoomSize[1], ParentPokemonRoom.EmptyFile().transform.position.x + ParentPokemonRoom.RoomSize[0])
            );
            Vector2 dir = -_mTool.TiltMainVector2((AuroraCenter - TargetPosition).normalized);
            rigidbody2D.position = t;
            SetDirector(dir);
        }




        
    }




    //=========================瞬间移动============================






    //=========================释放影子球============================

    /// <summary>
    /// 玩家在极光慕内时的影子球时间
    /// </summary>
    public static float TIME_SHADOWBALL_WHEN_TARGET_IN_AURORA = 2.0f;

    /// <summary>
    /// 玩家不在极光慕内时的影子球时间
    /// </summary>
    public static float TIME_SHADOWBALL_WHEN_TARGET_NOT_IN_AURORA = 2.0f;

    /// <summary>
    /// 玩家不在极光慕内时的影子球偏转角度
    /// </summary>
    public static float DEFLECTION_ANGLE_SHADOWBALL_WHEN_TARGET_NOT_IN_AURORA = 35.0f;


    /// <summary>
    /// 发射影子球的次数，满次数后进入长时间冷却
    /// </summary>
    public static int COUNT_LUNCH_SHADOW_BALL = 3;

    /// <summary>
    /// 发射影子球的次数
    /// </summary>
    int ShadowBallCount = 0;

    /// <summary>
    /// 影子球预制体
    /// </summary>
    public FroslassShadowBalll sb;

    //影子球速度
    static float SHADOWBALL_SPEED = 9.0f;


    /// <summary>
    /// 释放影子球计时器
    /// <summary>
    float UseShadowBallTimer = 0;

    /// <summary>
    /// 发射影子球的角度
    /// </summary>
    Vector2 LunchDir;

    /// <summary>
    /// 释放影子球开始
    /// <summary>
    public void UseShadowBallStart(float Timer)
    {
        UseShadowBallTimer = Timer;
        NowState = MainState.UseShadowBall;
        animator.SetTrigger("Atk");
    }

    /// <summary>
    /// 释放影子球结束
    /// <summary>
    public void UseShadowBallOver()
    {
        UseShadowBallTimer = 0;
        ShadowBallCount++;
        if (ShadowBallCount >= COUNT_LUNCH_SHADOW_BALL)
        {
            ShadowBallCount = 0;
            IdleStart(TIME_IDLE_USESHADOWBALL_LONG_TIME);
        }
        else
        {
            IdleStart(TIME_IDLE_USESHADOWBALL);
        }
    }


    public void LunchShadowBall()
    {
        //排空
        _mTool.RemoveNullInList<FroslassCloneBody>(CloneBodyList);

        float SBDpeed = SHADOWBALL_SPEED;
        if (isTargetInAurora)
        {
            LunchShadowBall_PlayerInAurora(SHADOWBALL_SPEED);
            
        }
        else
        {
            LunchShadowBall_PlayerNotInAurora(SHADOWBALL_SPEED);

        }
    }


    /// <summary>
    /// 玩家不在极光慕内发射影子球
    /// </summary>
    void LunchShadowBall_PlayerNotInAurora(float speed)
    {
        float DeflectionAngle = DEFLECTION_ANGLE_SHADOWBALL_WHEN_TARGET_NOT_IN_AURORA;
        if(isEmptyConfusionDone){ DeflectionAngle = 75.0f; }
        //Vector2 LunchDir2 = (TargetPosition - (Vector2)transform.position).normalized;
        Vector2 LunchDir2 = (LunchDir).normalized;
        LunchOneShadowBall(LunchDir2 , speed);
        LunchOneShadowBall(Quaternion.AngleAxis(DeflectionAngle, Vector3.forward) * LunchDir2, speed);
        LunchOneShadowBall(Quaternion.AngleAxis(-DeflectionAngle, Vector3.forward) * LunchDir2, speed);
        for (int i = 0; i < CloneBodyList.Count; i++)
        {
            CloneBodyList[i].LunchShadowBall_PlayerNotInAurora(TargetPosition , DeflectionAngle , speed);
        }
    }


    /// <summary>
    /// 玩家在极光慕内发射影子球
    /// </summary>
    void LunchShadowBall_PlayerInAurora(float speed)
    {
        LunchOneShadowBall(LunchDir , speed);
        for (int i = 0; i < CloneBodyList.Count; i++)
        {
            CloneBodyList[i].LunchShadowBall_PlayerInAurora(speed);
        }
    }
    

    /// <summary>
    /// 发射一个影子球
    /// </summary>
    void LunchOneShadowBall( Vector2 Dir , float speed)
    {
        Dir = Dir.normalized;
        FroslassShadowBalll s = Instantiate(sb , (Vector3)Dir * 1.0f + transform.position + Vector3.up * 0.4f , Quaternion.identity );
        s.empty = this;
        s.LaunchNotForce(Dir, speed);
    }

    //=========================释放影子球============================









    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    















}
