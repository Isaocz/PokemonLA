using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrode : Empty
{
    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;

    /// <summary>
    /// 敌人碰撞体
    /// </summary>
    Collider2D EmptyCollider2D;

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




    /// <summary>
    /// 雷电球快要爆炸时的闪光特效
    /// </summary>
    public ExplosionLightMask ElectrodeLightMask;



    /// <summary>
    /// 爆炸预制件
    /// </summary>
    public ElectrodeExplosion ExplosionPerfab;






    //==============================状态机枚举===================================

    /// <summary>
    /// 主状态
    /// </summary>
    enum MainState
    {
        Idle,      //发呆
        Move,      //移动
        Switch,    //电反
        Explosion, //爆炸
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
        { MainState.TODO, new[] { SubState.TODO,... }},
        { MainState.TODO, new[] { SubState.TODO,... }},
        ....
    };
    **/
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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint)/* * 1.2f */;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint)/* * 1.2f */;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //获取寻路ai
        AI = transform.GetComponent<MyAstarAI>();
        AI.isCanNotMove = true;
        //获取碰撞体
        EmptyCollider2D = GetComponent<Collider2D>();
        //启动计算方向携程
        StartCoroutine(CheckLook());
        MoveTimer = TIME_MOVE_MAX;


        StartOverEvent();
        IdleStart(TIME_IDLE_START);

        animator.SetFloat("Speed", 0.0f);
        animator.SetFloat("LookX", 0.0f);
        animator.SetFloat("LookY", -1.0f);

        //电反中心
        InstantiateVoltSwitch();
        VoltSwitchCenter = transform.position;

        ElectrodeLightMask.TurnOff();

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

            //Debug.Log(NowState);
            //■■开始判断状态机 当处于冰冻 睡眠 致盲 麻痹状态时状态机停运
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO状态机停运的额外条件 */
            {
                switch (NowState)
                {
                    //发呆状态
                    case MainState.Idle:
                        IdleTimer -= Time.deltaTime;//发呆计时器时间减少
                        if (IdleTimer <= 0)         //计时器时间到时间，结束发呆状态
                        {
                            IdleOver();
                            MoveStart();
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                    //移动状态
                    case MainState.Move:
                        if (!isFearDone) { MoveTimer -= Time.deltaTime;/*移动计时器时间减少*/ }

                        for (int i = 0; i < ExplosionMaskAlpha.Count; i++)
                        {
                            if (MoveTimer < ExplosionMaskAlpha[i].z && (ElectrodeLightMask.BlinkCycleTime != ExplosionMaskAlpha[i].x || ElectrodeLightMask.BlinkMaxAlpha != ExplosionMaskAlpha[i].y))
                            {
                                ElectrodeLightMask.SetBlink(ExplosionMaskAlpha[i].x, ExplosionMaskAlpha[i].y);
                                AI.SetSpeed(speed, SpeedAlphaList[i]);
                            }
                        }
                        if (MoveTimer <= 0)         //计时器时间到时间，结束移动状态, 爆炸
                        {
                            MoveOver();
                            ExplosionStart();
                            //TODO添加下一个状态的开始方法
                        }
                        //玩家距离电反中心距离小于顽皮蛋距离玩家距离时 电反
                        if (!isFearDone &&
                            Vector2.Distance((Vector2)player.transform.position, VoltSwitchCenter) <= Vector2.Distance((Vector2)transform.position, (Vector2)player.transform.position) &&
                            Vector2.Distance((Vector2)player.transform.position, VoltSwitchCenter) >= DISTENCE_SWITCH_TRIGGER_MIN &&
                            Vector2.Distance((Vector2)transform.position, VoltSwitchCenter) >= DISTENCE_SWITCH_TRIGGER_MIN 
                            )
                        {
                            MoveOver();
                            SwitchStart(TIME_SWITCH_NORMAL);
                        }
                        break;
                    //电反状态
                    case MainState.Switch:
                        SwitchTimer -= Time.deltaTime;//发呆计时器时间减少
                        transform.position += new Vector3(SwitchDir.x * SwitchSpeed * Time.deltaTime, SwitchDir.y * SwitchSpeed * Time.deltaTime, 0);
                        if (SwitchTimer <= 0)         //计时器时间到时间，结束发呆状态
                        {
                            SwitchOver();
                            MoveStart();
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                    //爆炸状态
                    case MainState.Explosion:
                        // ExplosionTimer -= Time.deltaTime;//爆炸计时器时间减少
                        //if ( ExplosionTimer <= 0 )         //计时器时间到时间，结束爆炸状态
                        //{
                        //ExplosionOver();
                        //TODO添加下一个状态的开始方法
                        //}
                        break;
                }
            }
            //■■结束判断状态机
            else if ((isEmptyFrozenDone || isSilence || isSleepDone) && (NowState == MainState.Move))
            {
                animator.SetTrigger("Sleep");
                animator.SetFloat("Speed", 0);
                MoveOver();
                ElectrodeLightMask.TurnOff();
                IdleStart(TIME_IDLE_STATE);
            }
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
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//未被魅惑 且与玩家碰撞时
        {
            //EmptyTouchHit(other.gameObject);//触发触碰伤害

        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//被魅惑 且与其他敌人碰撞时
        {
            //InfatuationEmptyTouchHit(other.gameObject);//触发魅惑后触碰伤害
        }
    }









    /**

    //=================================碰壁反弹=======================================


    private void OnCollisionStay2D(Collision2D other)
    {
        Rebound_BeLunch();
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
            Debug.Log("R" + DistenceList[0] + "U" + DistenceList[1] + "L" + DistenceList[2] + "D" + DistenceList[3] + "+" + MinDistence);
            HitVector = _mTool.MainVector2(Quaternion.AngleAxis(MinIndex * 90.0f, Vector3.forward) * HitVector);
        }

        //LunchDirector = new Vector2(LunchDirector.x * (HitVector.x == 0 ? 1 : -1), LunchDirector.y * (HitVector.y == 0 ? 1 : -1));
        return true;

    }


    //=================================碰壁反弹=======================================

    **/


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
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && NowState != MainState.Idle)
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


    //雷电球爆炸
    public override void EmptyEcplosionEvent()
    {
        //base.EmptyEcplosionEvent();
        animator.SetTrigger("Explosion");
        //AI.isCanNotMove = true;
        //AI.SetSpeed(speed, SpeedAlphaList[i]);
        ElectrodeLightMask.TurnOff();
        ParentPokemonRoom.CameraShake(1.2f , 5.0f , true);
    }

    public override void DieEvent()
    {
        base.DieEvent();
        VSObj.RemoveVoltSwitchPoint();
    }

    //InsertSubStateChange
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■














    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //=========================发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_START = 0.5f; //TODO需修改时间

    //状态结束后的冷却时间
    static float TIME_IDLE_STATE = 0.15f; //TODO需修改时间

    //移动后的冷却时间
    static float TIME_IDLE_MOVE = 0.5f; //TODO需修改时间


    //爆炸后的冷却时间
    static float TIME_IDLE_EXPLOSION = 0.0f; //TODO需修改时间



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
        AI.isCanNotMove = true;
    }

    /// <summary>
    /// 发呆结束
    /// <summary>
    public void IdleOver()
    {
        IdleTimer = 0;
    }


    //=========================发呆============================






    //=========================移动============================

    /// <summary>
    /// 爆炸预备频闪时的频闪参数，第一位为频闪周期，第二位是频闪最大亮度，第三位为进入该段频闪的时间
    /// </summary>
    static List<Vector3> ExplosionMaskAlpha = new List<Vector3> {
        new Vector3(0.5f, 0.0f, 6.0f),  //第一段 不频闪
        new Vector3(0.5f, 0.4f, 4.0f),  //第二段
        new Vector3(0.5f, 0.6f, 2.2f),  //第三段
        new Vector3(0.3f, 0.72f, 1.2f), //第四段
        new Vector3(0.15f, 0.8f, 0.4f)  //第五段
    };

    static List<float> SpeedAlphaList = new List<float>
    {
        1.0f,
        1.1f,
        1.25f,
        1.5f,
        1.8f,
    };


    //移动的时间
    static float TIME_MOVE_MAX = 6.0f; //TODO需修改时间

    /// <summary>
    /// 移动计时器
    /// <summary>
    float MoveTimer = 0;

    /// <summary>
    /// 移动开始
    /// <summary>
    public void MoveStart()
    {
        ElectrodeLightMask.TurnOff();
        //MoveTimer = Timer;
        NowState = MainState.Move;
        AI.isCanNotMove = false;
        AI.SetSpeed(speed, 1.0f);
        ElectrodeLightMask.TurnOn(ExplosionMaskAlpha[0].x, ExplosionMaskAlpha[0].y);
    }

    /// <summary>
    /// 移动结束
    /// <summary>
    public void MoveOver()
    {
        //MoveTimer = 0;
        //AI.isCanNotMove = true;
    }


    //=========================移动============================











    //=========================电反============================

    /// <summary>
    /// 电反触发的最短距离
    /// </summary>
    static float DISTENCE_SWITCH_TRIGGER_MIN = 2.0f; //TODO需修改时间


    /// <summary>
    /// 电反需要时间
    /// </summary>
    static float TIME_SWITCH_NORMAL = 0.25f; //TODO需修改时间

    /// <summary>
    /// 电反预制体
    /// </summary>
    public ElectrodeVoltSwitchPoint VSPerfab;

    /// <summary>
    /// 电反实体
    /// </summary>
    ElectrodeVoltSwitchPoint VSObj;

    /// <summary>
    /// 电反的返回中心
    /// </summary>
    public Vector2 VoltSwitchCenter;

    /// <summary>
    /// 电反特效预制件
    /// </summary>
    public GameObject VoltSwitchPS;

    /// <summary>
    /// 电反特效实例
    /// </summary>
    GameObject VoltSwitchPSObj;

    /// <summary>
    /// 电反波动实例
    /// </summary>
    public ElectrodeVoltSwitchThunderWave VSWavePerfab;

    /// <summary>
    /// 电反波动实例
    /// </summary>
    ElectrodeVoltSwitchThunderWave VSWaveObj;

    /// <summary>
    /// 电反遮罩图层
    /// </summary>
    public GameObject SpriteMaskVoltSwitch;

    /// <summary>
    /// 电反的速度
    /// </summary>
    float SwitchSpeed;

    /// <summary>
    /// 电反的方向
    /// </summary>
    Vector2 SwitchDir;

    /// <summary>
    /// 电反计时器
    /// <summary>
    float SwitchTimer = 0;


    /// <summary>
    /// 电反开始
    /// <summary>
    public void SwitchStart(float Timer)
    {
        SwitchTimer = Timer;
        NowState = MainState.Switch;
        AI.isCanNotMove = true;
        EmptyCollider2D.isTrigger = true;
        SwitchSpeed = Vector2.Distance((Vector2)transform.position, VoltSwitchCenter) / SwitchTimer;
        SwitchDir = (VoltSwitchCenter - (Vector2)transform.position).normalized;

        if (VoltSwitchPSObj != null) { Destroy(VoltSwitchPSObj.gameObject); }
        VoltSwitchPSObj = Instantiate(VoltSwitchPS, transform.position, Quaternion.identity, transform);
        //设置遮罩图层
        SpriteMaskVoltSwitch.SetActive(true);
        //发射波动
        LunchVoltSwitchWave();
        //无敌
        IsDefStateByNormal = true;
        Invincible = true;
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.01f, 1.5f, new Color(0.9716981f, 0.9647512f, 0.5271004f, 0.7f ), Vector2.up * -0.75f);
        }
    }

    /// <summary>
    ///  生成电反特效
    /// </summary>
    public void InstantiateVoltSwitch()
    {
        VSObj = Instantiate(VSPerfab, transform.position, Quaternion.identity);
    }


    /// <summary>
    /// 发射电反波动
    /// </summary>
    public void LunchVoltSwitchWave()
    {
        VSWaveObj = Instantiate(VSWavePerfab , VoltSwitchCenter , Quaternion.identity );
        VSWaveObj.empty = this;
    }

    /// <summary>
    /// 电反结束
    /// <summary>
    public void SwitchOver()
    {
        SwitchTimer = 0;
        AI.isCanNotMove = false;
        EmptyCollider2D.isTrigger = false;
        transform.position = VoltSwitchCenter;
        if (VoltSwitchPSObj != null) { VoltSwitchPSObj.transform.parent = null;  }

        //设置遮罩图层
        SpriteMaskVoltSwitch.SetActive(false);
        //无敌
        IsDefStateByNormal = false;
        Invincible = false;
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }


    //=========================电反============================









    //=========================爆炸============================

    //爆炸的时间
    static float TIME_EXPLOSION = 6.0f; //TODO需修改时间


    /// <summary>
    /// 爆炸计时器
    /// <summary>
    //float ExplosionTimer = 0;

    /// <summary>
    /// 爆炸开始
    /// <summary>
    public void ExplosionStart()
    {
        ElectrodeLightMask.ExplosionStart(0.5f);
        AI.isCanNotMove = false;
        //ExplosionTimer = Timer;
        NowState = MainState.Explosion;
        ElectrodeExplosion e = Instantiate(ExplosionPerfab, transform.position, Quaternion.identity, transform);
        e.empty = this;
        IsDefStateByExplosion = true;
        Invincible = true;

    }

    /// <summary>
    /// 爆炸结束
    /// <summary>
    public void ExplosionOver()
    {
        //ExplosionTimer = 0;
    }


    //=========================爆炸============================




    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■



}
