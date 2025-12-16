using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Eiscue : Empty
{
    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;

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
    /// 未冰冻状态的防御和特防种族值
    /// </summary>
    public int DefEmptyPointNotFrozen;
    public int SpDEmptyPointNotFrozen;


    //冰企鹅的冰砾
    public EiscueIceShard eiscueIceShard;

    //冰企鹅的冰雾
    public EiscueFrozenMist eiscueFrozenMist;





    //==============================状态机枚举===================================

    /// <summary>
    /// 主状态
    /// </summary>
    enum MainState
    {
        Frozen,    //冰冻
        NotFrozen  //不冰冻
    }
    MainState frozenState;


    /// <summary>
    /// 副状态
    /// </summary>
    enum SubState
    {
        Idle_Frozen,    //冰冻发呆
        Walk_Frozen,    //冰冻走路
        Shake_Frozen,    //冰冻摇晃
        Idle_NotFrozen, //不冰冻发呆
    }
    SubState NowState;



    /// <summary>
    /// 状态映射关系
    /// </summary>
    private static Dictionary<MainState, SubState[]> StateMap = new()
    {
        { MainState.Frozen, new[] { SubState.Idle_Frozen, SubState.Walk_Frozen, SubState.Shake_Frozen } },
        { MainState.NotFrozen, new[] { SubState.Idle_NotFrozen } }
    };

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


            //■■开始判断状态机

            switch (frozenState)
            {
                //●主状态：【冰冻】状态
                case MainState.Frozen:
                    // 当处于冰冻 睡眠 致盲 麻痹状态时主状态【冰冻】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) /* TODO【冰冻】状态停运的额外条件 */
                    {
                        animator.ResetTrigger("Sleep");
                        //判断副状态
                        switch (NowState)
                        {
                            //【冰冻发呆】状态
                            case SubState.Idle_Frozen:
                                Idle_FrozenTimer -= Time.deltaTime;//【冰冻发呆】计时器时间减少
                                if (Idle_FrozenTimer <= 0)         //计时器时间到时间，结束【冰冻发呆】状态
                                {
                                    Idle_FrozenOver();
                                    Walk_FrozenStart(TIME_WALK_FROZEN);
                                }
                                break;
                            //【冰冻走路】状态
                            case SubState.Walk_Frozen:
                                Walk_FrozenTimer -= Time.deltaTime;//【冰冻走路】计时器时间减少
                                if ( isWalkMoving )
                                {
                                    //移动
                                    rigidbody2D.position = new Vector2(
                                        Mathf.Clamp(rigidbody2D.position.x
                                            + (float)MoveVector.x * Time.deltaTime * speed ,       //方向*速度
                                        ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //最小值
                                        ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//最大值
                                        Mathf.Clamp(rigidbody2D.position.y
                                            + (float)MoveVector.y * Time.deltaTime * speed ,        //方向*速度 
                                        ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //最小值
                                        ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//最大值
                                }
                                if (Walk_FrozenTimer <= 0)         //计时器时间到时间，结束【冰冻走路】状态
                                {
                                    Walk_FrozenOver();
                                    Shake_FrozenStart();
                                }
                                break;
                            //【冰冻摇晃】状态
                            case SubState.Shake_Frozen:
                                //Shake_FrozenTimer -= Time.deltaTime;//【冰冻摇晃】计时器时间减少
                                //if (Shake_FrozenTimer <= 0)         //计时器时间到时间，结束【冰冻摇晃】状态
                                //{
                                //    Shake_FrozenOver();
                                    //TODO添加下一个状态的开始方法
                                //}
                                break;
                        }
                    }

                    if ( ( isSleepDone || isCanNotMoveWhenParalysis || isSilence || isFearDone ) && NowState != SubState.Idle_Frozen )
                    {
                        animator.SetTrigger("Sleep");
                        switch (NowState)
                        {
                            case SubState.Walk_Frozen:
                                Walk_FrozenOver();
                                break;
                            case SubState.Shake_Frozen:
                                Shake_FrozenOver();
                                break;
                        }
                        Idle_FrozenStart(TIME_IDLE_FROZEN_SLEEPAWAKE);
                    }
                    break;
                //●主状态：【不冰冻】状态
                case MainState.NotFrozen:
                    // 当处于冰冻 睡眠 致盲 麻痹状态时主状态【不冰冻】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) /* TODO【不冰冻】状态停运的额外条件 */
                    {
                        //判断副状态
                        switch (NowState)
                        {
                            //【不冰冻发呆】状态
                            case SubState.Idle_NotFrozen:
                                Idle_NotFrozenTimer -= Time.deltaTime;//【不冰冻发呆】计时器时间减少
                                if (Idle_NotFrozenTimer <= 0)         //计时器时间到时间，结束【不冰冻发呆】状态
                                {
                                    Idle_NotFrozenOver();
                                    //TODO添加下一个状态的开始方法
                                    FrozenHead();
                                }
                                break;
                        }
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

            /**
            //根据魅惑情况确实目标位置
            if (!isEmptyInfatuationDone || _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile()).Count <= 1 || InfatuationForDistanceEmpty() == null)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }
            **/
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
    /// 设置冰企鹅的动画机方向
    /// </summary>
    void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }


    //InsertSubStateChange


    /// <summary>
    /// 切换副状态
    /// </summary>
    void ChangeSubState(SubState targetSubstate)
    {
        NowState = targetSubstate;
        var mainState = GetMainBySub(targetSubstate);
        frozenState = mainState;
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














    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //==■==■==■==■==■==■==■主状态：冰冻状态■==■==■==■==■==■==■==



    //=========================冰冻发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_FROZEN_START = 0.1f; //TODO需修改时间

    //睡眠状态解除后的冷却时间
    static float TIME_IDLE_FROZEN_SLEEPAWAKE = 0.4f; //TODO需修改时间

    //重新冰冻后的冷却时间
    static float TIME_IDLE_REFROZEN = 1.2f; //TODO需修改时间

    //冰冻走路后的冷却时间
    static float TIME_IDLE_FROZEN_WALK_FROZEN = 0.0f; //TODO需修改时间


    //冰冻摇晃后的冷却时间
    static float TIME_IDLE_FROZEN_SHAKE_FROZEN = 1.4f; //TODO需修改时间


    void FrozenHead()
    {
        animator.SetBool("Frozen" , true);
        Idle_FrozenStart(TIME_IDLE_REFROZEN);
        GetShield((int)(maxHP/4.0f));
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint)/* * 1.2f */;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint)/* * 1.2f */;//设定特防
    }


    /// <summary>
    /// 冰冻发呆计时器
    /// <summary>
    float Idle_FrozenTimer = 0;

    /// <summary>
    /// 冰冻发呆开始
    /// <summary>
    public void Idle_FrozenStart(float Timer)
    {
        Idle_FrozenTimer = Timer;
        ChangeSubState(SubState.Idle_Frozen);
    }

    /// <summary>
    /// 冰冻发呆结束
    /// <summary>
    public void Idle_FrozenOver()
    {
        Idle_FrozenTimer = 0;
    }


    //=========================冰冻发呆============================






    //=========================冰冻走路============================



    //走路时长
    static float TIME_WALK_FROZEN = 1.0f; //TODO需修改时间

    /// <summary>
    /// 冰冻走路计时器
    /// <summary>
    float Walk_FrozenTimer = 0;

    /// <summary>
    /// 冰冻走路开始
    /// <summary>
    public void Walk_FrozenStart(float Timer)
    {
        Walk_FrozenTimer = Timer;
        animator.SetBool("Walk" , true);
        ChangeSubState(SubState.Walk_Frozen);
        NormalLunchCount = false;
    }

    /// <summary>
    /// 冰冻走路结束
    /// <summary>
    public void Walk_FrozenOver()
    {
        animator.SetBool("Walk", false);
        NormalLunchCount = false;
        Walk_FrozenTimer = 0;
    }


    /// <summary>
    /// 是否处于正在移动状态
    /// </summary>
    bool isWalkMoving;

    /// <summary>
    /// 移动方向
    /// </summary>
    Vector2 MoveVector;

    /// <summary>
    /// 移动开始
    /// </summary>
    public void WalkMoverStart()
    {
        isWalkMoving = true;
        MoveVector = Quaternion.AngleAxis( Random.Range(0 , 360) , Vector3.forward ) * Vector2.right;
    }


    /// <summary>
    /// 移动结束
    /// </summary>
    public void WalkMoverOver()
    {
        isWalkMoving = false;
        LunchNormalIceShard();
    }


    bool NormalLunchCount = false;

    /// <summary>
    /// 每走一步发射冰砾
    /// </summary>
    void LunchNormalIceShard()
    {
        if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) {
            if (isEmptyConfusionDone)
            {
                float startRotation = (NormalLunchCount) ? 30.0f : 90.0f;
                NormalLunchCount = !NormalLunchCount;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 LunchRotation = (Quaternion.AngleAxis(startRotation + i * 120.0f, Vector3.forward) * Vector2.right).normalized;
                    EiscueIceShard iS = Instantiate(eiscueIceShard, transform.position + (Vector3)LunchRotation, Quaternion.Euler(0, 0, startRotation + i * 120.0f));
                    iS.empty = this;
                    iS.LaunchNotForce(LunchRotation, 2.2f);
                    //iS.isSplit = false;
                    //iS.isMist = false;
                    iS.SetMaxDistence(10.6f);
                }
            }
            else
            {
                float startRotation = (NormalLunchCount) ? 45.0f : 90.0f;
                NormalLunchCount = !NormalLunchCount;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 LunchRotation = (Quaternion.AngleAxis(startRotation + i * 90.0f, Vector3.forward) * Vector2.right).normalized;
                    EiscueIceShard iS = Instantiate(eiscueIceShard, transform.position + (Vector3)LunchRotation, Quaternion.Euler(0, 0, startRotation + i * 90.0f));
                    iS.empty = this;
                    iS.LaunchNotForce(LunchRotation, 2.2f);
                    //iS.isSplit = false;
                    //iS.isMist = false;
                    iS.SetMaxDistence(10.6f);
                }
            }
        }
    }

    //=========================冰冻走路============================






    //=========================冰冻摇晃============================




    /// <summary>
    /// 冰冻摇晃计时器
    /// <summary>
    //float Shake_FrozenTimer = 0;

    /// <summary>
    /// 冰冻摇晃开始
    /// <summary>
    public void Shake_FrozenStart()
    {
        //Shake_FrozenTimer = Timer;
        ChangeSubState(SubState.Shake_Frozen);
    }

    /// <summary>
    /// 冰冻摇晃结束
    /// <summary>
    public void Shake_FrozenOver()
    {
        Idle_FrozenStart(TIME_IDLE_FROZEN_SHAKE_FROZEN);
        //Shake_FrozenTimer = 0;
    }


    //=========================冰冻摇晃============================



    //==■==■==■==■==■==■==■主状态：冰冻状态■==■==■==■==■==■==■==










    //==■==■==■==■==■==■==■主状态：不冰冻状态■==■==■==■==■==■==■==



    //=========================不冰冻发呆============================

    /// <summary>
    /// 破盾时转化为解冻头
    /// </summary>
    public override void ShieldBreakEvent()
    {
        base.ShieldBreakEvent();
        animator.SetBool("Frozen" , false);
        Idle_NotFrozenStart(TIME_IDLE_NOTFROZEN_START);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPointNotFrozen)/* * 1.2f */;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpDEmptyPointNotFrozen)/* * 1.2f */;//设定特防
        Instantiate(eiscueFrozenMist , transform.position , Quaternion.identity);
        //LunchIceShardShieldBreak();
    }

    void LunchIceShardShieldBreak()
    {
        float startRotation = 0.0f;
        for (int i = 0; i < 6; i++)
        {
            Vector2 LunchRotation = (Quaternion.AngleAxis(startRotation + i * 60.0f, Vector3.forward) * Vector2.right).normalized;
            EiscueIceShard iS = Instantiate(eiscueIceShard, transform.position + (Vector3)LunchRotation, Quaternion.Euler(0, 0, startRotation + i * 60.0f));
            iS.empty = this;
            iS.LaunchNotForce(LunchRotation, 4.2f);
            //iS.isSplit = true;
            //iS.isMist = true;
            //iS.SetMaxDistence(3.6f);
        }
    }


    //开始后的冷却时间
    static float TIME_IDLE_NOTFROZEN_START = 6.0f; //TODO需修改时间


    /// <summary>
    /// 不冰冻发呆计时器
    /// <summary>
    float Idle_NotFrozenTimer = 0;

    /// <summary>
    /// 不冰冻发呆开始
    /// <summary>
    public void Idle_NotFrozenStart(float Timer)
    {
        Idle_NotFrozenTimer = Timer;
        ChangeSubState(SubState.Idle_NotFrozen);
    }

    /// <summary>
    /// 不冰冻发呆结束
    /// <summary>
    public void Idle_NotFrozenOver()
    {
        Idle_NotFrozenTimer = 0;
    }


    //=========================不冰冻发呆============================



    //==■==■==■==■==■==■==■主状态：不冰冻状态■==■==■==■==■==■==■==








    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■









}

