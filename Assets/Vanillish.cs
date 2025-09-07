using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//多多冰-》有多多冰伙伴
//      -》无多多冰伙伴 --》冰光-》摇晃（有从属迷你冰发射 无从属迷你冰冰雾）


public class Vanillish : Empty
{
    //是否有多多冰伙伴
    enum HavePartnerState
    {
        No,     //没有伙伴
        Partner,   //有伙伴
        Father  //有双倍多多冰父
    }
    HavePartnerState havePartnerState;



    //没有伙伴时的状态
    enum SingleState
    {
        Idle,  //发呆
        Beam,  //射激光
        Shake, //摇晃
    }
    SingleState singleState;



    //有伙伴时的状态
    enum PartnerState
    {
    }
    PartnerState partnerState;



    //有父辈时的状态
    enum FatherState
    {
    }
    FatherState fatherState;



    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;

    public Vector2 TARGET_POSITION
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }
    Vector2 TargetPosition;

    Vector3 LastPosition;//计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行

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
        StartCoroutine(CheckLook());


        //初始化状态机（无父辈）
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Idle;
        IdleStart_Single(TIME_START_SINGLE);



        //初始化方向
        Director = new Vector2(-1, -1);
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
            switch (havePartnerState)
            {
                //无伙伴
                case HavePartnerState.No:
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        switch (singleState)
                        {
                            //发呆
                            case SingleState.Idle:
                                IdleTimer_Single -= Time.deltaTime;
                                //发呆结束后或恐惧时转进移动状态
                                if (IdleTimer_Single <= 0)
                                {
                                    IdleOver_Single();
                                    BeamStart_Single(TIME_BEAM);
                                }
                                break;
                            //激光
                            case SingleState.Beam:

                                BeamTimer_Single -= Time.deltaTime;
                                Debug.Log(BeamTimer_Single);
                                if (BeamTimer_Single >= 0) {
                                    //设置方向
                                    Vector2 MoveDirector = (TargetPosition - (Vector2)transform.position).normalized;
                                    //恐惧时移动
                                    if (isFearDone)
                                    {
                                        //设置方向
                                        MoveDirector = -MoveDirector * 2;
                                    }
                                    //不恐惧时移动并释放激光
                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE)
                                    {
                                        //移动
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha,       //方向*速度
                                            ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x, //最小值
                                            ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x),//最大值
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha,        //方向*速度 
                                            ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y,  //最小值
                                            ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y));//最大值

                                        //激光
                                        if (!isFearDone && BeamTimer_Single <= TIME_BEAM - DELAY_TIME_BEAM && NowLunchIceBeam == null)
                                        {
                                            LunchBeam();
                                        }
                                    }
                                }
                                else
                                {
                                    BeamOver_Single();
                                    ShakeStart_Single(0);
                                    StopBeam();
                                }


                                break;
                            //摇晃
                            case SingleState.Shake:
                                break;
                        }
                    }
                    break;
                case HavePartnerState.Partner:
                    break;
                case HavePartnerState.Father:
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
            EmptyBeKnock();//判定是否被击退
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
    /// 设置多多冰的动画机方向
    /// </summary>
    public void SetDirector(Vector2 director)
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
                (havePartnerState == HavePartnerState.No && singleState == SingleState.Beam)
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

    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■



















    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■无伙伴■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 子敌人列表
    /// </summary>
    Empty SonList = new Empty { };

    //========================发呆=================================

    //开始后的冷却时间
    static float TIME_START_SINGLE = 0.5f;
    //冲刺后的冷却时间
    static float TIME_AFTER_Shake_SINGLE = 3.5f;

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
        havePartnerState = HavePartnerState.No;
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










    //========================射激光=================================
    
    //发射激光的延迟
    static float DELAY_TIME_BEAM = 0.45f;

    //发射激光的时间
    static float TIME_BEAM = 13.00f;

    //停止移动的距离
    static float MOVE_STOP_DISTENCE = 3.8f;

    /// <summary>
    /// 激光
    /// </summary>
    public VanillishIceBeam IceBeam;

    /// <summary>
    /// 当前实例化发射的激光
    /// </summary>
    public VanillishIceBeam NowLunchIceBeam;

    /// <summary>
    /// 射激光计时器
    /// </summary>
    float BeamTimer_Single = 0;

    /// <summary>
    /// 激光发射初始角度
    /// </summary>
    float BeamLunchRotation;

    /// <summary>
    /// 射激光开始
    /// </summary>
    public void BeamStart_Single(float beamTimer)
    {
        BeamTimer_Single = beamTimer;
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Beam;
        BeamLunchRotation = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
    }

    void LunchBeam()
    {
        NowLunchIceBeam = Instantiate(IceBeam , transform.position , Quaternion.Euler(0,0,BeamLunchRotation) , transform);
        NowLunchIceBeam.ParentVanillish = this;
    }

    void StopBeam()
    {
        NowLunchIceBeam.StopBeam();
        NowLunchIceBeam = null;
    }

    /// <summary>
    /// 射激光结束
    /// </summary>
    public void BeamOver_Single()
    {
        BeamTimer_Single = 0.0f;
    }
    //========================射激光=================================












    //========================摇晃=================================

    /// <summary>
    /// 摇晃计时器
    /// </summary>
    float ShakeTimer_Single = 0;

    /// <summary>
    /// 摇晃开始
    /// </summary>
    public void ShakeStart_Single(float Timer)
    {
        animator.SetTrigger("Shake");
        ShakeTimer_Single = Timer;
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Shake;
    }

    /// <summary>
    /// 摇晃结束
    /// </summary>
    public void ShakeOver_Single()
    {
        ShakeTimer_Single = 0.0f;
        IdleStart_Single(TIME_AFTER_Shake_SINGLE);
    }
    //========================摇晃=================================

    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■无伙伴■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■




















    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■有伙伴■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■有伙伴■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
























    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■有父辈■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■有父辈■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
}
