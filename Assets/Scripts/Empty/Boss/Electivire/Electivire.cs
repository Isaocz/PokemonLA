using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electivire : Empty
{
    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;


    /// <summary>
    /// 计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行
    /// </summary>
    Vector3 LastPosition;


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
    /// 红眼
    /// </summary>
    public GameObject RedEyes;

    /// <summary>
    /// 吼叫残影
    /// </summary>
    public GameObject RoarShadow;

    /// <summary>
    /// 奔跑尘埃特效
    /// </summary>
    public GameObject RunDust;





    //==============================拳击类型===================================

    /// <summary>
    /// 拳击类型
    /// </summary>
    public enum PunchType
    {
        None,        //无特效
        Ice,         //冰拳
        Fire,        //火拳
        Thunder,     //雷拳
        NormalFight, //格斗拳
        DynamicPunch,//爆裂拳
        CloseCombat, //近身战
        StarPunch,   //流星拳波
        SuperChargePunch,  //超级雷电拳
    };

    /// <summary>
    /// 当前拳击类型
    /// </summary>
    public PunchType NowPunchType
    {
        get { return nowPunchType; }
        set { nowPunchType = value; }
    }
    PunchType nowPunchType = PunchType.Thunder;

    //==============================拳击类型===================================










    //==============================状态机枚举===================================

    /// <summary>
    /// 主状态
    /// </summary>
    enum MainState
    {
        Normal,   //一般_0
        Angry,    //愤怒_1
    }
    MainState NowMainState;


    /// <summary>
    /// 副状态
    /// </summary>
    enum SubState
    {
        Normal_Idle,         //一般_发呆_0
        Normal_Run,          //一般_奔跑追踪_1
        Normal_TriPunch,     //一般_连续三练拳（冰火雷）_2
        Normal_ChargePunch,  //一般_蓄力拳（爆裂拳）_3
        Normal_CloseCombat,  //一般_近身战（）_4
        Normal_ORaPunch,     //一般_快速连打拳（格斗小拳）_5
        Normal_SmallRoar,    //一般_小吼叫_6
        Normal_SmallJump,    //一般_小跳_7
        Normal_BigRoar,      //一般_大吼叫_8
        Angry_Idle,                //愤怒_发呆_9
        Angry_Run,                 //愤怒_奔跑追踪_10
        Angry_TriPunch,            //愤怒_连续三练拳（）_11
        Angry_ChargePunch,         //愤怒_蓄力拳_12
        Angry_OraPunch,            //愤怒_连续近身拳_13
        Angry_SmallRoar,           //愤怒_小吼叫_14
        Angry_SmallJump,           //愤怒_小跳_15
        Angry_BigRoar,             //愤怒_大吼叫_16
        Angry_BigJump,             //愤怒_大跳_16
        Angry_SuperChargePunch,    //愤怒_超级蓄力拳_18
        Angry_StarPunch,           //愤怒_流星拳波_19
        Angry_CloseCombat,         //愤怒_近身战（）_20
        Angry_MegaRoar,            //愤怒_转状态大吼_21
    }
    SubState NowSubState;


    /// <summary>
    /// 是否狂怒
    /// </summary>
    public bool IsSuperAngryState
    {
        get { return superAngry && NowMainState == MainState.Angry; }
    }
    bool superAngry = false;



    /// <summary>
    /// 状态映射关系
    /// </summary>
    private static Dictionary<MainState, SubState[]> StateMap = new()
    {
        { MainState.Normal, new[] { SubState.Normal_Idle, SubState.Normal_Run, SubState.Normal_TriPunch, SubState.Normal_ChargePunch, SubState.Normal_CloseCombat, SubState.Normal_ORaPunch, SubState.Normal_SmallRoar, SubState.Normal_SmallJump, SubState.Normal_BigRoar } },
        { MainState.Angry, new[] { SubState.Angry_Idle, SubState.Angry_Run, SubState.Angry_TriPunch, SubState.Angry_ChargePunch, SubState.Angry_OraPunch, SubState.Angry_SmallRoar, SubState.Angry_SmallJump, SubState.Angry_BigRoar, SubState.Angry_BigJump, SubState.Angry_SuperChargePunch, SubState.Angry_StarPunch, SubState.Angry_CloseCombat, SubState.Angry_MegaRoar } },
    };

    //==============================状态机枚举===================================



    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Electric;//敌人第一属性
        EmptyType02 = PokemonType.TypeEnum.No;//敌人第二属性
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        Emptylevel = SetLevel(player.Level, MaxLevel);//设定敌人等级
        EmptyHpForLevel(Emptylevel);//设定血量
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//设定攻击力
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//设定特攻
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint)/* 【TODO】MiniBoss * 1.2f / Boss * 1.5f; */;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint)/* 【TODO】MiniBoss * 1.2f / Boss * 1.5f; */;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //设置初始方向
        SetDirector(Vector2.down);

        //启动计算方向携程
        StartCoroutine(CheckLook());


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
            SetHitAnimationAble();//判断受击动画是否可用
            //InsertStateMechineSwitch




            //■■开始判断状态机
            //Debug.Log(NowSubState);
            switch (NowMainState)
            {
                //●主状态：【一般_0】状态
                case MainState.Normal:

                    //●判断是否转进愤怒状态
                    if (NowSubState != SubState.Normal_BigRoar && ((float)EmptyHp / (float)maxHP) < HP_NORMAL2ANGRY)
                    {
                        ResetAllState_Normal();
                        Normal_BigRoarStart();
                    }

                    //●当处于冰冻 睡眠 致盲 麻痹状态时主状态【一般_0】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO【一般_0】状态停运的额外条件 */
                    {
                        //判断副状态
                        switch (NowSubState)
                        {
                            //【一般_发呆_0】状态
                            case SubState.Normal_Idle:
                                Normal_IdleTimer -= Time.deltaTime;//【一般_发呆_0】计时器时间减少
                                if (Normal_IdleTimer <= 0)         //计时器时间到时间，结束【一般_发呆_0】状态
                                {
                                    Normal_IdleOver();
                                    Normal_RunStart();
                                }
                                break;
                            //【一般_奔跑追踪_1】状态
                            //路线1 长时间追击后触发三练拳近身战
                            //路线2 接近后触发吼跳打 跳吼打 跳吼打 连招
                            //路线3 距离远后触发蓄力拳
                            case SubState.Normal_Run:
                                Normal_RunTimer += Time.deltaTime;//【一般_奔跑追踪_1】计时器时间增加
                                //不恐惧时
                                if (!isFearDone)
                                {
                                    Vector2 MoveDir = (TargetPosition - (Vector2)transform.position).normalized;
                                    MoveBySpeedAndDir(MoveDir, speed, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    float TargetDistence = Vector2.Distance(TargetPosition, (Vector2)transform.position);
                                    //转换为其他状态
                                    //路线1 长时间追击后触发三练拳近身战
                                    if (Normal_RunTimer >= TIME_NORMAL_COMBAT_ONE)
                                    {
                                        Normal_RunOver();
                                        Normal_TriPunchStart();
                                    }
                                    //路线2 接近后触发连打吼叫小跳连打吼叫小跳连打
                                    if (TargetDistence <= DISTENCE_NORMAL_COMBAT_TOW || (TargetDistence <= (DISTENCE_NORMAL_COMBAT_TOW + 1.0f) && Normal_RunTimer >= TIME_NORMAL_COMBAT_EASYMODE_TOW))
                                    {
                                        Normal_RunOver();
                                        Normal_SmallRoarStart();
                                    }
                                    //路线3 距离远后触发蓄力拳
                                    if (TargetDistence >= DISTENCE_NORMAL_COMBAT_THREE || (TargetDistence >= DISTENCE_NORMAL_COMBAT_THREE - 1.0f && Normal_RunTimer >= TIME_NORMAL_COMBAT_EASYMODE_THREE))
                                    {
                                        Normal_RunOver();
                                        Normal_ChargePunchStart();
                                    }
                                }
                                //恐惧时
                                else
                                {
                                    if (Vector2.Distance(TargetPosition , (Vector2)transform.position) <= 6.0f)
                                    {
                                        Vector2 MoveDir = (TargetPosition - (Vector2)transform.position).normalized;
                                        MoveBySpeedAndDir(-MoveDir, speed, 2.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    }
                                }
                                break;
                            //【一般_连续三练拳_2】状态
                            case SubState.Normal_TriPunch:
                                //Normal_TriPunchTimer -= Time.deltaTime;//【一般_连续三练拳_2】计时器时间减少
                                //if (Normal_TriPunchTimer <= 0)         //计时器时间到时间，结束【一般_连续三练拳_2】状态
                                //{
                                //    Normal_TriPunchOver();
                                //    //TODO添加下一个状态的开始方法
                                //}
                                if (isMove_Normal_TriPunch)
                                {
                                    MoveBySpeedAndDir(Dir_TriPunch_Normal, speed, SPEEDALPHA_NORMAL_TRIPUNCH, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【一般_蓄力拳_3】状态
                            case SubState.Normal_ChargePunch:
                                //蓄力或者冲刺时计时器增加
                                if (isCharge_Normal_ChargePunch || isMove_Normal_ChargePunch)
                                {
                                    Normal_ChargePunchTimer += Time.deltaTime;//【一般_蓄力拳_3】计时器时间增加
                                }
                                //蓄力时可以转向
                                if (isCharge_Normal_ChargePunch)
                                {
                                    Dir_ChargePunch_Normal = (TargetPosition - (Vector2)transform.position).normalized;
                                    SetDirector(_mTool.MainVector2(Dir_ChargePunch_Normal));
                                }
                                //蓄力完毕后转进冲刺
                                if (isCharge_Normal_ChargePunch && Normal_ChargePunchTimer >= TIME_NORMAL_CHARGEPUNCH_CHARGE)
                                {
                                    animator.SetInteger("HeavyPunch", 2);
                                }
                                //冲刺完毕后转进发呆
                                if (isMove_Normal_ChargePunch && Normal_ChargePunchTimer >= TIME_NORMAL_CHARGEPUNCH_RUSH)
                                {
                                    animator.SetInteger("HeavyPunch", 3);
                                }
                                //冲刺
                                if (isMove_Normal_ChargePunch)
                                {
                                    MoveBySpeedAndDir(Dir_ChargePunch_Normal, speed, SPEEDALPHA_NORMAL_CHARGEPUNCH, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【一般_近身战_4】状态
                            case SubState.Normal_CloseCombat:
                                //蓄力或者冲刺时计时器增加
                                if (isCharge_Normal_CloseCombat || isMove_Normal_CloseCombat)
                                {
                                    Normal_CloseCombatTimer += Time.deltaTime;//【一般_近身战_4】计时器时间增加
                                }
                                //蓄力时可以转向
                                if (isCharge_Normal_CloseCombat)
                                {
                                    Dir_CloseCombat_Normal = (TargetPosition - (Vector2)transform.position).normalized;
                                    SetDirector(_mTool.MainVector2(Dir_CloseCombat_Normal));
                                }
                                //蓄力完毕后转进冲刺
                                if (isCharge_Normal_CloseCombat && Normal_CloseCombatTimer >= TIME_NORMAL_CLOSECOMBT_CHARGE)
                                {
                                    animator.SetInteger("HeavyPunch", 2);
                                }
                                //冲刺完毕后转进发呆
                                if (isMove_Normal_CloseCombat && Normal_CloseCombatTimer >= TIME_NORMAL_CLOSECOMBT_RUSH)
                                {
                                    animator.SetInteger("HeavyPunch", 3);
                                }
                                //冲刺
                                if (isMove_Normal_CloseCombat)
                                {
                                    MoveBySpeedAndDir(Dir_CloseCombat_Normal, speed, SPEEDALPHA_NORMAL_CLOSECOMBT, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【一般_快速连打拳_5】状态
                            case SubState.Normal_ORaPunch:
                                //Normal_ORaPunchTimer -= Time.deltaTime;//【一般_快速连打拳_5】计时器时间减少
                                //if (Normal_ORaPunchTimer <= 0)         //计时器时间到时间，结束【一般_快速连打拳_5】状态
                                //{
                                //    Normal_ORaPunchOver();
                                //    //TODO添加下一个状态的开始方法
                                //}
                                if (isMove_Normal_ORaPunch_Rush)
                                {
                                    MoveBySpeedAndDir(Dir_Normal_ORaPunch_Rush, speed, SPEEDALPHA_NORMAL_ORAPUNCH_RUSH, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【一般_小吼叫_6】状态
                            case SubState.Normal_SmallRoar:
                                //吼叫开始计时
                                if (isStart_Normal_Roar)
                                {
                                    Normal_SmallRoarTimer += Time.deltaTime;//【一般_小吼叫_6】计时器时间增加
                                    if (Normal_SmallRoarTimer >= TIME_NORMAL_SMALLROAR)
                                    {
                                        animator.SetInteger("Roar", 2);
                                    }
                                }
                                break;
                            //【一般_小跳_7】状态
                            case SubState.Normal_SmallJump:
                                //小跳开始计时
                                if (isStart_Normal_SmallJump)
                                {
                                    Normal_SmallJumpTimer += Time.deltaTime;//【一般_小跳_7】计时器时间增加
                                    if (Normal_SmallJumpTimer >= TIME_NORMAL_SMALLJUMP)
                                    {
                                        animator.SetInteger("SmallJump", 2);
                                    }
                                    //小跳移动
                                    float SpeedAlpha = (Vector2.Distance(TargetPosion_Normal_SmallJump, StartPosion_Normal_SmallJump) / (TIME_NORMAL_SMALLJUMP)) / speed;
                                    //Debug.Log(TargetPosion_SmallJump + "+" + transform.position + "+" + SpeedAlpha);
                                    Vector2 SpeedDir = (TargetPosion_Normal_SmallJump - StartPosion_Normal_SmallJump).normalized;
                                    MoveBySpeedAndDir(SpeedDir, speed, SpeedAlpha, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【一般_大吼叫_8】状态
                            case SubState.Normal_BigRoar:
                                //Normal_BigRoarTimer -= Time.deltaTime;//【一般_大吼叫_8】计时器时间减少
                                //if (Normal_BigRoarTimer <= 0)         //计时器时间到时间，结束【一般_大吼叫_8】状态
                                //{
                                //    Normal_BigRoarOver();
                                //    //TODO添加下一个状态的开始方法
                                //}
                                //大吼叫开始计时
                                if (isStart_Normal_BigRoar)
                                {
                                    Normal_BigRoarTimer += Time.deltaTime;//【一般_大吼叫_8】计时器时间增加
                                    if (Normal_BigRoarTimer >= TIME_NORMAL_BIGROAR)
                                    {
                                        animator.SetInteger("Roar", 2);
                                    }
                                }
                                break;
                        }
                    }

                    //●冰冻沉默睡眠时结束当前状态机
                    if ((isEmptyFrozenDone || isSilence || isSleepDone) && (NowSubState != SubState.Normal_Idle))
                    {
                        ResetAllState_Normal();
                        Normal_IdleStart(TIME_NORMAL_IDLE_START);
                    }

                    //●恐惧时结束当前状态机
                    if ((isFearDone) && (NowSubState != SubState.Normal_Run))
                    {
                        ResetAllState_Normal();
                        Normal_RunStart();
                    }
                    break;
                //●主状态：【愤怒_1】状态
                case MainState.Angry:


                    //●判断是否转进超级愤怒状态
                    if (!superAngry && NowSubState != SubState.Angry_MegaRoar && ((float)EmptyHp / (float)maxHP) < HP_ANGRY2SUPERANGRY)
                    {
                        ResetAllState_Normal();
                        Angry_MegaRoarStart();
                    }

                    //●当处于冰冻 睡眠 致盲 麻痹状态时主状态【愤怒_1】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO【愤怒_1】状态停运的额外条件 */
                    {
                        //判断副状态
                        switch (NowSubState)
                        {
                            //【愤怒_发呆_9】状态
                            case SubState.Angry_Idle:
                                Angry_IdleTimer -= Time.deltaTime;//【愤怒_发呆_9】计时器时间减少
                                if (Angry_IdleTimer <= 0)         //计时器时间到时间，结束【愤怒_发呆_9】状态
                                {
                                    Angry_IdleOver();
                                    Angry_RunStart();
                                    //TODO添加下一个状态的开始方法
                                }
                                break;
                            //【愤怒_奔跑追踪_10】状态
                            //路线11 长时间追击后触发（50%）三练拳近身战*2
                            //路线12 长时间追击后触发（50%）小跳 小跳 大跳 大吼
                            //路线21 接近后触发（50%）吼跳打 跳吼打 大跳吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
                            //路线22 接近后触发（50%）连打蓄力拳连打蓄力拳 连招
                            //路线31 距离远后触发（50%）流星拳波流星拳波流星拳波超级蓄力拳（真气拳）
                            //路线32 距离远后触发（50%）蓄力拳蓄力拳蓄力拳大跳大吼
                            case SubState.Angry_Run:
                                Angry_RunTimer += Time.deltaTime;//【愤怒_奔跑追踪_10】计时器时间增加
                                //不恐惧时
                                if (!isFearDone)
                                {
                                    Vector2 MoveDir = (TargetPosition - (Vector2)transform.position).normalized;
                                    if (IsSuperAngryState)
                                    {
                                        MoveBySpeedAndDir(MoveDir, speed, 1.2f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    }
                                    else
                                    {
                                        MoveBySpeedAndDir(MoveDir, speed, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    }
                                    
                                    float TargetDistence = Vector2.Distance(TargetPosition, (Vector2)transform.position);
                                    //转换为其他状态【TODO】
                                    //路线11 长时间追击后触发（50%）三练拳近身战*2
                                    //【TODO】路线12 长时间追击后触发（50%）小跳 小跳 大跳 大吼
                                    if (Angry_RunTimer >= TIME_ANGRY_COMBAT_ONE)
                                    {
                                        Angry_RunOver();
                                        float f = Random.Range(0.0f, 1.0f);
                                        if (f > 0.5f)//路线11
                                        {
                                            NowCombat_Angry = COMBATROUND.ANGRY_COMBAT11;
                                            Angry_TriPunchStart();
                                        }
                                        else//路线12
                                        {
                                            NowCombat_Angry = COMBATROUND.ANGRY_COMBAT12;
                                            Angry_SmallJumpStart();
                                        }
                                        //NowCombat_Angry = COMBATROUND.ANGRY_COMBAT11;
                                        //Angry_TriPunchStart();
                                        //NowCombat_Angry = COMBATROUND.ANGRY_COMBAT12;
                                        //Angry_SmallJumpStart();
                                    }
                                    //【TODO】路线21 接近后触发（50%）吼跳打 跳吼打 跳吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
                                    //【TODO】路线22 接近后触发（50%）连打蓄力拳连打蓄力拳 连招
                                    if (TargetDistence <= DISTENCE_ANGRY_COMBAT_TOW || (TargetDistence <= (DISTENCE_ANGRY_COMBAT_TOW + 1.0f) && Angry_RunTimer >= TIME_ANGRY_COMBAT_EASYMODE_TOW))
                                    {
                                        Angry_RunOver();
                                        float f = Random.Range(0.0f, 1.0f);
                                        if (f > 0.5f)//路线21
                                        {
                                            NowCombat_Angry = COMBATROUND.ANGRY_COMBAT21;
                                            Angry_SmallRoarStart();
                                        }
                                        else//路线22
                                        {
                                            NowCombat_Angry = COMBATROUND.ANGRY_COMBAT22;
                                            Angry_OraPunchStart();
                                        }
                                        //NowCombat_Angry = COMBATROUND.ANGRY_COMBAT21;
                                        //Angry_SmallRoarStart();
                                        //NowCombat_Angry = COMBATROUND.ANGRY_COMBAT22;
                                        //Angry_OraPunchStart();
                                    }
                                    //【TODO】路线31 距离远后触发（50%）流星拳波流星拳波流星拳波超级蓄力拳（真气拳）
                                    //【TODO】路线32 距离远后触发（50%）蓄力拳蓄力拳蓄力拳大跳大吼
                                    if (TargetDistence >= DISTENCE_ANGRY_COMBAT_THREE || (TargetDistence >= DISTENCE_ANGRY_COMBAT_THREE - 1.0f && Angry_RunTimer >= TIME_ANGRY_COMBAT_EASYMODE_THREE))
                                    {
                                        Angry_RunOver();
                                        float f = Random.Range(0.0f, 1.0f);
                                        if (f > 0.5f)//路线31
                                        {
                                            NowCombat_Angry = COMBATROUND.ANGRY_COMBAT31;
                                            Angry_StarPunchStart();
                                        }
                                        else//路线32
                                        {
                                            NowCombat_Angry = COMBATROUND.ANGRY_COMBAT32;
                                            Angry_ChargePunchStart();
                                        }
                                        //NowCombat_Angry = COMBATROUND.ANGRY_COMBAT31;
                                        //Angry_StarPunchStart();
                                        //NowCombat_Angry = COMBATROUND.ANGRY_COMBAT32;
                                        //Angry_ChargePunchStart();
                                    }
                                }
                                //恐惧时
                                else
                                {
                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= 6.0f)
                                    {
                                        Vector2 MoveDir = (TargetPosition - (Vector2)transform.position).normalized;
                                        MoveBySpeedAndDir(-MoveDir, speed, 2.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    }
                                }
                                break;
                            //【愤怒_连续三练拳_11】状态
                            case SubState.Angry_TriPunch:
                                //Angry_TriPunchTimer -= Time.deltaTime;//【愤怒_连续三练拳_11】计时器时间减少
                                //if (Angry_TriPunchTimer <= 0)         //计时器时间到时间，结束【愤怒_连续三练拳_11】状态
                                //{
                                //    Angry_TriPunchOver();
                                //    //TODO添加下一个状态的开始方法
                                //}
                                if (isMove_Angry_TriPunch)
                                {
                                    MoveBySpeedAndDir(Dir_TriPunch_Angry, speed, SPEEDALPHA_ANGRY_TRIPUNCH, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【愤怒_蓄力拳_12】状态
                            case SubState.Angry_ChargePunch:
                                //蓄力或者冲刺时计时器增加
                                if (isCharge_Angry_ChargePunch || isMove_Angry_ChargePunch)
                                {
                                    Angry_ChargePunchTimer += Time.deltaTime;//【愤怒_蓄力拳_12】计时器时间增加
                                }
                                //蓄力时可以转向
                                if (isCharge_Angry_ChargePunch)
                                {
                                    Dir_ChargePunch_Angry = (TargetPosition - (Vector2)transform.position).normalized;
                                    SetDirector(_mTool.MainVector2(Dir_ChargePunch_Angry));
                                }
                                //蓄力完毕后转进冲刺
                                if (isCharge_Angry_ChargePunch && Angry_ChargePunchTimer >= TIME_ANGRY_CHARGEPUNCH_CHARGE)
                                {
                                    animator.SetInteger("HeavyPunch", 2);
                                }
                                //冲刺完毕后转进发呆
                                if (isMove_Angry_ChargePunch && Angry_ChargePunchTimer >= TIME_ANGRY_CHARGEPUNCH_RUSH)
                                {
                                    animator.SetInteger("HeavyPunch", 3);
                                }
                                //冲刺
                                if (isMove_Angry_ChargePunch)
                                {
                                    MoveBySpeedAndDir(Dir_ChargePunch_Angry, speed, SPEEDALPHA_ANGRY_CHARGEPUNCH, 0.0f, 0.0f, 0.0f, 0.0f);
                                    //狂怒时生成闪电
                                    if (IsSuperAngryState)
                                    {
                                        //SuperAngry_ChargePunch_ThunderInterval_Timer += Time.deltaTime;
                                        //if (SuperAngry_ChargePunch_ThunderInterval_Timer >= TIME_SUPERANGRY_CHARGEPUNCH_THUNDER_INTERVAL)
                                        //{
                                        //    SuperAngry_ChargePunch_ThunderInterval_Timer = 0.0f;
                                        //    LunchThuderByVector(transform.position , _mTool.MainVector2(Dir_ChargePunch_Angry) , 0.15f , 2.0f , 8 , 1.0f);
                                        //}
                                    }
                                }
                                break;
                            //【愤怒_近身战_20】状态
                            case SubState.Angry_CloseCombat:
                                //蓄力或者冲刺时计时器增加
                                if (isCharge_Angry_CloseCombat || isMove_Angry_CloseCombat)
                                {
                                    Angry_CloseCombatTimer += Time.deltaTime;//【愤怒_近身战_20】计时器时间增加
                                }
                                //蓄力时可以转向
                                if (isCharge_Angry_CloseCombat)
                                {
                                    Dir_CloseCombat_Angry = (TargetPosition - (Vector2)transform.position).normalized;
                                    SetDirector(_mTool.MainVector2(Dir_CloseCombat_Angry));
                                }
                                //蓄力完毕后转进冲刺
                                if (isCharge_Angry_CloseCombat && Angry_CloseCombatTimer >= TIME_ANGRY_CLOSECOMBT_CHARGE)
                                {
                                    animator.SetInteger("HeavyPunch", 2);
                                }
                                //冲刺完毕后转进发呆
                                if (isMove_Angry_CloseCombat && Angry_CloseCombatTimer >= TIME_ANGRY_CLOSECOMBT_RUSH)
                                {
                                    animator.SetInteger("HeavyPunch", 3);
                                }
                                //冲刺
                                if (isMove_Angry_CloseCombat)
                                {
                                    MoveBySpeedAndDir(Dir_CloseCombat_Angry, speed, SPEEDALPHA_ANGRY_CLOSECOMBT, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【愤怒_连续近身拳_13】状态
                            case SubState.Angry_OraPunch:
                                if (isMove_Angry_ORaPunch_Rush)
                                {
                                    //雷电拳连打速度增加
                                    MoveBySpeedAndDir(Dir_Angry_ORaPunch_Rush, speed, 
                                        (nowPunchType == PunchType.Thunder) ? SPEEDALPHA_ANGRY_ORAPUNCH_RUSH * 1.2f : SPEEDALPHA_ANGRY_ORAPUNCH_RUSH, 
                                        0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【愤怒_小吼叫_14】状态
                            case SubState.Angry_SmallRoar:
                                //吼叫开始计时
                                if (isStart_Angry_Roar)
                                {
                                    Angry_SmallRoarTimer += Time.deltaTime;//【愤怒_小吼叫_14】计时器时间增加
                                    if (Angry_SmallRoarTimer >= TIME_ANGRY_SMALLROAR)
                                    {
                                        animator.SetInteger("Roar", 2);
                                    }
                                }
                                break;
                            //【愤怒_小跳_15】状态
                            case SubState.Angry_SmallJump:
                                //小跳开始计时
                                if (isStart_Angry_SmallJump)
                                {
                                    Angry_SmallJumpTimer += Time.deltaTime;//【愤怒_小跳_15】计时器时间增加
                                    if (Angry_SmallJumpTimer >= TIME_ANGRY_SMALLJUMP)
                                    {
                                        animator.SetInteger("SmallJump", 2);
                                    }
                                    //小跳移动
                                    float SpeedAlpha = (Vector2.Distance(TargetPosion_Angry_SmallJump, StartPosion_Angry_SmallJump) / (TIME_ANGRY_SMALLJUMP)) / speed;
                                    //Debug.Log(TargetPosion_SmallJump + "+" + transform.position + "+" + SpeedAlpha);
                                    Vector2 SpeedDir = (TargetPosion_Angry_SmallJump - StartPosion_Angry_SmallJump).normalized;
                                    MoveBySpeedAndDir(SpeedDir, speed, SpeedAlpha, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【愤怒_大吼叫_16】状态
                            case SubState.Angry_BigRoar:
                                //大吼叫开始计时
                                if (isStart_Angry_BigRoar)
                                {
                                    Angry_BigRoarTimer += Time.deltaTime;//【愤怒_大吼叫_16】计时器时间增加
                                    if (Angry_BigRoarTimer >= TIME_ANGRY_BIGROAR)
                                    {
                                        animator.SetInteger("Roar", 2);
                                    }
                                }
                                break;
                            //【愤怒_大跳_17】状态
                            case SubState.Angry_BigJump:
                                //大跳开始计时
                                if (isStart_Angry_BigJump)
                                {
                                    Angry_BigJumpTimer += Time.deltaTime;//【愤怒_大跳_16】计时器时间增加
                                    if (Angry_BigJumpTimer >= TIME_ANGRY_BIGJUMP_MAX || Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_ANGRY_BIGJUMP_MIN)
                                    {
                                        animator.SetInteger("BigJump", 2);
                                    }
                                    //大跳移动
                                    //float SpeedAlpha = (Vector2.Distance(TargetPosion_Angry_SmallJump, StartPosion_Angry_SmallJump) / (TIME_ANGRY_SMALLJUMP)) / speed;
                                    //Debug.Log(TargetPosion_SmallJump + "+" + transform.position + "+" + SpeedAlpha);
                                    Vector2 SpeedDir = (TargetPosition - (Vector2)transform.position).normalized;
                                    MoveBySpeedAndDir(SpeedDir, speed, SPEEDALPHA_ANGRY_BIGJUMP, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【愤怒_超级蓄力拳_18】状态
                            case SubState.Angry_SuperChargePunch:
                                //蓄力或者冲刺时计时器增加
                                if (isCharge_Angry_SuperChargePunch || isMove_Angry_SuperChargePunch)
                                {
                                    Angry_SuperChargePunchTimer += Time.deltaTime;//【愤怒_超级蓄力拳_18】计时器时间增加
                                }
                                //蓄力时可以转向
                                if (isCharge_Angry_SuperChargePunch)
                                {
                                    Dir_SuperChargePunch_Angry = (TargetPosition - (Vector2)transform.position).normalized;
                                    SetDirector(_mTool.MainVector2(Dir_SuperChargePunch_Angry));
                                }
                                //蓄力完毕后转进冲刺
                                if (isCharge_Angry_SuperChargePunch && Angry_SuperChargePunchTimer >= TIME_ANGRY_SUPERCHARGEPUNCH_CHARGE)
                                {
                                    animator.SetInteger("HeavyPunch", 2);
                                }
                                //冲刺完毕后转进发呆
                                if (isMove_Angry_SuperChargePunch && Angry_SuperChargePunchTimer >= TIME_ANGRY_SUPERCHARGEPUNCH_RUSH)
                                {
                                    animator.SetInteger("HeavyPunch", 3);
                                }
                                //冲刺
                                if (isMove_Angry_SuperChargePunch)
                                {
                                    MoveBySpeedAndDir(Dir_SuperChargePunch_Angry, speed, SPEEDALPHA_ANGRY_SUPERCHARGEPUNCH, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【愤怒_流星拳波_19】状态
                            case SubState.Angry_StarPunch:
                                //蓄力或者冲刺时计时器增加
                                if (isCharge_Angry_StarPunch)
                                {
                                    Angry_StarPunchTimer += Time.deltaTime;//【愤怒_流星拳波_19】计时器时间增加
                                }
                                //蓄力时可以转向
                                if (isCharge_Angry_StarPunch)
                                {
                                    Dir_Lunch_StartPunch = (TargetPosition - (Vector2)transform.position).normalized;
                                    SetDirector(_mTool.MainVector2(Dir_Lunch_StartPunch));
                                }
                                //发射后立刻转进发呆
                                if (animator.GetInteger("HeavyPunch") == 2)
                                {
                                    animator.SetInteger("HeavyPunch", 3);
                                }
                                //蓄力完毕后转进发射
                                if (isCharge_Angry_StarPunch && Angry_StarPunchTimer >= TIME_ANGRY_STARPUNCH_CHARGE * (IsSuperAngryState ? 0.2f : 1.0f))
                                {
                                    animator.SetInteger("HeavyPunch", 2);
                                }
                                break;
                            //【愤怒_转状态大吼_21】状态
                            case SubState.Angry_MegaRoar:
                                //转状态大吼开始计时
                                if (isStart_Angry_MegaRoar)
                                {
                                    Angry_MegaRoarTimer += Time.deltaTime;//【愤怒_转状态大吼_21】计时器时间增加
                                    if (Angry_MegaRoarTimer >= TIME_ANGRY_MEGAROAR)
                                    {
                                        animator.SetInteger("Roar", 2);
                                    }
                                }
                                break;
                        }
                    }

                    //●冰冻沉默睡眠时结束当前状态机
                    if ((isEmptyFrozenDone || isSilence || isSleepDone) && (NowSubState != SubState.Angry_Idle))
                    {
                        ResetAllState_Normal();
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                    }

                    //●恐惧时结束当前状态机
                    if ((isFearDone) && (NowSubState != SubState.Angry_Run))
                    {
                        ResetAllState_Normal();
                        Angry_RunStart();
                    }
                    break;
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































    //■■■■■■■■■■■■■■■■■■■■碰撞■■■■■■■■■■■■■■■■■■■■■■

    private void OnCollisionEnter2D(Collision2D other)
    {
        CollisionPlayer(other);
        CollisionRoomOrEnviroment(other);
    }

    //冰拳伤害
    static int DMAGE_ICEPUNCH = 75;
    //冰拳击退值
    static float KOPOINT_ICEPUNCH = 4.0f;
    //火拳伤害
    static int DMAGE_FIREPUNCH = 75;
    //火拳击退值
    static float KOPOINT_FIREPUNCH = 4.0f;
    //雷拳伤害
    static int DMAGE_THUNDERPUNCH = 75;
    //雷拳击退值
    static float KOPOINT_THUNDERPUNCH = 4.0f;




    //快速连打拳伤害
    static int DMAGE_ORAPUNCH = 40;
    //快速连打拳击退值
    static float KOPOINT_ORAPUNCH = 4.0f;

    //快速连打雷电拳伤害
    static int DMAGE_ORAPUNCH_THUNDER = 50;
    //快速连打拳击退值
    static float KOPOINT_ORAPUNCH_THUNDER = 4.0f;



    //爆裂拳伤害
    static int DMAGE_DYNAMICPUNCH = 100;
    //爆裂拳击退值
    static float KOPOINT_DYNAMICPUNCH = 9.0f;

    //爆裂拳伤害
    static int DMAGE_SUPERCHARGEPUNCH = 130;
    //爆裂拳击退值
    static float KOPOINT_SUPERCHARGEPUNCH = 9.0f;

    //小爆裂拳（近身战状态）伤害
    static int DMAGE_SMALLDYNAMICPUNCH = 85;
    //小爆裂拳（近身战状态）击退值
    static float KOPOINT_SMALLDYNAMICPUNCH = 7.5f;



    /// <summary>
    /// 与玩家碰撞
    /// </summary>
    public void CollisionPlayer(Collision2D other)
    {
        if (other.transform.tag == ("Player"))//与玩家碰撞时
        {
            PlayerControler p = other.gameObject.GetComponent<PlayerControler>();
            if (p != null) {
                switch (NowMainState)
                {
                    case MainState.Normal:
                        switch (NowSubState)
                        {
                            case SubState.Normal_Idle:         //一般_发呆_0
                                EmptyTouchHit(other.gameObject);//触发触碰伤害
                                break;
                            case SubState.Normal_Run:          //一般_奔跑追踪_1
                                EmptyTouchHit(other.gameObject);//触发触碰伤害
                                break;
                            case SubState.Normal_TriPunch:     //一般_连续三练拳_2
                                IceFireThuderPunchHit(p);
                                break;
                            case SubState.Normal_ChargePunch:  //一般_蓄力拳_3
                                if (isMove_Normal_ChargePunch)
                                {
                                    DynamicPunchHit(p.gameObject);
                                }
                                else { EmptyTouchHit(other.gameObject); }
                                break;
                            case SubState.Normal_CloseCombat:  //一般_近身战(小爆裂拳（近身战状态）)_4
                                if (isMove_Normal_CloseCombat)
                                {
                                    CloseCombathHit(p);
                                }
                                else { EmptyTouchHit(other.gameObject); }
                                break;
                            case SubState.Normal_ORaPunch:     //一般_快速连打拳_5
                                ORaPunchHit(p);
                                break;
                            case SubState.Normal_SmallRoar:    //一般_小吼叫_6
                                EmptyTouchHit(other.gameObject);//触发触碰伤害
                                break;
                            case SubState.Normal_SmallJump:    //一般_小跳_7
                                EmptyTouchHit(other.gameObject);//触发触碰伤害
                                break;
                            case SubState.Normal_BigRoar:      //一般_大吼叫_8
                                //(无触碰伤害 防止无法触发地震伤害)
                                break;
                        }
                        break;
                    case MainState.Angry:
                        switch (NowSubState)
                        {
                            case SubState.Angry_Idle:                //愤怒_发呆_9
                                EmptyTouchHit(other.gameObject);//触发触碰伤害
                                break;
                            case SubState.Angry_Run:                 //愤怒_奔跑追踪_10
                                EmptyTouchHit(other.gameObject);//触发触碰伤害
                                break;
                            case SubState.Angry_TriPunch:            //愤怒_连续三练拳_11
                                IceFireThuderPunchHit(p);
                                break;
                            case SubState.Angry_ChargePunch:         //愤怒_蓄力拳_12
                                if (isMove_Angry_ChargePunch)
                                {
                                    DynamicPunchHit(p.gameObject);
                                }
                                else { EmptyTouchHit(other.gameObject); }
                                break;
                            case SubState.Angry_CloseCombat:         //愤怒_近身战_20
                                if (isMove_Angry_CloseCombat)
                                {
                                    CloseCombathHit(p);
                                }
                                else { EmptyTouchHit(other.gameObject); }
                                break;
                            case SubState.Angry_OraPunch:            //愤怒_连续近身拳_13
                                ORaPunchHit(p);
                                break;
                            case SubState.Angry_SmallRoar:           //愤怒_小吼叫_14
                                EmptyTouchHit(other.gameObject);//触发触碰伤害
                                break;
                            case SubState.Angry_SmallJump:           //愤怒_小跳_15
                                EmptyTouchHit(other.gameObject);//触发触碰伤害
                                break;
                            case SubState.Angry_BigRoar:             //愤怒_大吼叫_16
                                //(无触碰伤害 防止无法触发大吼叫伤害)
                                break;
                            case SubState.Angry_BigJump:             //愤怒_大跳_16
                                //(无触碰伤害 防止无法触发地震伤害)
                                break;
                            case SubState.Angry_SuperChargePunch:    //愤怒_超级蓄力拳_18
                                if (isMove_Angry_SuperChargePunch)
                                {
                                    DynamicPunchHit(p.gameObject);
                                }
                                else { EmptyTouchHit(other.gameObject); }
                                break;
                            case SubState.Angry_StarPunch:           //愤怒_流星拳波_19
                                //(无触碰伤害 防止无法触发流星拳波伤害)
                                break;
                            case SubState.Angry_MegaRoar:             //愤怒_转状态大吼_16
                                //(无触碰伤害 防止无法触发转状态大吼伤害)
                                break;
                        }
                        break;
                }
            }
        }
    }


    /// <summary>
    /// 属性（冰火雷）拳
    /// </summary>
    /// <param name="player"></param>
    void IceFireThuderPunchHit(PlayerControler player)
    {
        switch (nowPunchType)
        {
            case PunchType.Ice:
                Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_ICEPUNCH, 0, 0, PokemonType.TypeEnum.Ice);
                player.KnockOutPoint = KOPOINT_ICEPUNCH;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
                player.PlayerFrozenFloatPlus(0.5f, 0.8f);
                break;
            case PunchType.Fire:
                Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_FIREPUNCH, 0, 0, PokemonType.TypeEnum.Ice);
                player.KnockOutPoint = KOPOINT_FIREPUNCH;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
                player.BurnFloatPlus(0.35f);
                break;
            case PunchType.Thunder:
                Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_THUNDERPUNCH, 0, 0, PokemonType.TypeEnum.Ice);
                player.KnockOutPoint = KOPOINT_THUNDERPUNCH;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
                player.ParalysisFloatPlus(0.15f);
                break;
        }
    }


    /// <summary>
    /// 快速连打拳
    /// </summary>
    /// <param name="player"></param>
    void ORaPunchHit(PlayerControler player)
    {
        switch (nowPunchType)
        {
            case PunchType.NormalFight:
                Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_ORAPUNCH, 0, 0, PokemonType.TypeEnum.Fighting);
                player.KnockOutPoint = KOPOINT_ORAPUNCH;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
                break;
            case PunchType.Thunder:
                Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_ORAPUNCH_THUNDER, 0, 0, PokemonType.TypeEnum.Electric);
                player.KnockOutPoint = KOPOINT_ORAPUNCH_THUNDER;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
                player.ParalysisFloatPlus(0.1f);
                break;
        }
    }


    /// <summary>
    /// 小爆裂拳（近身战状态）
    /// </summary>
    /// <param name="player"></param>
    void CloseCombathHit(PlayerControler player)
    {
        if (nowPunchType == PunchType.Thunder)
        {
            Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_SMALLDYNAMICPUNCH, 0, 0, PokemonType.TypeEnum.Electric);
            player.KnockOutPoint = KOPOINT_SMALLDYNAMICPUNCH;
            player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
            player.ParalysisFloatPlus(0.1f);
        }
        else
        {
            Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_SMALLDYNAMICPUNCH, 0, 0, PokemonType.TypeEnum.Fighting);
            player.KnockOutPoint = KOPOINT_SMALLDYNAMICPUNCH;
            player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
            player.ConfusionFloatPlus(0.1f);
        }
    }



    /// <summary>
    /// 爆裂拳
    /// </summary>
    /// <param name="player"></param>
    public void DynamicPunchHit(GameObject PlayerGameobj)
    {
        PlayerControler p = PlayerGameobj.gameObject.GetComponent<PlayerControler>();
        if (p != null)
        {
            if (NowSubState == SubState.Angry_SuperChargePunch)
            {
                Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_SUPERCHARGEPUNCH, 0, 0, PokemonType.TypeEnum.Electric);
                player.KnockOutPoint = KOPOINT_SUPERCHARGEPUNCH;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
                player.ParalysisFloatPlus(0.2f);
            }
            else
            {
                Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_DYNAMICPUNCH, 0, 0, PokemonType.TypeEnum.Fighting);
                player.KnockOutPoint = KOPOINT_DYNAMICPUNCH;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
                player.ConfusionFloatPlus(0.25f);
            }
        }
    }




    /// <summary>
    /// 超级蓄力拳
    /// </summary>
    /// <param name="player"></param>
    public void SuperChargePunchHit(GameObject PlayerGameobj)
    {
        PlayerControler p = PlayerGameobj.gameObject.GetComponent<PlayerControler>();
        if (p != null)
        {
            Pokemon.PokemonHpChange(this.gameObject, player.gameObject, DMAGE_SUPERCHARGEPUNCH, 0, 0, PokemonType.TypeEnum.Electric);
            player.KnockOutPoint = KOPOINT_SUPERCHARGEPUNCH;
            player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
            player.ParalysisFloatPlus(0.2f);
        }
    }





    /// <summary>
    /// 与房间或者环境物碰撞
    /// </summary>
    /// <param name="other"></param>
    private void CollisionRoomOrEnviroment(Collision2D other)
    {
        if (other.transform.tag == "Room" || other.transform.tag == "Enviroment")
        {
            switch (NowSubState)
            {
                case SubState.Normal_CloseCombat:
                    if (isMove_Normal_CloseCombat && Normal_CloseCombatTimer >= TIME_NORMAL_CLOSECOMBT_RUSH / 10.0f) { animator.SetInteger("HeavyPunch", 3); }
                    break;
                case SubState.Normal_ChargePunch:
                    if (isMove_Normal_ChargePunch && Normal_ChargePunchTimer >= TIME_NORMAL_CHARGEPUNCH_RUSH / 10.0f)
                    {
                        animator.SetInteger("HeavyPunch", 3);
                        ParentPokemonRoom.CameraShake(0.3f, 2.5f, true);
                    }
                    break;
                case SubState.Angry_CloseCombat:
                    if (isMove_Angry_CloseCombat && Angry_CloseCombatTimer >= TIME_ANGRY_CLOSECOMBT_RUSH / 10.0f) { animator.SetInteger("HeavyPunch", 3); }
                    break;
                case SubState.Angry_ChargePunch:
                    if (isMove_Angry_ChargePunch && Angry_ChargePunchTimer >= TIME_ANGRY_CHARGEPUNCH_RUSH / 10.0f)
                    {
                        animator.SetInteger("HeavyPunch", 3);
                        ParentPokemonRoom.CameraShake(0.3f, 2.5f, true);
                    }
                    break;
                case SubState.Angry_SuperChargePunch:
                    if (isMove_Angry_SuperChargePunch && Angry_SuperChargePunchTimer >= TIME_ANGRY_SUPERCHARGEPUNCH_RUSH / 10.0f)
                    {
                        animator.SetInteger("HeavyPunch", 3);
                        ParentPokemonRoom.CameraShake(0.5f, 4.2f, true);
                    }
                    break;
            }
        }
    }




    //■■■■■■■■■■■■■■■■■■■■碰撞■■■■■■■■■■■■■■■■■■■■■■













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


    /// <summary>
    /// 检查是否在移动和朝向
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence /*&& 更多条件*/ )
            {
                if (NowSubState == SubState.Normal_Run || NowSubState == SubState.Angry_Run)
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
                else
                {
                    animator.SetFloat("Speed", 0);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

    }



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
    /// 生成奔跑尘埃
    /// </summary>
    public void InstantiateRunDust()
    {
        Instantiate(RunDust, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 动画机换拳
    /// </summary>
    public void ChangePunch()
    {
        if (animator.GetFloat("isLPunch") == 0)
        {
            animator.SetFloat("isLPunch", 1);
        }
        else {
            animator.SetFloat("isLPunch", 0);
        }
    }

    /// <summary>
    /// 混乱时角度偏转
    /// </summary>
    Vector2 ConfusionDir( Vector2 dir , float Alpha)
    {
        if (isEmptyConfusionDone)
        {
            return Quaternion.AngleAxis(Random.Range(-Alpha, Alpha), Vector3.forward) * dir;
        }
        return dir;
    }


    //InsertSubStateChange


    /// <summary>
    /// 切换副状态
    /// </summary>
    void ChangeSubState(SubState targetSubstate)
    {
        NowSubState = targetSubstate;
        var mainState = GetMainBySub(targetSubstate);
        NowMainState = mainState;
    }


    /// <summary>
    /// 通过副状态查找主状态
    /// </summary>
    private MainState GetMainBySub(SubState SearchSub)
    {
        foreach (KeyValuePair<MainState, SubState[]> kvp in StateMap)
        {
            foreach (SubState sub in kvp.Value)
            {
                if (sub == SearchSub) { return kvp.Key; }
            }
        }
        return 0;
    }

    /// <summary>
    /// 设置是否可以触发受击动画
    /// </summary>
    void SetHitAnimationAble()
    {
        if(NowSubState == SubState.Normal_Idle || NowSubState == SubState.Normal_Run || NowSubState == SubState.Angry_Idle || NowSubState == SubState.Angry_Run)
        {
            isCanHitAnimation = false;
        }
        else
        {
            isCanHitAnimation = true;
        }
    }


    /// <summary>
    /// 一般_清空所有状态机
    /// </summary>
    void ResetAllState_Normal()
    {
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        //设置方向和速度
        SetDirector(Vector2.down);
        animator.SetFloat("Speed", 0.0f);

        //动画机转进Idle
        animator.SetTrigger("Sleep");

        //重置主状态机参数
       //NextState_Normal = SubState.Normal_Idle;
        Count_Combat_Tow_ORaPunch = 0;
        IsDefStateByNormal = false;
        NowCombat_Angry = COMBATROUND.None;
        Count_Combat_11_CloseCombat = 0;
        Count_Combat_12_SmallJump = 0;
        Count_Combat_21_ORaPunch = 0;
        Count_Combat_22_ChargePunch = 0;
        Count_Combat_31_StarPunch = 0;
        Count_Combat_32_ChargePunch = 0;


        //结束所有副状态机 重置所有副状态机参数
        switch (NowSubState)
        {
            case SubState.Normal_Idle: Normal_IdleOver(); break;
            case SubState.Normal_Run: Normal_RunOver(); break;
            case SubState.Normal_TriPunch: Normal_TriPunchOver(); break;
            case SubState.Normal_ChargePunch: Normal_ChargePunchOver(); break;
            case SubState.Normal_CloseCombat: Normal_CloseCombatOver(); break;
            case SubState.Normal_ORaPunch: Normal_ORaPunchOver(); break;
            case SubState.Normal_SmallRoar: Normal_SmallRoarOver(); break;
            case SubState.Normal_SmallJump: Normal_SmallJumpOver(); break;
            case SubState.Normal_BigRoar: Normal_BigRoarOver(); break;

            case SubState.Angry_Idle: Angry_IdleOver(); break;
            case SubState.Angry_Run: Angry_RunOver(); break;
            case SubState.Angry_TriPunch: Angry_TriPunchOver(); break;
            case SubState.Angry_ChargePunch: Angry_ChargePunchOver(); break;
            case SubState.Angry_CloseCombat: Angry_CloseCombatOver(); break;
            case SubState.Angry_OraPunch: Angry_OraPunchOver(); break;
            case SubState.Angry_SmallJump: Angry_SmallJumpOver(); break;
            case SubState.Angry_SmallRoar: Angry_SmallRoarOver(); break;
            case SubState.Angry_BigJump: Angry_BigJumpOver(); break;
            case SubState.Angry_BigRoar: Angry_BigRoarOver(); break;
            case SubState.Angry_StarPunch: Angry_StarPunchOver(); break;
            case SubState.Angry_SuperChargePunch: Angry_SuperChargePunchOver(); break;
            case SubState.Angry_MegaRoar: Angry_MegaRoarOver(); break;
        }

        //重置所有动画机参数状态
        animator.SetInteger("HeavyPunch", 0);
        animator.ResetTrigger("LightPunch");
        animator.ResetTrigger("ORaPunch");
        animator.SetInteger("Roar", 0);
        animator.SetInteger("BigJump", 0);
        animator.SetInteger("SmallJump", 0);


        //消除吼叫实例
        if( NormalBigRoarObj != null ) { Destroy(NormalBigRoarObj); }
        if (NormalSmallRoarObj != null ) { Destroy(NormalSmallRoarObj); }
        if (AngryBigRoarObj != null ) { Destroy(AngryBigRoarObj); }
        if( SuperAngrySmallRoarObj != null ) { Destroy(SuperAngrySmallRoarObj); }
        if( AngryMegaRoarObj != null ) { Destroy(AngryMegaRoarObj); }
        
        //eqObj
    }







    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■









































    //■■■■■■■■■■■■■■■■■■■■动画机事件■■■■■■■■■■■■■■■■■■■■■■





    //==■==■==■==■==■==■==■动画机事件：快拳■==■==■==■==■==■==■==


    /// <summary>
    /// 快拳作为动画开始
    /// </summary>
    public void AnimatorEvent_LightPunch_In()
    {
        switch (NowSubState)
        {
            case SubState.Normal_TriPunch:
                Dir_TriPunch_Normal = (TargetPosition - (Vector2)transform.position).normalized;
                Dir_TriPunch_Normal = ConfusionDir(Dir_TriPunch_Normal,30.0f);
                SetDirector(_mTool.MainVector2(Dir_TriPunch_Normal));
                break;
            case SubState.Normal_ORaPunch:
                Dir_Normal_ORaPunch_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                Dir_Normal_ORaPunch_Rush = ConfusionDir(Dir_Normal_ORaPunch_Rush, 20.0f);
                SetDirector(_mTool.MainVector2(Dir_Normal_ORaPunch_Rush));
                break;
            case SubState.Angry_TriPunch:
                Dir_TriPunch_Angry = (TargetPosition - (Vector2)transform.position).normalized;
                Dir_TriPunch_Angry = ConfusionDir(Dir_TriPunch_Angry, 30.0f);
                SetDirector(_mTool.MainVector2(Dir_TriPunch_Angry));
                break;
            case SubState.Angry_OraPunch:
                Dir_Angry_ORaPunch_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                Dir_Angry_ORaPunch_Rush = ConfusionDir(Dir_Angry_ORaPunch_Rush, 20.0f);
                //愤怒连招路线21 最后连打变为电属性
                if (NowCombat_Angry == COMBATROUND.ANGRY_COMBAT21 && Count_Combat_21_ORaPunch == 2) { nowPunchType = PunchType.Thunder; }
                //极怒模式也变成电属性
                if (IsSuperAngryState) { 
                    nowPunchType = PunchType.Thunder;
                }
                SetDirector(_mTool.MainVector2(Dir_Angry_ORaPunch_Rush));
                break;
        }
    }
    /// <summary>
    /// 快拳作为动作事件开始
    /// </summary>
    public void AnimatorEvent_LightPunch_Start()
    {
        switch (NowSubState)
        {
            case SubState.Normal_TriPunch:
                isMove_Normal_TriPunch = true;
                TriPunchCount_Normal++;
                SetTriPunchType();
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
            case SubState.Normal_ORaPunch:
                isMove_Normal_ORaPunch_Rush = true;
                ORaPunchCount_Normal++;
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
            case SubState.Angry_TriPunch:
                isMove_Angry_TriPunch = true;
                TriPunchCount_Angry++;
                SetTriPunchType();
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
            case SubState.Angry_OraPunch:
                isMove_Angry_ORaPunch_Rush = true;
                ORaPunchCount_Angry++;
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
        }
    }
    /// <summary>
    /// 快拳作为动作事件结束
    /// </summary>
    public void AnimatorEvent_LightPunch_Over()
    {
        switch (NowSubState)
        {
            case SubState.Normal_TriPunch:
                isMove_Normal_TriPunch = false;
                InstantiateRunDust();
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
            case SubState.Normal_ORaPunch:
                isMove_Normal_ORaPunch_Rush = false;
                InstantiateRunDust();
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
            case SubState.Angry_TriPunch:
                isMove_Angry_TriPunch = false;
                InstantiateRunDust();
                if (IsSuperAngryState)
                {
                    LunchThuderByVector_90(transform.position , _mTool.MainVector2(Dir_TriPunch_Angry) , 0.28f , 2.4f , 4 , 1.2f);
                }
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
            case SubState.Angry_OraPunch:
                isMove_Angry_ORaPunch_Rush = false;
                InstantiateRunDust();
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
        }
    }
    /// <summary>
    /// 快拳作为动画结束
    /// </summary>
    public void AnimatorEvent_LightPunch_Out()
    {
        switch (NowSubState)
        {
            case SubState.Normal_TriPunch:
                if (TriPunchCount_Normal >= 3)
                {
                    Normal_TriPunchOver();
                    //Normal_RunStart();
                    Normal_CloseCombatStart();
                }
                else
                {
                    ChangePunch();
                    animator.SetTrigger("LightPunch");
                }
                break;
            case SubState.Normal_ORaPunch:
                if (ORaPunchCount_Normal >= COUNT_NORMAL_ORAPUNCH)
                {
                    Normal_ORaPunchOver();
                    //完成一次连打
                    Count_Combat_Tow_ORaPunch++;
                    //路线2 接近后触发吼0跳0打1 跳1吼1打2 跳2吼2打3 连招
                    //根据连招顺序决定下一状态 Count_Combat_Tow_ORaPunch
                    switch (Count_Combat_Tow_ORaPunch)
                    {
                        case 1://吼0跳0打1 → 跳1
                            Normal_SmallJumpStart();
                            break;
                        case 2://吼0跳0打1跳1吼1打2 → 跳2
                            Normal_SmallJumpStart();
                            break;
                        case 3://吼0跳0打1跳1吼1打2跳2吼2打3 → 发呆
                            Normal_IdleStart(TIME_NORMAL_IDLE_COMBATTWO);
                            break;
                    }
                }
                else
                {
                    ChangePunch();
                    animator.SetTrigger("ORaPunch");
                }
                break;
            case SubState.Angry_TriPunch:
                if (TriPunchCount_Angry >= 3)
                {
                    Angry_TriPunchOver();
                    Angry_CloseCombatStart();
                }
                else
                {
                    ChangePunch();
                    animator.SetTrigger("LightPunch");
                }
                break;
            case SubState.Angry_OraPunch:
                switch (NowCombat_Angry)
                {
                    case COMBATROUND.ANGRY_COMBAT21:
                        //最后一次连打连打次数增加
                        if (ORaPunchCount_Angry >= ((Count_Combat_21_ORaPunch == 2) ? COUNT_ANGRY_ORAPUNCH * 1.3f : COUNT_ANGRY_ORAPUNCH))
                        {
                            Angry_OraPunchOver();
                            //完成一次连打
                            Count_Combat_21_ORaPunch++;
                            //路线21 接近后触发（50%）吼跳打 跳吼打 大跳吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
                            //根据连招顺序决定下一状态 Count_Combat_Tow_ORaPunch
                            switch (Count_Combat_21_ORaPunch)
                            {
                                case 1://吼0跳0打1 → 跳1
                                    Angry_SmallJumpStart();
                                    break;
                                case 2://吼0跳0打1跳1吼1打2 → 跳2
                                    Angry_SmallJumpStart();
                                    break;
                                case 3://吼0跳0打1跳1吼1打2跳2大吼2打3 → （近身战） 距离不远不触发（快速休息））
                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_ANGRY_COMBAT_21_LASTCOMBAT)
                                    {
                                        Angry_CloseCombatStart();
                                    }
                                    else
                                    {
                                        Angry_IdleStart(TIME_ANGRY_IDLE_COMBAT21_FAST);
                                        Count_Combat_21_ORaPunch = 0;
                                    }
                                    break;
                                default:
                                    Angry_IdleStart(TIME_ANGRY_IDLE_START);
                                    break;
                            }
                        }
                        else
                        {
                            ChangePunch();
                            animator.SetTrigger("ORaPunch");
                        }
                        break;
                    case COMBATROUND.ANGRY_COMBAT22:
                        //路线22 接近后触发（50%）连打蓄力拳连打蓄力拳 连招
                        //根据连招顺序决定下一状态 Count_Combat_22_ORaPunch
                        //最后一次连打连打次数增加
                        if (ORaPunchCount_Angry >= COUNT_ANGRY_ORAPUNCH)
                        {
                            Angry_OraPunchOver();
                            switch (Count_Combat_22_ChargePunch)
                            {
                                case 0://连打0 → 蓄力拳1连打1蓄力拳2
                                    Angry_ChargePunchStart();
                                    break;
                                case 1://连打0蓄力拳1连打1 → 蓄力拳2
                                    Angry_ChargePunchStart();
                                    break;
                            }
                        }
                        else
                        {
                            ChangePunch();
                            animator.SetTrigger("ORaPunch");
                        }

                        break;
                    default:
                        Angry_OraPunchOver();
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
        }
    }


    //==■==■==■==■==■==■==■动画机事件：快拳■==■==■==■==■==■==■==







    //==■==■==■==■==■==■==■动画机事件：重拳■==■==■==■==■==■==■==


    /// <summary>
    /// 重拳作为动画开始
    /// </summary>
    public void AnimatorEvent_HeavyPunch_In()
    {
    }
    /// <summary>
    /// 重拳开始蓄力
    /// </summary>
    public void AnimatorEvent_HeavyPunch_Prepare()
    {
        switch (NowSubState)
        {
            case SubState.Normal_CloseCombat:
                isCharge_Normal_CloseCombat = true;
                break;
            case SubState.Normal_ChargePunch:
                isCharge_Normal_ChargePunch = true;
                break;
            case SubState.Angry_CloseCombat:
                isCharge_Angry_CloseCombat = true;
                break;
            case SubState.Angry_ChargePunch:
                isCharge_Angry_ChargePunch = true;
                break;
            case SubState.Angry_StarPunch:
                isCharge_Angry_StarPunch = true;
                break;
            case SubState.Angry_SuperChargePunch:
                isCharge_Angry_SuperChargePunch = true;
                break;
        }
    }
    /// <summary>
    /// 重拳蓄力完毕，发射
    /// </summary>
    public void AnimatorEvent_HeavyPunch_Lunch()
    {
        switch (NowSubState)
        {
            case SubState.Normal_CloseCombat:
                isCharge_Normal_CloseCombat = false;
                Normal_CloseCombatTimer = 0;
                InstantiateRunDust();
                break;
            case SubState.Normal_ChargePunch:
                //Debug.Log(_mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized , Vector2.right));
                LsatTargetPosition_Predict_Normal_ChargePunch_Angle = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                isCharge_Normal_ChargePunch = false;
                Normal_ChargePunchTimer = 0;
                InstantiateRunDust();
                break;
            case SubState.Angry_CloseCombat:
                isCharge_Angry_CloseCombat = false;
                Angry_CloseCombatTimer = 0;
                InstantiateRunDust();
                break;
            case SubState.Angry_ChargePunch:
                LsatTargetPosition_Predict_Angry_ChargePunch_Angle = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                isCharge_Angry_ChargePunch = false;
                Angry_ChargePunchTimer = 0;
                InstantiateRunDust();
                break;
            case SubState.Angry_StarPunch:
                LsatTargetPosition_Predict_Angry_StarPunch_Angle = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                isCharge_Angry_StarPunch = false;
                Angry_StarPunchTimer = 0;
                InstantiateRunDust();
                break;
            case SubState.Angry_SuperChargePunch:
                LsatTargetPosition_Predict_Angry_SuperChargePunch_Angle = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                isCharge_Angry_SuperChargePunch = false;
                Angry_SuperChargePunchTimer = 0;
                InstantiateRunDust();
                break;
        }
    }
    /// <summary>
    /// 重拳作为动作事件开始
    /// </summary>
    public void AnimatorEvent_HeavyPunch_Start()
    {
        switch (NowSubState)
        {
            case SubState.Normal_CloseCombat:
                Dir_CloseCombat_Normal = (TargetPosition - (Vector2)transform.position).normalized;
                Dir_TriPunch_Angry = ConfusionDir(Dir_TriPunch_Angry, 30.0f);
                SetDirector(_mTool.MainVector2(Dir_CloseCombat_Normal));
                isMove_Normal_CloseCombat = true;
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
            case SubState.Normal_ChargePunch:
                //Debug.Log(_mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right));
                float LsatTargetPosition_Predict_ChargePunch_Angle02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                float PredictAngle = 2.0f * LsatTargetPosition_Predict_ChargePunch_Angle02 - LsatTargetPosition_Predict_Normal_ChargePunch_Angle;
                Dir_ChargePunch_Normal = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                Dir_ChargePunch_Normal = ConfusionDir(Dir_ChargePunch_Normal, 30.0f);
                //Dir_ChargePunch = (TargetPosition - (Vector2)transform.position).normalized;
                SetDirector(_mTool.MainVector2(Dir_ChargePunch_Normal));
                isMove_Normal_ChargePunch = true;
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.025f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
            case SubState.Angry_CloseCombat:
                Dir_CloseCombat_Angry = (TargetPosition - (Vector2)transform.position).normalized;
                Dir_CloseCombat_Angry = ConfusionDir(Dir_CloseCombat_Angry, 30.0f);
                SetDirector(_mTool.MainVector2(Dir_CloseCombat_Angry));
                isMove_Angry_CloseCombat = true;
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
            case SubState.Angry_ChargePunch:
                float LsatTargetPosition_Predict_ChargePunch_Angle02_Angry = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                float PredictAngle_Angry = 2.0f * LsatTargetPosition_Predict_ChargePunch_Angle02_Angry - LsatTargetPosition_Predict_Angry_ChargePunch_Angle;
                Dir_ChargePunch_Angry = Quaternion.AngleAxis(PredictAngle_Angry, Vector3.forward) * Vector3.right;
                Dir_ChargePunch_Angry = ConfusionDir(Dir_ChargePunch_Angry, 30.0f);
                SetDirector(_mTool.MainVector2(Dir_ChargePunch_Angry));
                isMove_Angry_ChargePunch = true;
                if (IsSuperAngryState)
                {
                    Vector2 dirR = (Quaternion.AngleAxis(90, Vector3.forward) * Dir_ChargePunch_Angry).normalized;
                    LunchThuderByVector(transform.position, Dir_ChargePunch_Angry, 0.03f, 2.0f, 20, Vector2.zero, 0.0f);
                    LunchThuderByVector(transform.position, Dir_ChargePunch_Angry, 0.03f, 2.0f, 20, dirR * 2.0f, 0.15f);
                    LunchThuderByVector(transform.position, Dir_ChargePunch_Angry, 0.03f, 2.0f, 20, -dirR * 2.0f, 0.15f);
                }
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.025f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
            case SubState.Angry_StarPunch:
                float LsatTargetPosition_Predict_StarPunch_Angle02_Angry = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                float PredictAngle_Angry_StarPunch = 3.0f * LsatTargetPosition_Predict_StarPunch_Angle02_Angry - 2.0f * LsatTargetPosition_Predict_Angry_StarPunch_Angle;
                Dir_Lunch_StartPunch = Quaternion.AngleAxis(PredictAngle_Angry_StarPunch, Vector3.forward) * Vector3.right;
                Dir_Lunch_StartPunch = ConfusionDir(Dir_Lunch_StartPunch, 30.0f);
                //Debug.Log(LsatTargetPosition_Predict_Angry_StarPunch_Angle + "+" + LsatTargetPosition_Predict_StarPunch_Angle02_Angry + "+" + PredictAngle_Angry_StarPunch);
                SetDirector(_mTool.MainVector2(Dir_Lunch_StartPunch));
                break;
            case SubState.Angry_SuperChargePunch:
                float LsatTargetPosition_Predict_SuperChargePunch_Angle02_Angry = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                float PredictAngle_Angry_SuperChargePunch = 2.0f * LsatTargetPosition_Predict_SuperChargePunch_Angle02_Angry - LsatTargetPosition_Predict_Angry_SuperChargePunch_Angle;
                Dir_SuperChargePunch_Angry = Quaternion.AngleAxis(PredictAngle_Angry_SuperChargePunch, Vector3.forward) * Vector3.right;
                Dir_SuperChargePunch_Angry = ConfusionDir(Dir_SuperChargePunch_Angry, 30.0f);
                SetDirector(_mTool.MainVector2(Dir_SuperChargePunch_Angry));
                isMove_Angry_SuperChargePunch = true;
                if (IsSuperAngryState)
                {
                    Vector2 dirR = (Quaternion.AngleAxis(90, Vector3.forward) * Dir_ChargePunch_Angry).normalized;
                    LunchThuderByVector(transform.position, Dir_SuperChargePunch_Angry, 0.03f, 2.0f, 20, Vector2.zero, 0.0f);
                    LunchThuderByVector(transform.position, Dir_SuperChargePunch_Angry, 0.03f, 2.0f, 20, dirR * 2.0f, 0.15f);
                    LunchThuderByVector(transform.position, Dir_SuperChargePunch_Angry, 0.03f, 2.0f, 20, -dirR * 2.0f, 0.15f);
                    LunchThuderByVector(transform.position, Dir_SuperChargePunch_Angry, 0.03f, 2.0f, 20, -dirR * 4.0f, 0.3f);
                    LunchThuderByVector(transform.position, Dir_SuperChargePunch_Angry, 0.03f, 2.0f, 20, -dirR * 4.0f, 0.3f);
                }
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.025f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                break;
        }
    }
    /// <summary>
    /// 重拳作为动作事件结束
    /// </summary>
    public void AnimatorEvent_HeavyPunch_Over()
    {
        switch (NowSubState)
        {
            case SubState.Normal_CloseCombat:
                isMove_Normal_CloseCombat = false;
                Normal_CloseCombatTimer = 0;
                InstantiateRunDust();
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
            case SubState.Normal_ChargePunch:
                isMove_Normal_ChargePunch = false;
                Normal_ChargePunchTimer = 0;
                InstantiateRunDust();
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
            case SubState.Angry_CloseCombat:
                isMove_Angry_CloseCombat = false;
                Angry_CloseCombatTimer = 0;
                InstantiateRunDust();
                if (IsSuperAngryState)
                {
                    LunchThuderByVector_90(transform.position,(Vector2.up + Vector2.right).normalized, 0.15f, 2.0f, 14 , 1.0f);
                    LunchThuderByVector_90(transform.position, (Vector2.up + Vector2.left).normalized, 0.15f, 2.0f, 14, 1.0f);
                }
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
            case SubState.Angry_ChargePunch:
                isMove_Angry_ChargePunch = false;
                Angry_ChargePunchTimer = 0;
                InstantiateRunDust();
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
            case SubState.Angry_StarPunch:
                Angry_StarPunchTimer = 0;
                break;
            case SubState.Angry_SuperChargePunch:
                isMove_Angry_SuperChargePunch = false;
                Angry_SuperChargePunchTimer = 0;
                InstantiateRunDust();
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                break;
        }
    }
    /// <summary>
    /// 重拳作为动画结束
    /// </summary>
    public void AnimatorEvent_HeavyPunch_Out()
    {
        switch (NowSubState)
        {
            case SubState.Normal_CloseCombat:
                Normal_CloseCombatOver();
                Normal_IdleStart(TIME_NORMAL_IDLE_COMBATONE);
                break;
            case SubState.Normal_ChargePunch:
                Normal_ChargePunchOver();
                Normal_IdleStart(TIME_NORMAL_IDLE_COMBATTHREE);
                break;
            case SubState.Angry_CloseCombat:

                switch (NowCombat_Angry)
                {
                    case COMBATROUND.ANGRY_COMBAT11:
                        //路线11 长时间追击后触发（50%）三练拳近身战*2
                        Angry_CloseCombatOver();
                        Count_Combat_11_CloseCombat++;
                        if (Count_Combat_11_CloseCombat >= 2)
                        {
                            Angry_IdleStart(TIME_ANGRY_IDLE_COMBAT11);
                            Count_Combat_11_CloseCombat = 0;
                        }
                        else
                        {
                            Angry_TriPunchStart();
                        }
                        break;
                    case COMBATROUND.ANGRY_COMBAT21:
                        Angry_CloseCombatOver();
                        Angry_IdleStart(TIME_ANGRY_IDLE_COMBAT21);
                        Count_Combat_21_ORaPunch = 0;
                        break;
                    default:
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
            case SubState.Angry_ChargePunch:
                switch (NowCombat_Angry)
                {
                    case COMBATROUND.ANGRY_COMBAT22:
                        //路线22 接近后触发（50%）连打蓄力拳连打蓄力拳 连招
                        //根据连招顺序决定下一状态 Count_Combat_22_ORaPunch
                        Angry_ChargePunchOver();
                        Count_Combat_22_ChargePunch++;
                        switch (Count_Combat_22_ChargePunch)
                        {
                            case 1://连打0蓄力拳1 → 连打1蓄力拳2
                                Angry_OraPunchStart();
                                break;
                            case 2://连打0蓄力拳1连打1蓄力拳2 → 
                                Angry_IdleStart(TIME_ANGRY_IDLE_COMBAT22);
                                Count_Combat_22_ChargePunch = 0;
                                break;
                        }
                        break;
                    case COMBATROUND.ANGRY_COMBAT32:
                        //路线32 距离远后触发（50%）蓄力拳蓄力拳蓄力拳大跳大吼
                        //根据连招顺序决定下一状态 Count_Combat_32_ChargePunch
                        Angry_ChargePunchOver();
                        Count_Combat_32_ChargePunch++;
                        switch (Count_Combat_32_ChargePunch)
                        {
                            case 1://蓄力拳1 → 蓄力拳2蓄力拳3大跳3大吼3
                                Angry_ChargePunchStart();
                                break;
                            case 2://蓄力拳1蓄力拳2 → 蓄力拳3大跳3大吼3
                                Angry_ChargePunchStart();
                                break;
                            case 3://蓄力拳1蓄力拳2蓄力拳3 → 大跳3大吼3
                                Angry_BigJumpStart();
                                break;
                        }
                        break;
                    default:
                        Angry_ChargePunchOver();
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
            case SubState.Angry_StarPunch:
                switch (NowCombat_Angry)
                {
                    case COMBATROUND.ANGRY_COMBAT31:
                        //路线31 距离远后触发（50%）流星拳波1流星拳波2流星拳波3超级蓄力拳（真气拳）3
                        //根据连招顺序决定下一状态 Count_Combat_31_StarPunch
                        Angry_StarPunchOver();
                        Count_Combat_31_StarPunch++;
                        switch (Count_Combat_31_StarPunch)
                        {
                            case 1://流星拳波1 → 流星拳波2流星拳波3超级蓄力拳（真气拳）3
                                Angry_StarPunchStart();
                                break;
                            case 2://流星拳波1流星拳波2 → 流星拳波3超级蓄力拳（真气拳）3
                                Angry_StarPunchStart();
                                break;
                            case 3://流星拳波1流星拳波2流星拳波3 → 超级蓄力拳（真气拳）3
                                Angry_SuperChargePunchStart();
                                break;
                        }
                        break;
                    default:
                        Angry_StarPunchOver();
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
            case SubState.Angry_SuperChargePunch:
                switch (NowCombat_Angry)
                {
                    case COMBATROUND.ANGRY_COMBAT31:
                        //路线31 距离远后触发（50%）流星拳波流星拳波流星拳波超级蓄力拳（真气拳）
                        Angry_ChargePunchOver();
                        Count_Combat_31_StarPunch = 0;
                        Angry_IdleStart(TIME_ANGRY_IDLE_COMBAT31);
                        break;
                    default:
                        Angry_ChargePunchOver();
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
        }
    }


    //==■==■==■==■==■==■==■动画机事件：重拳■==■==■==■==■==■==■==







    //==■==■==■==■==■==■==■动画机事件：吼叫■==■==■==■==■==■==■==


    /// <summary>
    /// 吼叫作为动画开始
    /// </summary>
    public void AnimatorEvent_Roar_In()
    {
        SetDirector(Vector2.down);
    }
    /// <summary>
    /// 吼叫作为动作事件开始
    /// </summary>
    public void AnimatorEvent_Roar_Start()
    {
        switch (NowSubState)
        {
            case SubState.Normal_SmallRoar:
                isStart_Normal_Roar = true;
                Normal_SmallRoarTimer = 0;
                LunchSmallRoar();
                break;
            case SubState.Normal_BigRoar:
                isStart_Normal_BigRoar = true;
                Normal_BigRoarTimer = 0;
                ParentPokemonRoom.CameraShake(TIME_NORMAL_BIGROAR + 0.2f, 3.8f, true);
                LunchBigRoar();
                break;
            case SubState.Angry_SmallRoar:
                isStart_Angry_Roar = true;
                Angry_SmallRoarTimer = 0;
                LunchSmallRoar();
                break;
            case SubState.Angry_BigRoar:
                isStart_Angry_BigRoar = true;
                Angry_BigRoarTimer = 0;
                ParentPokemonRoom.CameraShake(TIME_ANGRY_BIGROAR + 0.2f, 3.8f, true);
                LunchBigRoar_Angry();
                break;
            case SubState.Angry_MegaRoar:
                isStart_Angry_MegaRoar = true;
                Angry_MegaRoarTimer = 0;
                ParentPokemonRoom.CameraShake(TIME_ANGRY_MEGAROAR + 0.2f, 5.5f, true);
                LunchMegaRoar_Angry();
                break;
        }
    }
    /// <summary>
    /// 吼叫作为动作事件结束
    /// </summary>
    public void AnimatorEvent_Roar_Over()
    {
        switch (NowSubState)
        {
            case SubState.Normal_SmallRoar:
                isStart_Normal_Roar = false;
                Normal_SmallRoarTimer = 0;
                break;
            case SubState.Normal_BigRoar:
                isStart_Normal_BigRoar = false;
                Normal_BigRoarTimer = 0;
                break;
            case SubState.Angry_SmallRoar:
                isStart_Angry_Roar = false;
                Angry_SmallRoarTimer = 0;
                break;
            case SubState.Angry_BigRoar:
                isStart_Angry_BigRoar = false;
                Angry_BigRoarTimer = 0;
                break;
            case SubState.Angry_MegaRoar:
                isStart_Angry_MegaRoar = false;
                Angry_MegaRoarTimer = 0;
                break;
        }

    }
    /// <summary>
    /// 吼叫作为动画结束
    /// </summary>
    public void AnimatorEvent_Roar_Out()
    {
        switch (NowSubState)
        {
            case SubState.Normal_SmallRoar:
                Normal_SmallRoarOver();
                //路线2 接近后触发吼0跳0打1 跳1吼1打2 跳2吼2打3 连招
                //根据连招顺序决定下一状态 Count_Combat_Tow_ORaPunch
                switch (Count_Combat_Tow_ORaPunch)
                {
                    case 0://吼0 → 跳0
                        Normal_SmallJumpStart();
                        break;
                    case 1://吼0跳0打1跳1吼1 → 打2
                        Normal_ORaPunchStart();
                        break;
                    case 2://吼0跳0打1跳1吼1打2跳2吼2 → 打3
                        Normal_ORaPunchStart();
                        break;
                }
                //Normal_IdleStart(TIME_NORMAL_IDLE_START);
                break;
            case SubState.Normal_BigRoar:
                Normal_BigRoarOver();
                //State2Angry();
                Angry_IdleStart(TIME_ANGRY_IDLE_START);
                break;
            case SubState.Angry_SmallRoar:
                Normal_SmallRoarOver();
                switch (NowCombat_Angry)
                {
                    case COMBATROUND.ANGRY_COMBAT21:
                        //路线21 接近后触发（50%）吼跳打 跳吼打 大跳吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
                        //根据连招顺序决定下一状态 Count_Combat_Tow_ORaPunch
                        switch (Count_Combat_21_ORaPunch)
                        {
                            case 0://吼0 → 跳0
                                Angry_SmallJumpStart();
                                break;
                            case 1://吼0跳0打1跳1吼1 → 打2
                                Angry_OraPunchStart();
                                break;
                            case 2://吼0跳0打1跳1吼1打2跳2吼2 → 打3
                                Angry_OraPunchStart();
                                break;
                        }
                        break;

                    default:
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
            case SubState.Angry_BigRoar:
                Angry_BigRoarOver();
                switch (NowCombat_Angry)
                {

                    case COMBATROUND.ANGRY_COMBAT12:
                        //路线12 长时间追击后触发（50%）小跳 小跳 大跳 大吼
                        Angry_IdleStart(TIME_ANGRY_IDLE_COMBAT12);
                        Count_Combat_12_SmallJump = 0;
                        break;
                    case COMBATROUND.ANGRY_COMBAT21:
                        //路线21 接近后触发（50%）吼跳打 跳吼打 大跳吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
                        //根据连招顺序决定下一状态 Count_Combat_Tow_ORaPunch
                        switch (Count_Combat_21_ORaPunch)
                        {
                            case 2://吼0跳0打1跳1吼1打2跳2大吼2 → 打3
                                Angry_OraPunchStart();
                                break;
                            default:
                                Angry_IdleStart(TIME_ANGRY_IDLE_START);
                                break;
                        }
                        break;
                    case COMBATROUND.ANGRY_COMBAT32:
                        //路线32 距离远后触发（50%）蓄力拳蓄力拳蓄力拳大跳 → 大吼
                        Angry_IdleStart(TIME_ANGRY_IDLE_COMBAT32);
                        Count_Combat_32_ChargePunch = 0;
                        break;
                    default:
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
            case SubState.Angry_MegaRoar:
                Angry_MegaRoarOver();
                Angry_IdleStart(TIME_ANGRY_IDLE_START);
                break;
        }
    }

    //==■==■==■==■==■==■==■动画机事件：吼叫■==■==■==■==■==■==■==







    //==■==■==■==■==■==■==■动画机事件：小跳■==■==■==■==■==■==■==


    /// <summary>
    /// 小跳作为动画开始
    /// </summary>
    public void AnimatorEvent_SmallJump_In()
    {

    }
    /// <summary>
    /// 小跳作为动作事件开始
    /// </summary>
    public void AnimatorEvent_SmallJump_Start()
    {
        switch (NowSubState)
        {
            case SubState.Normal_SmallJump:
                isStart_Normal_SmallJump = true;
                Normal_SmallJumpTimer = 0;
                SetTargetPosion_Normal_SmallJump();
                break;
            case SubState.Angry_SmallJump:
                isStart_Angry_SmallJump = true;
                Angry_SmallJumpTimer = 0;
                SetTargetPosion_Angry_SmallJump();
                break;
        }
    }
    /// <summary>
    /// 小跳作为动作事件结束
    /// </summary>
    public void AnimatorEvent_SmallJump_Over()
    {
        switch (NowSubState)
        {
            case SubState.Normal_SmallJump:
                ParentPokemonRoom.CameraShake(0.3f, 2.5f, false);
                isStart_Normal_SmallJump = false;
                Normal_SmallJumpTimer = 0;
                InstantiateRunDust();
                SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
                break;
            case SubState.Angry_SmallJump:
                if (IsSuperAngryState ) {
                    LunchThuderByVector_90(transform.position, (Vector2.up + Vector2.right).normalized, 0.15f, 2.0f, 8, 2.8284f);
                    LunchThuderByVector_90(transform.position, (Vector2.up + Vector2.left).normalized, 0.15f, 2.0f, 8, 2.8284f);
                }
                else { LunchThuder(1, transform.position); }
                ParentPokemonRoom.CameraShake(0.3f, 2.5f, false);
                isStart_Angry_SmallJump = false;
                Angry_SmallJumpTimer = 0;
                InstantiateRunDust();
                SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
                break;
        }
    }
    /// <summary>
    /// 小跳作为动画结束
    /// </summary>
    public void AnimatorEvent_SmallJump_Out()
    {
        switch (NowSubState)
        {
            case SubState.Normal_SmallJump:
                Normal_SmallJumpOver();
                //路线2 接近后触发吼0跳0打1 跳1吼1打2 跳2吼2打3 连招
                //根据连招顺序决定下一状态 Count_Combat_Tow_ORaPunch
                switch (Count_Combat_Tow_ORaPunch)
                {
                    case 0://吼0跳0 → 打1
                        Normal_ORaPunchStart();
                        break;
                    case 1://吼0跳0打1跳1 → 吼1打2跳2
                        //如果玩家已经被吼叫则不触发第二次第三次吼叫
                        if (IsMarked_Player_Normal_SmallRoar)
                        { Normal_ORaPunchStart(); }
                        else
                        { Normal_SmallRoarStart(); }
                        break;
                    case 2://吼0跳0打1跳1吼1打2跳2 → 吼2(→)打3
                        //如果玩家已经被吼叫则不触发第二次第三次吼叫
                        if (IsMarked_Player_Normal_SmallRoar)
                        { Normal_ORaPunchStart(); }
                        else
                        { Normal_SmallRoarStart(); }
                        break;
                }
                //Normal_IdleStart(TIME_NORMAL_IDLE_START);
                break;
            case SubState.Angry_SmallJump:
                Angry_SmallJumpOver();
                switch (NowCombat_Angry) {
                    //路线12 长时间追击后触发（50%）小跳 小跳 大跳 大吼
                    case COMBATROUND.ANGRY_COMBAT12:
                        Count_Combat_12_SmallJump++;
                        if (Count_Combat_12_SmallJump >= 3)
                        {
                            Angry_BigJumpStart();
                        }
                        else
                        {
                            Angry_SmallJumpStart();
                        }
                        break;
                    case COMBATROUND.ANGRY_COMBAT21:
                        //路线21 接近后触发（50%）吼跳打 跳吼打 跳大吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
                        //根据连招顺序决定下一状态 Count_Combat_Tow_ORaPunch
                        switch (Count_Combat_21_ORaPunch)
                        {
                            case 0://吼0跳0 → 打1
                                Angry_OraPunchStart();
                                break;
                            case 1://吼0跳0打1跳1 → 吼1(→)打2跳2
                                   //如果玩家已经被吼叫则不触发第二次第三次吼叫
                                if (IsMarked_Player_Normal_SmallRoar)
                                { Angry_OraPunchStart(); }
                                else
                                { Angry_SmallRoarStart(); }
                                break;
                            case 2://吼0跳0打1跳1吼1打2跳2 → 大吼2打3
                                Angry_BigRoarStart();
                                break;
                        }
                        break;
                    default:
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
        }
    }


    //==■==■==■==■==■==■==■动画机事件：小跳■==■==■==■==■==■==■==







    //==■==■==■==■==■==■==■动画机事件：大跳■==■==■==■==■==■==■==


    /// <summary>
    /// 大跳作为动画开始
    /// </summary>
    public void AnimatorEvent_BigJump_In()
    {

    }
    /// <summary>
    /// 大跳作为动作事件开始
    /// </summary>
    public void AnimatorEvent_BigJump_Start()
    {
        switch (NowSubState)
        {
            case SubState.Angry_BigJump:
                isStart_Angry_BigJump = true;
                Angry_BigJumpTimer = 0;
                break;
        }
    }
    /// <summary>
    /// 大跳作为动作事件结束
    /// </summary>
    public void AnimatorEvent_BigJump_Over()
    {
        switch (NowSubState)
        {
            case SubState.Angry_BigJump:
                ParentPokemonRoom.CameraShake(0.8f, 4.5f, false);
                isStart_Angry_BigJump = false;
                Angry_BigJumpTimer = 0;
                InstantiateRunDust();
                LunchEarthquake();
                break;
        }
    }
    /// <summary>
    /// 大跳作为动画结束
    /// </summary>
    public void AnimatorEvent_BigJump_Out()
    {
        switch (NowSubState)
        {
            case SubState.Angry_BigJump:
                Angry_BigJumpOver();
                switch (NowCombat_Angry)
                {
                    //路线12 长时间追击后触发（50%）小跳 小跳 大跳 大吼
                    case COMBATROUND.ANGRY_COMBAT12:
                        //Angry_IdleStart(TIME_ANGRY_IDLE_COMBAT12);
                        Angry_BigRoarStart();
                        break;
                    case COMBATROUND.ANGRY_COMBAT32:
                        //路线32 距离远后触发（50%）蓄力拳蓄力拳蓄力拳大跳 → 大吼
                        Angry_BigRoarStart();
                        break;
                    default:
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
        }
    }


    //==■==■==■==■==■==■==■动画机事件：大跳■==■==■==■==■==■==■==




    //■■■■■■■■■■■■■■■■■■■■动画机事件■■■■■■■■■■■■■■■■■■■■■■












































    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //==■==■==■==■==■==■==■主状态：一般_0状态■==■==■==■==■==■==■==


    /// <summary>
    /// 下一个状态
    /// </summary>
    //SubState NextState_Normal;

    /// <summary>
    /// 连招路线2发动连打的次数
    /// </summary>
    int Count_Combat_Tow_ORaPunch = 0;





    //由一般状态转为愤怒状态的血线
    static float HP_NORMAL2ANGRY = 0.75f;//todo;


    //路线1 长时间追击后触发三练拳近身战
    //路线2 接近后触发吼0跳0打1 跳1吼1打2 跳2吼2打3 连招
    //路线3 距离远后触发蓄力拳

    //发动连招1需要的时间
    static float TIME_NORMAL_COMBAT_ONE = 7.0f/*7.0f*/;


    //发动连招2需要的距离
    static float DISTENCE_NORMAL_COMBAT_TOW = 3.0f;
    //发动连招2长时间易触发)需要的时间
    static float TIME_NORMAL_COMBAT_EASYMODE_TOW = 5.0f;


    //发动连招3需要的距离
    static float DISTENCE_NORMAL_COMBAT_THREE = 12.0f/*12.0f*/;

    //发动连招3长时间易触发)需要的时间
    static float TIME_NORMAL_COMBAT_EASYMODE_THREE = 5.0f;






    //=========================一般_发呆_0============================


    //开始后的冷却时间
    static float TIME_NORMAL_IDLE_START = 0.5f; //TODO需修改时间

    //一般_连招1后的冷却时间
    static float TIME_NORMAL_IDLE_COMBATONE = 2.8f; //TODO需修改时间

    //一般_连招2后的冷却时间
    static float TIME_NORMAL_IDLE_COMBATTWO = 4.3f; //TODO需修改时间

    //一般_连招3后的冷却时间
    static float TIME_NORMAL_IDLE_COMBATTHREE = 1.5f; //TODO需修改时间





    /// <summary>
    /// 一般_发呆_0计时器
    /// <summary>
    float Normal_IdleTimer = 0;



    /// <summary>
    /// 一般_发呆_0开始
    /// <summary>
    public void Normal_IdleStart(float Timer)
    {
        Normal_IdleTimer = Timer;
        ChangeSubState(SubState.Normal_Idle);
        Count_Combat_Tow_ORaPunch = 0;
    }

    /// <summary>
    /// 一般_发呆_0结束
    /// <summary>
    public void Normal_IdleOver()
    {
        Normal_IdleTimer = 0;
        Count_Combat_Tow_ORaPunch = 0;
    }


    //=========================一般_发呆_0============================






    //=========================一般_奔跑追踪_1============================


    /// <summary>
    /// 一般_奔跑追踪_1计时器
    /// <summary>
    float Normal_RunTimer = 0;



    /// <summary>
    /// 一般_奔跑追踪_1开始
    /// <summary>
    public void Normal_RunStart()
    {
        Normal_RunTimer = 0;
        ChangeSubState(SubState.Normal_Run);
    }

    /// <summary>
    /// 一般_奔跑追踪_1结束
    /// <summary>
    public void Normal_RunOver()
    {
        Normal_RunTimer = 0;
    }


    //=========================一般_奔跑追踪_1============================






    //=========================一般_连续三练拳_2============================

    //三练拳冲刺速度加成
    static float SPEEDALPHA_NORMAL_TRIPUNCH = 5.5f;


    List<int> PunchOrder = new List<int> { 0, 1, 2 };

    /// <summary>
    /// 一般_连续三练拳_2计时器
    /// <summary>
    //float Normal_TriPunchTimer = 0;

    /// <summary>
    /// 是否处于三练拳冲刺阶段
    /// </summary>
    bool isMove_Normal_TriPunch;

    /// <summary>
    /// 三练拳计数器
    /// </summary>
    int TriPunchCount_Normal;

    /// <summary>
    /// 三练拳冲刺方向
    /// </summary>
    Vector2 Dir_TriPunch_Normal = Vector2.right;




    /// <summary>
    /// 一般_连续三练拳_2开始
    /// <summary>
    public void Normal_TriPunchStart(/*float Timer*/)
    {
        ChangePunch();
        //Normal_TriPunchTimer = Timer;
        ChangeSubState(SubState.Normal_TriPunch);
        animator.SetTrigger("LightPunch");
        RandomPunchOrder();
    }

    /// <summary>
    /// 一般_连续三练拳_2结束
    /// <summary>
    public void Normal_TriPunchOver()
    {
        //Normal_TriPunchTimer = 0;
        Dir_TriPunch_Normal = Vector2.right;
        TriPunchCount_Normal = 0;
        isMove_Normal_TriPunch = false;
    }


    /// <summary>
    /// 随机乱序冰火雷拳序列
    /// </summary>
    void RandomPunchOrder()
    {
        _mTool.RandomShuffleList<int>(PunchOrder);
        _mTool.DebugLogList<int>(PunchOrder);
    }



    void SetTriPunchType()
    {
        if (NowMainState == MainState.Normal)
        {
            switch (PunchOrder[TriPunchCount_Normal - 1])
            {
                case 0:
                    nowPunchType = PunchType.Ice;
                    break;
                case 1:
                    nowPunchType = PunchType.Fire;
                    break;
                case 2:
                    nowPunchType = PunchType.Thunder;
                    break;
            }
        }
        else
        {
            if (IsSuperAngryState == true)
            {
                nowPunchType = PunchType.Thunder;            }
            else
            {
                switch (PunchOrder[TriPunchCount_Angry - 1])
                {
                    case 0:
                        nowPunchType = PunchType.Ice;
                        break;
                    case 1:
                        nowPunchType = PunchType.Fire;
                        break;
                    case 2:
                        nowPunchType = PunchType.Thunder;
                        break;
                }
            }
        }
    }

    //=========================一般_连续三练拳_2============================






    //=========================一般_蓄力拳_3============================


    //蓄力拳冲刺速度加成
    static float SPEEDALPHA_NORMAL_CHARGEPUNCH = 16.0f;

    //蓄力拳蓄力时间
    static float TIME_NORMAL_CHARGEPUNCH_CHARGE = 1.2f;

    //蓄力拳冲刺时间
    static float TIME_NORMAL_CHARGEPUNCH_RUSH = 0.75f;

    /// <summary>
    ///  蓄力拳冲刺方向
    /// </summary>
    Vector2 Dir_ChargePunch_Normal = Vector2.right;

    /// <summary>
    ///  蓄力拳用来预测目标位置的预留目标角度
    /// </summary>
    float LsatTargetPosition_Predict_Normal_ChargePunch_Angle = 0;

    /// <summary>
    /// 一般_蓄力拳_3计时器
    /// <summary>
    float Normal_ChargePunchTimer = 0;

    /// <summary>
    /// 是否处于蓄力拳蓄力阶段
    /// </summary>
    bool isCharge_Normal_ChargePunch = false;

    /// <summary>
    /// 是否处于蓄力拳冲刺阶段
    /// </summary>
    bool isMove_Normal_ChargePunch = false;




    /// <summary>
    /// 一般_蓄力拳_3开始
    /// <summary>
    public void Normal_ChargePunchStart(/*float Timer*/)
    {
        LsatTargetPosition_Predict_Normal_ChargePunch_Angle = 0;
        ChangePunch();
        Normal_ChargePunchTimer = 0;
        ChangeSubState(SubState.Normal_ChargePunch);
        isCharge_Normal_ChargePunch = false;
        isMove_Normal_ChargePunch = false;
        animator.SetInteger("HeavyPunch", 1);
        nowPunchType = PunchType.DynamicPunch;
    }

    /// <summary>
    /// 一般_蓄力拳_3结束
    /// <summary>
    public void Normal_ChargePunchOver()
    {
        Dir_ChargePunch_Normal = Vector2.right;
        LsatTargetPosition_Predict_Normal_ChargePunch_Angle = 0;
        Normal_ChargePunchTimer = 0;
        isCharge_Normal_ChargePunch = false;
        isMove_Normal_ChargePunch = false;
        animator.SetInteger("HeavyPunch", 0);
    }


    //=========================一般_蓄力拳_3============================






    //=========================一般_近身战_4============================


    //近身战冲刺速度加成
    static float SPEEDALPHA_NORMAL_CLOSECOMBT = 8.5f;

    //近身战蓄力时间
    static float TIME_NORMAL_CLOSECOMBT_CHARGE = 0.3f;

    //近身战冲刺时间
    static float TIME_NORMAL_CLOSECOMBT_RUSH = 0.3f;



    /// <summary>
    ///  近身战冲刺方向
    /// </summary>
    Vector2 Dir_CloseCombat_Normal = Vector2.right;

    /// <summary>
    /// 一般_近身战_4计时器
    /// <summary>
    float Normal_CloseCombatTimer = 0;

    /// <summary>
    /// 是否处于近身战蓄力阶段
    /// </summary>
    bool isCharge_Normal_CloseCombat = false;

    /// <summary>
    /// 是否处于近身战冲刺阶段
    /// </summary>
    bool isMove_Normal_CloseCombat = false;




    /// <summary>
    /// 一般_近身战_4开始
    /// <summary>
    public void Normal_CloseCombatStart(/*float Timer*/)
    {
        ChangePunch();
        //Normal_CloseCombatTimer = 0;
        ChangeSubState(SubState.Normal_CloseCombat);
        isCharge_Normal_CloseCombat = false;
        isMove_Normal_CloseCombat = false;
        animator.SetInteger("HeavyPunch", 1);
        nowPunchType = PunchType.NormalFight;
    }

    /// <summary>
    /// 一般_近身战_4结束
    /// <summary>
    public void Normal_CloseCombatOver()
    {
        Dir_CloseCombat_Normal = Vector2.right;
        Normal_CloseCombatTimer = 0;
        isCharge_Normal_CloseCombat = false;
        isMove_Normal_CloseCombat = false;
        animator.SetInteger("HeavyPunch", 0);
    }


    //=========================一般_近身战_4============================






    //=========================一般_快速连打拳_5============================


    //快速连打拳 连打次数
    static int COUNT_NORMAL_ORAPUNCH = 10;

    //快速连打拳冲刺速度加成
    static float SPEEDALPHA_NORMAL_ORAPUNCH_RUSH = 3.2f;



    /// <summary>
    /// 一般_快速连打拳_5计时器
    /// <summary>
    float Normal_ORaPunchTimer = 0;

    /// <summary>
    /// 快速连打拳计数器
    /// </summary>
    int ORaPunchCount_Normal = 0;

    /// <summary>
    /// 快速连打拳冲刺方向
    /// </summary>
    Vector2 Dir_Normal_ORaPunch_Rush = Vector2.right;

    /// <summary>
    /// 快速连打拳是否开始冲刺
    /// </summary>
    bool isMove_Normal_ORaPunch_Rush = false;




    /// <summary>
    /// 一般_快速连打拳_5开始
    /// <summary>
    public void Normal_ORaPunchStart(/*float Timer*/)
    {
        //Debug.Log("ElectivireORa");
        ChangePunch();
        Normal_ORaPunchTimer = 0;
        ChangeSubState(SubState.Normal_ORaPunch);
        animator.SetTrigger("ORaPunch");
        //Debug.Log(animator.speed);
        ORaPunchCount_Normal = 0;
        Dir_Normal_ORaPunch_Rush = Vector2.right;
        isMove_Normal_ORaPunch_Rush = false;
        nowPunchType = PunchType.NormalFight;
    }

    /// <summary>
    /// 一般_快速连打拳_5结束
    /// <summary>
    public void Normal_ORaPunchOver()
    {
        Normal_ORaPunchTimer = 0;
        //Debug.Log(animator.speed);
        ORaPunchCount_Normal = 0;
        Dir_Normal_ORaPunch_Rush = Vector2.right;
        isMove_Normal_ORaPunch_Rush = false;
    }


    //=========================一般_快速连打拳_5============================






    //=========================一般_小吼叫_6============================




    //小吼叫时间
    static float TIME_NORMAL_SMALLROAR = 0.4f;

    //小吼叫实例生成位置偏移量（电击魔兽嘴的位置）
    static Vector2 POSITION_NORMAL_SMALLROAR_YALPHA = new Vector2(0.0f, 1.75f);



    //小吼叫预制件
    public ElectivireSmallRoar NormalSmallRoarPrefab;

    //小吼叫实例
    ElectivireSmallRoar NormalSmallRoarObj;

    //极怒小吼叫预制件
    public ElectivireBigRoar SuperAngryRoarPrefab;

    //极怒小吼叫实例
    ElectivireBigRoar SuperAngrySmallRoarObj;



    /// <summary>
    /// 一般_小吼叫_6计时器
    /// <summary>
    float Normal_SmallRoarTimer = 0;

    /// <summary>
    /// 是否开始吼叫
    /// </summary>
    bool isStart_Normal_Roar;

    /// <summary>
    /// 玩家是否被施加小吼叫(一般)状态
    /// </summary>
    bool IsMarked_Player_Normal_SmallRoar
    {
        get { return (player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom) < -1; }
        //set { isMarked_Player_SmallRoar = value; }
    }
    //public bool isMarked_Player_SmallRoar = false;//TODO



    /// <summary>
    /// 一般_小吼叫_6开始
    /// <summary>
    public void Normal_SmallRoarStart(/*float Timer*/)
    {
        animator.SetInteger("Roar", 1);
        Normal_SmallRoarTimer = 0;
        ChangeSubState(SubState.Normal_SmallRoar);

    }

    /// <summary>
    /// 一般_小吼叫_6结束
    /// <summary>
    public void Normal_SmallRoarOver()
    {
        Normal_SmallRoarTimer = 0;
        isStart_Normal_Roar = false;

    }

    void LunchSmallRoar()
    {

        if (NowMainState == MainState.Angry && IsSuperAngryState)
        {
            SuperAngrySmallRoarObj = Instantiate(SuperAngryRoarPrefab, transform.position + (Vector3)POSITION_NORMAL_SMALLROAR_YALPHA, Quaternion.identity, transform);
            SuperAngrySmallRoarObj.ParentElectivire = this;
        }
        else
        {
            NormalSmallRoarObj = Instantiate(NormalSmallRoarPrefab, transform.position + (Vector3)POSITION_NORMAL_SMALLROAR_YALPHA, Quaternion.identity, transform);
            NormalSmallRoarObj.ParentElectivire = this;
        }
    }


    /// <summary>
    /// 标记玩家被吼叫
    /// </summary>
    public void MarkPlayerBeSmallRoar(PlayerControler MarkedPlayer)
    {
        //isMarked_Player_SmallRoar = true;
        Debug.Log("Roar");
        MarkedPlayer.playerData.DefBounsJustOneRoom--;
        MarkedPlayer.ReFreshAbllityPoint();
    }



    //=========================一般_小吼叫_6============================






    //=========================一般_小跳_7============================


    //小跳时间
    static float TIME_NORMAL_SMALLJUMP = 0.2f;

    //小跳最大半径
    static float RADIUS_NORMAL_SMALLJUMP_MAX = 7.0f;

    //小跳跳至玩家背后的距离
    static float DISTENCE_NORMAL_SMALLJUMP_BACKSTAB = 1.7f;



    /// <summary>
    /// 是否开始小跳
    /// </summary>
    bool isStart_Normal_SmallJump = false;

    /// <summary>
    /// 一般_小跳_7计时器
    /// <summary>
    float Normal_SmallJumpTimer = 0;

    /// <summary>
    /// 小跳目标位置
    /// </summary>
    Vector2 TargetPosion_Normal_SmallJump = Vector2.zero;

    /// <summary>
    /// 小跳起始位置
    /// </summary>
    Vector2 StartPosion_Normal_SmallJump = Vector2.zero;



    /// <summary>
    /// 一般_小跳_7开始
    /// <summary>
    public void Normal_SmallJumpStart(/*float Timer*/)
    {
        animator.SetInteger("SmallJump", 1);
        Normal_SmallJumpTimer = 0;
        ChangeSubState(SubState.Normal_SmallJump);
    }

    /// <summary>
    /// 一般_小跳_7结束
    /// <summary>
    public void Normal_SmallJumpOver()
    {
        isStart_Normal_SmallJump = false;
        TargetPosion_Normal_SmallJump = Vector2.zero;
        StartPosion_Normal_SmallJump = Vector2.zero;
        Normal_SmallJumpTimer = 0;
    }

    /// <summary>
    /// 获取一般小跳目标位置
    /// </summary>
    void SetTargetPosion_Normal_SmallJump()
    {
        Vector2 TargetDir = (TargetPosition - (Vector2)transform.position).normalized;
        float distence = Mathf.Clamp(Vector2.Distance(TargetPosition, (Vector2)transform.position) + DISTENCE_NORMAL_SMALLJUMP_BACKSTAB, DISTENCE_NORMAL_SMALLJUMP_BACKSTAB, RADIUS_NORMAL_SMALLJUMP_MAX);
        StartPosion_Normal_SmallJump = transform.position;
        TargetPosion_Normal_SmallJump = StartPosion_Normal_SmallJump + distence * TargetDir;

        //Debug.Log("Self" + (Vector2)transform.position);
        //Debug.Log("TargetPosition"+TargetPosition);
        //Debug.Log("TargetDir" + TargetDir);
        //Debug.Log("TargetPosion_SmallJump"+TargetPosion_SmallJump);
        //Debug.Log("Distence" + distence);
    }

    //=========================一般_小跳_7============================






    //=========================一般_大吼叫_8============================



    //大吼叫时间
    static float TIME_NORMAL_BIGROAR = 1.0f;


    //大吼叫预制件
    public ElectivireBigRoar NormalBigRoarPrefabs;

    //大吼叫实例
    ElectivireBigRoar NormalBigRoarObj;




    /// <summary>
    /// 是否开始大吼
    /// </summary>
    bool isStart_Normal_BigRoar = false;

    /// <summary>
    /// 一般_大吼叫_8计时器
    /// <summary>
    float Normal_BigRoarTimer = 0;



    /// <summary>
    /// 一般_大吼叫_8开始
    /// <summary>
    public void Normal_BigRoarStart(/*float Timer*/)
    {
        Normal_BigRoarTimer = 0;
        animator.SetInteger("Roar", 1);
        ChangeSubState(SubState.Normal_BigRoar);
        IsDefStateByNormal = true;
        DefChange(1,0);
    }

    /// <summary>
    /// 一般_大吼叫_8结束
    /// <summary>
    public void Normal_BigRoarOver()
    {
        Normal_BigRoarTimer = 0;
        isStart_Normal_BigRoar = false;
        IsDefStateByNormal = false;
    }


    void LunchBigRoar()
    {
        NormalBigRoarObj = Instantiate(NormalBigRoarPrefabs, transform.position + (Vector3)POSITION_NORMAL_SMALLROAR_YALPHA, Quaternion.identity, transform);
        NormalBigRoarObj.ParentElectivire = this;
    }


    //=========================一般_大吼叫_8============================



    //==■==■==■==■==■==■==■主状态：一般_0状态■==■==■==■==■==■==■==










































    //==■==■==■==■==■==■==■主状态：愤怒_1状态■==■==■==■==■==■==■==






    /// <summary>
    /// 下一个状态
    /// </summary>
    //SubState NextState_Angry;


    /// <summary>
    /// 连招路线
    /// 路线11 长时间追击后触发（50%）三练拳近身战*2
    /// 路线12 长时间追击后触发（50%）小跳 小跳 大跳 大吼
    /// 路线21 接近后触发（50%）吼跳打 跳吼打 大跳吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
    /// 路线22 接近后触发（50%）连打蓄力拳连打蓄力拳 连招
    /// 路线31 距离远后触发（50%）流星拳波流星拳波流星拳波超级蓄力拳（真气拳）
    /// 路线32 距离远后触发（50%）蓄力拳蓄力拳蓄力拳大跳大吼
    /// </summary>
    enum COMBATROUND
    {
        None,
        ANGRY_COMBAT11,    // 路线11 长时间追击后触发（50%）三练拳近身战*2
        ANGRY_COMBAT12,    // 路线12 长时间追击后触发（50%）小跳 小跳 大跳 大吼
        ANGRY_COMBAT21,    // 路线21 接近后触发（50%）吼跳打 跳吼打 大跳吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
        ANGRY_COMBAT22,    // 路线22 接近后触发（50%）连打蓄力拳连打蓄力拳 连招
        ANGRY_COMBAT31,    // 路线31 距离远后触发（50%）流星拳波流星拳波流星拳波超级蓄力拳（真气拳）
        ANGRY_COMBAT32,    // 路线32 距离远后触发（50%）蓄力拳蓄力拳蓄力拳大跳大吼
        SUPERANGRY_ROAR,   // 狂怒爆吼
    }
    COMBATROUND NowCombat_Angry = COMBATROUND.None;




    /// <summary>
    /// 连招路线11发动近身战的次数
    /// </summary>
    int Count_Combat_11_CloseCombat = 0;

    /// <summary>
    /// 连招路线12发动小跳的次数
    /// </summary>
    int Count_Combat_12_SmallJump = 0;

    /// <summary>
    /// 连招路线21发动连打的次数
    /// </summary>
    int Count_Combat_21_ORaPunch = 0;

    /// <summary>
    /// 连招路线22发动连打的次数
    /// </summary>
    int Count_Combat_22_ChargePunch = 0;

    /// <summary>
    /// 连招路线31发动流星拳的次数
    /// </summary>
    int Count_Combat_31_StarPunch = 0;

    /// <summary>
    /// 连招路线32发动蓄力拳的次数
    /// </summary>
    int Count_Combat_32_ChargePunch = 0;


    //由愤怒状态转为狂怒状态的血线
    static float HP_ANGRY2SUPERANGRY = 0.5f;//todo;





    //发动连招10需要的时间
    static float TIME_ANGRY_COMBAT_ONE = 4.8f/*7.0f*/;


    //发动连招20需要的距离
    static float DISTENCE_ANGRY_COMBAT_TOW = 3.0f;
    //发动连招20长时间易触发)需要的时间
    static float TIME_ANGRY_COMBAT_EASYMODE_TOW = 3.0f;


    //发动连招30需要的距离
    static float DISTENCE_ANGRY_COMBAT_THREE = 12.0f/*12.0f*/;

    //发动连招30长时间易触发)需要的时间
    static float TIME_ANGRY_COMBAT_EASYMODE_THREE = 3.0f;







    //=====================================极怒模式=====================================


    /// <summary>
    /// 极怒模式自伤的间隔
    /// </summary>
    static float TIME_SUPERANGRY_SELFDMAGE_INTERVAL = 3.0f;


    /// <summary>
    /// 极怒模式自伤的间隔
    /// </summary>
    static float TIME_SUPERANGRY_RUN_RANDOMTHUNDER_INTERVAL = 2.0f;



    /// <summary>
    /// 开启红眼
    /// </summary>
    public void StartRedEyes()
    {
        RedEyes.gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭红眼
    /// </summary>
    public void StopRedEyes()
    {
        RedEyes.gameObject.SetActive(false);
    }







    IEnumerator StartSuperAngrySelfdmage()
    {
        while (IsSuperAngryState)
        {
            Pokemon.PokemonHpChange(null , this.gameObject , 1 , 0 , 0 , PokemonType.TypeEnum.IgnoreType);
            yield return new WaitForSeconds(TIME_SUPERANGRY_SELFDMAGE_INTERVAL); // 等待间隔时间
        }
    }



    IEnumerator SuperAngryRunRandomThunder()
    {
        while (true)
        {
            if (IsSuperAngryState && NowSubState == SubState.Angry_Run)
            {
                ElectivireLightningStrikeManger m = Instantiate(LightningStrikeManger, transform.position, Quaternion.identity);
                m.ParentElectivire = this;
                m.transform.position = ParentPokemonRoom.transform.position;
                m.RandomSetThunder(4, this, 8.2f , 0.1f);
            }
            yield return new WaitForSeconds(TIME_SUPERANGRY_RUN_RANDOMTHUNDER_INTERVAL); // 等待间隔时间
        }
    }















    //=========================愤怒_发呆_9============================
    //路线11 长时间追击后触发（50%）三练拳近身战*2
    //路线12 长时间追击后触发（50%）小跳 小跳 大跳 大吼
    //路线21 接近后触发（50%）吼跳打 跳吼打 大跳吼快打 连招（距离远触发小爆裂拳（近身战） 距离不远不触发（快速休息））
    //路线22 接近后触发（50%）连打蓄力拳连打蓄力拳 连招
    //路线31 距离远后触发（50%）流星拳波流星拳波流星拳波超级蓄力拳（真气拳）
    //路线32 距离远后触发（50%）蓄力拳蓄力拳蓄力拳大跳大吼

    //开始后的冷却时间
    static float TIME_ANGRY_IDLE_START = 0.5f; //TODO需修改时间

    //一般_连招11后的冷却时间
    static float TIME_ANGRY_IDLE_COMBAT11 = 2.0f; //TODO需修改时间

    //一般_连招12后的冷却时间
    static float TIME_ANGRY_IDLE_COMBAT12 = 3.5f; //TODO需修改时间

    //一般_连招21后的冷却时间
    static float TIME_ANGRY_IDLE_COMBAT21 = 6.2f; //TODO需修改时间

    //一般_连招21后的（快速）冷却时间
    static float TIME_ANGRY_IDLE_COMBAT21_FAST = 3.5f; //TODO需修改时间

    //一般_连招22后的冷却时间
    static float TIME_ANGRY_IDLE_COMBAT22 = 2.5f; //TODO需修改时间

    //一般_连招31后的冷却时间
    static float TIME_ANGRY_IDLE_COMBAT31 = 3.5f; //TODO需修改时间

    //一般_连招32后的冷却时间
    static float TIME_ANGRY_IDLE_COMBAT32 = 3.5f; //TODO需修改时间





    //一般_连招21后发动近身战需要的距离
    static float DISTENCE_ANGRY_COMBAT_21_LASTCOMBAT = 6.0f/*12.0f*/;


    /// <summary>
    /// 愤怒_发呆_9计时器
    /// <summary>
    float Angry_IdleTimer = 0;

    /// <summary>
    /// 愤怒_发呆_9开始
    /// <summary>
    public void Angry_IdleStart(float Timer)
    {
        Angry_IdleTimer = Timer;
        ChangeSubState(SubState.Angry_Idle);
    }

    /// <summary>
    /// 愤怒_发呆_9结束
    /// <summary>
    public void Angry_IdleOver()
    {
        Angry_IdleTimer = 0;
        NowCombat_Angry = COMBATROUND.None;
        Count_Combat_11_CloseCombat = 0;
        Count_Combat_12_SmallJump = 0;
        Count_Combat_21_ORaPunch = 0;
        Count_Combat_22_ChargePunch = 0;
        Count_Combat_31_StarPunch = 0;
        Count_Combat_32_ChargePunch = 0;
    }


    //=========================愤怒_发呆_9============================






    //=========================愤怒_奔跑追踪_10============================




    /// <summary>
    /// 愤怒_奔跑追踪_10计时器
    /// <summary>
    float Angry_RunTimer = 0;

    /// <summary>
    /// 愤怒_奔跑追踪_10开始
    /// <summary>
    public void Angry_RunStart(/*float Timer*/)
    {
        Angry_RunTimer = 0;
        ChangeSubState(SubState.Angry_Run);
    }

    /// <summary>
    /// 愤怒_奔跑追踪_10结束
    /// <summary>
    public void Angry_RunOver()
    {
        Angry_RunTimer = 0;
    }


    //=========================愤怒_奔跑追踪_10============================






    //=========================愤怒_连续三练拳_11============================

    //三练拳冲刺速度加成
    static float SPEEDALPHA_ANGRY_TRIPUNCH = 6.8f;


    /// <summary>
    /// 愤怒_连续三练拳_11计时器
    /// <summary>
    //float Angry_TriPunchTimer = 0;

    /// <summary>
    /// 是否处于三练拳冲刺阶段
    /// </summary>
    bool isMove_Angry_TriPunch;

    /// <summary>
    /// 三练拳计数器
    /// </summary>
    int TriPunchCount_Angry;

    /// <summary>
    /// 三练拳冲刺方向
    /// </summary>
    Vector2 Dir_TriPunch_Angry = Vector2.right;



    /// <summary>
    /// 愤怒_连续三练拳_11开始
    /// <summary>
    public void Angry_TriPunchStart(/*float Timer*/)
    {
        ChangePunch();
        //Angry_TriPunchTimer = Timer;
        ChangeSubState(SubState.Angry_TriPunch);
        animator.SetTrigger("LightPunch");
        RandomPunchOrder();
    }





    /// <summary>
    /// 愤怒_连续三练拳_11结束
    /// <summary>
    public void Angry_TriPunchOver()
    {
        //Angry_TriPunchTimer = 0;
        Dir_TriPunch_Angry = Vector2.right;
        TriPunchCount_Angry = 0;
        isMove_Angry_TriPunch = false;
    }


    //=========================愤怒_连续三练拳_11============================






    //=========================愤怒_蓄力拳_12============================


    //蓄力拳冲刺速度加成
    static float SPEEDALPHA_ANGRY_CHARGEPUNCH = 16.0f;

    //蓄力拳蓄力时间
    static float TIME_ANGRY_CHARGEPUNCH_CHARGE = 1.2f;

    //蓄力拳冲刺时间
    static float TIME_ANGRY_CHARGEPUNCH_RUSH = 0.75f;




    /// <summary>
    ///  蓄力拳冲刺方向
    /// </summary>
    Vector2 Dir_ChargePunch_Angry = Vector2.right;

    /// <summary>
    ///  蓄力拳用来预测目标位置的预留目标角度
    /// </summary>
    float LsatTargetPosition_Predict_Angry_ChargePunch_Angle = 0;









    /// <summary>
    /// 愤怒_蓄力拳_12计时器
    /// <summary>
    float Angry_ChargePunchTimer = 0;

    /// <summary>
    /// 是否处于蓄力拳蓄力阶段
    /// </summary>
    bool isCharge_Angry_ChargePunch = false;

    /// <summary>
    /// 是否处于蓄力拳冲刺阶段
    /// </summary>
    bool isMove_Angry_ChargePunch = false;



    /// <summary>
    /// 愤怒_蓄力拳_12开始
    /// <summary>
    public void Angry_ChargePunchStart(/*float Timer*/)
    {
        LsatTargetPosition_Predict_Angry_ChargePunch_Angle = 0;
        ChangePunch();
        Angry_ChargePunchTimer = 0;
        ChangeSubState(SubState.Angry_ChargePunch);
        isCharge_Angry_ChargePunch = false;
        isMove_Angry_ChargePunch = false;
        animator.SetInteger("HeavyPunch", 1);
        nowPunchType = PunchType.DynamicPunch;
    }

    /// <summary>
    /// 愤怒_蓄力拳_12结束
    /// <summary>
    public void Angry_ChargePunchOver()
    {
        Dir_ChargePunch_Angry = Vector2.right;
        LsatTargetPosition_Predict_Angry_ChargePunch_Angle = 0;
        Angry_ChargePunchTimer = 0;
        isCharge_Angry_ChargePunch = false;
        isMove_Angry_ChargePunch = false;
        animator.SetInteger("HeavyPunch", 0);
    }


    //=========================愤怒_蓄力拳_12============================















    //========================= 愤怒_近身战_20============================


    //近身战冲刺速度加成
    static float SPEEDALPHA_ANGRY_CLOSECOMBT = 9.2f;

    //近身战蓄力时间
    static float TIME_ANGRY_CLOSECOMBT_CHARGE = 0.18f;

    //近身战冲刺时间
    static float TIME_ANGRY_CLOSECOMBT_RUSH = 0.25f;




    /// <summary>
    ///  近身战冲刺方向
    /// </summary>
    Vector2 Dir_CloseCombat_Angry = Vector2.right;

    /// <summary>
    /// 愤怒_近身战_4计时器
    /// <summary>
    float Angry_CloseCombatTimer = 0;

    /// <summary>
    /// 是否处于近身战蓄力阶段
    /// </summary>
    bool isCharge_Angry_CloseCombat = false;

    /// <summary>
    /// 是否处于近身战冲刺阶段
    /// </summary>
    bool isMove_Angry_CloseCombat = false;




    /// <summary>
    /// 愤怒_近身战_20开始
    /// <summary>
    public void Angry_CloseCombatStart(/*float Timer*/)
    {
        ChangePunch();
        ChangeSubState(SubState.Angry_CloseCombat);
        isCharge_Angry_CloseCombat = false;
        isMove_Angry_CloseCombat = false;
        animator.SetInteger("HeavyPunch", 1);
        if (IsSuperAngryState)
        {
            nowPunchType = PunchType.Thunder;
        }
        else
        {
            nowPunchType = PunchType.NormalFight;

        }
        
    }

    /// <summary>
    /// 愤怒_近身战_20结束
    /// <summary>
    public void Angry_CloseCombatOver()
    {
        Dir_CloseCombat_Angry = Vector2.right;
        Angry_CloseCombatTimer = 0;
        isCharge_Angry_CloseCombat = false;
        isMove_Angry_CloseCombat = false;
        animator.SetInteger("HeavyPunch", 0);
    }


    //=========================愤怒_近身战_20============================















    //=========================愤怒_连续近身拳_13============================



    //快速连打拳 连打次数
    static int COUNT_ANGRY_ORAPUNCH = 10;

    //快速连打拳冲刺速度加成
    static float SPEEDALPHA_ANGRY_ORAPUNCH_RUSH = 3.2f;



    /// <summary>
    /// 愤怒_连续近身拳_13计时器
    /// <summary>
    float Angry_OraPunchTimer = 0;

    /// <summary>
    /// 快速连打拳计数器
    /// </summary>
    int ORaPunchCount_Angry = 0;

    /// <summary>
    /// 快速连打拳冲刺方向
    /// </summary>
    Vector2 Dir_Angry_ORaPunch_Rush = Vector2.right;

    /// <summary>
    /// 快速连打拳是否开始冲刺
    /// </summary>
    bool isMove_Angry_ORaPunch_Rush = false;



    /// <summary>
    /// 愤怒_连续近身拳_13开始
    /// <summary>
    public void Angry_OraPunchStart(/*float Timer*/)
    {
        //Debug.Log("ElectivireORa");
        ChangePunch();
        Angry_OraPunchTimer = 0;
        ChangeSubState(SubState.Angry_OraPunch);
        animator.SetTrigger("ORaPunch");
        //Debug.Log(animator.speed);
        ORaPunchCount_Angry = 0;
        Dir_Angry_ORaPunch_Rush = Vector2.right;
        isMove_Angry_ORaPunch_Rush = false;
        nowPunchType = PunchType.NormalFight;
    }

    /// <summary>
    /// 愤怒_连续近身拳_13结束
    /// <summary>
    public void Angry_OraPunchOver()
    {
        Angry_OraPunchTimer = 0;
        //Debug.Log(animator.speed);
        ORaPunchCount_Angry = 0;
        Dir_Angry_ORaPunch_Rush = Vector2.right;
        isMove_Angry_ORaPunch_Rush = false;
    }


    //=========================愤怒_连续近身拳_13============================






    //=========================愤怒_小吼叫_14============================



    //小吼叫时间
    static float TIME_ANGRY_SMALLROAR = 0.4f;



    /// <summary>
    /// 愤怒_小吼叫_14计时器
    /// <summary>
    float Angry_SmallRoarTimer = 0;

    /// <summary>
    /// 是否开始吼叫
    /// </summary>
    bool isStart_Angry_Roar;



    /// <summary>
    /// 愤怒_小吼叫_14开始
    /// <summary>
    public void Angry_SmallRoarStart(/*float Timer*/)
    {
        animator.SetInteger("Roar", 1);
        Angry_SmallRoarTimer = 0;
        ChangeSubState(SubState.Angry_SmallRoar);
    }

    /// <summary>
    /// 愤怒_小吼叫_14结束
    /// <summary>
    public void Angry_SmallRoarOver()
    {
        Angry_SmallRoarTimer = 0;
        isStart_Angry_Roar = false;
    }


    //=========================愤怒_小吼叫_14============================






    //=========================愤怒_小跳_15============================



    //小跳时间
    static float TIME_ANGRY_SMALLJUMP = 0.175f;

    //小跳最大半径
    static float RADIUS_ANGRY_SMALLJUMP_MAX = 8.5f;

    //小跳跳至玩家背后的距离
    static float DISTENCE_ANGRY_SMALLJUMP_BACKSTAB = 1.7f;



    /// <summary>
    /// 是否开始小跳
    /// </summary>
    bool isStart_Angry_SmallJump = false;

    /// <summary>
    /// 愤怒_小跳_15计时器
    /// <summary>
    float Angry_SmallJumpTimer = 0;

    /// <summary>
    /// 小跳目标位置
    /// </summary>
    Vector2 TargetPosion_Angry_SmallJump = Vector2.zero;

    /// <summary>
    /// 小跳起始位置
    /// </summary>
    Vector2 StartPosion_Angry_SmallJump = Vector2.zero;



    /// <summary>
    /// 愤怒_小跳_15开始
    /// <summary>
    public void Angry_SmallJumpStart(/*float Timer*/)
    {
        animator.SetInteger("SmallJump", 1);
        Angry_SmallJumpTimer = 0;
        ChangeSubState(SubState.Angry_SmallJump);
    }

    /// <summary>
    /// 愤怒_小跳_15结束
    /// <summary>
    public void Angry_SmallJumpOver()
    {
        isStart_Angry_SmallJump = false;
        TargetPosion_Angry_SmallJump = Vector2.zero;
        StartPosion_Angry_SmallJump = Vector2.zero;
        Angry_SmallJumpTimer = 0;
    }


    /// <summary>
    /// 获取愤怒小跳目标位置
    /// </summary>
    void SetTargetPosion_Angry_SmallJump()
    {
        Vector2 TargetDir = (TargetPosition - (Vector2)transform.position).normalized;
        float distence = Mathf.Clamp(Vector2.Distance(TargetPosition, (Vector2)transform.position) + DISTENCE_ANGRY_SMALLJUMP_BACKSTAB, DISTENCE_ANGRY_SMALLJUMP_BACKSTAB, RADIUS_ANGRY_SMALLJUMP_MAX);
        StartPosion_Angry_SmallJump = transform.position;
        TargetPosion_Angry_SmallJump = StartPosion_Angry_SmallJump + distence * TargetDir;

        //Debug.Log("Self" + (Vector2)transform.position);
        //Debug.Log("TargetPosition"+TargetPosition);
        //Debug.Log("TargetDir" + TargetDir);
        //Debug.Log("TargetPosion_SmallJump"+TargetPosion_SmallJump);
        //Debug.Log("Distence" + distence);
    }

    //=========================愤怒_小跳_15============================






    //=========================愤怒_大吼叫_16============================



    //大吼叫时间
    static float TIME_ANGRY_BIGROAR = 1.0f;

    //大吼叫预制件
    public ElectivireBigRoar AngryBigRoarPrefabs;

    //狂怒大吼叫预制件
    public ElectivireBigRoar SuperAngryBigRoarPrefabs;

    //大吼叫实例
    ElectivireBigRoar AngryBigRoarObj;





    /// <summary>
    /// 愤怒_大吼叫_16计时器
    /// <summary>
    float Angry_BigRoarTimer = 0;

    /// <summary>
    /// 是否开始大吼
    /// </summary>
    bool isStart_Angry_BigRoar = false;




    /// <summary>
    /// 愤怒_大吼叫_16开始
    /// <summary>
    public void Angry_BigRoarStart(/*float Timer*/)
    {
        Angry_BigRoarTimer = 0;
        ChangeSubState(SubState.Angry_BigRoar);
        animator.SetInteger("Roar", 1);
    }

    /// <summary>
    /// 愤怒_大吼叫_16结束
    /// <summary>
    public void Angry_BigRoarOver()
    {
        Angry_BigRoarTimer = 0;
        isStart_Angry_BigRoar = false;
    }

    void LunchBigRoar_Angry()
    {
        if(IsSuperAngryState){
            AngryBigRoarObj = Instantiate(SuperAngryBigRoarPrefabs, transform.position + (Vector3)POSITION_NORMAL_SMALLROAR_YALPHA, Quaternion.identity, transform);
            LunchThuder(5, AngryBigRoarObj.transform.position);
        }
        else
        {
            AngryBigRoarObj = Instantiate(AngryBigRoarPrefabs, transform.position + (Vector3)POSITION_NORMAL_SMALLROAR_YALPHA, Quaternion.identity, transform);
        }
        AngryBigRoarObj.ParentElectivire = this;
    }


    //=========================愤怒_大吼叫_16============================






    //=========================愤怒_大跳_17============================




    //大跳最长时间
    static float TIME_ANGRY_BIGJUMP_MAX = 1.0f;

    //大跳跳至玩家头上停止跳跃的距离
    static float DISTENCE_ANGRY_BIGJUMP_MIN = 2.8f;

    //大跳移速加成
    static float SPEEDALPHA_ANGRY_BIGJUMP = 8.0f;



    /// <summary>
    /// 地震预制件
    /// </summary>
    public ElectivireEarthquake eqPrefabs;

    /// <summary>
    /// 地震实例
    /// </summary>
    ElectivireEarthquake eqObj;



    /// <summary>
    /// 愤怒_大跳_16计时器
    /// <summary>
    float Angry_BigJumpTimer = 0;

    /// <summary>
    /// 是否开始大跳
    /// </summary>
    bool isStart_Angry_BigJump = false;




    /// <summary>
    /// 愤怒_大跳_16开始
    /// <summary>
    public void Angry_BigJumpStart(/*float Timer*/)
    {
        animator.SetInteger("BigJump", 1);
        Angry_BigJumpTimer = 0;
        ChangeSubState(SubState.Angry_BigJump);
        isStart_Angry_BigJump = false;
    }

    /// <summary>
    /// 愤怒_大跳_16结束
    /// <summary>
    public void Angry_BigJumpOver()
    {
        isStart_Angry_BigJump = false;
        Angry_BigJumpTimer = 0;
    }


    public void LunchEarthquake()
    {
        eqObj = Instantiate(eqPrefabs, transform.position, Quaternion.identity);
        eqObj.ParentElectivire = this;
    }


    //=========================愤怒_大跳_17============================











    //=========================愤怒_超级蓄力拳_18============================



    //超级蓄力拳冲刺速度加成
    static float SPEEDALPHA_ANGRY_SUPERCHARGEPUNCH = 18.0f;

    //超级蓄力拳蓄力时间
    static float TIME_ANGRY_SUPERCHARGEPUNCH_CHARGE = 0.6f;

    //超级蓄力拳冲刺时间
    static float TIME_ANGRY_SUPERCHARGEPUNCH_RUSH = 0.75f;




    /// <summary>
    ///  超级蓄力拳冲刺方向
    /// </summary>
    Vector2 Dir_SuperChargePunch_Angry = Vector2.right;

    /// <summary>
    ///  超级蓄力拳用来预测目标位置的预留目标角度
    /// </summary>
    float LsatTargetPosition_Predict_Angry_SuperChargePunch_Angle = 0;




    /// <summary>
    /// 愤怒_超级蓄力拳_18计时器
    /// <summary>
    float Angry_SuperChargePunchTimer = 0;

    /// <summary>
    /// 是否处于超级蓄力拳蓄力阶段
    /// </summary>
    bool isCharge_Angry_SuperChargePunch = false;

    /// <summary>
    /// 是否处于超级蓄力拳冲刺阶段
    /// </summary>
    bool isMove_Angry_SuperChargePunch = false;




    /// <summary>
    /// 愤怒_超级蓄力拳_18开始
    /// <summary>
    public void Angry_SuperChargePunchStart(/*float Timer*/)
    {
        LsatTargetPosition_Predict_Angry_SuperChargePunch_Angle = 0;
        ChangePunch();
        Angry_SuperChargePunchTimer = 0;
        ChangeSubState(SubState.Angry_SuperChargePunch);
        isCharge_Angry_SuperChargePunch = false;
        isMove_Angry_SuperChargePunch = false;
        animator.SetInteger("HeavyPunch", 1);
        nowPunchType = PunchType.SuperChargePunch;//TODO 超级追踪拳特效
    }

    /// <summary>
    /// 愤怒_超级蓄力拳_18结束
    /// <summary>
    public void Angry_SuperChargePunchOver()
    {
        Dir_SuperChargePunch_Angry = Vector2.right;
        LsatTargetPosition_Predict_Angry_SuperChargePunch_Angle = 0;
        Angry_SuperChargePunchTimer = 0;
        isCharge_Angry_SuperChargePunch = false;
        isMove_Angry_SuperChargePunch = false;
        animator.SetInteger("HeavyPunch", 0);
    }


    //=========================愤怒_超级蓄力拳_18============================






    //=========================愤怒_流星拳波_19============================




    //流星拳波蓄力时间
    static float TIME_ANGRY_STARPUNCH_CHARGE = 0.2f;

    //流星拳波飞行速度
    static float SPEED_ANGRY_STARPUNCH_PRO = 65.0f;



    /// <summary>
    ///  流星拳波用来预测目标位置的预留目标角度
    /// </summary>
    float LsatTargetPosition_Predict_Angry_StarPunch_Angle = 0;



    /// <summary>
    /// 愤怒_流星拳波_19计时器
    /// <summary>
    float Angry_StarPunchTimer = 0;

    /// <summary>
    /// 是否处于流星拳波蓄力阶段
    /// </summary>
    bool isCharge_Angry_StarPunch = false;

    /// <summary>
    /// 流星拳波发啥角度
    /// </summary>
    Vector2 Dir_Lunch_StartPunch = Vector2.right;




    /// <summary>
    /// 愤怒_流星拳波_19开始
    /// <summary>
    public void Angry_StarPunchStart(/*float Timer*/)
    {
        ChangePunch();
        Angry_StarPunchTimer = 0;
        ChangeSubState(SubState.Angry_StarPunch);
        isCharge_Angry_StarPunch = false;
        Dir_Lunch_StartPunch = Vector2.right;
        animator.SetInteger("HeavyPunch", 1);
        nowPunchType = PunchType.StarPunch;
        LsatTargetPosition_Predict_Angry_StarPunch_Angle = 0;
    }

    /// <summary>
    /// 愤怒_流星拳波_19结束
    /// <summary>
    public void Angry_StarPunchOver()
    {
        Angry_StarPunchTimer = 0;
        isCharge_Angry_StarPunch = false;
        Dir_Lunch_StartPunch = Vector2.right;
        animator.SetInteger("HeavyPunch", 0);
        LsatTargetPosition_Predict_Angry_StarPunch_Angle = 0;
    }


    /// <summary>
    /// 发射流星拳波
    /// </summary>
    public void LunchStarPunch(ElectivireStarPunch punch)
    {
        punch.empty = this;
        if (IsSuperAngryState)
        {
            punch.LaunchNotForce(Dir_Lunch_StartPunch, SPEED_ANGRY_STARPUNCH_PRO);
        }
        else
        {
            punch.LaunchNotForce(Dir_Lunch_StartPunch, SPEED_ANGRY_STARPUNCH_PRO);
        }
    }


    //=========================愤怒_流星拳波_19============================






    //=========================愤怒_转状态大吼_21============================



    //大吼叫时间
    static float TIME_ANGRY_MEGAROAR = 4.0f;

    //大吼叫预制件
    public ElectivireBigRoar AngryMegaRoarPrefabs;

    //大吼叫实例
    ElectivireBigRoar AngryMegaRoarObj;





    /// <summary>
    /// 愤怒_大吼叫_16计时器
    /// <summary>
    float Angry_MegaRoarTimer = 0;

    /// <summary>
    /// 是否开始大吼
    /// </summary>
    bool isStart_Angry_MegaRoar = false;




    /// <summary>
    /// 愤怒_大吼叫_16开始
    /// <summary>
    public void Angry_MegaRoarStart(/*float Timer*/)
    {
        Angry_MegaRoarTimer = 0;
        ChangeSubState(SubState.Angry_MegaRoar);
        animator.SetInteger("Roar", 1);
        superAngry = true;
        IsDefStateByNormal = true;
        StartRedEyes();
        StartCoroutine(StartSuperAngrySelfdmage());
        StartCoroutine(SuperAngryRunRandomThunder());
        DefChange(1, 0);
        AtkChange(1, 0);
    }

    /// <summary>
    /// 愤怒_大吼叫_16结束
    /// <summary>
    public void Angry_MegaRoarOver()
    {
        Angry_MegaRoarTimer = 0;
        isStart_Angry_MegaRoar = false;
        IsDefStateByNormal = false;
    }

    void LunchMegaRoar_Angry()
    {
        AngryMegaRoarObj = Instantiate(AngryMegaRoarPrefabs, transform.position + (Vector3)POSITION_NORMAL_SMALLROAR_YALPHA, Quaternion.identity, transform);
        AngryMegaRoarObj.ParentElectivire = this;
        LunchThuder(4, AngryMegaRoarObj.transform.position);
    }


    //=========================愤怒_转状态大吼_21============================



    //==■==■==■==■==■==■==■主状态：愤怒_1状态■==■==■==■==■==■==■==













    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■





































    //■■■■■■■■■■■■■■■■■■■■闪电生成未知列表■■■■■■■■■■■■■■■■■■■■■■


    /// <summary>
    /// 闪电生成器
    /// </summary>
    public ElectivireLightningStrikeManger LightningStrikeManger;





    //闪电生成在电击魔兽上下左右各一个
    static Dictionary<float, List<Vector2>> ThunderList01 = new Dictionary<float, List<Vector2>> {
        //{ 0.0f , new List<Vector2>{ new Vector2(3,0) , new Vector2(-3, 0) , new Vector2(0, 3), new Vector2(0, -3) } },
        { 0.0f , new List<Vector2>{ new Vector2(2,2) , new Vector2(-2, 2) , new Vector2(2, -2), new Vector2(-2, -2) } },
    };

    //闪电生成圆环 一层
    static Dictionary<float, List<Vector2>> ThunderList02 = new Dictionary<float, List<Vector2>> {
        { 0.2f , _mTool.StepCircleVectorList(8,2.8f,0) },
        //{ 0.2f , _mTool.StepCircleVectorList(12,3.0f,0) },
    };

    //闪电生成圆环 两层
    static Dictionary<float, List<Vector2>> ThunderList03 = new Dictionary<float, List<Vector2>> {
        { 0.25f , _mTool.StepCircleVectorList(8,6.0f,22.5f) },
        { 0.4f , _mTool.StepCircleVectorList(16,8.8f,0) }
    };


    //闪电生成圆环 两层
    static Dictionary<float, List<Vector2>> ThunderList04 = new Dictionary<float, List<Vector2>> {
        { 3.6f , _mTool.StepCircleVectorList(30,10.0f,0) },
        { 3.0f , _mTool.StepCircleVectorList(36,12.5f,0) },
        { 2.4f , _mTool.StepCircleVectorList(40,15.0f,0) },
        { 1.8f , _mTool.StepCircleVectorList(44,17.5f,0) },
        { 1.2f , _mTool.StepCircleVectorList(48,20.0f,0) },
        { 0.6f , _mTool.StepCircleVectorList(52,22.5f,0) },
    };

    //闪电生成圆环 两层
    static Dictionary<float, List<Vector2>> ThunderList05 = new Dictionary<float, List<Vector2>> {
        { 0.2f , _mTool.StepCircleVectorList(24,10.0f ,0) },
    };

    //闪电生成圆环 3层
    static Dictionary<float, List<Vector2>> ThunderList06 = new Dictionary<float, List<Vector2>> {
        { 0.25f , _mTool.StepCircleVectorList(8,6.0f,22.5f) },
        { 0.4f , _mTool.StepCircleVectorList(16,8.8f,0) },
        { 0.55f , _mTool.StepCircleVectorList(20,11.6f,9) },
    };



    /// <summary>
    /// 发射闪电
    /// </summary>
    /// <param name="i"></param>
    /// <param name="p"></param>
    public void LunchThuder(int i , Vector2 p)
    {
        Debug.Log(i);
        ElectivireLightningStrikeManger m = Instantiate(LightningStrikeManger, transform.position, Quaternion.identity);
        m.ParentElectivire = this;
        m.transform.position = p;
        switch (i){
            case 1:
                m.SetThunderList(ThunderList01 , this);
                break;
            case 2:
                m.SetThunderList(ThunderList02, this);
                break;
            case 3:
                m.SetThunderList(ThunderList03, this);
                break;
            case 4:
                m.SetThunderList(ThunderList04, this);
                break;
            case 5:
                m.SetThunderList(ThunderList05, this);
                break;
            case 6:
                m.SetThunderList(ThunderList06, this);
                break;
        }
    }


    /// <summary>
    /// 沿某一方向垂直向两边发射闪电
    /// </summary>
    /// <param name="i"></param>
    /// <param name="p"></param>
    public void LunchThuderByVector_90( Vector2 p , Vector2 dir , float intervalTime, float intervalDis, int maxCount , float StartAlpha)
    {
        ElectivireLightningStrikeManger m = Instantiate(LightningStrikeManger, transform.position, Quaternion.identity);
        m.ParentElectivire = this;
        m.transform.position = p;

        dir = dir.normalized;
        Vector2 dirL = Quaternion.AngleAxis(90, Vector3.forward) * dir;
        Vector2 dirR = Quaternion.AngleAxis(-90, Vector3.forward) * dir;

        Dictionary<float, List<Vector2>> ThunderList = new Dictionary<float, List<Vector2>>();
        for (int i = 0; i < maxCount; i++)
        {
            var newList = new List<Vector2> { dirL*( (intervalDis * ((float)i)) + StartAlpha) , dirR * ((intervalDis * ((float)i)) + intervalDis / 2.0f) };
            ThunderList[intervalTime * ((float)i)] = newList;
        }
        m.SetThunderList(ThunderList, this);
        ThunderList.Clear();
    }


    /// <summary>
    /// 沿某一方向发射闪电
    /// </summary>
    /// <param name="i"></param>
    /// <param name="p"></param>
    public void LunchThuderByVector(Vector2 p, Vector2 dir, float intervalTime, float intervalDis, int maxCount, Vector2 StartVector , float StartDelay)
    {
        ElectivireLightningStrikeManger m = Instantiate(LightningStrikeManger, transform.position, Quaternion.identity);
        m.ParentElectivire = this;
        m.transform.position = p;

        dir = dir.normalized;

        Dictionary<float, List<Vector2>> ThunderList = new Dictionary<float, List<Vector2>>();
        for (int i = 0; i < maxCount; i++)
        {
            var newList = new List<Vector2> { dir * ((intervalDis * ((float)i)) ) + StartVector };
            ThunderList[intervalTime * ((float)i) + StartDelay] = newList;
        }
        m.SetThunderList(ThunderList, this);
        ThunderList.Clear();
    }


    //■■■■■■■■■■■■■■■■■■■■闪电生成未知列表■■■■■■■■■■■■■■■■■■■■■■





}
