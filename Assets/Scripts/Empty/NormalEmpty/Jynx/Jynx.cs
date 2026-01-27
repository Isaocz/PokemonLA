using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jynx : Empty
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

    MyAstarAI AI;//寻路A*ai


    //==============================状态机枚举===================================

    /// <summary>
    /// 主状态
    /// </summary>
    public enum MainState
    {
        idle,        //发呆
        Move,        //移动
        Atk,         //攻击（亲吻）
        Sing,        //使用回音唱歌
        UseBlizzard  //使用暴风雪
    }
    public MainState NowState;

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
        //获取寻路ai
        AI = transform.GetComponent<MyAstarAI>();
        AI.InfatuationDistence = 10.0f;

        //启动计算方向携程
        StartCoroutine(CheckLook());

        //初始化状态机
        idleStart(TIME_IDLE_START);
        //初始化朝向
        SetDirector(new Vector2(-1, -1));
        AI.isCanNotMove = false;

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
                switch (NowState)
                {
                    //发呆状态
                    case MainState.idle:
                        idleTimer -= Time.deltaTime;//发呆计时器时间减少
                        if ( idleTimer <= 0 )         //计时器时间到时间，结束发呆状态
                        {
                            idleOver();
                            MoveStart(TIME_MOVE);
                        }
                        break;
                    //移动状态
                    case MainState.Move:
                        if (!isFearDone) {
                            MoveTimer -= Time.deltaTime;//移动计时器时间减少
                            if (MoveTimer <= 0)         //计时器时间到时间，结束移动状态
                            {
                                MoveOver();
                                Debug.Log("K");
                                AtkStart(TIME_ATK_KISS);
                                break;
                            }
                            if (Vector2.Distance(TargetPosition, (Vector2)transform.position) < DISTENCE_USE_BLIZZARD)
                            {
                                MoveOver();
                                Debug.Log("B");
                                UseBlizzardStart(TIME_ATK_BLIZZARD);
                                break;
                            }
                        }
                        break;
                    //攻击（亲吻）状态
                    case MainState.Atk:
                        if (AtkTimer > 0)
                        {
                            AtkTimer -= Time.deltaTime;//攻击（亲吻）计时器时间减少
                        }
                        else        //计时器时间到时间，结束攻击（亲吻）状态
                        {
                            animator.SetBool("Atk", false);
                            //AtkOver();
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                    //使用回音唱歌状态
                    case MainState.Sing:
                        if (SingTimer > 0)
                        {
                            SingTimer -= Time.deltaTime;//使用回音唱歌计时器时间减少
                        }
                        else       //计时器时间到时间，结束使用回音唱歌状态
                        {
                            animator.SetBool("Atk", false);
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                    //使用暴风雪状态
                    case MainState.UseBlizzard:
                        if (UseBlizzardTimer > 0)
                        {
                            UseBlizzardTimer -= Time.deltaTime;//使用暴风雪计时器时间减少
                        }
                        else//计时器时间到时间，结束使用暴风雪状态
                        {
                            animator.SetBool("Atk", false);
                            //UseBlizzardOver();
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                }
            }

            if ((isEmptyFrozenDone || isSleepDone || isSilence) && NowState != MainState.idle)
            {
                switch (NowState)
                {
                    case MainState.Move:
                        MoveOver();
                        break;
                    case MainState.Atk:
                        AtkOver();
                        break;
                    case MainState.Sing:
                        SingOver();
                        break;
                    case MainState.UseBlizzard:
                        UseBlizzardOver();
                        break;
                }
                animator.SetTrigger("Sleep");
                animator.SetBool("Atk", false);
                animator.SetFloat("Speed", 0.0f);
                DestroyJynxBlizzardObj();
                //DestroyKissObj();
                DestroySingCircleObj();
                idleStart(TIME_IDLE_START);
            }

            if ((isFearDone) && NowState != MainState.Move)
            {
                switch (NowState)
                {
                    case MainState.idle:
                        idleOver();
                        break;
                    case MainState.Atk:
                        AtkOver();
                        break;
                    case MainState.Sing:
                        SingOver();
                        break;
                    case MainState.UseBlizzard:
                        UseBlizzardOver();
                        break;
                }
                animator.SetTrigger("Sleep");
                animator.SetBool("Atk", false);
                animator.SetFloat("Speed", 0.0f);
                DestroyJynxBlizzardObj();
                //DestroyKissObj();
                DestroySingCircleObj();
                MoveStart(TIME_MOVE);
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


    /// <summary>
    /// 检查是否在移动和朝向
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence )
            {
                //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                if (NowState == MainState.Move) {
                    //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                    if (transform.position != LastPosition) { Director = _mTool.TiltMainVector2((transform.position - LastPosition)); }
                    SetDirector(Director);
                    //animator.SetFloat("LookX", Director.x);
                    //animator.SetFloat("LookY", Director.y);
                    //Debug.Log(Director + "+" + NowState);
                }
                //重置位置
                LastPosition = transform.position;
            }
            yield return new WaitForSeconds(0.1f);
        }

    }


    /// <summary>
    /// 各种攻击发射时触发的事件
    /// </summary>
    public void AtkActive()
    {
        switch (NowState)
        {
            //亲吻攻击
            case MainState.Atk:
                LunchKiss();
                break;
            case MainState.UseBlizzard:
                LunchBlizard();
                break;
            case MainState.Sing:
                LunchEchoedVoice();
                break;
        }
    }


    /// <summary>
    /// 各种攻击结束时触发的事件
    /// </summary>
    public void AtkOverEvent()
    {
        switch (NowState)
        {
            //亲吻攻击
            case MainState.Atk:
                AtkOver();
                break;
            //暴风雪攻击
            case MainState.UseBlizzard:
                UseBlizzardOver();
                break;
            //唱歌攻击
            case MainState.Sing:
                SingOver();
                break;
        }
    }

    /**
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
    **/


    //InsertSubStateChange
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■














    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //=========================发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_START = 0.8f; //TODO需修改时间

    //移动后的冷却时间
    static float TIME_IDLE_MOVE = 0.0f; //TODO需修改时间


    //攻击（亲吻）后的冷却时间
    static float TIME_IDLE_ATK = 2.5f; //TODO需修改时间


    //使用回音唱歌后的冷却时间
    static float TIME_IDLE_SING = 10.0f; //TODO需修改时间


    //使用暴风雪后的冷却时间
    static float TIME_IDLE_USEBLIZZARD = 10.0f; //TODO需修改时间



    /// <summary>
    /// 发呆计时器
    /// <summary>
    float idleTimer = 0;

    /// <summary>
    /// 发呆开始
    /// <summary>
    public void idleStart(float Timer)
    {
        idleTimer = Timer;
        NowState = MainState.idle;
    }

    /// <summary>
    /// 发呆结束
    /// <summary>
    public void idleOver()
    {
        idleTimer = 0;
    }


    //=========================发呆============================






    //=========================移动============================


    static float TIME_MOVE = 5.0f;
    /// <summary>
    /// 移动计时器
    /// <summary>
    float MoveTimer = 0;

    /// <summary>
    /// 移动开始
    /// <summary>
    public void MoveStart(float Timer)
    {
        AI.isCanNotMove = false;
        MoveTimer = Timer;
        NowState = MainState.Move;
    }

    /// <summary>
    /// 移动结束
    /// <summary>
    public void MoveOver()
    {
        AI.isCanNotMove = true;
        MoveTimer = 0;
    }


    //=========================移动============================






    //=========================攻击（亲吻）============================

    //亲吻攻击的事件
    static float TIME_ATK_KISS = 0.6f;

    /// <summary>
    /// 天使之吻
    /// </summary>
    public JynxSweetKiss SKiss;


    /// <summary>
    /// 恶魔之吻
    /// </summary>
    public JynxLovelyKiss LKiss;

    GameObject KissObj;

    /// <summary>
    /// 攻击（亲吻）计时器
    /// <summary>
    float AtkTimer = 0.0f;

    //float AtkCDIdleTime = 0.0f;

    /// <summary>
    /// 攻击（亲吻）开始
    /// <summary>
    public void AtkStart(float Timer)
    {
        AtkTimer = Timer;
        NowState = MainState.Atk;
        animator.SetBool("Atk", true);
    }

    /// <summary>
    /// 攻击（亲吻）结束
    /// <summary>
    public void AtkOver()
    {
        AtkTimer = 0;
        idleStart(TIME_IDLE_ATK);
    }


    /// <summary>
    /// 发射亲吻射弹
    /// </summary>
    /// <param name="LunchDir"></param>
    public void LunchKiss()
    {
        Vector2 LunchDir = (TargetPosition - (Vector2)transform.position).normalized;
        if (isEmptyConfusionDone) { LunchDir = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), Vector3.forward) * LunchDir; }
        //发射天使之吻
        if (Random.Range(0.0f, 1.0f) >= 0.5f)
        {
            JynxSweetKiss sk = Instantiate(SKiss , transform.position + Vector3.up * 0.5f + (Vector3)LunchDir * 0.75f , Quaternion.identity);
            sk.empty = this;
            sk.LaunchNotForce(LunchDir , 8.0f);
            KissObj = sk.gameObject;
        }
        //发射恶魔之吻
        else
        {
            JynxLovelyKiss lk = Instantiate(LKiss, transform.position + Vector3.up * 0.5f + (Vector3)LunchDir * 0.75f, Quaternion.identity);
            lk.empty = this;
            lk.LaunchNotForce(LunchDir, 8.0f);
            KissObj = lk.gameObject;
        }
    }


    void DestroyKissObj()
    {
        if (KissObj != null)
        {
            Destroy(KissObj.gameObject);
        }
    }


    //=========================攻击（亲吻）============================






    //=========================使用回音唱歌============================


    //唱歌时的技能圈
    public EmptyEchoedVoice singCircle;
    EmptyEchoedVoice singCircleObj;
    GameObject singCircleGameObj;

    /// <summary>
    /// 使用回音唱歌计时器
    /// <summary>
    float SingTimer = 0;

    //唱歌攻击的事件
    static float TIME_ATK_SING = 3.6f;


    /// <summary>
    /// 使用回音唱歌开始
    /// <summary>
    public void SingStart(float Timer)
    {
        SingTimer = Timer;
        NowState = MainState.Sing;
        animator.SetBool("Atk", true);
    }

    /// <summary>
    /// 使用回音唱歌结束
    /// <summary>
    public void SingOver()
    {
        SingTimer = 0;
        idleStart(TIME_IDLE_SING);
        isEchoedVoiceLunched = false;
    }


    /// <summary>
    /// 判定当前敌人是否可以回声
    /// </summary>
    public override bool isEchoedVoiceisReady()
    {
        if (NowState == MainState.Move && !isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) { return true; }
        else { return false; }
    }


    /// <summary>
    /// 回声是否被发射
    /// </summary>
    bool isEchoedVoiceLunched = false;

    /// <summary>
    /// 使用回声
    /// </summary>
    public override void UseEchoedVoice(int echoedVoiceLevel)
    {
        //Debug.Log(name + "+" + transform.position + "+" + echoedVoiceLevel);
        animator.SetBool("Atk", true);
        SingStart(TIME_ATK_SING);
        animator.SetFloat("Speed", 0);
        animator.SetFloat("LookY", -1);
        AI.isCanNotMove = true;
        JynxEchoedVoiceLevel = echoedVoiceLevel;
        isEchoedVoiceLunched = true;
    }




    int JynxEchoedVoiceLevel = 0;
    /// <summary>
    /// 发射回音
    /// </summary>
    void LunchEchoedVoice()
    {
        if (isEchoedVoiceLunched) {
            isEchoedVoiceLunched = false;
            singCircleObj = Instantiate(singCircle, this.transform.position, Quaternion.identity, transform);
            singCircleObj.SetEchoedVoiceLevel(JynxEchoedVoiceLevel);
            singCircleObj.ParentEmpty = this;
            singCircleGameObj = singCircleObj.gameObject;
        }
    }


    void DestroySingCircleObj()
    {
        Debug.Log(singCircleGameObj);
        if (singCircleGameObj != null)
        {
            Debug.Log("Des");
            singCircleGameObj.SetActive(false);
            Destroy(singCircleGameObj);
        }
    }


    //=========================使用回音唱歌============================






    //=========================使用暴风雪============================



    //触发暴风雪的距离
    static float DISTENCE_USE_BLIZZARD = 2.4f;

    //亲吻攻击的时间
    static float TIME_ATK_BLIZZARD = 0.6f;

    /// <summary>
    /// 迷纯姐的暴风雪
    /// </summary>
    public JynxBlizzard jynxBlizzard;

    GameObject jynxBlizzardObj;

    /// <summary>
    /// 使用暴风雪计时器
    /// <summary>
    float UseBlizzardTimer = 0;

    /// <summary>
    /// 使用暴风雪开始
    /// <summary>
    public void UseBlizzardStart(float Timer)
    {
        UseBlizzardTimer = Timer;
        NowState = MainState.UseBlizzard;
        animator.SetBool("Atk", true);
    }

    /// <summary>
    /// 使用暴风雪结束
    /// <summary>
    public void UseBlizzardOver()
    {
        UseBlizzardTimer = 0;
        idleStart(TIME_IDLE_USEBLIZZARD);
    }


    /// <summary>
    /// 发射暴风雪
    /// </summary>
    public void LunchBlizard()
    {
        JynxBlizzard jb = Instantiate(jynxBlizzard , transform.position , Quaternion.identity);
        jb.ParentEmpty = this;
        jynxBlizzardObj = jb.gameObject;
    }

    
    void DestroyJynxBlizzardObj()
    {

        if (jynxBlizzardObj != null)
        {
            Destroy(jynxBlizzardObj.gameObject);
        }
    }


    //=========================使用暴风雪============================




    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■






}
