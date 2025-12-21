using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Klang : Empty
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




    //==============================状态机枚举===================================

    /// <summary>
    /// 主状态
    /// </summary>
    enum MainState
    {
        Single,     //单个中齿轮 
        HaveSGear,  //携带小齿轮
        WithLGear,  //跟随大齿轮
    }
    MainState FamilyState;


    /// <summary>
    /// 副状态
    /// </summary>
    enum SubState
    {
        Idle_Single,         //单个中齿轮发呆
        Rush_Single,         //单个中齿轮冲刺
        CircleAtk_Single,    //单个中齿轮圆圈攻击
        Idle_HaveSGear,      //携带小齿轮发呆
        Rush_WithLGear,      //携带小齿轮冲刺
        LunchSGear_WithLGear,//携带小齿轮发射小齿轮
        Idle_WithLGear,      //跟随大齿轮发呆
        Surround_WithLGear,  //跟随大齿轮环绕
        Back_WithLGear,      //跟随大齿轮返回
    }
    SubState subState;



    /// <summary>
    /// 状态映射关系
    /// </summary>
    private static Dictionary<MainState, SubState[]> StateMap = new()
    {
        { MainState.Single, new[] { SubState.Idle_Single, SubState.Rush_Single, SubState.CircleAtk_Single } },
        { MainState.HaveSGear, new[] { SubState.Idle_HaveSGear, SubState.Rush_WithLGear, SubState.LunchSGear_WithLGear } },
        { MainState.WithLGear, new[] { SubState.Idle_WithLGear, SubState.Surround_WithLGear, SubState.Back_WithLGear } },
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

        //启动计算方向携程
        //StartCoroutine(CheckLook());


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

            switch (FamilyState)
            {
                //●主状态：【单个中齿轮】状态
                case MainState.Single:
                    // 当处于冰冻 睡眠 致盲 麻痹状态时主状态【单个中齿轮】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) /* TODO【单个中齿轮】状态停运的额外条件 */
                    {
                        //判断副状态
                        switch (subState)
                        {
                            //【单个中齿轮发呆】状态
                            case SubState.Idle_Single:
                                Idle_SingleTimer -= Time.deltaTime;//【单个中齿轮发呆】计时器时间减少
                                if (Idle_SingleTimer <= 0)         //计时器时间到时间，结束【单个中齿轮发呆】状态
                                {
                                    if (isHighSpeed)
                                    {
                                        Idle_SingleOver();
                                        Rush_SingleStart(TIME_RUSH_SINGLE);
                                    }
                                    else
                                    {
                                        if (!animator.GetBool("HighSpeed")) {
                                            HighSpeedModeEnter();
                                        }
                                    }
                                    
                                    //TODO添加下一个状态的开始方法
                                }
                                break;
                            //【单个中齿轮冲刺】状态
                            case SubState.Rush_Single:
                                Rush_SingleTimer -= Time.deltaTime;//【单个中齿轮冲刺】计时器时间减少


                                if (Rush_SingleTimer <= 0)         //计时器时间到时间，结束【单个中齿轮冲刺】状态
                                {
                                    Rush_SingleOver();
                                    if (RushCount >= 3)
                                    {
                                        RushCount = 0;
                                        CircleAtk_SingleStart(TIME_CIRCLE_ATK_SINGLE);
                                    }
                                    else
                                    {
                                        Idle_SingleStart(TIME_IDLE_SINGLE_RUSH_SINGLE_SHORT);
                                    } 
                                }

                                //冲刺
                                MoveBySpeedAndDir(RushDir, speed, SpeedAlpha * SPEED_ALPHA_RUSH * (isEmptyConfusionDone ? 0.5f : 1.0f), 0.2f, 0.2f, 0.2f, 0.2f);
                                break;
                            //【单个中齿轮圆圈攻击】状态
                            case SubState.CircleAtk_Single:
                                CircleAtk_SingleTimer -= Time.deltaTime;//【单个中齿轮圆圈攻击】计时器时间减少
                                //移动
                                MoveBySpeedAndDir(Quaternion.AngleAxis(-CircleAtkTurn * (TIME_CIRCLE_ATK_SINGLE - CircleAtk_SingleTimer) * (isEmptyConfusionDone ? 0.5f : 1.0f) * Omega_CIRCLEATK, Vector3.forward) * CircleAtkDir, V_CIRCLEATK, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                Debug.Log(CircleAtkDir);
                                Debug.Log(RadiusCircleAtk);
                                Debug.Log(Omega_CIRCLEATK);
                                if (CircleAtk_SingleTimer <= 0)         //计时器时间到时间，结束【单个中齿轮圆圈攻击】状态
                                {
                                    CircleAtk_SingleOver();
                                    Idle_SingleStart(TIME_IDLE_SINGLE_CIRCLEATK_SINGLE);
                                    //TODO添加下一个状态的开始方法
                                }
                                break;
                        }
                    }
                    if ( (isSleepDone || isSilence || isFearDone) && (subState != SubState.Idle_Single || isHighSpeed || animator.GetBool("HighSpeed") ) )
                    {
                        RushCount = 0;
                        animator.SetTrigger("Sleep");
                        animator.SetBool("HighSpeed", false);
                        switch (subState)
                        {
                            case SubState.Rush_Single:
                                Rush_SingleOver();
                                break;
                            case SubState.CircleAtk_Single:
                                CircleAtk_SingleOver();
                                break;
                        }
                        HighSpeedModeOver();
                        HighSpeedModeOut();
                        Idle_SingleStart(TIME_IDLE_SINGLE_START);
                    }
                    break;
                //●主状态：【携带小齿轮】状态
                case MainState.HaveSGear:
                    // 当处于冰冻 睡眠 致盲 麻痹状态时主状态【携带小齿轮】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO【携带小齿轮】状态停运的额外条件 */
                    {
                        //判断副状态
                        switch (subState)
                        {
                            //【携带小齿轮发呆】状态
                            case SubState.Idle_HaveSGear:
                                Idle_HaveSGearTimer -= Time.deltaTime;//【携带小齿轮发呆】计时器时间减少
                                if (Idle_HaveSGearTimer <= 0)         //计时器时间到时间，结束【携带小齿轮发呆】状态
                                {
                                    Idle_HaveSGearOver();
                                    //TODO添加下一个状态的开始方法
                                }
                                break;
                            //【携带小齿轮冲刺】状态
                            case SubState.Rush_WithLGear:
                                Rush_WithLGearTimer -= Time.deltaTime;//【携带小齿轮冲刺】计时器时间减少
                                if (Rush_WithLGearTimer <= 0)         //计时器时间到时间，结束【携带小齿轮冲刺】状态
                                {
                                    Rush_WithLGearOver();
                                    //TODO添加下一个状态的开始方法
                                }
                                break;
                            //【携带小齿轮发射小齿轮】状态
                            case SubState.LunchSGear_WithLGear:
                                LunchSGear_WithLGearTimer -= Time.deltaTime;//【携带小齿轮发射小齿轮】计时器时间减少
                                if (LunchSGear_WithLGearTimer <= 0)         //计时器时间到时间，结束【携带小齿轮发射小齿轮】状态
                                {
                                    LunchSGear_WithLGearOver();
                                    //TODO添加下一个状态的开始方法
                                }
                                break;
                        }
                    }
                    break;
                //●主状态：【跟随大齿轮】状态
                case MainState.WithLGear:
                    // 当处于冰冻 睡眠 致盲 麻痹状态时主状态【跟随大齿轮】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO【跟随大齿轮】状态停运的额外条件 */
                    {
                        //判断副状态
                        switch (subState)
                        {
                            //【跟随大齿轮发呆】状态
                            case SubState.Idle_WithLGear:
                                Idle_WithLGearTimer -= Time.deltaTime;//【跟随大齿轮发呆】计时器时间减少
                                if (Idle_WithLGearTimer <= 0)         //计时器时间到时间，结束【跟随大齿轮发呆】状态
                                {
                                    Idle_WithLGearOver();
                                    //TODO添加下一个状态的开始方法
                                }
                                break;
                            //【跟随大齿轮环绕】状态
                            case SubState.Surround_WithLGear:
                                Surround_WithLGearTimer -= Time.deltaTime;//【跟随大齿轮环绕】计时器时间减少
                                if (Surround_WithLGearTimer <= 0)         //计时器时间到时间，结束【跟随大齿轮环绕】状态
                                {
                                    Surround_WithLGearOver();
                                    //TODO添加下一个状态的开始方法
                                }
                                break;
                            //【跟随大齿轮返回】状态
                            case SubState.Back_WithLGear:
                                Back_WithLGearTimer -= Time.deltaTime;//【跟随大齿轮返回】计时器时间减少
                                if (Back_WithLGearTimer <= 0)         //计时器时间到时间，结束【跟随大齿轮返回】状态
                                {
                                    Back_WithLGearOver();
                                    //TODO添加下一个状态的开始方法
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

            
            //根据魅惑情况确实目标位置
            if (!isEmptyInfatuationDone || _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile()).Count <= 1 || InfatuationForDistanceEmpty() == null)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }
            
        }
    }



















    //■■■■■■■■■■■■■■■■■■■■碰撞■■■■■■■■■■■■■■■■■■■■■■



    /// <summary>
    /// 高速模式碰撞威力
    /// </summary>
    static int DMAGE_HIGH_SPEED = 25;

    /// <summary>
    /// 齿轮飞盘碰撞威力
    /// </summary>
    static int DMAGE_CIRCLE_ATK = 50;



    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (FamilyState)
        {
            case MainState.Single:
                if (isHighSpeed)
                {
                    //圆盘攻击
                    if (subState == SubState.CircleAtk_Single)
                    {
                        CircleAtkTouch(other);
                    }
                    //高速攻击
                    else
                    {
                        HighSpeedTouch(other);
                    }
                }
                //低速攻击
                else
                {
                    NormalTouch(other);
                }
                break;
            case MainState.HaveSGear:
                break;
            case MainState.WithLGear:
                break;
        }
    }





    /// <summary>
    /// 低速模式触碰伤害
    /// </summary>
    void NormalTouch(Collision2D other)
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


    /// <summary>
    /// 高速模式触碰伤害
    /// </summary>
    void HighSpeedTouch(Collision2D other)
    {
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//未被魅惑 且与玩家碰撞时
        {
            PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(this.gameObject, other.gameObject, DMAGE_HIGH_SPEED, 0, 0, PokemonType.TypeEnum.Steel);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 3.0f;
                playerControler.KnockOutDirection = (new Vector2(Director.y, Director.x)).normalized;
            }
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//被魅惑 且与其他敌人碰撞时
        {
            Empty e = other.gameObject.GetComponent<Empty>();
            Pokemon.PokemonHpChange(this.gameObject, e.gameObject, DMAGE_HIGH_SPEED, 0, 0, PokemonType.TypeEnum.Steel);
        }
    }


    /// <summary>
    /// 圆盘攻击触碰伤害
    /// </summary>
    void CircleAtkTouch(Collision2D other)
    {
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//未被魅惑 且与玩家碰撞时
        {
            PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(this.gameObject, other.gameObject, DMAGE_CIRCLE_ATK, 0, 0, PokemonType.TypeEnum.Steel);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 7.0f;
                playerControler.KnockOutDirection = (new Vector2(Director.y, Director.x)).normalized;
            }
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//被魅惑 且与其他敌人碰撞时
        {
            Empty e = other.gameObject.GetComponent<Empty>();
            Pokemon.PokemonHpChange(this.gameObject, e.gameObject, DMAGE_CIRCLE_ATK, 0, 0, PokemonType.TypeEnum.Steel);
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
    


    //InsertSubStateChange


    /// <summary>
    /// 切换副状态
    /// </summary>
    void ChangeSubState(SubState targetSubstate)
    {
        subState = targetSubstate;
        var mainState = GetMainBySub(targetSubstate);
        FamilyState = mainState;
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































    //■■■■■■■■■■■■■■■■■■■■换挡■■■■■■■■■■■■■■■■■■■■■■


    //高速状态时的速度
    static float SPEED_ALPHA_HIGH = 3.7f;

    //普通状态时的速度
    static float SPEED_ALPHA_NORMAL = 1.0f;


    /// <summary>
    /// 速度是否提升
    /// </summary>
    bool isHighSpeed = false;

    /// <summary>
    /// 攻击力是否提升
    /// </summary>
    bool isAtkUP = false;

    public float SpeedAlpha = SPEED_ALPHA_NORMAL;

    /// <summary>
    /// 准备进入高速模式
    /// </summary>
    public void HighSpeedModeEnter()
    {

        animator.SetBool("HighSpeed", true);
    }

    /// <summary>
    /// 进入高速模式
    /// </summary>
    public void HighSpeedModeStart()
    {
        if (isHighSpeed == false)
        {
            isHighSpeed = true;
            SpeedAlpha = SPEED_ALPHA_HIGH;
        }
        if (isAtkUP == false)
        {
            AtkChange(2, 0.0f);
            isAtkUP = true;
        }
    }

    /// <summary>
    /// 高速模式结束 开始减速
    /// </summary>
    public void HighSpeedModeOver()
    {
        if (isHighSpeed == true)
        {
            isHighSpeed = false;
            animator.SetBool("HighSpeed", false);
        }
        SpeedAlpha = SPEED_ALPHA_NORMAL;
        if (isAtkUP == true)
        {
            AtkChange(-2, 0.0f);
            isAtkUP = false;
        }
    }

    /// <summary>
    /// 减速完毕，回到低速模式
    /// </summary>
    public void HighSpeedModeOut()
    {

    }
    //■■■■■■■■■■■■■■■■■■■■换挡■■■■■■■■■■■■■■■■■■■■■■


































    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //==■==■==■==■==■==■==■主状态：单个中齿轮状态■==■==■==■==■==■==■==



    //=========================单个中齿轮发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_SINGLE_START = 0.5f;


    //单个中齿轮冲刺后的短冷却时间
    static float TIME_IDLE_SINGLE_RUSH_SINGLE_SHORT = 0.25f;


    //单个中齿轮冲刺三次后的冷却时间
    static float TIME_IDLE_SINGLE_RUSH_SINGLE_LONG = 1.0f;


    //单个中齿轮圆圈攻击后的冷却时间
    static float TIME_IDLE_SINGLE_CIRCLEATK_SINGLE = 7.0f; //TODO需修改时间



    /// <summary>
    /// 单个中齿轮发呆计时器
    /// <summary>
    float Idle_SingleTimer = 0;

    /// <summary>
    /// 单个中齿轮发呆开始
    /// <summary>
    public void Idle_SingleStart(float Timer)
    {
        Idle_SingleTimer = Timer;
        ChangeSubState(SubState.Idle_Single);
    }

    /// <summary>
    /// 单个中齿轮发呆结束
    /// <summary>
    public void Idle_SingleOver()
    {
        Idle_SingleTimer = 0;
    }


    //=========================单个中齿轮发呆============================






    //=========================单个中齿轮冲刺============================

    /// <summary>
    /// 冲刺时间
    /// </summary>
    static float TIME_RUSH_SINGLE = 0.4f; //TODO需修改时间

    /// <summary>
    /// 冲刺速度加成
    /// </summary>
    static float SPEED_ALPHA_RUSH = 2.0f; //TODO需修改时间


    /// <summary>
    /// 单个中齿轮冲刺计时器
    /// <summary>
    float Rush_SingleTimer = 0;

    /// <summary>
    /// 冲刺方向
    /// </summary>
    Vector2 RushDir;

    /// <summary>
    /// 冲刺的次数
    /// </summary>
    int RushCount = 0;

    /// <summary>
    /// 单个中齿轮冲刺开始
    /// <summary>
    public void Rush_SingleStart(float Timer)
    {
        Rush_SingleTimer = Timer;
        ChangeSubState(SubState.Rush_Single);
        RushDir = (TargetPosition - (Vector2)transform.position).normalized;
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
    }

    /// <summary>
    /// 单个中齿轮冲刺结束
    /// <summary>
    public void Rush_SingleOver()
    {
        Rush_SingleTimer = 0;
        RushCount++;
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        RushDir = Vector2.zero;
    }


    //=========================单个中齿轮冲刺============================











    //=========================单个中齿轮圆圈攻击============================






    //圆形攻击的角速度
    static float Omega_CIRCLEATK { get { return 360.0f / TIME_CIRCLE_ATK_SINGLE; } }

    /// <summary>
    /// 圆形攻击的周期（转一圈）
    /// </summary>
    static float TIME_CIRCLE_ATK_SINGLE = 0.8f; //TODO需修改时间







    /// <summary>
    /// 圆形攻击的半径
    /// </summary>
    float RadiusCircleAtk ;

    /// <summary>
    /// 圆形攻击的线速度
    /// </summary>
    float V_CIRCLEATK { get { return Mathf.Deg2Rad * Omega_CIRCLEATK * RadiusCircleAtk; } }

    /// <summary>
    /// 圆形攻击的速度向量
    /// </summary>
    Vector2 CircleAtkDir = new Vector2(0, 0);

    /// <summary>
    /// 圆形攻击的顺逆时针方向
    /// </summary>
    float CircleAtkTurn;

    /// <summary>
    /// 单个中齿轮圆圈攻击计时器
    /// <summary>
    float CircleAtk_SingleTimer = 0;






    /// <summary>
    /// 单个中齿轮圆圈攻击开始
    /// <summary>
    public void CircleAtk_SingleStart(float Timer)
    {
        CircleAtk_SingleTimer = Timer;
        ChangeSubState(SubState.CircleAtk_Single);

        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            CircleAtkTurn = 1;
        }
        else { CircleAtkTurn = -1; }

        RadiusCircleAtk = Mathf.Clamp( Vector2.Distance(TargetPosition, (Vector2)transform.position) , 3.0f , 6.0f );
        CircleAtkDir = Quaternion.AngleAxis(CircleAtkTurn * 90.0f , Vector3.forward) * (TargetPosition - (Vector2)transform.position).normalized;
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.04f, 1.8f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
    }

    /// <summary>
    /// 单个中齿轮圆圈攻击结束
    /// <summary>
    public void CircleAtk_SingleOver()
    {
        CircleAtk_SingleTimer = 0;
        CircleAtkTurn = 1;
        HighSpeedModeOver();
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }





    //=========================单个中齿轮圆圈攻击============================



    //==■==■==■==■==■==■==■主状态：单个中齿轮状态■==■==■==■==■==■==■==










    //==■==■==■==■==■==■==■主状态：携带小齿轮状态■==■==■==■==■==■==■==



    //=========================携带小齿轮发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_HAVESGEAR_START = 0.0f; //TODO需修改时间

    //携带小齿轮冲刺后的冷却时间
    static float TIME_IDLE_HAVESGEAR_RUSH_WITHLGEAR = 0.0f; //TODO需修改时间


    //携带小齿轮发射小齿轮后的冷却时间
    static float TIME_IDLE_HAVESGEAR_LUNCHSGEAR_WITHLGEAR = 0.0f; //TODO需修改时间



    /// <summary>
    /// 携带小齿轮发呆计时器
    /// <summary>
    float Idle_HaveSGearTimer = 0;

    /// <summary>
    /// 携带小齿轮发呆开始
    /// <summary>
    public void Idle_HaveSGearStart(float Timer)
    {
        Idle_HaveSGearTimer = Timer;
        ChangeSubState(SubState.Idle_HaveSGear);
    }

    /// <summary>
    /// 携带小齿轮发呆结束
    /// <summary>
    public void Idle_HaveSGearOver()
    {
        Idle_HaveSGearTimer = 0;
    }


    //=========================携带小齿轮发呆============================






    //=========================携带小齿轮冲刺============================




    /// <summary>
    /// 携带小齿轮冲刺计时器
    /// <summary>
    float Rush_WithLGearTimer = 0;

    /// <summary>
    /// 携带小齿轮冲刺开始
    /// <summary>
    public void Rush_WithLGearStart(float Timer)
    {
        Rush_WithLGearTimer = Timer;
        ChangeSubState(SubState.Rush_WithLGear);
    }

    /// <summary>
    /// 携带小齿轮冲刺结束
    /// <summary>
    public void Rush_WithLGearOver()
    {
        Rush_WithLGearTimer = 0;
    }


    //=========================携带小齿轮冲刺============================






    //=========================携带小齿轮发射小齿轮============================




    /// <summary>
    /// 携带小齿轮发射小齿轮计时器
    /// <summary>
    float LunchSGear_WithLGearTimer = 0;

    /// <summary>
    /// 携带小齿轮发射小齿轮开始
    /// <summary>
    public void LunchSGear_WithLGearStart(float Timer)
    {
        LunchSGear_WithLGearTimer = Timer;
        ChangeSubState(SubState.LunchSGear_WithLGear);
    }

    /// <summary>
    /// 携带小齿轮发射小齿轮结束
    /// <summary>
    public void LunchSGear_WithLGearOver()
    {
        LunchSGear_WithLGearTimer = 0;
    }


    //=========================携带小齿轮发射小齿轮============================



    //==■==■==■==■==■==■==■主状态：携带小齿轮状态■==■==■==■==■==■==■==










    //==■==■==■==■==■==■==■主状态：跟随大齿轮状态■==■==■==■==■==■==■==



    //=========================跟随大齿轮发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_WITHLGEAR_START = 0.0f; //TODO需修改时间

    //跟随大齿轮环绕后的冷却时间
    static float TIME_IDLE_WITHLGEAR_SURROUND_WITHLGEAR = 0.0f; //TODO需修改时间


    //跟随大齿轮返回后的冷却时间
    static float TIME_IDLE_WITHLGEAR_BACK_WITHLGEAR = 0.0f; //TODO需修改时间



    /// <summary>
    /// 跟随大齿轮发呆计时器
    /// <summary>
    float Idle_WithLGearTimer = 0;

    /// <summary>
    /// 跟随大齿轮发呆开始
    /// <summary>
    public void Idle_WithLGearStart(float Timer)
    {
        Idle_WithLGearTimer = Timer;
        ChangeSubState(SubState.Idle_WithLGear);
    }

    /// <summary>
    /// 跟随大齿轮发呆结束
    /// <summary>
    public void Idle_WithLGearOver()
    {
        Idle_WithLGearTimer = 0;
    }


    //=========================跟随大齿轮发呆============================






    //=========================跟随大齿轮环绕============================




    /// <summary>
    /// 跟随大齿轮环绕计时器
    /// <summary>
    float Surround_WithLGearTimer = 0;

    /// <summary>
    /// 跟随大齿轮环绕开始
    /// <summary>
    public void Surround_WithLGearStart(float Timer)
    {
        Surround_WithLGearTimer = Timer;
        ChangeSubState(SubState.Surround_WithLGear);
    }

    /// <summary>
    /// 跟随大齿轮环绕结束
    /// <summary>
    public void Surround_WithLGearOver()
    {
        Surround_WithLGearTimer = 0;
    }


    //=========================跟随大齿轮环绕============================






    //=========================跟随大齿轮返回============================




    /// <summary>
    /// 跟随大齿轮返回计时器
    /// <summary>
    float Back_WithLGearTimer = 0;

    /// <summary>
    /// 跟随大齿轮返回开始
    /// <summary>
    public void Back_WithLGearStart(float Timer)
    {
        Back_WithLGearTimer = Timer;
        ChangeSubState(SubState.Back_WithLGear);
    }

    /// <summary>
    /// 跟随大齿轮返回结束
    /// <summary>
    public void Back_WithLGearOver()
    {
        Back_WithLGearTimer = 0;
    }


    //=========================跟随大齿轮返回============================



    //==■==■==■==■==■==■==■主状态：跟随大齿轮状态■==■==■==■==■==■==■==








    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    



}
