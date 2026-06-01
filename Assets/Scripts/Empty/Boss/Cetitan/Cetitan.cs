using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cetitan : Empty
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
    /// 奔跑尘埃特效
    /// </summary>
    public GameObject RunDust;



    //==============================音效枚举===================================

    /// <summary>
    /// 音效种类枚举
    /// </summary>
    public enum CetitanSE
    {

        BigFall, 
        Drift,
        Fall,
        IceFang,
        Roar,
        Step,

        Blizzard,
        Avalanche,
    }

    /// <summary>
    /// 一次性音效播放器生成
    /// </summary>
    public EnemyAudioPlayer audioPlayer;
    /// <summary>
    /// 循环音效音效播放器生成
    /// </summary>
    public LoopingSEAudioPlayer loopPlayer;    



    //==============================音效枚举===================================






    //==============================状态机枚举===================================

    /// <summary>
    /// 主状态
    /// </summary>
    enum MainState
    {
        Normal, //一般
        Angry,  //愤怒
    }
    MainState NowMainState;


    /// <summary>
    /// 副状态
    /// </summary>
    enum SubState
    {
        Normal_Idle,           //一般_发呆
        Normal_Run,            //一般_奔跑追踪
        Normal_IcicleCrash,    //一般_冰柱坠击
        Normal_Spinner,        //一般_反弹冰旋
        Normal_IceFang,        //一般_冰牙
        Normal_Jump,           //一般_跳跃地震
        Normal_Roar,           //一般_吼叫暴雪
        Angry_Idle,            //愤怒_发呆
        Angry_Run,             //愤怒_奔跑追踪
        Angry_IcicleCrash,     //愤怒_冰柱坠击
        Angry_Spinner,         //愤怒_反弹冰旋
        Angry_IceFang,         //愤怒_冰牙
        Angry_Jump,            //愤怒_跳跃地震
        Angry_Drift,           //愤怒_漂移滑行
        Angry_IceBeam,         //愤怒_冰光
        Angry_Rush,            //愤怒_十万马力冲刺
        Angry_Roar,            //愤怒_吼叫暴雪
        Angry_BlizzardCenterTurn, //围绕暴风雪中心旋转
    }
    SubState NowSubState;


    /// <summary>
    /// 状态映射关系
    /// </summary>
    private static Dictionary<MainState, SubState[]> StateMap = new()
    {
        { MainState.Normal, new[] { SubState.Normal_Idle, SubState.Normal_Run, SubState.Normal_IcicleCrash, SubState.Normal_Spinner, SubState.Normal_IceFang, SubState.Normal_Jump, SubState.Normal_Roar } },
        { MainState.Angry, new[] { SubState.Angry_Idle, SubState.Angry_Run, SubState.Angry_IcicleCrash, SubState.Angry_Spinner, SubState.Angry_IceFang, SubState.Angry_Jump, SubState.Angry_Drift, SubState.Angry_IceBeam, SubState.Angry_Rush, SubState.Angry_Roar, SubState.Angry_BlizzardCenterTurn } },
    };


    /// <summary>
    /// 愤怒等级
    /// </summary>
    enum AngryLevel
    {
        LEVEL1,  //1级愤怒
        LEVEL2,  //2级愤怒
        LEVEL3,  //3级愤怒
    }
    AngryLevel NowAngryLevel;


    /// <summary>
    /// 连招
    /// </summary>
    public enum CombatList
    {
        None, //无连招
        Normal_Combat010,//[冰柱坠击0]
        Normal_Combat020,//[反弹冰旋0]
        Normal_Combat030,//[冰牙0]
        Normal_Combat040,//[冰柱坠击0][跳跃地震0]
        Angry_Level1_Combat110,//[冰柱坠击0][冰柱坠击1]
        Angry_Level1_Combat120,//[反弹冰旋0]
        Angry_Level1_Combat130,//[冰牙0][冰柱坠击0]
        Angry_Level1_Combat141,//[冰柱坠击0][跳跃地震0]
        Angry_Level1_Combat142,//[冰光0][十万马力冲刺0]
        Angry_Level1_Combat150,//[漂移滑行0][冰牙0]
        Angry_Level2_Combat230,//[冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
        Angry_Level2_Combat250,//[漂移滑行0][冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2]
    }
    public CombatList NowCombat = CombatList.None;




    //发动连招010[冰柱坠击0] 需要的时间
    static float TIME_COMBAT_CONDITION_010 = 3.1f;

    //发动连招020[反弹冰旋0] 需要的冰柱数量
    static float COUNT_COMBAT_CONDITION_020 = 6;

    //发动连招030[冰牙0] 需要的时间
    static float TIME_COMBAT_CONDITION_030 = 0.7f;

    //发动连招030[冰牙0] 需要的距离
    static float DISTENCE_COMBAT_CONDITION_030 = 5.0f;

    //发动连招030[冰牙0] 需要的玩家和浩大鲸最大夹角
    static float ANGLE_COMBAT_CONDITION_030 = 15.0f * Mathf.Deg2Rad;

    //发动连招040[冰柱坠击0][跳跃地震0] 需要的距离
    static float DISTENCE_COMBAT_CONDITION_040 = 14.2f;

    //发动连招110 150 250[冰柱坠击0][冰柱坠击1]OR[漂移滑行0][冰牙0]OR[漂移滑行0][冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2] 需要的时间
    static float TIME_COMBAT_CONDITION_110 = 2.6f;//3.1f

    //发动连招120[反弹冰旋0] 需要的冰柱数量
    static float COUNT_COMBAT_CONDITION_120 = 10;

    //发动连招130和230[冰牙0][冰柱坠击0]OR[冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]  需要的时间
    static float TIME_COMBAT_CONDITION_130_230 = 0.5f;

    //发动连招130和230[冰牙0][冰柱坠击0]OR[冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2] 需要的距离
    static float DISTENCE_COMBAT_CONDITION_130_230 = 4.8f;

    //发动连招130和230[冰牙0][冰柱坠击0]OR[冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2] 需要的玩家和浩大鲸最大夹角
    static float ANGLE_COMBAT_CONDITION_130_230 = 15.0f * Mathf.Deg2Rad;

    //发动连招141和142[冰柱坠击0][跳跃地震0]OR[冰光0][十万马力冲刺0]需要的距离
    static float DISTENCE_COMBAT_CONDITION_141_142 = 13.0f;//13.2


    //一般状态转入1级愤怒的血线
    static float HP_NORMAL2ANGRYL1 = 0.75f;

    //1级愤怒状态转入2级愤怒的血线
    static float HP_ANGRYL12ANGRYL2 = 0.45f;

    //2级愤怒状态转入3级愤怒的血线
    static float HP_ANGRYL22ANGRYL3 = 0.25f;



    //连招010的冷却时间 [冰柱坠击0]
    static float TIME_NORMAL_COMBAT010_CD = 1.2f;

    //连招020的冷却时间 [反弹冰旋0]
    static float TIME_NORMAL_COMBAT020_CD = 3.5f;

    //连招030的冷却时间 [冰牙0]
    static float TIME_NORMAL_COMBAT030_CD = 1.7f;

    //连招040的冷却时间 [冰柱坠击0][跳跃地震0]
    static float TIME_NORMAL_COMBAT040_CD = 4.5f;


    //连招110的冷却时间 [冰柱坠击0][冰柱坠击1]
    static float TIME_ANGRYL_COMBAT110_CD = 1.2f;

    //连招120的冷却时间 [反弹冰旋0]
    static float TIME_ANGRYL_COMBAT120_CD = 3.0f;

    //连招130的冷却时间 [冰牙0][冰柱坠击0]
    static float TIME_ANGRYL_COMBAT130_CD = 2.0f;

    //连招141的冷却时间 [冰柱坠击0][跳跃地震0]
    static float TIME_ANGRYL_COMBAT141_CD = 9.0f;

    //连招142的冷却时间 [冰光0][十万马力冲刺0]
    static float TIME_ANGRYL_COMBAT142_CD = 7.5f;

    //连招150的冷却时间 [漂移滑行0][冰牙0]
    static float TIME_ANGRYL_COMBAT150_CD = 3.5f;



    //连招230的冷却时间 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
    static float TIME_ANGRYL_COMBAT230_CD = 6.0f;

    //连招250的冷却时间 [漂移滑行0][冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2]
    static float TIME_ANGRYL_COMBAT250_CD = 9.0f;


    //围绕暴风雪中心旋转时的冷却时间
    static float TIME_ANGRYL__BLIZZARDCENTERTURN_CD = 4.5f;


    //==============================状态机枚举===================================



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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.5f/* Boss * 1.5f; */;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.5f/* Boss * 1.5f; */;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //设置初始方向
        SetDirector(Vector2.down);

        //启动计算方向携程
        StartCoroutine(CheckLook());

        //初始发呆
        Normal_IdleStart(TIME_NORMAL_IDLE_START);

        StartOverEvent();
    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();//如果玩家组件丢失，重新获取
        if (!isDie && !isBorn && player.NowRoom == ParentPokemonRoom.RoomIndex)//不处于正在死亡状态或正在出生状态时
        {
            EmptyDie();//判定是否执行死亡
            UpdateEmptyChangeHP();//判定生命值是否变化
            StateMaterialChange();//判定是否更换状态材质

            //InsertStateMechineSwitch

            animator.ResetTrigger("Sleep");

            //■■开始判断状态机
            Debug.Log(NowSubState);
            switch (NowMainState)
            {
                //●主状态：【一般】状态
                case MainState.Normal:

                    //●判断是否转进愤怒状态
                    if (NowSubState != SubState.Normal_Roar && ((float)EmptyHp / (float)maxHP) < HP_NORMAL2ANGRYL1 && (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isFearDone && !isCanNotMoveWhenParalysis) )
                    {
                        ResetAllState_Normal();
                        Normal_RoarStart(TIME_NORMAL_ROAR);
                        AngryEffect(new Vector2(1.0f, 0.7f), this.gameObject, new Vector3(1.8f, 1.8f, 0));
                        //提升双防
                        DefChange(2, 0.0f);
                        SpDChange(2, 0.0f);
                        
                    }

                    //●当处于冰冻 睡眠 致盲 麻痹状态时主状态【一般】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        
                        //判断副状态
                        switch (NowSubState)
                        {
                            //【一般_发呆】状态
                            case SubState.Normal_Idle:
                                Normal_IdleTimer -= Time.deltaTime;//【一般_发呆】计时器时间减少
                                if (Normal_IdleTimer <= 0)         //计时器时间到时间，结束【一般_发呆】状态
                                {
                                    Normal_IdleOver();
                                    Normal_RunStart();
                                }
                                break;
                            //【一般_奔跑追踪】状态
                            case SubState.Normal_Run:
                                Normal_RunTimer += Time.deltaTime;//【一般_奔跑追踪】计时器时间增加
                                //不恐惧时
                                if (!isFearDone)
                                {
                                    Vector2 MoveDir = (TargetPosition - (Vector2)transform.position).normalized;
                                    MoveBySpeedAndDir(MoveDir, speed, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    float TargetDistence = Vector2.Distance(TargetPosition, (Vector2)transform.position);
                                    //转换为其他状态
                                    //连招010 [冰柱坠击0]
                                    if (Normal_RunTimer >= TIME_COMBAT_CONDITION_010)
                                    {
                                        NowCombat = CombatList.Normal_Combat010;
                                        Normal_RunOver();
                                        Normal_IcicleCrashStart();
                                    }
                                    //Debug.Log(Normal_RunTimer + "+" + TIME_COMBAT_CONDITION_030 + "+" + (Normal_RunTimer >= TIME_COMBAT_CONDITION_030) /*追击时间大于TIME_COMBAT_CONDITION_030*/ );
                                    //Debug.Log(Vector2.Distance(TargetPosition, (Vector2)transform.position) + "+" + (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_COMBAT_CONDITION_030)  /*和玩家距离小于DISTENCE_COMBAT_CONDITION_030*/);
                                    //Debug.Log((TargetPosition - (Vector2)transform.position).normalized.x + "+" + ANGLE_COMBAT_CONDITION_030 + "+" + Mathf.Sin(ANGLE_COMBAT_CONDITION_030) + "+" + ((Mathf.Abs((TargetPosition - (Vector2)transform.position).normalized.x) < Mathf.Sin(ANGLE_COMBAT_CONDITION_030)))  /*和玩家夹角小于ANGLE_COMBAT_CONDITION_030*/);
                                    //Debug.Log((TargetPosition - (Vector2)transform.position).normalized.y + "+" + ANGLE_COMBAT_CONDITION_030 + "+" + Mathf.Sin(ANGLE_COMBAT_CONDITION_030) + "+" + ((Mathf.Abs((TargetPosition - (Vector2)transform.position).normalized.y) < Mathf.Sin(ANGLE_COMBAT_CONDITION_030)))  /*和玩家夹角小于ANGLE_COMBAT_CONDITION_030*/);

                                    //连招030 [冰牙0]
                                    else if (
                                        Normal_RunTimer >= TIME_COMBAT_CONDITION_030 &&  /*追击时间大于TIME_COMBAT_CONDITION_030*/
                                        Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_COMBAT_CONDITION_030 && /*和玩家距离小于DISTENCE_COMBAT_CONDITION_030*/
                                        ((Mathf.Abs((TargetPosition - (Vector2)transform.position).normalized.x) < Mathf.Sin(ANGLE_COMBAT_CONDITION_030)) || ((Mathf.Abs((TargetPosition - (Vector2)transform.position).normalized.y) < Mathf.Sin(ANGLE_COMBAT_CONDITION_030))))  /*和玩家夹角小于ANGLE_COMBAT_CONDITION_030*/
                                        )
                                    {
                                        //Debug.Log(Normal_RunTimer + "+" + TIME_COMBAT_CONDITION_030 + "+" + (Normal_RunTimer >= TIME_COMBAT_CONDITION_030) /*追击时间大于TIME_COMBAT_CONDITION_030*/ );
                                        //Debug.Log(Vector2.Distance(TargetPosition, (Vector2)transform.position) + "+" + (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_COMBAT_CONDITION_030)  /*和玩家距离小于DISTENCE_COMBAT_CONDITION_030*/);
                                        //Debug.Log((TargetPosition - (Vector2)transform.position).normalized + "+" + ((Mathf.Abs((TargetPosition - (Vector2)transform.position).normalized.x) < Mathf.Sin(ANGLE_COMBAT_CONDITION_030)) || ((Mathf.Abs((TargetPosition - (Vector2)transform.position).normalized.y) < Mathf.Sin(ANGLE_COMBAT_CONDITION_030))))  /*和玩家夹角小于ANGLE_COMBAT_CONDITION_030*/);
                                        NowCombat = CombatList.Normal_Combat030;
                                        Normal_RunOver();
                                        Normal_IceFangStart();
                                    }
                                    //连招040 [冰柱坠击0][跳跃地震0]
                                    else if (Vector2.Distance(TargetPosition, (Vector2)transform.position) >= DISTENCE_COMBAT_CONDITION_040  /*和玩家距离da于DISTENCE_COMBAT_CONDITION_030*/)
                                    {
                                        NowCombat = CombatList.Normal_Combat040;
                                        Normal_RunOver();
                                        Normal_IcicleCrashStart();
                                    }
                                    //连招020 [反弹冰旋0]【触发优先度最低】
                                    else if (ICObjList.Count >= COUNT_COMBAT_CONDITION_020)
                                    {
                                        NowCombat = CombatList.Normal_Combat020;
                                        Normal_RunOver();
                                        Normal_SpinnerStart();
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
                            //【一般_冰柱坠击】状态
                            case SubState.Normal_IcicleCrash:
                                break;
                            //【一般_反弹冰旋】状态
                            case SubState.Normal_Spinner:
                                MoveBySpeedAndDir(Dir_Normall_Spinner, speed, SpeedAlpha_Normal_Spinner * (isEmptyConfusionDone ? 0.6f : 1.0f), 3.0f, 3.0f, 3.0f, 3.0f);
                                if (!isOver_Normal_Spinner)
                                {
                                    Normal_SpinnerTimer += Time.deltaTime;//【一般_反弹冰旋】计时器时间增加
                                    if (Normal_SpinnerTimer <= TIME_SPEEDALPHA_FADEIN_SPINNER)
                                    {
                                        SpeedAlpha_Normal_Spinner += (SPEEDALPHA_MAX_NORMAL_SPINNER / TIME_SPEEDALPHA_FADEIN_SPINNER) * Time.deltaTime;
                                    }
                                    if (Normal_SpinnerTimer >= TIME_FADEOUT_START_MIN_NORMAL_SPINNER && Count_Rebound_Normal_Spinner >= COUNT_REBOUND_FADEOUT_START_NORMAL_SPINNER)
                                    {
                                        //结束冰旋 开始减速
                                        FadeOut_Start_Normal_Spinner();
                                        loopPlayer.StopLoop(1.0f);
                                    }
                                }
                                else
                                {
                                    SpeedAlpha_Normal_Spinner -= (SPEEDALPHA_MAX_NORMAL_SPINNER / TIME_SPEEDALPHA_FADEIN_SPINNER) * Time.deltaTime;
                                    if (SpeedAlpha_Normal_Spinner <= 0)
                                    {
                                        switch (NowCombat)
                                        {
                                            case CombatList.Normal_Combat020:
                                                Normal_SpinnerOver();
                                                Normal_IdleStart(TIME_NORMAL_COMBAT020_CD);
                                                break;
                                            default:
                                                Normal_SpinnerOver();
                                                Normal_IdleStart(TIME_NORMAL_IDLE_START);
                                                break;
                                        }
                                    }
                                }
                                break;
                            //【一般_冰牙】状态
                            case SubState.Normal_IceFang:
                                if (isRush_Normal_IceFang)
                                {
                                    MoveBySpeedAndDir(Dir_Normal_IceFang_Rush, speed, SPEEDALPHA_NORMAL_ICEFANG, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【一般_跳跃地震】状态
                            case SubState.Normal_Jump:
                                //蓄力
                                if (isCharge_Normal_Jump) {
                                    Normal_JumpTimer += Time.deltaTime;//【一般_跳跃地震】计时器时间增加
                                    //蓄力完毕转进跳跃
                                    if (Normal_JumpTimer >= Time_Normal_Jump_Charge) {
                                        isCharge_Normal_Jump = false;
                                        Normal_JumpTimer = 0;
                                        animator.SetInteger("Jump", 2);
                                    }
                                }
                                //跳跃
                                if (isMove_Normal_Jump)
                                {
                                    Dir_Normal_Jump = (Position_Combat040_Normal_Jump_Target - (Vector2)transform.position).normalized;
                                    MoveBySpeedAndDir(Dir_Normal_Jump, Speed_Normal_Jump, 1, 0.0f, 0.0f, 0.0f, 0.0f);
                                    Normal_JumpTimer += Time.deltaTime;//【一般_跳跃地震】计时器时间增加
                                    //到达目的地后或长时间跳跃时 跳跃完毕 转进发呆
                                    Debug.Log(Vector2.Distance(Position_Combat040_Normal_Jump_Target, (Vector2)transform.position) + "+" + (Vector2.Distance(Position_Combat040_Normal_Jump_Target, (Vector2)transform.position) < 0.5f));
                                    Debug.Log(Normal_JumpTimer + "+" + Time_Normal_Jump + "+" + (Normal_JumpTimer >= 2 * Time_Normal_Jump));
                                    if (Vector2.Distance(Position_Combat040_Normal_Jump_Target, (Vector2)transform.position) < 0.5f || Normal_JumpTimer >= 2 * Time_Normal_Jump)
                                    {
                                        animator.SetInteger("Jump", 3);
                                        isMove_Normal_Jump = false;
                                    }

                                }


                                break;
                            //【一般_吼叫暴雪】状态
                            case SubState.Normal_Roar:
                                Normal_RoarTimer -= Time.deltaTime;//【一般_吼叫暴雪】计时器时间减少
                                if (Normal_RoarTimer <= 0)         //计时器时间到时间，结束【一般_吼叫暴雪】状态
                                {
                                    Normal_RoarOver();
                                    NowAngryLevel = AngryLevel.LEVEL1;
                                    Angry_IdleStart(TIME_ANGRY_IDLE_START);
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
                //●主状态：【愤怒】状态
                case MainState.Angry:

                    //●判断是否转进愤怒2状态
                    if (NowAngryLevel == AngryLevel.LEVEL1 && NowSubState != SubState.Angry_Roar && ((float)EmptyHp / (float)maxHP) < HP_ANGRYL12ANGRYL2 && (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isFearDone && !isCanNotMoveWhenParalysis))
                    {
                        ResetAllState_Normal();
                        Angry_RoarStart(TIME_ANGRY_ROAR);
                        AngryEffect(new Vector2(1.0f, 0.7f), this.gameObject, new Vector3(1.8f, 1.8f, 0));
                        DefChange(1, 0.0f);
                        SpDChange(1, 0.0f);
                    }

                    //●判断是否转进愤怒3状态
                    if ((NowAngryLevel == AngryLevel.LEVEL1 || NowAngryLevel == AngryLevel.LEVEL2) && NowSubState != SubState.Angry_Roar && ((float)EmptyHp / (float)maxHP) < HP_ANGRYL22ANGRYL3 && (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isFearDone && !isCanNotMoveWhenParalysis))
                    {
                        ResetAllState_Normal();
                        Angry_RoarStart(TIME_ANGRY_ROAR);
                        AngryEffect(new Vector2(1.0f, 0.7f), this.gameObject, new Vector3(1.8f, 1.8f, 0));
                        DefChange(1, 0.0f);
                        SpDChange(1, 0.0f);
                    }

                    //●当处于冰冻 睡眠 致盲 麻痹状态时主状态【愤怒】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        //判断副状态
                        switch (NowSubState)
                        {
                            //【愤怒_发呆】状态
                            case SubState.Angry_Idle:
                                Angry_IdleTimer -= Time.deltaTime;//【愤怒_发呆】计时器时间减少
                                if (Angry_IdleTimer <= 0)         //计时器时间到时间，结束【愤怒_发呆】状态
                                {
                                    Angry_IdleOver();
                                    Angry_RunStart();
                                }
                                break;
                            //【愤怒_奔跑追踪】状态
                            case SubState.Angry_Run:
                                Angry_RunTimer += Time.deltaTime;//【愤怒_奔跑追踪】计时器时间增加
                                //不恐惧时
                                if (!isFearDone)
                                {
                                    Vector2 MoveDir = (TargetPosition - (Vector2)transform.position).normalized;
                                    MoveBySpeedAndDir(MoveDir, speed, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    float TargetDistence = Vector2.Distance(TargetPosition, (Vector2)transform.position);
                                    int Count_Bouns_Combat120 = 0;
                                    switch (NowAngryLevel)
                                    {
                                        case AngryLevel.LEVEL1: Count_Bouns_Combat120 = 0; break;
                                        case AngryLevel.LEVEL2: Count_Bouns_Combat120 = 1; break;
                                        case AngryLevel.LEVEL3: Count_Bouns_Combat120 = 2; break;
                                    }
                                    //转换为其他状态
                                    //连招110[冰柱坠击0][冰柱坠击1]
                                    //连招150 [漂移滑行0][冰牙0]
                                    //连招250 [漂移滑行0][冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2]
                                    if (Angry_RunTimer >= TIME_COMBAT_CONDITION_110)
                                    {
                                        float f = Random.Range(0.0f, 1.0f);
                                        if (f > 0.5f)//连招110[冰柱坠击0][冰柱坠击1]
                                        {
                                            NowCombat = CombatList.Angry_Level1_Combat110;
                                            Angry_RunOver();
                                            Angry_IcicleCrashStart();
                                        }
                                        else//连招150 [漂移滑行0][冰牙0]
                                            //连招250 [漂移滑行0][冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2]
                                        {
                                            //一级愤怒 连招150 [漂移滑行0][冰牙0]
                                            if (NowAngryLevel == AngryLevel.LEVEL1)
                                            {
                                                NowCombat = CombatList.Angry_Level1_Combat150;
                                                Angry_RunOver();
                                                Angry_DriftStart();
                                            }
                                            //二三级愤怒 连招250 [漂移滑行0][冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2]
                                            else
                                            {
                                                NowCombat = CombatList.Angry_Level2_Combat250;
                                                Angry_RunOver();
                                                Angry_DriftStart();
                                            }
                                        }


                                    }
                                    //连招130 [冰牙0][冰柱坠击0]
                                    //连招230 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
                                    else if (
                                        Angry_RunTimer >= TIME_COMBAT_CONDITION_130_230 &&  /*追击时间大于TIME_COMBAT_CONDITION_030*/
                                        Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_COMBAT_CONDITION_130_230 && /*和玩家距离小于DISTENCE_COMBAT_CONDITION_030*/
                                        ((Mathf.Abs((TargetPosition - (Vector2)transform.position).normalized.x) < Mathf.Sin(ANGLE_COMBAT_CONDITION_130_230)) || ((Mathf.Abs((TargetPosition - (Vector2)transform.position).normalized.y) < Mathf.Sin(ANGLE_COMBAT_CONDITION_130_230))))  /*和玩家夹角小于ANGLE_COMBAT_CONDITION_030*/
                                        )
                                    {
                                        //一级愤怒 连招130 [冰牙0][冰柱坠击0]
                                        if (NowAngryLevel == AngryLevel.LEVEL1)
                                        {
                                            NowCombat = CombatList.Angry_Level1_Combat130;
                                            Angry_RunOver();
                                            Angry_IceFangStart();
                                        }
                                        //二三级愤怒 连招230 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
                                        else
                                        {
                                            NowCombat = CombatList.Angry_Level2_Combat230;
                                            Angry_RunOver();
                                            Angry_IceFangStart();
                                        }
                                    }
                                    //连招141 [冰柱坠击0][跳跃地震0]
                                    //连招142 [冰光0][十万马力冲刺0]
                                    else if (Vector2.Distance(TargetPosition, (Vector2)transform.position) >= DISTENCE_COMBAT_CONDITION_141_142  /*和玩家距离da于DISTENCE_COMBAT_CONDITION_030*/)
                                    {
                                        float f = Random.Range(0.0f, 1.0f);
                                        if (f > 0.6f)//连招141 [冰柱坠击0][跳跃地震0]
                                        {
                                            NowCombat = CombatList.Angry_Level1_Combat141;
                                            Angry_RunOver();
                                            Angry_IcicleCrashStart();
                                        }
                                        else//连招142 [冰光0][十万马力冲刺0]
                                        {
                                            NowCombat = CombatList.Angry_Level1_Combat142;
                                            Angry_RunOver();
                                            Angry_IceBeamStart();
                                        }
                                        //NowCombat = CombatList.Angry_Level1_Combat141;
                                        //Angry_RunOver();
                                        //Angry_IcicleCrashStart();
                                        //NowCombat = CombatList.Angry_Level1_Combat142;
                                        //Angry_RunOver();
                                        //Angry_IceBeamStart();
                                    }
                                    //连招120 [反弹冰旋0]【触发优先度最低】
                                    else if (ICObjList.Count >= COUNT_COMBAT_CONDITION_120- Count_Bouns_Combat120)
                                    {
                                        NowCombat = CombatList.Angry_Level1_Combat120;
                                        Angry_RunOver();
                                        Angry_SpinnerStart();
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
                            //【愤怒_冰柱坠击】状态
                            case SubState.Angry_IcicleCrash:
                                break;
                            //【愤怒_反弹冰旋】状态
                            case SubState.Angry_Spinner:
                                MoveBySpeedAndDir(Dir_Angry_Spinner, speed, SpeedAlpha_Angry_Spinner * (isEmptyConfusionDone ? 0.6f : 1.0f), 3.0f, 3.0f, 3.0f, 3.0f);
                                if (!isOver_Angry_Spinner)
                                {
                                    Angry_SpinnerTimer += Time.deltaTime;//【愤怒_反弹冰旋】计时器时间增加
                                    if (Angry_SpinnerTimer <= TIME_SPEEDALPHA_FADEIN_SPINNER)
                                    {
                                        SpeedAlpha_Angry_Spinner += (SPEEDALPHA_MAX_ANGRY_SPINNER / TIME_SPEEDALPHA_FADEIN_SPINNER) * Time.deltaTime;
                                    }
                                    if (Angry_SpinnerTimer >= TIME_FADEOUT_START_MIN_ANGRY_SPINNER && Count_Rebound_Angry_Spinner >= COUNT_REBOUND_FADEOUT_START_ANGRY_SPINNER)
                                    {
                                        //结束冰旋 开始减速
                                        FadeOut_Start_Angry_Spinner();
                                        loopPlayer.StopLoop(1.0f);
                                    }
                                }
                                else
                                {
                                    SpeedAlpha_Angry_Spinner -= (SPEEDALPHA_MAX_ANGRY_SPINNER / TIME_SPEEDALPHA_FADEIN_SPINNER) * Time.deltaTime;
                                    if (SpeedAlpha_Angry_Spinner <= 0)
                                    {
                                        switch (NowCombat)
                                        {
                                            case CombatList.Angry_Level1_Combat120:
                                                Angry_SpinnerOver();
                                                Angry_IdleStart(TIME_ANGRYL_COMBAT120_CD);
                                                break;
                                            default:
                                                Angry_SpinnerOver();
                                                Angry_IdleStart(TIME_ANGRY_IDLE_START);
                                                break;
                                        }
                                    }
                                }
                                break;
                            //【愤怒_冰牙】状态
                            case SubState.Angry_IceFang:
                                if (isRush_Angry_IceFang)
                                {
                                    MoveBySpeedAndDir(Dir_Angry_IceFang_Rush, speed, SPEEDALPHA_ANGRY_ICEFANG, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //【愤怒_跳跃地震】状态
                            case SubState.Angry_Jump:
                                //蓄力
                                if (isCharge_Angry_Jump)
                                {
                                    Angry_JumpTimer += Time.deltaTime;//【愤怒_跳跃地震】计时器时间增加
                                    //蓄力完毕转进跳跃
                                    if (Angry_JumpTimer >= Time_Angry_Jump_Charge)
                                    {
                                        isCharge_Angry_Jump = false;
                                        Angry_JumpTimer = 0;
                                        animator.SetInteger("Jump", 2);
                                    }
                                }
                                //跳跃
                                if (isMove_Angry_Jump)
                                {
                                    Dir_Angry_Jump = (Position_Combat141_Angry_Jump_Target - (Vector2)transform.position).normalized;
                                    MoveBySpeedAndDir(Dir_Angry_Jump, Speed_Angry_Jump, 1, 0.0f, 0.0f, 0.0f, 0.0f);
                                    Angry_JumpTimer += Time.deltaTime;//【愤怒_跳跃地震】计时器时间增加
                                    //到达目的地后或长时间跳跃时 跳跃完毕 转进发呆
                                    //Debug.Log(Vector2.Distance(Position_Combat141_Angry_Jump_Target, (Vector2)transform.position) + "+" + (Vector2.Distance(Position_Combat040_Angry_Jump_Target, (Vector2)transform.position) < 0.5f));
                                    //Debug.Log(Angry_JumpTimer + "+" + Time_Angry_Jump + "+" + (Angry_JumpTimer >= 2 * Time_Angry_Jump));
                                    if (Vector2.Distance(Position_Combat141_Angry_Jump_Target, (Vector2)transform.position) < 0.5f || Angry_JumpTimer >= 2 * Time_Angry_Jump)
                                    {
                                        animator.SetInteger("Jump", 3);
                                        isMove_Angry_Jump = false;
                                    }

                                }
                                break;
                            //【愤怒_漂移滑行】状态
                            case SubState.Angry_Drift:
                                //四向射线检测墙壁
                                Vector2 pos = transform.position;
                                Vector2 targetPos = TargetPosition;

                                Vector2 center = pos + GetComponent<Collider2D>().offset;

                                RaycastHit2D R = Physics2D.Raycast(center, Vector2.right, wallCheckDistance, LayerMask.GetMask("Room"));
                                RaycastHit2D U = Physics2D.Raycast(center, Vector2.up, wallCheckDistance, LayerMask.GetMask("Room"));
                                RaycastHit2D L = Physics2D.Raycast(center, Vector2.left, wallCheckDistance, LayerMask.GetMask("Room"));
                                RaycastHit2D D = Physics2D.Raycast(center, Vector2.down, wallCheckDistance, LayerMask.GetMask("Room"));

                                float distR = R ? R.distance : 999f;
                                float distU = U ? U.distance : 999f;
                                float distL = L ? L.distance : 999f;
                                float distD = D ? D.distance : 999f;


                                //冲刺阶段
                                if (isRush_ANGRY_DRIFT)
                                {
                                    SetDirector(Dir_Angle_Drift);
                                    MoveBySpeedAndDir(Dir_Angle_Drift, speed, SPEEDALPHA_ANGRY_DRIFT_RUSH * (isEmptyConfusionDone ? 0.6f : 1.0f), 0, 0, 0, 0);
                                    Angry_DriftTimer += Time.deltaTime;
                                    if (Angry_DriftTimer >= TIME_ANGRY_DRIFT_RUSH
                                        || distR < wallSafeDistance || distL < wallSafeDistance || distU < wallSafeDistance || distD < wallSafeDistance )
                                    {
                                        isRush_ANGRY_DRIFT = false;
                                        isDrift_ANGRY_DRIFT = true;
                                        Angry_DriftTimer = 0;
                                        DriftDust.StartPSL();
                                        DriftDust.StartPSR();
                                    }
                                }

                                if (isDrift_ANGRY_DRIFT) {
                                    //漂移
                                    {


                                        // ============================
                                        // 1. 计算目标方向
                                        // ============================
                                        Vector2 TargetD = (targetPos - pos).normalized;

                                        // ============================
                                        // 2. 计算夹角（控制漂移强度）
                                        // ============================
                                        float angleToTarget = Vector2.Angle(Dir_Angle_Drift, TargetD);
                                        //Debug.Log(angleToTarget);
                                        // 夹角越大 → 转向越慢 → 漂移越明显
                                        float maxAngle = 120f;
                                        float t = Mathf.Clamp01(angleToTarget / maxAngle);

                                        // 基础转向速度（可调）
                                        float turnSpeed = Mathf.Lerp(10f, 25f, t)*Mathf.Lerp(10f, 25f, t);

                                        // ============================
                                        // 3. 四向射线检测墙壁压力
                                        // ============================

                                        // 压力值（0~1）
                                        float pR = Mathf.Clamp01(1f - distR / wallSafeDistance);
                                        float pU = Mathf.Clamp01(1f - distU / wallSafeDistance);
                                        float pL = Mathf.Clamp01(1f - distL / wallSafeDistance);
                                        float pD = Mathf.Clamp01(1f - distD / wallSafeDistance);

                                        // 墙壁推力方向（远离墙）
                                        Vector2 wallPush =
                                            Vector2.right * pL +   // 左墙 → 推向右
                                            Vector2.left * pR +    // 右墙 → 推向左
                                            Vector2.up * pD +      // 下墙 → 推向上
                                            Vector2.down * pU;     // 上墙 → 推向下

                                        // 墙壁额外转向速度
                                        float wallTurnSpeed = wallPush.magnitude * wallTurnBoost;

                                        // 加入墙壁影响
                                        turnSpeed += wallTurnSpeed;

                                        // 加入时间影响
                                        turnSpeed *=  Mathf.Lerp( 0.4f , 2.2f , Angry_DriftTimer / TIME_ANGRY_DRIFT_DRIFT);

                                        // ============================
                                        // 4. 根据 cross 判断左右旋转
                                        // ============================
                                        float cross = Dir_Angle_Drift.x * TargetD.y - Dir_Angle_Drift.y * TargetD.x;
                                        float angle = turnSpeed * Time.deltaTime;

                                        if (cross > 0)
                                        {
                                            Dir_Angle_Drift = Quaternion.AngleAxis(angle, Vector3.forward) * Dir_Angle_Drift;
                                            DriftDust.StopPSL();
                                            DriftDust.StartPSR();
                                        }
                                        else
                                        {
                                            Dir_Angle_Drift = Quaternion.AngleAxis(-angle, Vector3.forward) * Dir_Angle_Drift;
                                            DriftDust.StartPSL();
                                            DriftDust.StopPSR();
                                        }
                                        Dir_Angle_Drift.Normalize();
                                        // ============================
                                        // 5. 移动（保持你的原系统）
                                        // ============================
                                        SetDirector(Dir_Angle_Drift);
                                        Angry_DriftTimer += Time.deltaTime;
                                        MoveBySpeedAndDir(Dir_Angle_Drift, speed, SPEEDALPHA_ANGRY_DRIFT_DRIFT * (isEmptyConfusionDone ? 0.6f : 1.0f), 0, 0, 0, 0);
                                        DriftDust.SetRotation(Dir_Angle_Drift);
                                        if (Angry_DriftTimer >= TIME_ANGRY_DRIFT_DRIFT ||
                                            (Angry_DriftTimer >= (TIME_ANGRY_DRIFT_DRIFT/3.0f) && (angleToTarget < 12.0f || Vector2.Distance(TargetPosition , (Vector2)transform.position) < 3.2f)))
                                        {
                                            isRush_ANGRY_DRIFT = false;
                                            isDrift_ANGRY_DRIFT = false;
                                            Angry_DriftTimer = 0;
                                            animator.SetInteger("Drift", 2);
                                            loopPlayer.StopLoop(0.3f);
                                        }

                                        // 6. 冰柱坠击
                                        Angry_Drift_IcicleCrash_Timer += Time.deltaTime;
                                        if (Angry_Drift_IcicleCrash_Timer > TIME_INTERVEL_ANGRY_DRIFT_ICICLECRASH)
                                        {
                                            Angry_Drift_IcicleCrash_Timer = 0.0f;
                                            Vector2 p = Quaternion.AngleAxis(((cross > 0) ? -1 : 1) * 90.0f , Vector3.forward) * Dir_Angle_Drift * RADIUS_ANGRY_DRIFT_ICICLECRASH + transform.position;
                                            Lunch_IcicleCrash_Point(p);
                                        }
                                    }

                                }

                                break;
                            //【愤怒_冰光】状态
                            case SubState.Angry_IceBeam:
                                //发射冰光期间
                                if (isMove_Launch_Angry_IceBeam)
                                {
                                    MoveBySpeedAndDir(-Dir_TargetAngle_IceBeam, SPEED_LAUNCH_ICEBEAM, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    Angry_IceBeamTimer += Time.deltaTime;//【愤怒_冰光】计时器时间增加
                                    //到达发射时间
                                    if (Angry_IceBeamTimer >= TIME_LAUNCH_ICEBEAM_LAUNCH)
                                    {
                                        isMove_Launch_Angry_IceBeam = false;
                                        isMove_Reaction_Angry_IceBeam = true;
                                        Angry_IceBeamTimer = 0;
                                        animator.SetInteger("IceBeam", 2);
                                        OverIceBeam();
                                        //计算十万马力冲刺目标点
                                        Position_Target_Combat142_Rush = ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_TargetAngle_IceBeam); ;
                                    }
                                }
                                if (isMove_Reaction_Angry_IceBeam)
                                {
                                    MoveBySpeedAndDir(-Dir_TargetAngle_IceBeam, SPEED_REACTION_ICEBEAM, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                    Angry_IceBeamTimer += Time.deltaTime;//【愤怒_冰光】计时器时间增加
                                    //到达发射时间
                                    if (Angry_IceBeamTimer >= TIME_REACTION_ICEBEAM_LAUNCH)
                                    {
                                        Angry_IceBeamOver();
                                        Angry_RushStart();
                                    }
                                }
                                break;
                            //【愤怒_十万马力冲刺】状态
                            case SubState.Angry_Rush:

                                Angry_RushTimer += Time.deltaTime;//【愤怒_十万马力冲刺】计时器时间增加
                                //准备阶段
                                if (Angry_RushTimer < TIME_ANGRY_RUSH_PREPARE)
                                {
  
                                }
                                //准备阶段结束 转进冲刺
                                if (!isMove_Angry_Rush && Angry_RushTimer >= TIME_ANGRY_RUSH_PREPARE)
                                {
                                    isMove_Angry_Rush = true;
                                    animator.SetInteger("Rush", 1);
                                }
                                //冲刺阶段
                                if (isMove_Angry_Rush && isMove_Angry_Rush_Jump && Angry_RushTimer >= TIME_ANGRY_RUSH_PREPARE)
                                {
                                    MoveBySpeedAndDir(Dir_TargetAngle_Rush, speed, SPEEDALPHA_ANGRY_RUSH, 0.0f, 0.0f, 0.0f, 0.0f);
                                    //接近玩家或者接近房间墙壁
                                    if (Vector2.Distance(Position_Target_Combat142_Rush, (Vector2)transform.position) < DISTENCE_ANGRY_RUSH_OVER_CLOSEMIN ||
                                        Vector2.Distance(TargetPosition, (Vector2)transform.position) < DISTENCE_ANGRY_RUSH_OVER_CLOSEMIN ||
                                        Angry_RushTimer >= (7.0f + TIME_ANGRY_RUSH_PREPARE)
                                        )
                                    {
                                        animator.SetInteger("Rush" , 2);
                                        isMove_Angry_Rush = false;
                                    }
                                }
                                break;
                            //【愤怒_吼叫暴雪】状态
                            case SubState.Angry_Roar:
                                Angry_RoarTimer -= Time.deltaTime;//【愤怒_吼叫暴雪】计时器时间减少
                                if (Angry_RoarTimer <= 0)         //计时器时间到时间，结束【愤怒_吼叫暴雪】状态
                                {
                                    Angry_RoarOver();
                                    // 愤怒等级1 2时转进发呆
                                    if (NowAngryLevel == AngryLevel.LEVEL1 || NowAngryLevel == AngryLevel.LEVEL2)
                                    {
                                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                                    }
                                    //愤怒等级3时转进围绕暴风雪中心旋转时
                                    else
                                    {
                                        Angry_BlizzardCenterTurnStart(TIME_ANGRY_BLIZZARDCENTERTURN);
                                    }
                                }
                                break;
                            //【愤怒_围绕暴风雪中心旋转】状态
                            case SubState.Angry_BlizzardCenterTurn:
                                if (isMove_BlizzardCenterTurn) {
                                    Angry_BlizzardCenterTurnTimer -= Time.deltaTime;//【愤怒_围绕暴风雪中心旋转】计时器时间减少
                                    //确定方向
                                    Dir_Angry_BlizzardCenterTurn = (Position_Center_Angry_BlizzardCenterTurn - (Vector2)transform.position).normalized;
                                    Dir_Angry_BlizzardCenterTurn = (Quaternion.AngleAxis(90.0f, Vector3.forward) * Dir_Angry_BlizzardCenterTurn).normalized;
                                    if (Dir_Angry_BlizzardCenterTurn == Vector2.zero) { Dir_Angry_BlizzardCenterTurn = Vector2.right; }
                                    //移动半径
                                    if (Mathf.Abs(Vector2.Distance(Position_Center_Angry_BlizzardCenterTurn , (Vector2)transform.position) - RADIUS_ANGRY_BLIZZARDCENTERTURN) > DISTENCE_OFFSET_ANGRY_BLIZZARDCENTERTURN) {
                                        //在直径外
                                        if ((Vector2.Distance(Position_Center_Angry_BlizzardCenterTurn, (Vector2)transform.position) - RADIUS_ANGRY_BLIZZARDCENTERTURN) > 0 )
                                        {
                                            Debug.Log("Ou");
                                            Dir_Angry_BlizzardCenterTurn += (Position_Center_Angry_BlizzardCenterTurn - (Vector2)transform.position).normalized;
                                            Dir_Angry_BlizzardCenterTurn.Normalize();
                                        }
                                        //在直径内
                                        else
                                        {
                                            Debug.Log("In");
                                            Dir_Angry_BlizzardCenterTurn -= (Position_Center_Angry_BlizzardCenterTurn - (Vector2)transform.position).normalized;
                                            Dir_Angry_BlizzardCenterTurn.Normalize();
                                        }
                                    }
                                    //仅在移动半径上释放冰柱
                                    else
                                    {
                                        //float a = 360.0f / (float)COUNT_ANGRY_BLIZZARDCENTERTURN_ICICLECOUNT;
                                        //if (_mTool.Angle_360Y(Dir_Angry_BlizzardCenterTurn,Vector3.right) % a < 5.0f )
                                        //{
                                        //       //向移动方向反方向 屁股后面 发射冰柱
                                        //        Vector2 p = (Vector2)transform.position;
                                        //        Lunch_IcicleCrash_Point(p);
                                        //}
                                        // 冰柱坠击
                                        Angry_BlizzardCenterTurn_IcicleCrash_Timer += Time.deltaTime;
                                        if (Angry_BlizzardCenterTurn_IcicleCrash_Timer > TIME_INTERVAL_ANGRY_BLIZZARDCENTERTURN_ICICLECRASH)
                                        {
                                            Angry_BlizzardCenterTurn_IcicleCrash_Timer = 0.0f;
                                            //向移动方向反方向 屁股后面 发射冰柱
                                            Vector2 p = -Dir_Angry_BlizzardCenterTurn * 1.5f + (Vector2)transform.position;
                                            Lunch_IcicleCrash_Point(p);
                                        }
                                    }
                                    //移动
                                    SetDirector(_mTool.MainVector2(Dir_Angry_BlizzardCenterTurn.normalized));
                                    MoveBySpeedAndDir(Dir_Angry_BlizzardCenterTurn, speed, SPEEDALPHA_ANGRY_BLIZZARDCENTERTURN * (isEmptyConfusionDone ? 0.6f : 1.0f), 0.0f, 0.0f, 0.0f, 0.0f);
                                    DriftDust.SetRotation(Dir_Angry_BlizzardCenterTurn);
                                    if (Angry_BlizzardCenterTurnTimer <= 0 && Vector2.Angle(Dir_Angry_BlizzardCenterTurn , Vector2.right) < 5.0f)         //计时器时间到时间，进入结束结束
                                    {
                                        isMove_BlizzardCenterTurn = false;
                                        animator.SetInteger("Drift", 2);
                                        loopPlayer.StopLoop(0.3f);
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


            //确实目标位置
            TargetPosition = player.transform.position;
            if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }

        }
    }



























    //■■■■■■■■■■■■■■■■■■■■碰撞■■■■■■■■■■■■■■■■■■■■■■

    //冰旋伤害
    static int DMAGE_ICESPINNER = 80;
    //冰旋击退值
    static float KOPOINT_ICESPINNER = 7.0f;


    //冰牙伤害
    static int DMAGE_ICEFANG = 65;
    //冰牙击退值
    static float KOPOINT_ICEFANG = 4.0f;


    //冰牙伤害
    static int DMAGE_HIGHHOURSEPOWER = 95;
    //冰牙击退值
    static float KOPOINT_HIGHHOURSEPOWER = 7.0f;








    private void OnCollisionEnter2D(Collision2D other)
    {
        CollisionRoomOrEnviroment(other);
        if (other.transform.tag == ("Player"))//与玩家碰撞时
        {
            //判断状态机
            switch (NowMainState)
            {
                //一般状态碰撞
                case MainState.Normal:
                    switch (NowSubState)
                    {
                        //一般_发呆
                        case SubState.Normal_Idle:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                        //一般_奔跑追踪
                        case SubState.Normal_Run:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                        //一般_冰柱坠击
                        case SubState.Normal_IcicleCrash:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                        //一般_反弹冰旋
                        case SubState.Normal_Spinner:
                            CollisionEnter2DEvent_IceSpinner(other.gameObject);//冰旋碰撞
                            break;
                        //一般_冰牙
                        case SubState.Normal_IceFang:
                            CollisionEnter2DEvent_IceFang(other.gameObject);//冰牙碰撞
                            break;
                        //一般_跳跃地震
                        case SubState.Normal_Jump:
                            //无触碰 防止误触
                            break;
                        //一般_吼叫暴雪
                        case SubState.Normal_Roar:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                    }
                    break;
                //愤怒状态碰撞
                case MainState.Angry:
                    switch (NowSubState)
                    {
                        //愤怒_发呆
                        case SubState.Angry_Idle:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                        //愤怒_奔跑追踪
                        case SubState.Angry_Run:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                        //愤怒_冰柱坠击
                        case SubState.Angry_IcicleCrash:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                        //愤怒_反弹冰旋
                        case SubState.Angry_Spinner:
                            CollisionEnter2DEvent_IceSpinner(other.gameObject);//冰旋碰撞
                            break;
                        //愤怒_冰牙
                        case SubState.Angry_IceFang:
                            CollisionEnter2DEvent_IceFang(other.gameObject);//冰牙碰撞
                            break;
                        //愤怒_跳跃地震
                        case SubState.Angry_Jump:
                            //无触碰 防止误触
                            break;
                        //愤怒_吼叫暴雪
                        case SubState.Angry_Roar:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                        //愤怒_冰光
                        case SubState.Angry_IceBeam:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                        //愤怒_漂移滑行
                        case SubState.Angry_Drift:
                            CollisionEnter2DEvent_HighHoursePower(other.gameObject); //十万马力碰撞
                            break;
                        //愤怒_十万马力冲刺
                        case SubState.Angry_Rush:
                            //无触碰 防止误触
                            break;
                        //愤怒_围绕暴风雪中心旋转
                        case SubState.Angry_BlizzardCenterTurn:
                            EmptyTouchHit(other.gameObject);//触发触碰伤害
                            break;
                    }
                    break;
            }
        }
        //EmptyTouchHit(other.gameObject);//触发触碰伤害
    }






    /// <summary>
    /// 冰旋碰撞
    /// </summary>
    /// <param name="target"></param>
    public void CollisionEnter2DEvent_IceSpinner(GameObject target)
    {
        if (target.tag == "Player")
        {
            PlayerControler player = target.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(this.gameObject, target.gameObject, DMAGE_ICESPINNER, 0, 0, PokemonType.TypeEnum.Ice);
            if (player != null)
            {
                player.KnockOutPoint = KOPOINT_ICESPINNER;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
            }
        }
    }



    /// <summary>
    /// 冰牙碰撞
    /// </summary>
    /// <param name="target"></param>
    public void CollisionEnter2DEvent_IceFang(GameObject target)
    {
        if (target.tag == "Player")
        {
            PlayerControler player = target.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(this.gameObject, target.gameObject, DMAGE_ICEFANG, 0, 0, PokemonType.TypeEnum.Ice);
            if (player != null)
            {
                player.KnockOutPoint = KOPOINT_ICEFANG;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
                player.PlayerFrozenFloatPlus(0.35f, 2.0f);
            }
        }
    }

    /// <summary>
    /// 十万马力碰撞
    /// </summary>
    /// <param name="target"></param>
    public void CollisionEnter2DEvent_HighHoursePower(GameObject target)
    {
        if (target.tag == "Player")
        {
            PlayerControler player = target.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(this.gameObject, target.gameObject, DMAGE_HIGHHOURSEPOWER, 0, 0, PokemonType.TypeEnum.Ground);
            if (player != null)
            {
                player.KnockOutPoint = KOPOINT_HIGHHOURSEPOWER;
                player.KnockOutDirection = (player.transform.position - this.transform.position).normalized;
            }
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
                case SubState.Normal_IceFang:
                    if (isRush_Normal_IceFang) { isRush_Normal_IceFang = false; ParentPokemonRoom.CameraShake(1.0f, 2.5f, true); }
                    break;
                case SubState.Angry_IceFang:
                    if (isRush_Angry_IceFang) { isRush_Angry_IceFang = false; ParentPokemonRoom.CameraShake(1.0f, 2.5f, true); }
                    break;
            }
        }
    }














    //=================================碰壁反弹=======================================


    private void OnCollisionStay2D(Collision2D other)
    {
        ReboundCheck(other.gameObject);
    }

    /// <summary>
    /// 反弹检查
    /// </summary>
    /// <param name="other"></param>
    public void ReboundCheck(GameObject other)
    {
        if (other.tag == "Room" && (NowSubState == SubState.Normal_Spinner || NowSubState == SubState.Angry_Spinner))
        {
            Rebound_BeLunch();
        }
    }


    /// <summary>
    /// 反弹限制计时器的时间上限
    /// </summary>
    static float TIME_REBOUND_CD = 0.1f;

    /// <summary>
    /// 反弹限制计时器 防止在一次碰撞中多次反弹
    /// </summary>
    float ReboundTimer;

    /// <summary>
    /// 被发射时发生反弹
    /// </summary>
    bool Rebound_BeLunch()
    {
        //当前时间
        float now = Time.time;

        if (now - ReboundTimer < TIME_REBOUND_CD) { return false; } // 防抖：0.1s 内忽略

        ReboundTimer = now;

        //碰撞方向
        Vector2 HitVector = Vector2.right;
        {
            Vector2 EmptyCenter = (Vector2)transform.position + GetComponent<Collider2D>().offset;
            float[] DistenceList = new float[] { 0.0f, 0.0f, 0.0f, 0.0f };
            RaycastHit2D RPRay = Physics2D.Raycast(EmptyCenter, Vector2.right, LayerMask.GetMask("Room"));
            RaycastHit2D UPRay = Physics2D.Raycast(EmptyCenter, Vector2.up, LayerMask.GetMask("Room"));
            RaycastHit2D LPRay = Physics2D.Raycast(EmptyCenter, Vector2.left, LayerMask.GetMask("Room"));
            RaycastHit2D DPRay = Physics2D.Raycast(EmptyCenter, Vector2.down, LayerMask.GetMask("Room"));
            if (RPRay) { DistenceList[0] = RPRay.distance; Debug.DrawLine(EmptyCenter, RPRay.point, Color.red, 0.1f); }
            if (UPRay) { DistenceList[1] = UPRay.distance; Debug.DrawLine(EmptyCenter, UPRay.point, Color.red, 0.1f); }
            if (LPRay) { DistenceList[2] = LPRay.distance; Debug.DrawLine(EmptyCenter, LPRay.point, Color.red, 0.1f); }
            if (DPRay) { DistenceList[3] = DPRay.distance; Debug.DrawLine(EmptyCenter, DPRay.point, Color.red, 0.1f); }
            float MinDistence = DistenceList[0];
            int MinIndex = 0;
            for (int i = 1; i < 4; i++)
            {
                if (MinDistence > DistenceList[i])
                {
                    MinDistence = DistenceList[i];
                    MinIndex = i;
                }
            }
            //Debug.Log("R" + DistenceList[0] + "U" + DistenceList[1] + "L" + DistenceList[2] + "D" + DistenceList[3] + "+" + MinDistence);
            HitVector = _mTool.MainVector2(Quaternion.AngleAxis(MinIndex * 90.0f, Vector3.forward) * HitVector);
        }


        switch (NowSubState)
        {
            case SubState.Normal_Spinner:
                ParentPokemonRoom.CameraShake(1.0f, 3.0f, true);
                Count_Rebound_Normal_Spinner++;
                Dir_Normall_Spinner = _mTool.TiltMainVector2(Dir_Normall_Spinner);
                Dir_Normall_Spinner = new Vector2(Dir_Normall_Spinner.x * (HitVector.x == 0 ? 1 : -1), Dir_Normall_Spinner.y * (HitVector.y == 0 ? 1 : -1));
                break;
            case SubState.Angry_Spinner:
                ParentPokemonRoom.CameraShake(1.0f, 3.0f, true);
                Count_Rebound_Angry_Spinner++;
                if (Count_Rebound_Angry_Spinner % 3 == 0)
                {
                    Dir_Angry_Spinner = (TargetPosition - (Vector2)this.transform.position).normalized;
                }
                else
                {
                    Dir_Angry_Spinner = _mTool.TiltMainVector2(Dir_Angry_Spinner);
                    Dir_Angry_Spinner = new Vector2(Dir_Angry_Spinner.x * (HitVector.x == 0 ? 1 : -1), Dir_Angry_Spinner.y * (HitVector.y == 0 ? 1 : -1));
                }
                break;
        }



        return true;

    }


    //=================================碰壁反弹=======================================






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
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence)
            {
                if (NowSubState == SubState.Normal_Run || NowSubState == SubState.Angry_Run) {
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
        ParentPokemonRoom.CameraShake(0.3f, 0.8f, true);
        audioPlayer.Play(CetitanSE.Step, transform.position);
    }



    /// <summary>
    /// 清空所有状态机
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

        IsDefStateByNormal = false;


        //重置主状态机参数
        Position_Combat040_Normal_Jump_Target = Vector2.zero;
        Combat110_Count_IcicleCrash = 0;
        Combat230_Count_IcileCrash = 0;
        Position_Combat141_Angry_Jump_Target = Vector2.zero;
        Position_Target_Combat142_Rush = Vector2.zero;
        Dir_IceFangRush_Combat150250_Rush = Vector2.zero;
        Combat250_Count_IceFang = 0; 


        //结束所有副状态机 重置所有副状态机参数
        switch (NowSubState)
        {

            case SubState.Normal_Idle: Normal_IdleOver(); break;           //一般_发呆
            case SubState.Normal_Run: Normal_RunOver(); break;            //一般_奔跑追踪
            case SubState.Normal_IcicleCrash: Normal_IcicleCrashOver(); break;    //一般_冰柱坠击
            case SubState.Normal_Spinner: Normal_SpinnerOver(); break;        //一般_反弹冰旋
            case SubState.Normal_IceFang: Normal_IceFangOver(); break;        //一般_冰牙
            case SubState.Normal_Jump: Normal_JumpOver(); break;           //一般_跳跃地震
            case SubState.Normal_Roar: Normal_RoarOver(); break;           //一般_吼叫暴雪

            case SubState.Angry_Idle: Angry_IdleOver(); break;            //愤怒_发呆
            case SubState.Angry_Run: Angry_RunOver(); break;             //愤怒_奔跑追踪
            case SubState.Angry_IcicleCrash: Angry_IcicleCrashOver(); break;     //愤怒_冰柱坠击
            case SubState.Angry_Spinner: Angry_SpinnerOver(); break;         //愤怒_反弹冰旋
            case SubState.Angry_IceFang: Angry_IceFangOver(); break;         //愤怒_冰牙
            case SubState.Angry_Jump: Angry_JumpOver(); break;            //愤怒_跳跃地震
            case SubState.Angry_Drift: Angry_DriftOver(); break;           //愤怒_漂移滑行
            case SubState.Angry_IceBeam: Angry_IceBeamOver(); break;         //愤怒_冰光
            case SubState.Angry_Rush: Angry_RushOver(); break;            //愤怒_十万马力冲刺
            case SubState.Angry_Roar: Angry_RoarOver(); break;            //愤怒_吼叫暴雪
            case SubState.Angry_BlizzardCenterTurn: Angry_BlizzardCenterTurnOver(); break;            //愤怒_围绕暴风雪中心旋转
        }

        //重置所有动画机参数状态
        animator.SetFloat("Speed", 0);
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Atk");
        animator.ResetTrigger("Roar");
        animator.SetInteger("Drift", 0);
        animator.SetInteger("Jump", 0);
        animator.SetInteger("Spinner", 0);
        animator.SetInteger("Rush", 0);
        animator.SetInteger("IceBeam", 0);


        //消除实例
        Break_AllIcicleCrash();
        if (SpinnerObj != null) { SpinnerObj.SpinnerOver(); }
        if (IceFangObj != null) { Destroy(IceFangObj.gameObject); }
        if (EarthquakeObj != null) { EarthquakeObj.EarthquakeOver(); }
        if (IceBeamOBJ_1 != null) { IceBeamOBJ_1.StopBeam(); }
        if (IceBeamOBJ_2 != null) { IceBeamOBJ_2.StopBeam(); }
        DriftDust.StopPSL();
        DriftDust.StopPSR();

        loopPlayer.StopLoop(0.0f);
    }



    /// <summary>
    /// 清除所有冰柱
    /// </summary>
    void Break_AllIcicleCrash()
    {
        if (icObjList.Count != 0)
        {
            List<CetitanIcicleCrash> l = new List<CetitanIcicleCrash> { };
            for (int i = 0; i < icObjList.Count; i++)
            {
                l.Add(icObjList[i]);
            }

            for (int i = 0; i < l.Count; i++)
            {
                l[i].icOBJ.BreakByCetitan();
            }
        }
    }




    /// <summary>
    /// 清除某个圆/圆环内所有冰柱
    /// </summary>
    public void Break_IcleCrash_Circle( Vector2 Center , float Radius ,float InnerRadius = 0 )
    {
        if (icObjList.Count != 0)
        {
            List<CetitanIcicleCrash> l = new List<CetitanIcicleCrash> { };
            for (int i = 0; i < icObjList.Count; i++)
            {
                if (Vector2.Distance( Center , (Vector2)icObjList[i].transform.position) <= Radius &&
                    Vector2.Distance(Center, (Vector2)icObjList[i].transform.position) >= InnerRadius
                    ) {
                    l.Add(icObjList[i]); 
                }
            }

            for (int i = 0; i < l.Count; i++)
            {
                l[i].icOBJ.BreakByCetitan();
            }
        }
    }


    public override void DieEvent()
    {
        base.DieEvent();
        //暴风雪结束
        if (BlizzardObj != null) { BlizzardObj.BlizzardOver(); }
    }



    public override void StoreDestoryEvent()
    {
        base.StoreDestoryEvent();
        Break_AllIcicleCrash();
        //暴风雪销毁
        if (BlizzardObj != null) { Destroy(BlizzardObj.gameObject); }
    }


    /// <summary>
    /// 混乱时角度偏转
    /// </summary>
    Vector2 ConfusionDir(Vector2 dir, float Alpha)
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
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■










































    //■■■■■■■■■■■■■■■■■■■■动画机事件■■■■■■■■■■■■■■■■■■■■■■


    //========================攻击动画==============================

    //攻击作为动画开始
    public void Atk_In()
    {
        switch (NowSubState)
        {
            //一般冰柱状态
            case SubState.Normal_IcicleCrash:
                LastTargetPosition_Normal_IcicleCrash = TargetPosition;
                break;
            //愤怒冰柱状态
            case SubState.Angry_IcicleCrash:
                switch (NowCombat)
                {
                    //连招110[冰柱坠击0][冰柱坠击1]
                    case CombatList.Angry_Level1_Combat110:
                        //预判角度
                        LastTargetPosition_Angry_IcicleCrash = TargetPosition;
                        break;
                    //连招130 [冰牙0][冰柱坠击0]
                    case CombatList.Angry_Level1_Combat130:
                        //预判角度
                        LastTargetPosition_Angry_IcicleCrash = TargetPosition;
                        break;
                    //连招230 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
                    case CombatList.Angry_Level2_Combat230:
                        //预判角度
                        LastTargetPosition_Angry_IcicleCrash = TargetPosition;
                        break;
                    //连招141[冰柱坠击0][跳跃地震0](圆环冰柱封锁玩家)
                    case CombatList.Angry_Level1_Combat141:
                        break;
                    default:
                        break;
                }
                break;

        }
    }


    //攻击作为行为开始
    public void Atk_Start()
    {
        InstantiateRunDust();
        switch (NowSubState)
        {
            //一般冰柱状态
            case SubState.Normal_IcicleCrash:
                switch (NowCombat)
                {
                    //连招010[冰柱坠击0](三点冰柱)
                    case CombatList.Normal_Combat010:
                        LunchIcicleCrash((TargetPosition - LastTargetPosition_Normal_IcicleCrash).normalized, RADIUS_NORMAL_ICICLECRASH_COMBAT010);
                        break;
                    //连招040[冰柱坠击0][跳跃地震0](圆环冰柱封锁玩家)
                    case CombatList.Normal_Combat040:
                        //设置跳跃目标点
                        Position_Combat040_Normal_Jump_Target = TargetPosition;
                        Lunch_IcicleCrash_Circle(TargetPosition, RADIUS_ICICLECRASH_COMBAT040_141, COUNT_ICICLECRASH_COMBAT040_141);
                        break;
                }
                break;
            //一般冰牙状态
            case SubState.Normal_IceFang:
                //isRush_Normal_IceFang = true;
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                isRush_Normal_IceFang = true;
                Dir_Normal_IceFang_Rush = ConfusionDir(Dir_Normal_IceFang_Rush, 30.0f);
                Lunch_IceFang();
                break;
            //愤怒冰柱状态
            case SubState.Angry_IcicleCrash:
                switch (NowCombat)
                {
                    //连招110[冰柱坠击0][冰柱坠击1]
                    case CombatList.Angry_Level1_Combat110:
                        //[冰柱坠击0]第一次放冰柱预判角度
                        if (Combat110_Count_IcicleCrash == 0) {
                            LunchIcicleCrash((TargetPosition - LastTargetPosition_Angry_IcicleCrash).normalized, RADIUS_ANGRY_ICICLECRASH_COMBAT110_1);
                        }
                        //[冰柱坠击1]第二次放冰柱预判角度
                        else if (Combat110_Count_IcicleCrash == 1) {
                            LunchIcicleCrash((TargetPosition - LastTargetPosition_Angry_IcicleCrash).normalized, RADIUS_ANGRY_ICICLECRASH_COMBAT110_2);
                        }
                        break;
                    //连招130 [冰牙0][冰柱坠击0]
                    case CombatList.Angry_Level1_Combat130:
                        //预判角度
                        LunchIcicleCrash((TargetPosition - LastTargetPosition_Angry_IcicleCrash).normalized, RADIUS_ANGRY_ICICLECRASH_COMBAT110_1);
                        break;
                    //连招230 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
                    case CombatList.Angry_Level2_Combat230:
                        LunchIcicleCrash((TargetPosition - LastTargetPosition_Angry_IcicleCrash).normalized, RADIUS_ANGRY_ICICLECRASH_COMBAT110_2);
                        break;
                    //连招141[冰柱坠击0][跳跃地震0](圆环冰柱封锁玩家)
                    case CombatList.Angry_Level1_Combat141:
                        //设置跳跃目标点
                        Position_Combat141_Angry_Jump_Target = TargetPosition;
                        Lunch_IcicleCrash_Circle(TargetPosition, RADIUS_ICICLECRASH_COMBAT040_141, COUNT_ICICLECRASH_COMBAT040_141);
                        break;
                    default:
                        break;
                }
                break;
            //愤怒冰牙状态
            case SubState.Angry_IceFang:
                switch (NowCombat)
                {
                    //连招150的冰牙更改方向
                    case CombatList.Angry_Level1_Combat150:
                        Dir_Angry_IceFang_Rush = Dir_IceFangRush_Combat150250_Rush.normalized;
                        SetDirector(_mTool.MainVector2(Dir_Angry_IceFang_Rush));
                        break;
                    //连招250的冰牙更改方向
                    case CombatList.Angry_Level2_Combat250:
                        Dir_Angry_IceFang_Rush = Dir_IceFangRush_Combat150250_Rush.normalized;
                        SetDirector(_mTool.MainVector2(Dir_Angry_IceFang_Rush));
                        break;
                    default:
                        //连招230的后两次冰牙更改方向
                        if ((NowCombat == CombatList.Angry_Level2_Combat230 && Combat230_Count_IcileCrash > 0)
                            /* || NowCombat == CombatList.Angry_Level1_Combat150 */
                        )
                        {
                            Dir_Angry_IceFang_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                            SetDirector(_mTool.MainVector2(Dir_Angry_IceFang_Rush));
                        }
                        break;
                }
                //isRush_Angry_IceFang = true;
                if (ShadowCoroutine == null)
                {
                    StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                }
                isRush_Angry_IceFang = true;
                Dir_Angry_IceFang_Rush = ConfusionDir(Dir_Angry_IceFang_Rush, 30.0f);
                Lunch_IceFang();
                break;
            //愤怒冰光状态
            case SubState.Angry_IceBeam:
                LunchIceBeam();
                break;
        }
    }


    //攻击作为行为结束
    public void Atk_Over()
    {
        switch (NowSubState)
        {
            //一般冰牙状态
            case SubState.Normal_IceFang:
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                isRush_Normal_IceFang = false;
                break;
            //愤怒冰牙状态
            case SubState.Angry_IceFang:
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
                isRush_Angry_IceFang = false;
                break;

        }
    }


    //攻击作为动画结束
    public void Atk_Out()
    {
        switch (NowSubState)
        {
            //一般冰柱状态
            case SubState.Normal_IcicleCrash:
                switch (NowCombat)
                {
                    //连招010[冰柱坠击0]
                    case CombatList.Normal_Combat010:
                        Normal_IcicleCrashOver();
                        Normal_IdleStart(TIME_NORMAL_COMBAT010_CD);
                        break;
                    //连招040[冰柱坠击0]→[跳跃地震0]
                    case CombatList.Normal_Combat040:
                        Normal_IcicleCrashOver();
                        Normal_JumpStart();
                        break;
                    default:
                        Normal_IcicleCrashOver();
                        Normal_IdleStart(TIME_NORMAL_IDLE_START);
                        break;
                }
                break;
            //一般冰牙状态
            case SubState.Normal_IceFang:
                switch (NowCombat)
                {
                    //连招030[冰牙0]
                    case CombatList.Normal_Combat030:
                        Normal_IceFangOver();
                        Normal_IdleStart(TIME_NORMAL_COMBAT030_CD);
                        break;
                    default:
                        Normal_IceFangOver();
                        Normal_IdleStart(TIME_NORMAL_IDLE_START);
                        break;
                }
                break;
            //愤怒冰柱状态
            case SubState.Angry_IcicleCrash:
                switch (NowCombat)
                {
                    //连招110[冰柱坠击0][冰柱坠击1]
                    case CombatList.Angry_Level1_Combat110:
                        //[冰柱坠击0]→[冰柱坠击1] 增加释放次数
                        if (Combat110_Count_IcicleCrash == 0)
                        {
                            Angry_IcicleCrashOver();
                            Angry_IcicleCrashStart();
                            Combat110_Count_IcicleCrash++;
                        }
                        //[冰柱坠击0][冰柱坠击1]→休息 
                        else
                        {
                            Angry_IcicleCrashOver();
                            Combat110_Count_IcicleCrash = 0;
                            Angry_IdleStart(TIME_ANGRYL_COMBAT110_CD);
                        }
                        break;
                    //连招130 [冰牙0][冰柱坠击0]→休息
                    case CombatList.Angry_Level1_Combat130:
                        Angry_IcicleCrashOver();
                        Angry_IdleStart(TIME_ANGRYL_COMBAT130_CD);
                        break;
                    //连招230 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
                    case CombatList.Angry_Level2_Combat230:
                        //连招230 [冰牙0][冰柱坠击0]→[冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
                        //连招230 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1]→[冰牙2][冰柱坠击2]
                        if (Combat230_Count_IcileCrash == 0 || Combat230_Count_IcileCrash == 1)
                        {
                            Combat230_Count_IcileCrash++;
                            Angry_IcicleCrashOver();
                            Angry_IceFangStart();
                        }
                        //连招230 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]→休息
                        else if (Combat230_Count_IcileCrash == 2)
                        {
                            Combat230_Count_IcileCrash = 0;
                            Angry_IcicleCrashOver();
                            Angry_IdleStart(TIME_ANGRYL_COMBAT230_CD);
                        }
                        else
                        {
                            Combat230_Count_IcileCrash = 0;
                            Angry_IcicleCrashOver();
                            Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        }
                        break;
                    //连招040[冰柱坠击0]→[跳跃地震0]
                    case CombatList.Angry_Level1_Combat141:
                        Angry_IcicleCrashOver();
                        Angry_JumpStart();
                        break;
                    default:
                        Angry_IcicleCrashOver();
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
            //愤怒冰牙状态
            case SubState.Angry_IceFang:
                switch (NowCombat)
                {
                    //连招130 [冰牙0]→[冰柱坠击0]
                    case CombatList.Angry_Level1_Combat130:
                        Angry_IceFangOver();
                        Angry_IcicleCrashStart();
                        break;
                    //连招230 [冰牙0]→[冰柱坠击0][冰牙1][冰柱坠击1][冰牙2][冰柱坠击2]
                    //连招230 [冰牙0][冰柱坠击0][冰牙1]→[冰柱坠击1][冰牙2][冰柱坠击2]
                    //连招230 [冰牙0][冰柱坠击0][冰牙1][冰柱坠击1][冰牙2]→[冰柱坠击2]
                    case CombatList.Angry_Level2_Combat230:
                        Angry_IceFangOver();
                        Angry_IcicleCrashStart();
                        break;
                    //连招150 [漂移滑行0][冰牙0]→休息
                    case CombatList.Angry_Level1_Combat150:
                        Angry_IceFangOver();
                        Dir_IceFangRush_Combat150250_Rush = Vector2.zero;
                        Angry_IdleStart(TIME_ANGRYL_COMBAT150_CD);
                        break;
                    //连招250 [漂移滑行0][冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2]
                    case CombatList.Angry_Level2_Combat250:
                        Angry_IceFangOver();
                        //连招250 [漂移滑行0][冰牙0]→[漂移滑行1][冰牙1][漂移滑行2][冰牙2]
                        //连招250 [漂移滑行0][冰牙0][漂移滑行1][冰牙1]→[漂移滑行2][冰牙2]
                        if (Combat250_Count_IceFang == 0 || Combat250_Count_IceFang == 1)
                        {
                            Combat250_Count_IceFang += 1;
                            Dir_IceFangRush_Combat150250_Rush = Vector2.zero;
                            Angry_DriftStart();
                        }
                        //连招250 [漂移滑行0][冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2]→休息
                        else
                        {
                            Combat250_Count_IceFang = 0;
                            Dir_IceFangRush_Combat150250_Rush = Vector2.zero;
                            Angry_IdleStart(TIME_ANGRYL_COMBAT250_CD);
                        }
                        break;
                    default:
                        Angry_IceFangOver();
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
            //愤怒冰光状态
            case SubState.Angry_IceBeam:
                //开始调整角度
                animator.SetInteger("IceBeam", 1);
                isMove_Launch_Angry_IceBeam = true;
                isMove_Reaction_Angry_IceBeam = false;
                break;
        }
    }

    //========================攻击动画==============================








    //========================跳跃动画==============================

    /// <summary>
    /// 跳跃作为动画开始
    /// </summary>
    public void AnimatorEvent_Jump_In()
    {
    }
    /// <summary>
    /// 跳跃开始蓄力
    /// </summary>
    public void AnimatorEvent_Jump_Prepare()
    {
        switch (NowSubState)
        {
            //一般跳跃
            case SubState.Normal_Jump:
                isCharge_Normal_Jump = true;
                break;
            //愤怒跳跃
            case SubState.Angry_Jump:
                isCharge_Angry_Jump = true;
                break;
        }
    }
    /// <summary>
    /// 跳跃蓄力完毕，发射
    /// </summary>
    public void AnimatorEvent_Jump_Lunch()
    {

    }
    /// <summary>
    /// 跳跃作为动作事件开始
    /// </summary>
    public void AnimatorEvent_Jump_Start()
    {
        switch (NowSubState)
        {
            //一般跳跃
            case SubState.Normal_Jump:
                isMove_Normal_Jump = true;
                Speed_Normal_Jump = (Vector2.Distance(Position_Combat040_Normal_Jump_Target, (Vector2)transform.position) / Time_Normal_Jump);
                //Dir_Normal_Jump = (TargetPosition - (Vector2)transform.position).normalized;
                break;
            //愤怒跳跃
            case SubState.Angry_Jump:
                isMove_Angry_Jump = true;
                Speed_Angry_Jump = (Vector2.Distance(Position_Combat141_Angry_Jump_Target, (Vector2)transform.position) / Time_Angry_Jump);
                break;
        }
    }
    /// <summary>
    /// 跳跃作为动作事件结束
    /// </summary>
    public void AnimatorEvent_Jump_Over()
    {
        switch (NowSubState)
        {
            //一般跳跃
            case SubState.Normal_Jump:
                Lunch_Earthquake();
                Break_AllIcicleCrash();
                break;
            //愤怒跳跃
            case SubState.Angry_Jump:
                Lunch_Earthquake();
                Break_AllIcicleCrash();
                break;
        }
    }
    /// <summary>
    /// 跳跃作为动画结束
    /// </summary>
    public void AnimatorEvent_Jump_Out()
    {
        switch (NowSubState)
        {
            //一般跳跃
            case SubState.Normal_Jump:
                switch (NowCombat)
                {
                    //连招040[冰柱坠击0][跳跃地震0]→
                    case CombatList.Normal_Combat040:
                        Normal_JumpOver();
                        Normal_IdleStart(TIME_NORMAL_COMBAT040_CD);
                        break;
                    default:
                        Normal_IcicleCrashOver();
                        Normal_IdleStart(TIME_NORMAL_IDLE_START);
                        break;
                }
                break;
            //愤怒跳跃
            case SubState.Angry_Jump:
                switch (NowCombat)
                {
                    //连招141[冰柱坠击0][跳跃地震0]→
                    case CombatList.Angry_Level1_Combat141:
                        Angry_JumpOver();
                        Angry_IdleStart(TIME_ANGRYL_COMBAT141_CD);
                        break;
                    default:
                        Angry_IcicleCrashOver();
                        Angry_IdleStart(TIME_ANGRY_IDLE_START);
                        break;
                }
                break;
        }
    }

    //========================跳跃动画==============================










    //========================冲刺结束动画==============================


    public void AnimatorEvent_RushOver_Over()
    {
        Lunch_HighHorsepower();
    }



    public void AnimatorEvent_RushOver_Out()
    {
        Angry_RushOver();
        Angry_IdleStart(TIME_ANGRYL_COMBAT142_CD);
    }


    //========================冲刺结束动画==============================



    //========================滑行结束动画==============================


    public void AnimatorEvent_DriftOver_Over()
    {
        switch (NowSubState)
        {
            //愤怒滑行漂移状态
            case SubState.Angry_Drift:
                //Lunch_Bulldoze();
                Lunch_HighHorsepower();
                DriftDust.StopPSL();
                DriftDust.StopPSR();
                break;
            //愤怒围绕暴风雪中心旋转时
            case SubState.Angry_BlizzardCenterTurn:
                Lunch_HighHorsepower();
                DriftDust.StopPSL();
                break;
        }
    }



    public void AnimatorEvent_DriftOver_Out()
    {
        switch (NowSubState)
        {
            //愤怒滑行漂移状态
            case SubState.Angry_Drift:
                //连招150 [漂移滑行0]→[冰牙0]
                //连招250 [漂移滑行0]→[冰牙0][漂移滑行1][冰牙1][漂移滑行2][冰牙2]
                //连招250 [漂移滑行0]→[冰牙0][漂移滑行1]→[冰牙1][漂移滑行2][冰牙2]
                //连招250 [漂移滑行0]→[冰牙0][漂移滑行1][冰牙1][漂移滑行2]→[冰牙2]
                Angry_DriftOver();
                Angry_IceFangStart();
                //确定冰牙方向
                Dir_IceFangRush_Combat150250_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                break;
            //愤怒围绕暴风雪中心旋转时
            case SubState.Angry_BlizzardCenterTurn:
                Angry_BlizzardCenterTurnOver();
                Angry_IdleStart(TIME_ANGRYL__BLIZZARDCENTERTURN_CD);
                SetDirector(Vector3.down);
                break;
        }
    }


    //========================滑行结束动画==============================




    //■■■■■■■■■■■■■■■■■■■■动画机事件■■■■■■■■■■■■■■■■■■■■■■






























    //■■■■■■■■■■■■■■■■■■■■预制件■■■■■■■■■■■■■■■■■■■■■■


    /// <summary>
    /// 冰柱预制件
    /// </summary>
    public CetitanIcicleCrash icPrefab;

    /// <summary>
    /// 冰柱实例列表
    /// </summary>
    public List<CetitanIcicleCrash> ICObjList
    {
        get { return icObjList; }
        set { icObjList = value; }
    }
    public List<CetitanIcicleCrash> icObjList = new List<CetitanIcicleCrash> { };


    /// <summary>
    /// 冰粒预制件
    /// </summary>
    public CetitanIceShard isPrefab;


    /// <summary>
    /// 冰旋预制件
    /// </summary>
    public CetitanIceSpinner SpinnerPrefabs;

    /// <summary>
    /// 冰旋实例
    /// </summary>
    CetitanIceSpinner SpinnerObj;


    /// <summary>
    /// 冰牙预制件
    /// </summary>
    public CetitanIceFang IceFangPrefabs;

    /// <summary>
    /// 冰牙实例
    /// </summary>
    CetitanIceFang IceFangObj;


    /// <summary>
    /// 地震预制件
    /// </summary>
    public CetitanEarthquake EarthquakePrefabs;

    /// <summary>
    /// 地震实例
    /// </summary>
    CetitanEarthquake EarthquakeObj;


    /// <summary>
    /// 暴风雪预制件
    /// </summary>
    public CetitanBlizzard BlizzardPrefabs;

    /// <summary>
    /// 暴风雪实例
    /// </summary>
    CetitanBlizzard BlizzardObj;


    /// <summary>
    /// 雪崩预制件
    /// </summary>
    public CetitanAvalancheManger AvalancheMPrefabs;

    /// <summary>
    /// 雪崩实例
    /// </summary>
    CetitanAvalancheManger AvalancheMOBJ;


    /// <summary>
    /// 冰光预制件
    /// </summary>
    public CetitanIceBeam IceBeamPrefabs;

    /// <summary>
    /// 冰光实例1
    /// </summary>
    CetitanIceBeam IceBeamOBJ_1;

    /// <summary>
    /// 冰光实例2
    /// </summary>
    CetitanIceBeam IceBeamOBJ_2;

    /// <summary>
    /// 重踏预制件
    /// </summary>
    public CetitanBulldoze BulldozePrefabs;

    /// <summary>
    /// 十万马力预制件
    /// </summary>
    public CetitanHighHoursePower HighHoursePowerPrefabs;

    /// <summary>
    /// 滑行尘土特效
    /// </summary>
    public CetitanDriftDust DriftDust;


    //■■■■■■■■■■■■■■■■■■■■预制件■■■■■■■■■■■■■■■■■■■■■■



















    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction



    /// <summary>
    /// 连招040跳跃目标
    /// </summary>
    Vector2 Position_Combat040_Normal_Jump_Target = Vector2.zero;

    /// <summary>
    /// 连招110释放冰柱的次数
    /// </summary>
    int Combat110_Count_IcicleCrash = 0;

    /// <summary>
    /// 连招230释放冰柱的次数
    /// </summary>
    int Combat230_Count_IcileCrash = 0;

    /// <summary>
    /// 连招140跳跃目标
    /// </summary>
    Vector2 Position_Combat141_Angry_Jump_Target = Vector2.zero;

    /// <summary>
    /// 十万马力冲刺的目标点
    /// </summary>
    Vector2 Position_Target_Combat142_Rush;

    /// <summary>
    /// 连招150250的冰牙冲刺目标
    /// </summary>
    Vector2 Dir_IceFangRush_Combat150250_Rush;

    /// <summary>
    /// 连招250的释放冰牙次数
    /// </summary>
    int Combat250_Count_IceFang;



    //==■==■==■==■==■==■==■主状态：一般状态■==■==■==■==■==■==■==



    //=========================一般_发呆============================


    //开始后/默认的冷却时间
    static float TIME_NORMAL_IDLE_START = 0.5f;









    /// <summary>
    /// 一般_发呆计时器
    /// <summary>
    float Normal_IdleTimer = 0;



    /// <summary>
    /// 一般_发呆开始
    /// <summary>
    public void Normal_IdleStart(float Timer)
    {
        Normal_IdleTimer = Timer;
        ChangeSubState(SubState.Normal_Idle);
        IsDefStateByNormal = false;
    }

    /// <summary>
    /// 一般_发呆结束
    /// <summary>
    public void Normal_IdleOver()
    {
        Normal_IdleTimer = 0;
    }


    //=========================一般_发呆============================






    //=========================一般_奔跑追踪============================




    /// <summary>
    /// 一般_奔跑追踪计时器
    /// <summary>
    float Normal_RunTimer = 0;



    /// <summary>
    /// 一般_奔跑追踪开始
    /// <summary>
    public void Normal_RunStart(/*float Timer*/)
    {
        Normal_RunTimer = 0;
        ChangeSubState(SubState.Normal_Run);
    }

    /// <summary>
    /// 一般_奔跑追踪结束
    /// <summary>
    public void Normal_RunOver()
    {
        Normal_RunTimer = 0;
    }


    //=========================一般_奔跑追踪============================






    //=========================一般_冰柱坠击============================

    //三点冰柱距离目标的半径
    static float RADIUS_NORMAL_ICICLECRASH_COMBAT010 = 5.5f;

    //三点冰柱破裂后发出冰粒的个数
    static int COUNT_ICESHARD_ICBREAK = 6;


    //圆环冰柱距离目标的半径
    static float RADIUS_ICICLECRASH_COMBAT040_141 = 4.3f;

    //圆环冰柱的个数
    static int COUNT_ICICLECRASH_COMBAT040_141 = 28;





    //预判玩家用玩家旧坐标
    Vector2 LastTargetPosition_Normal_IcicleCrash = Vector2.zero;



    /// <summary>
    /// 一般_冰柱坠击开始
    /// <summary>
    public void Normal_IcicleCrashStart(/*float Timer*/)
    {
        //Normal_IcicleCrashTimer = 0;
        ChangeSubState(SubState.Normal_IcicleCrash);
        animator.SetTrigger("Atk");
        LastTargetPosition_Normal_IcicleCrash = Vector2.zero;
    }

    /// <summary>
    /// 一般_冰柱坠击结束
    /// <summary>
    public void Normal_IcicleCrashOver()
    {
        LastTargetPosition_Normal_IcicleCrash = Vector2.zero;
        //Normal_IcicleCrashTimer = 0;
    }


    /// <summary>
    /// 发射三点冰柱坠击
    /// </summary>
    void LunchIcicleCrash(Vector2 dir, float radius)
    {
        //Debug.Log(Dir_TargetMove);
        if (isEmptyConfusionDone) { dir = -dir;  }
        Vector2 Position_ic1 = TargetPosition + dir * radius;
        Vector2 Position_ic2 = TargetPosition + (Vector2)(Quaternion.AngleAxis(120.0f, Vector3.forward) * dir * radius);
        Vector2 Position_ic3 = TargetPosition + (Vector2)(Quaternion.AngleAxis(-120.0f, Vector3.forward) * dir * radius);
        //Debug.Log(Position_ic1 + "+" + Position_ic2 + "+" + Position_ic3);
        Position_ic1 = new Vector2(
Mathf.Clamp((float)Position_ic1.x, ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),
Mathf.Clamp((float)Position_ic1.y, ParentPokemonRoom.RoomSize[1] + transform.parent.position.y, ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));
        Position_ic2 = new Vector2(
Mathf.Clamp((float)Position_ic2.x, ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),
Mathf.Clamp((float)Position_ic2.y, ParentPokemonRoom.RoomSize[1] + transform.parent.position.y, ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));
        Position_ic3 = new Vector2(
Mathf.Clamp((float)Position_ic3.x, ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),
Mathf.Clamp((float)Position_ic3.y, ParentPokemonRoom.RoomSize[1] + transform.parent.position.y, ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));
        //Debug.Log(Position_ic1 +"+"+ Position_ic2 + "+" + Position_ic3);
        CetitanIcicleCrash ic1 = Instantiate(icPrefab, Position_ic1, Quaternion.identity);
        ic1.ParentCetitan = this; ICObjList.Add(ic1);
        CetitanIcicleCrash ic2 = Instantiate(icPrefab, Position_ic2, Quaternion.identity);
        ic2.ParentCetitan = this; ICObjList.Add(ic2);
        CetitanIcicleCrash ic3 = Instantiate(icPrefab, Position_ic3, Quaternion.identity);
        ic3.ParentCetitan = this; ICObjList.Add(ic3);

        //ParentPokemonRoom.CameraShake(0.5f, 2.5f, true);

    }


    /// <summary>
    /// 发射圆环冰柱
    /// </summary>
    void Lunch_IcicleCrash_Circle(Vector2 Center, float Radius, int Count)
    {
        float Angle = 360.0f / (float)Count;
        for (int i = 0; i < Count; i++)
        {
            Vector2 p = Center + (Vector2)(Quaternion.AngleAxis(Angle * i, Vector3.forward) * Vector2.right) * Radius;
            p = new Vector2(
                Mathf.Clamp((float)p.x, ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),
                Mathf.Clamp((float)p.y, ParentPokemonRoom.RoomSize[1] + transform.parent.position.y, ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));
            if ((isEmptyConfusionDone && Random.Range(0.0f,1.0f) < 0.85f) || !isEmptyConfusionDone)
            {
                CetitanIcicleCrash ic = Instantiate(icPrefab, p, Quaternion.identity);
                ic.ParentCetitan = this; ICObjList.Add(ic);
            }
        }
    }

    void Lunch_IcicleCrash_Point(Vector2 p)
    {
        p = new Vector2(
            Mathf.Clamp((float)p.x, ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),
            Mathf.Clamp((float)p.y, ParentPokemonRoom.RoomSize[1] + transform.parent.position.y, ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));
        CetitanIcicleCrash ic = Instantiate(icPrefab, p, Quaternion.identity);
        ic.ParentCetitan = this; ICObjList.Add(ic);
    }

    /// <summary>
    /// 发射线形冰柱
    /// </summary>
    /// <param name="StartPos">起始点</param>
    /// <param name="OverPos">终结点</param>
    /// <param name="Interval">时间间隔</param>
    /// <param name="Spacing">距离间隔</param>
    public void Lunch_IcicleCrash_Line(Vector2 StartPos, Vector2 OverPos, float Interval, float Spacing)
    {
        //确保起始点和终点在房间内
        StartPos = ParentPokemonRoom.EnsurePointReachesRoom(StartPos);
        OverPos = ParentPokemonRoom.EnsurePointReachesRoom(OverPos);
        StartCoroutine(Lunch_IcicleCrash_Line_Coroutine(StartPos, OverPos, Interval, Spacing));
    }

    private IEnumerator Lunch_IcicleCrash_Line_Coroutine(Vector2 StartPos, Vector2 OverPos, float Interval, float Spacing)
    {
        Vector2 dir = (OverPos - StartPos).normalized;
        float maxDis = Vector2.Distance(StartPos, OverPos);

        for (int i = 0; Vector2.Distance(StartPos, StartPos + dir * Spacing * i) < maxDis; i++)
        {
            Vector2 p = StartPos + dir * Spacing * i;

            // 房间边界限制
            p = new Vector2(
                Mathf.Clamp(p.x, ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),
                Mathf.Clamp(p.y, ParentPokemonRoom.RoomSize[1] + transform.parent.position.y, ParentPokemonRoom.RoomSize[0] + transform.parent.position.y)
            );


            if ((isEmptyConfusionDone && Random.Range(0.0f, 1.0f) < 0.85f) || !isEmptyConfusionDone)
            {
                CetitanIcicleCrash ic = Instantiate(icPrefab, p, Quaternion.identity);
                ic.ParentCetitan = this;
                if ((int)(i / 3) != 0)
                {
                    ic.icOBJ.BeHit((int)(i / 3));
                }
                ICObjList.Add(ic);
            }

            // 等待一点点时间再生成下一个
            if (Interval > 0f)
                yield return new WaitForSeconds(Interval);
            else
                yield return null; // 即使是 0，也至少等一帧，避免瞬间全部生成
        }
    }



    /// <summary>
    /// 发射冰粒
    /// </summary>
    public void LunchIceShard(Vector2 position)
    {
        int count = COUNT_ICESHARD_ICBREAK;
        float angle = 360.0f / (float)count;
        for (int i = 0; i < count; i++)
        {
            Vector2 LunchDir = Quaternion.AngleAxis(angle * i, Vector3.forward) * Vector2.right;
            CetitanIceShard IS = Instantiate(isPrefab, position + LunchDir * 0.5f, Quaternion.Euler(0, 0, angle * i));
            IS.empty = this;
            IS.LaunchNotForce(LunchDir, IS.MoveSpeed);
            //Debug.Log(LunchDir);
        }
    }



    //=========================一般_冰柱坠击============================








    //=========================一般_反弹冰旋============================


    //冰旋提速需至最大要时间
    static float TIME_SPEEDALPHA_FADEIN_SPINNER = 1.0f;

    //冰旋减速需至0要时间
    static float TIME_SPEEDALPHA_FADEOUT_SPINNER = 1.0f;

    //冰旋生成的位置偏移量
    static Vector2 POSITION_OFFSET_SPINNER = new Vector2(0.0f, 0.3f);



    //冰旋速度最大加成
    static float SPEEDALPHA_MAX_NORMAL_SPINNER = 4.0f;

    //冰旋结束开始减速需要的最小时间
    static float TIME_FADEOUT_START_MIN_NORMAL_SPINNER = 6.0f;

    ///冰旋结束开始减速需要的反弹次数
    static float COUNT_REBOUND_FADEOUT_START_NORMAL_SPINNER = 7;



    /// <summary>
    /// 一般_反弹冰旋计时器
    /// <summary>
    float Normal_SpinnerTimer = 0;

    /// <summary>
    /// 冰旋方向
    /// </summary>
    Vector2 Dir_Normall_Spinner = Vector2.zero;

    /// <summary>
    /// 一般_反弹冰旋速度加成（有提速减速）
    /// <summary>
    float SpeedAlpha_Normal_Spinner = 0;

    /// <summary>
    /// 冰旋是否准备结束，开始减速
    /// </summary>
    bool isOver_Normal_Spinner = false;

    /// <summary>
    /// 碰撞次数
    /// </summary>
    int Count_Rebound_Normal_Spinner = 0;



    /// <summary>
    /// 一般_反弹冰旋开始
    /// <summary>
    public void Normal_SpinnerStart(/*float Timer*/)
    {
        Normal_SpinnerTimer = 0;
        ChangeSubState(SubState.Normal_Spinner);
        animator.SetInteger("Spinner", 1);
        LunchIceSpinner();
        Dir_Normall_Spinner = _mTool.TiltMainVector2(Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector2.right);
        SpeedAlpha_Normal_Spinner = 0;
        isOver_Normal_Spinner = false;
        Count_Rebound_Normal_Spinner = 0;
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
        SetDirector(Vector2.down);

        var clip = audioPlayer.sfxTable.GetClip(CetitanSE.Drift.ToString());
        loopPlayer.PlayLoop(clip, 1f);
    }

    /// <summary>
    /// 一般_反弹冰旋结束
    /// <summary>
    public void Normal_SpinnerOver()
    {
        Normal_SpinnerTimer = 0;
        Dir_Normall_Spinner = Vector2.zero;
        SpeedAlpha_Normal_Spinner = 0;
        isOver_Normal_Spinner = false;
        Count_Rebound_Normal_Spinner = 0;
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }

    }

    /// <summary>
    /// 发射冰旋
    /// </summary>
    void LunchIceSpinner()
    {
        if (SpinnerObj != null) {
            SpinnerObj.SpinnerOver();
            SpinnerObj = null;
        }
        SpinnerObj = Instantiate(SpinnerPrefabs, transform.position + (Vector3)POSITION_OFFSET_SPINNER, Quaternion.identity, transform);
        SpinnerObj.ParentCetitan = this;
    }

    /// <summary>
    /// 冰旋开始减速
    /// </summary>
    void FadeOut_Start_Normal_Spinner()
    {
        isOver_Normal_Spinner = true;
        animator.SetInteger("Spinner", 2);
        if (SpinnerObj != null) { SpinnerObj.SpinnerOver(); }

    }


    //=========================一般_反弹冰旋============================








    //=========================一般_冰牙============================


    //冰牙生成的位置偏移量
    static Vector2 POSITION_OFFSET_ICEFANG = new Vector2(0.0f, 0.75f);






    //冰牙冲刺速度
    static float SPEEDALPHA_NORMAL_ICEFANG = 9.0f;



    /// <summary>
    /// 一般_冰牙计时器
    /// <summary>
    //float Normal_IceFangTimer = 0;

    /// <summary>
    /// 冰牙冲刺方向
    /// </summary>
    Vector2 Dir_Normal_IceFang_Rush = Vector2.zero;

    /// <summary>
    /// 是否冰牙冲刺
    /// </summary>
    bool isRush_Normal_IceFang = false;




    /// <summary>
    /// 一般_冰牙开始
    /// <summary>
    public void Normal_IceFangStart(/*float Timer*/)
    {
        //Normal_IceFangTimer = 0;
        ChangeSubState(SubState.Normal_IceFang);
        Dir_Normal_IceFang_Rush = _mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized);
        SetDirector(Dir_Normal_IceFang_Rush);
        animator.SetTrigger("Atk");
        isRush_Normal_IceFang = false;
        //Lunch_IceFang();
    }

    /// <summary>
    /// 一般_冰牙结束
    /// <summary>
    public void Normal_IceFangOver()
    {
        //Normal_IceFangTimer = 0;
        Dir_Normal_IceFang_Rush = Vector2.zero;
        isRush_Normal_IceFang = false;
    }


    public void Lunch_IceFang()
    {
        if (IceFangObj != null)
        {
            IceFangObj = null;
        }
        IceFangObj = Instantiate(IceFangPrefabs, transform.position + (Vector3)POSITION_OFFSET_ICEFANG + (Vector3)Dir_Normal_IceFang_Rush * 2.0f, Quaternion.identity, transform);
        IceFangObj.ParentCetitan = this;
        audioPlayer.Play(CetitanSE.IceFang, transform.position);
    }


    //=========================一般_冰牙============================






    //=========================一般_跳跃地震============================



    /// <summary>
    /// 跳跃地震的蓄力时间
    /// </summary>
    static float Time_Normal_Jump_Charge = 2.0f;

    /// <summary>
    /// 跳跃地震的移动时间
    /// </summary>
    static float Time_Normal_Jump = 0.5f;







    /// <summary>
    /// 一般_跳跃地震计时器
    /// <summary>
    float Normal_JumpTimer = 0;

    /// <summary>
    /// 跳跃是否移动
    /// </summary>
    bool isMove_Normal_Jump = false;

    /// <summary>
    /// 跳跃是否蓄力
    /// </summary>
    bool isCharge_Normal_Jump = false;

    /// <summary>
    /// 跳跃速度
    /// </summary>
    float Speed_Normal_Jump = 0;

    /// <summary>
    /// 跳跃角度
    /// </summary>
    Vector2 Dir_Normal_Jump = Vector2.zero;




    /// <summary>
    /// 一般_跳跃地震开始
    /// <summary>
    public void Normal_JumpStart(/*float Timer*/)
    {
        Normal_JumpTimer = 0;
        ChangeSubState(SubState.Normal_Jump);
        isMove_Normal_Jump = false;
        isCharge_Normal_Jump = false;
        animator.SetInteger("Jump", 1);
        Speed_Normal_Jump = 0;
        Dir_Normal_Jump = Vector2.zero;
        SetDirector(Vector2.down);
    }

    /// <summary>
    /// 一般_跳跃地震结束
    /// <summary>
    public void Normal_JumpOver()
    {
        Normal_JumpTimer = 0;
        isMove_Normal_Jump = false;
        isCharge_Normal_Jump = false;
        animator.SetInteger("Jump", 0);
        Speed_Normal_Jump = 0;
        Dir_Normal_Jump = Vector2.zero;
    }

    /// <summary>
    /// 发射地震
    /// </summary>
    void Lunch_Earthquake()
    {
        if (EarthquakeObj != null)
        {
            EarthquakeObj = null;
        }
        EarthquakeObj = Instantiate(EarthquakePrefabs, transform.position, Quaternion.identity, transform);
        EarthquakeObj.ParentCetitan = this;
        ParentPokemonRoom.CameraShake(5.0f, 6.0f, true);
        if (NowMainState == MainState.Angry)
        {
            LunchAvalanche(EarthquakeObj.transform.position);
        }
        audioPlayer.Play(CetitanSE.BigFall, transform.position);
    }



    //=========================一般_跳跃地震============================






    //=========================一般_吼叫暴雪============================



    static float TIME_NORMAL_ROAR = 2.5f;



    /// <summary>
    /// 一般_吼叫暴雪计时器
    /// <summary>
    float Normal_RoarTimer = 0;



    /// <summary>
    /// 一般_吼叫暴雪开始
    /// <summary>
    public void Normal_RoarStart(float Timer)
    {
        Normal_RoarTimer = Timer;
        ChangeSubState(SubState.Normal_Roar);
        animator.SetTrigger("Roar");
        LunchBlizzard();
        ParentPokemonRoom.CameraShake(3.0f, 5.0f, true);
        SetDirector(Vector2.down);
        audioPlayer.Play(CetitanSE.Roar, transform.position);
        IsDefStateByNormal = true;
    }

    /// <summary>
    /// 一般_吼叫暴雪结束
    /// <summary>
    public void Normal_RoarOver()
    {
        Normal_RoarTimer = 0;
        IsDefStateByNormal = false;
    }

    public void LunchBlizzard()
    {
        if (BlizzardObj != null)
        {
            Destroy(BlizzardObj.gameObject);
        }
        BlizzardObj = Instantiate(BlizzardPrefabs, ParentPokemonRoom.transform.position, Quaternion.identity);
        BlizzardObj.ParentCetitan = this;
        BlizzardObj.SetBlizzardLevel(0);
    }



    //=========================一般_吼叫暴雪============================



    //==■==■==■==■==■==■==■主状态：一般状态■==■==■==■==■==■==■==
































    //==■==■==■==■==■==■==■主状态：愤怒状态■==■==■==■==■==■==■==



    //=========================愤怒_发呆============================


    //开始后的冷却时间
    static float TIME_ANGRY_IDLE_START = 0.5f;








    /// <summary>
    /// 愤怒_发呆计时器
    /// <summary>
    float Angry_IdleTimer = 0;

    /// <summary>
    /// 愤怒_发呆开始
    /// <summary>
    public void Angry_IdleStart(float Timer)
    {
        Angry_IdleTimer = Timer;
        ChangeSubState(SubState.Angry_Idle);
        IsDefStateByNormal = false;
    }

    /// <summary>
    /// 愤怒_发呆结束
    /// <summary>
    public void Angry_IdleOver()
    {
        Angry_IdleTimer = 0;
    }


    //=========================愤怒_发呆============================






    //=========================愤怒_奔跑追踪============================




    /// <summary>
    /// 愤怒_奔跑追踪计时器
    /// <summary>
    float Angry_RunTimer = 0;

    /// <summary>
    /// 愤怒_奔跑追踪开始
    /// <summary>
    public void Angry_RunStart(/*float Timer*/)
    {
        Angry_RunTimer = 0;
        ChangeSubState(SubState.Angry_Run);
    }

    /// <summary>
    /// 愤怒_奔跑追踪结束
    /// <summary>
    public void Angry_RunOver()
    {
        Angry_RunTimer = 0;
    }


    //=========================愤怒_奔跑追踪============================






    //=========================愤怒_冰柱坠击============================


    //愤怒_第一次_三点冰柱距离目标的半径
    static float RADIUS_ANGRY_ICICLECRASH_COMBAT110_1 = 5.5f;

    //愤怒_第二次_三点冰柱距离目标的半径
    static float RADIUS_ANGRY_ICICLECRASH_COMBAT110_2 = 3.8f;




    //预判玩家用玩家旧坐标
    Vector2 LastTargetPosition_Angry_IcicleCrash = Vector2.zero;




    /// <summary>
    /// 愤怒_冰柱坠击开始
    /// <summary>
    public void Angry_IcicleCrashStart(/*float Timer*/)
    {
        ChangeSubState(SubState.Angry_IcicleCrash);
        animator.SetTrigger("Atk");
        SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
        LastTargetPosition_Angry_IcicleCrash = Vector2.zero;
    }

    /// <summary>
    /// 愤怒_冰柱坠击结束
    /// <summary>
    public void Angry_IcicleCrashOver()
    {
        LastTargetPosition_Angry_IcicleCrash = Vector2.zero;
    }


    //=========================愤怒_冰柱坠击============================






    //=========================愤怒_反弹冰旋============================




    //冰旋速度最大加成
    static float SPEEDALPHA_MAX_ANGRY_SPINNER = 5.2f;

    //冰旋结束开始减速需要的最小时间
    static float TIME_FADEOUT_START_MIN_ANGRY_SPINNER = 4.5f;

    ///冰旋结束开始减速需要的反弹次数
    static float COUNT_REBOUND_FADEOUT_START_ANGRY_SPINNER = 5;



    /// <summary>
    /// 愤怒_反弹冰旋计时器
    /// <summary>
    float Angry_SpinnerTimer = 0;

    /// <summary>
    /// 冰旋方向
    /// </summary>
    Vector2 Dir_Angry_Spinner = Vector2.zero;

    /// <summary>
    /// 愤怒_反弹冰旋速度加成（有提速减速）
    /// <summary>
    float SpeedAlpha_Angry_Spinner = 0;

    /// <summary>
    /// 愤怒_冰旋是否准备结束，开始减速
    /// </summary>
    bool isOver_Angry_Spinner = false;

    /// <summary>
    /// 愤怒_碰撞次数
    /// </summary>
    int Count_Rebound_Angry_Spinner = 0;



    /// <summary>
    /// 愤怒_反弹冰旋开始
    /// <summary>
    public void Angry_SpinnerStart(/*float Timer*/)
    {
        Angry_SpinnerTimer = 0;
        ChangeSubState(SubState.Angry_Spinner);
        animator.SetInteger("Spinner", 1);
        LunchIceSpinner();
        Dir_Angry_Spinner = _mTool.TiltMainVector2(Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector2.right);
        SpeedAlpha_Angry_Spinner = 0;
        isOver_Angry_Spinner = false;
        Count_Rebound_Angry_Spinner = 0;
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
        SetDirector(Vector2.down);

        var clip = audioPlayer.sfxTable.GetClip(CetitanSE.Drift.ToString());
        loopPlayer.PlayLoop(clip, 1f);
    }

    /// <summary>
    /// 愤怒_反弹冰旋结束
    /// <summary>
    public void Angry_SpinnerOver()
    {
        Angry_SpinnerTimer = 0;
        Dir_Angry_Spinner = Vector2.zero;
        SpeedAlpha_Angry_Spinner = 0;
        isOver_Angry_Spinner = false;
        Count_Rebound_Angry_Spinner = 0;
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }

    /// <summary>
    /// 愤怒_冰旋开始减速
    /// </summary>
    void FadeOut_Start_Angry_Spinner()
    {
        isOver_Angry_Spinner = true;
        animator.SetInteger("Spinner", 2);
        if (SpinnerObj != null) { SpinnerObj.SpinnerOver(); }

    }



    //=========================愤怒_反弹冰旋============================






    //=========================愤怒_冰牙============================



    //冰牙冲刺速度
    static float SPEEDALPHA_ANGRY_ICEFANG = 9.0f;



    /// <summary>
    /// 愤怒_冰牙冲刺方向
    /// </summary>
    Vector2 Dir_Angry_IceFang_Rush = Vector2.zero;

    /// <summary>
    /// 愤怒_是否冰牙冲刺
    /// </summary>
    bool isRush_Angry_IceFang = false;



    /// <summary>
    /// 愤怒_冰牙开始
    /// <summary>
    public void Angry_IceFangStart(/*float Timer*/)
    {
        //Angry_IceFangTimer = 0;
        ChangeSubState(SubState.Angry_IceFang);
        Dir_Angry_IceFang_Rush = _mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized);
        SetDirector(_mTool.MainVector2(Dir_Angry_IceFang_Rush));
        animator.SetTrigger("Atk");
        isRush_Angry_IceFang = false;
    }

    /// <summary>
    /// 愤怒_冰牙结束
    /// <summary>
    public void Angry_IceFangOver()
    {
        //Angry_IceFangTimer = 0;
        Dir_Angry_IceFang_Rush = Vector2.zero;
        isRush_Angry_IceFang = false;
    }


    //=========================愤怒_冰牙============================






    //=========================愤怒_跳跃地震============================



    /// <summary>
    /// 愤怒_跳跃地震的蓄力时间
    /// </summary>
    static float Time_Angry_Jump_Charge = 1.5f;

    /// <summary>
    /// 愤怒_跳跃地震的移动时间
    /// </summary>
    static float Time_Angry_Jump = 0.5f;







    /// <summary>
    /// 愤怒_跳跃地震计时器
    /// <summary>
    float Angry_JumpTimer = 0;

    /// <summary>
    /// 愤怒_跳跃是否移动
    /// </summary>
    bool isMove_Angry_Jump = false;

    /// <summary>
    /// 愤怒_跳跃是否蓄力
    /// </summary>
    bool isCharge_Angry_Jump = false;

    /// <summary>
    /// 愤怒_跳跃速度
    /// </summary>
    float Speed_Angry_Jump = 0;

    /// <summary>
    /// 愤怒_跳跃角度
    /// </summary>
    Vector2 Dir_Angry_Jump = Vector2.zero;




    /// <summary>
    /// 愤怒_跳跃地震开始
    /// <summary>
    public void Angry_JumpStart(/*float Timer*/)
    {
        Angry_JumpTimer = 0;
        ChangeSubState(SubState.Angry_Jump);
        isMove_Angry_Jump = false;
        isCharge_Angry_Jump = false;
        animator.SetInteger("Jump", 1);
        Speed_Angry_Jump = 0;
        Dir_Angry_Jump = Vector2.zero;
        SetDirector(Vector2.down);
    }

    /// <summary>
    /// 愤怒_跳跃地震结束
    /// <summary>
    public void Angry_JumpOver()
    {
        Angry_JumpTimer = 0;
        isMove_Angry_Jump = false;
        isCharge_Angry_Jump = false;
        animator.SetInteger("Jump", 0);
        Speed_Angry_Jump = 0;
        Dir_Angry_Jump = Vector2.zero;
    }

    /// <summary>
    /// 发射雪崩
    /// </summary>
    void LunchAvalanche(Vector2 p)
    {
        if (AvalancheMOBJ != null)
        {
            Destroy(AvalancheMOBJ.gameObject);
        }
        AvalancheMOBJ = Instantiate(AvalancheMPrefabs, p, Quaternion.identity);
        AvalancheMOBJ.SetAvalanche(this);
    }

    //=========================愤怒_跳跃地震============================






    //=========================愤怒_漂移滑行============================


    /// <summary>
    /// 愤怒_漂移滑行的速度加成
    /// </summary>
    static float SPEEDALPHA_ANGRY_DRIFT_RUSH = 7.0f;

    /// <summary>
    /// 愤怒_漂移滑行的速度加成
    /// </summary>
    static float SPEEDALPHA_ANGRY_DRIFT_DRIFT = 4.5f;

    /// <summary>
    /// 漂移滑行的冲刺时间
    /// </summary>
    static float TIME_ANGRY_DRIFT_RUSH = 0.3f;

    /// <summary>
    /// 漂移滑行的漂移时间
    /// </summary>
    static float TIME_ANGRY_DRIFT_DRIFT = 2.0f;

    /// <summary>
    /// 漂移滑行的冰柱坠击间隔时间
    /// </summary>
    static float TIME_INTERVEL_ANGRY_DRIFT_ICICLECRASH = 0.1f;

    /// <summary>
    /// 漂移滑行的冰柱坠击发射的半径
    /// </summary>
    static float RADIUS_ANGRY_DRIFT_ICICLECRASH = 1.5f;

    // 墙壁检测参数
    static float wallCheckDistance = 3f;
    static float wallSafeDistance = 1.5f;
    static float wallTurnBoost = 200f;




    /// <summary>
    /// 愤怒_漂移滑行计时器
    /// <summary>
    float Angry_DriftTimer = 0;

    /// <summary>
    /// 愤怒_漂移冰柱计时器
    /// <summary>
    float Angry_Drift_IcicleCrash_Timer = 0;


    /// <summary>
    /// 漂移滑行的角度
    /// </summary>
    Vector2 Dir_Angle_Drift = Vector2.zero;

    /// <summary>
    /// 是否处于漂移的冲刺阶段
    /// </summary>
    bool isRush_ANGRY_DRIFT = false;

    /// <summary>
    /// 是否处于漂移的漂移阶段
    /// </summary>
    bool isDrift_ANGRY_DRIFT = false;


    /// <summary>
    /// 愤怒_漂移滑行开始
    /// <summary>
    public void Angry_DriftStart(/*float Timer*/)
    {
        Angry_DriftTimer = 0;
        ChangeSubState(SubState.Angry_Drift);
        Dir_Angle_Drift = (TargetPosition - (Vector2)transform.position).normalized;
        animator.SetInteger("Drift" , 1);
        isRush_ANGRY_DRIFT = true;
        isDrift_ANGRY_DRIFT = false;
        Angry_Drift_IcicleCrash_Timer = 0;

        var clip = audioPlayer.sfxTable.GetClip(CetitanSE.Drift.ToString());
        loopPlayer.PlayLoop(clip, 1f);
    }

    /// <summary>
    /// 愤怒_漂移滑行结束
    /// <summary>
    public void Angry_DriftOver()
    {
        Angry_DriftTimer = 0;
        Dir_Angle_Drift = Vector2.zero;
        animator.SetInteger("Drift", 0);
        isRush_ANGRY_DRIFT = false;
        isDrift_ANGRY_DRIFT = false;
        Angry_Drift_IcicleCrash_Timer = 0;
        DriftDust.StopPSL();
        DriftDust.StopPSR();
    }


    //=========================愤怒_漂移滑行============================






    //=========================愤怒_冰光============================


    /// <summary>
    /// 发射冰光并调整角度的时间
    /// </summary>
    public static float TIME_LAUNCH_ICEBEAM_LAUNCH = 2.0f;

    /// <summary>
    /// 发射冰光后被反作用力击退的时间
    /// </summary>
    static float TIME_REACTION_ICEBEAM_LAUNCH = 0.7f;

    /// <summary>
    /// 发射冰光期间缓慢移动的速度
    /// </summary>
    static float SPEED_LAUNCH_ICEBEAM = 0.8f;

    /// <summary>
    /// 发射冰光后被击退的移动速度
    /// </summary>
    static float SPEED_REACTION_ICEBEAM = 6.0f;

    /// <summary>
    /// 冰光开始时的角度
    /// </summary>
    public static float ANGLE_ICEBEAM_START = 50;

    /// <summary>
    /// 冰光结束时的角度
    /// </summary>
    public static float ANGLE_ICEBEAM_OVER = 6;





    /// <summary>
    /// 愤怒_冰光计时器
    /// <summary>
    float Angry_IceBeamTimer = 0;

    /// <summary>
    /// 是否处于冰光发射期间
    /// </summary>
    public bool IsMove_Launch_Angry_IceBeam
    {
        get { return isMove_Launch_Angry_IceBeam; }
        set { isMove_Launch_Angry_IceBeam = value; }
    }
    bool isMove_Launch_Angry_IceBeam;

    /// <summary>
    /// 是否处于冰光发射后后作用力移动期间
    /// </summary>
    bool isMove_Reaction_Angry_IceBeam;

    /// <summary>
    /// 冰光发射目标角度
    /// </summary>
    Vector2 Dir_TargetAngle_IceBeam;


    /// <summary>
    /// 愤怒_冰光开始
    /// <summary>
    public void Angry_IceBeamStart(/*float Timer*/)
    {
        Angry_IceBeamTimer = 0;
        ChangeSubState(SubState.Angry_IceBeam);
        animator.SetTrigger("Atk");
        isMove_Launch_Angry_IceBeam = false;
        isMove_Reaction_Angry_IceBeam = false;
        Dir_TargetAngle_IceBeam = (TargetPosition - (Vector2)transform.position).normalized;
        Dir_TargetAngle_IceBeam = Quaternion.AngleAxis( ((Random.Range(0.0f , 1.0f) > 0.5f)?-1:1) * Random.Range(10.0f , 35.0f) , Vector3.forward) * Dir_TargetAngle_IceBeam;
        SetDirector(_mTool.MainVector2(Dir_TargetAngle_IceBeam));
    }

    /// <summary>
    /// 愤怒_冰光结束
    /// <summary>
    public void Angry_IceBeamOver()
    {
        Angry_IceBeamTimer = 0;
        animator.SetInteger("IceBeam", 0);
        isMove_Launch_Angry_IceBeam = false;
        isMove_Reaction_Angry_IceBeam = false;
        Dir_TargetAngle_IceBeam = Vector2.zero;
    }

    /// <summary>
    /// 发射冰光
    /// </summary>
    void LunchIceBeam()
    {
        Dir_TargetAngle_IceBeam = (TargetPosition - (Vector2)transform.position).normalized;
        Dir_TargetAngle_IceBeam = Quaternion.AngleAxis(((Random.Range(0.0f, 1.0f) > 0.5f) ? -1 : 1) * Random.Range(10.0f, 35.0f), Vector3.forward) * Dir_TargetAngle_IceBeam;
        SetDirector(_mTool.MainVector2(Dir_TargetAngle_IceBeam));
        if (IceBeamOBJ_1 != null) { IceBeamOBJ_1.StopBeam(); }
        Vector2 p1 = Quaternion.AngleAxis(ANGLE_ICEBEAM_START , Vector3.forward) * Dir_TargetAngle_IceBeam;
        IceBeamOBJ_1 = Instantiate(IceBeamPrefabs, transform.position + (Vector3)p1 * 2.0f, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Dir_TargetAngle_IceBeam, Vector2.right) + ANGLE_ICEBEAM_START), transform);
        IceBeamOBJ_1.ParentCetitan = this;
        IceBeamOBJ_1.isTurnClockwise = false;
        if (IceBeamOBJ_2 != null) { IceBeamOBJ_2.StopBeam(); }
        Vector2 p2 = Quaternion.AngleAxis(-ANGLE_ICEBEAM_START, Vector3.forward) * Dir_TargetAngle_IceBeam;
        IceBeamOBJ_2 = Instantiate(IceBeamPrefabs, transform.position + (Vector3)p2 * 2.0f, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Dir_TargetAngle_IceBeam, Vector2.right) - ANGLE_ICEBEAM_START), transform);
        IceBeamOBJ_2.ParentCetitan = this;
        IceBeamOBJ_2.isTurnClockwise = true;
    }

    void OverIceBeam()
    {
        if (IceBeamOBJ_1 != null) { IceBeamOBJ_1.StopBeam(); }
        if (IceBeamOBJ_2 != null) { IceBeamOBJ_2.StopBeam(); }
    }


    //=========================愤怒_冰光============================






    //=========================愤怒_十万马力冲刺============================



    /// <summary>
    /// 愤怒_十万马力冲刺的准备时间
    /// </summary>
    static float TIME_ANGRY_RUSH_PREPARE = 3.1f;

    /// <summary>
    /// 愤怒_十万马力冲刺的速度加成
    /// </summary>
    static float SPEEDALPHA_ANGRY_RUSH = 3.0f;

    /// <summary>
    /// 冲刺结束所需要的目标点时和浩大鲸的距离的最小值
    /// </summary>
    static float DISTENCE_ANGRY_RUSH_OVER_CLOSEMIN = 3.0f;




    /// <summary>
    /// 愤怒_十万马力冲刺计时器
    /// <summary>
    float Angry_RushTimer = 0;

    /// <summary>
    /// 是否处于愤怒_十万马力冲刺阶段
    /// </summary>
    bool isMove_Angry_Rush = false;

    /// <summary>
    /// 处于愤怒_十万马力冲刺阶段时，是否在跳跃的可移动期间
    /// </summary>
    public bool isMove_Angry_Rush_Jump = false;

    /// <summary>
    /// 十万马力冲刺的方向
    /// </summary>
    Vector2 Dir_TargetAngle_Rush = Vector2.zero;




    /// <summary>
    /// 愤怒_十万马力冲刺开始
    /// <summary>
    public void Angry_RushStart(/*float Timer*/)
    {
        Angry_RushTimer = 0;
        ChangeSubState(SubState.Angry_Rush);
        isMove_Angry_Rush = false;
        isMove_Angry_Rush_Jump = false;
        Dir_TargetAngle_Rush = (Position_Target_Combat142_Rush - (Vector2)transform.position).normalized;
    }

    /// <summary>
    /// 愤怒_十万马力冲刺结束
    /// <summary>
    public void Angry_RushOver()
    {
        Angry_RushTimer = 0;
        isMove_Angry_Rush = false;
        isMove_Angry_Rush_Jump = false;
        animator.SetInteger("Rush", 0);
        Dir_TargetAngle_Rush = Vector2.zero;
        Position_Target_Combat142_Rush = Vector2.zero;
    }

    /// <summary>
    /// 发射重踏
    /// </summary>
    public void Lunch_Bulldoze()
    {
        InstantiateRunDust();
        CetitanBulldoze b = Instantiate(BulldozePrefabs , transform.position , Quaternion.identity);
        b.ParentCetitan = this;
        ParentPokemonRoom.CameraShake(0.3f , 2.5f , false);
        audioPlayer.Play(CetitanSE.Fall, transform.position);
    }

    /// <summary>
    /// 发射十万马力
    /// </summary>
    public void Lunch_HighHorsepower()
    {
        InstantiateRunDust();
        CetitanHighHoursePower hhp = Instantiate(HighHoursePowerPrefabs, transform.position, Quaternion.identity);
        hhp.ParentCetitan = this;
        ParentPokemonRoom.CameraShake(1.0f, 4.5f, false);
        audioPlayer.Play(CetitanSE.Fall, transform.position);
    }

    //=========================愤怒_十万马力冲刺============================






    //=========================愤怒_吼叫暴雪============================


    static float TIME_ANGRY_ROAR = 2.5f;



    /// <summary>
    /// 愤怒_吼叫暴雪计时器
    /// <summary>
    float Angry_RoarTimer = 0;

    /// <summary>
    /// 愤怒_吼叫暴雪开始
    /// <summary>
    public void Angry_RoarStart(float Timer)
    {
        Angry_RoarTimer = Timer;
        ChangeSubState(SubState.Angry_Roar);
        animator.SetTrigger("Roar");
        if (BlizzardObj == null ) { LunchBlizzard(); }
        ParentPokemonRoom.CameraShake(3.0f, 5.0f, true);
        SetDirector(Vector2.down);

        switch (NowAngryLevel)
        {
            //愤怒一级时转进愤怒二
            case AngryLevel.LEVEL1:
                NowAngryLevel = AngryLevel.LEVEL2;
                BlizzardObj.SetBlizzardLevel(1);
                break;
            //愤怒二级时转进愤怒三
            case AngryLevel.LEVEL2:
                NowAngryLevel = AngryLevel.LEVEL3;
                BlizzardObj.SetBlizzardLevel(2);
                break;
        }
        audioPlayer.Play(CetitanSE.Roar, transform.position);
        IsDefStateByNormal = true;
    }

    /// <summary>
    /// 愤怒_吼叫暴雪结束
    /// <summary>
    public void Angry_RoarOver()
    {
        Angry_RoarTimer = 0;
        IsDefStateByNormal = false;
    }


    //=========================愤怒_吼叫暴雪============================








    //=========================愤怒_围绕暴风雪中心旋转============================


    /// <summary>
    /// 围绕暴风雪中心旋转时的半径
    /// </summary>
    static float RADIUS_ANGRY_BLIZZARDCENTERTURN = 6.0f;

    /// <summary>
    /// 围绕暴风雪中心旋转时的速度加成
    /// </summary>
    static float SPEEDALPHA_ANGRY_BLIZZARDCENTERTURN = 4.0f;

    /// <summary>
    /// 围绕暴风雪中心旋转时的总时间
    /// </summary>
    static float TIME_ANGRY_BLIZZARDCENTERTURN = 14.0f;

    /// <summary>
    /// 围绕暴风雪中心旋转时的发射冰柱的时间间隔
    /// </summary>
    static float TIME_INTERVAL_ANGRY_BLIZZARDCENTERTURN_ICICLECRASH = 0.55f;

    /// <summary>
    /// 围绕暴风雪中心旋转时距离半径的位置偏移上限 ，超过则重回轨道
    /// </summary>
    static float DISTENCE_OFFSET_ANGRY_BLIZZARDCENTERTURN = 0.2f;

    /// <summary>
    /// 发射冰柱的次数
    /// </summary>
    static int COUNT_ANGRY_BLIZZARDCENTERTURN_ICICLECOUNT = 12;




    /// <summary>
    /// 围绕暴风雪中心旋转时的运动角度
    /// </summary>
    Vector2 Dir_Angry_BlizzardCenterTurn = Vector2.zero;

    /// <summary>
    /// 围绕暴风雪中心旋转时的中心点
    /// </summary>
    Vector2 Position_Center_Angry_BlizzardCenterTurn = Vector2.zero;

    /// <summary>
    /// 愤怒_围绕暴风雪中心旋转计时器
    /// <summary>
    float Angry_BlizzardCenterTurnTimer = 0;

    /// <summary>
    /// 愤怒_围绕暴风雪中心旋转冰柱计时器
    /// <summary>
    float Angry_BlizzardCenterTurn_IcicleCrash_Timer = 0;

    /// <summary>
    /// 围绕暴风雪中心旋转时是否移动
    /// </summary>
    bool isMove_BlizzardCenterTurn = false;




    /// <summary>
    /// 愤怒_围绕暴风雪中心旋转开始
    /// <summary>
    public void Angry_BlizzardCenterTurnStart(float Timer)
    {
        Angry_BlizzardCenterTurnTimer = Timer;
        Angry_BlizzardCenterTurn_IcicleCrash_Timer = 0;
        ChangeSubState(SubState.Angry_BlizzardCenterTurn);
        Dir_Angry_BlizzardCenterTurn = Vector2.zero;
        Position_Center_Angry_BlizzardCenterTurn = ParentPokemonRoom.transform.position;
        isMove_BlizzardCenterTurn = true;
        animator.SetInteger("Drift", 1);
        DriftDust.StartPSL();

        var clip = audioPlayer.sfxTable.GetClip(CetitanSE.Drift.ToString());
        loopPlayer.PlayLoop(clip, 1f);
    }

    /// <summary>
    /// 愤怒_围绕暴风雪中心旋转结束
    /// <summary>
    public void Angry_BlizzardCenterTurnOver()
    {
        Angry_BlizzardCenterTurnTimer = 0;
        Angry_BlizzardCenterTurn_IcicleCrash_Timer = 0;
        Dir_Angry_BlizzardCenterTurn = Vector2.zero;
        Position_Center_Angry_BlizzardCenterTurn = Vector2.zero;
        isMove_BlizzardCenterTurn = false;
    }


    //=========================愤怒_围绕暴风雪中心旋转============================


















    //==■==■==■==■==■==■==■主状态：愤怒状态■==■==■==■==■==■==■==








    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■






}
