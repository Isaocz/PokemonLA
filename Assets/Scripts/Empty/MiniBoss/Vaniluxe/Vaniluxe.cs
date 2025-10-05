using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaniluxe : Empty
{
    //状态机
    enum State
    {
        Idle,    //发呆状态
        Normal,  //一般状态
    }
    State NowState;


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
    /// 双倍多多冰的中心点，偏移后会返回
    /// </summary>
    Vector2 HomePosition;

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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.2f;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.2f;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //目标位置为出生点
        HomePosition = transform.position;

        //初始发呆
        IdleStart(TIME_START);



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





            //特性雪隐 移速增加
            float WeatherSpeedAlpha = (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
            {
                switch (NowState)
                {
                    //发呆
                    case State.Idle:
                        IdleTimer -= Time.deltaTime;
                        //发呆结束后或恐惧时转进移动状态
                        if (IdleTimer <= 0)
                        {
                            IdleOver();
                            NormalStart(180.0f / BEAM_ROTATION_SPEED);
                        }
                        break;
                    //激光
                    case State.Normal:
                        NormalTimer -= Time.deltaTime * WeatherSpeedAlpha;
                        if (NormalTimer >= 0)
                        {
                            //设置方向
                            Vector2 MoveDirector = (HomePosition - (Vector2)transform.position).normalized;
                            //恐惧时移动
                            if (isFearDone)
                            {
                                //设置方向
                                MoveDirector = MoveDirector * 2;
                            }
                            //不恐惧时移动并释放激光
                            if (Vector2.Distance(HomePosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE)
                            {
                                //移动
                                rigidbody2D.position = new Vector2(
                                    Mathf.Clamp(rigidbody2D.position.x
                                        + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha * 2.0f,       //方向*速度
                                    ParentPokemonRoom.RoomSize[2] + 3.0f + transform.parent.position.x, //最小值
                                    ParentPokemonRoom.RoomSize[3] - 3.0f + transform.parent.position.x),//最大值
                                    Mathf.Clamp(rigidbody2D.position.y
                                        + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha * 2.0f,        //方向*速度 
                                    ParentPokemonRoom.RoomSize[1] + 3.0f + transform.parent.position.y,  //最小值
                                    ParentPokemonRoom.RoomSize[0] - 3.0f + transform.parent.position.y));//最大值
                            }

                            //激光
                            if (!isFearDone && NowLunchIceBeam == null)
                            {
                                SetBeamLunchRotation();
                                LunchBeam();
                            }
                        }
                        else
                        {
                            NormalOver();
                            IdleStart(TIME_IDLE_AFTER_NORMAL);
                            StopBeam();
                        }
                        break;
                }
                //排序多多冰迷你冰

            }
            if ((isSleepDone || isFearDone) && NowState != State.Idle)
            {
                NormalOver();
                IdleStart(TIME_START);
                StopBeam();
            }
            SortChildren();
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//如果玩家组件丢失，重新获取
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//如果被魅惑，计算魅惑时间
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            EmptyBeKnockByTransform();//判定是否被击退


            //根据魅惑情况确实目标位置
            if (!isEmptyInfatuationDone || _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile()).Count <= 1 || InfatuationForDistanceEmpty() == null)
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

    public Vanillish ChildVanillish;

    /// <summary>
    /// 设置双倍多多冰的动画机方向
    /// </summary>
    public void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    public override void DieEvent()
    {
        //清除激光
        if (NowLunchIceBeam != null)
        {
            NowLunchIceBeam.StopBeam();
            if (NowLunchIceBeam.transform.parent != null) { NowLunchIceBeam.transform.parent = null; }
        }
        Instantiate(ChildVanillish, transform.position + Vector3.right, Quaternion.identity, ParentPokemonRoom.EmptyFile());
        Instantiate(ChildVanillish, transform.position - Vector3.right, Quaternion.identity, ParentPokemonRoom.EmptyFile());
        ParentPokemonRoom.isClear += 2;
        base.DieEvent();
    }




    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■






















    //■■■■■■■■■■■■■■■■■■■■状态■■■■■■■■■■■■■■■■■■■■■■

    //========================发呆=================================

    //开始后的冷却时间
    static float TIME_START = 0.5f;
    //一般状态后的冷却时间
    static float TIME_IDLE_AFTER_NORMAL = 5.0f;

    /// <summary>
    /// 发呆计时器
    /// </summary>
    float IdleTimer = 0;

    /// <summary>
    /// 发呆开始
    /// </summary>
    public void IdleStart(float Timer)
    {
        IdleTimer = Timer;
        NowState = State.Idle;
    }

    /// <summary>
    /// 发呆结束
    /// </summary>
    public void IdleOver()
    {
        IdleTimer = 0.0f;
    }
    //========================发呆=================================










    //========================一般状态=================================

    //停止移动的距离
    static float MOVE_STOP_DISTENCE = 0.3f;

    //激光的旋转角速度
    public static float BEAM_ROTATION_SPEED = 48.0f;

    //冰柱的释放间隔
    static float ICICLE_CRASH_COUNT = 2;

    //冰光
    public VaniluxeIceBeam IceBeam;

    //冰柱
    public VaniluxeIcicleCrash IcicleCrash;

    //玩家最后的位置
    Vector2 LastTargetPosition;

    //正在发射的冰光
    public VaniluxeIceBeam NowLunchIceBeam
    {
        get { return nowLunchIceBeam; }
        set { nowLunchIceBeam = value; }
    }
    VaniluxeIceBeam nowLunchIceBeam;

    /// <summary>
    /// 一般状态计时器
    /// </summary>
    float NormalTimer = 0;

    /// <summary>
    /// 激光发射初始角度
    /// </summary>
    float BeamLunchRotation;

    /// <summary>
    /// 是否反转激光旋转方向
    /// </summary>
    public bool isReverseBeam;




    /// <summary>
    /// 移动开始
    /// </summary>
    public void NormalStart(float Timer)
    {
        NormalTimer = Timer;
        NowState = State.Normal;
        StartCoroutine(StartLunchIcicelCrash());
    }

    /// <summary>
    /// 移动结束
    /// </summary>
    public void NormalOver()
    {
        NormalTimer = 0.0f;
        isReverseBeam = !isReverseBeam;
        StopCoroutine(StartLunchIcicelCrash());
    }

    /// <summary>
    /// 发射激光
    /// </summary>
    void LunchBeam()
    {
        NowLunchIceBeam = Instantiate(IceBeam, transform.position, Quaternion.Euler(0, 0, BeamLunchRotation), transform);
        NowLunchIceBeam.ParentVaniluex = this;
    }


    /// <summary>
    /// 停止激光
    /// </summary>
    void StopBeam()
    {
        if (NowLunchIceBeam != null)
        {
            NowLunchIceBeam.StopBeam();
            NowLunchIceBeam = null;
        }
    }

    void SetBeamLunchRotation()
    {
        BeamLunchRotation = isReverseBeam ? 90 : 270;
    }


    void LunchIcicelCrash(Vector2 targetPosition)
    {
        VaniluxeIcicleCrash ic = Instantiate(IcicleCrash , targetPosition, Quaternion.identity);
        ic.empty = this;
    }

    /// <summary>
    /// 发射冰柱
    /// </summary>
    /// <returns></returns>
    IEnumerator StartLunchIcicelCrash()
    {

        Debug.Log(isSleepDone);
        float cd = (180.0f / BEAM_ROTATION_SPEED) / (float)ICICLE_CRASH_COUNT;
        LastTargetPosition = TargetPosition;
        for (int i = 0; i <= ICICLE_CRASH_COUNT; i++)
        {
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone)
            {
                if (isEmptyConfusionDone || isEmptyInfatuationDone)
                {
                    LunchIcicelCrash(TargetPosition);
                }
                else
                {
                    LunchIcicelCrash(PredictTargetPosition(LastTargetPosition, TargetPosition));
                }
            }
            LastTargetPosition = TargetPosition;
            yield return new WaitForSeconds(cd);
        }
    }

    /// <summary>
    /// 预测玩家位置
    /// </summary>
    Vector2 PredictTargetPosition( Vector2 Last , Vector2 Now )
    {
        float distance1 = Vector2.Distance(transform.position, Last);
        float distance2 = Vector2.Distance(transform.position, Now);
        float distance = Mathf.Clamp(((1.8f * distance2) - 0.8f * distance1), 3.0f, 20.0f);
        float Angle1 = _mTool.Angle_360Y(((Vector3)Last - transform.position), Vector3.right);
        float Angle2 = _mTool.Angle_360Y(((Vector3)Now - transform.position), Vector3.right);
        float Angle = (1.8f * Angle2) - 0.8f * Angle1;
        //Debug.Log(distance1 +"+"+ distance2 + "+" + distance + "+" + Angle1 + "+" + Angle2 + "+" + Angle);
        Vector2 output = (transform.position + Quaternion.AngleAxis(Angle, Vector3.forward) * Vector2.right * distance);
        output = new Vector2(Mathf.Clamp(output.x, ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[2], ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[3]),
                            (Mathf.Clamp(output.y, ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[1], ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[0])));
        return output;
    }

    //========================一般状态=================================













    //========================子迷你冰or多多冰环绕=================================

    void SortChildren()
    {
        if (ChildrenList.Count != 0)
        {
            //夹角列表
            List<Vector2> AngleList = new List<Vector2> { };
            //正在环绕的子对象的序列
            List<Empty> babyVList = new List<Empty> { };



            //遍历所有迷你冰 确认是否环绕
            for (int i = 0; i < ChildrenList.Count; i++)
            {
                Vanillite babyVS = ChildrenList[i].GetComponent<Vanillite>();
                if (babyVS != null && babyVS.FamilyState == Vanillite.VanilaFamilyState.Grandfather && babyVS.grandfatherState == Vanillite.GrandfatherState.Surround)
                {
                    babyVList.Add(babyVS);
                }
                Vanillish babyVM = ChildrenList[i].GetComponent<Vanillish>();
                if (babyVM != null && babyVM.havePartnerState == Vanillish.HavePartnerState.Father && babyVM.fatherState == Vanillish.FatherState.Surround)
                {
                    babyVList.Add(babyVM);
                }
            }

            //遍历所有环绕的迷你冰 获得迷你冰夹角
            for (int i = 0; i < babyVList.Count; i++)
            {
                float angle = _mTool.Angle_360Y(((Vector2)babyVList[i].transform.position - (Vector2)transform.position).normalized, Vector2.right);
                AngleList.Add(new Vector2(angle, i));
            }

            AngleList.Sort((a, b) => a.x.CompareTo(b.x));
            //Debug.Log(string.Join(",", ChildrenList));
            //Debug.Log(string.Join(",", babyVList));
            //Debug.Log(string.Join(",", AngleList));

            //(21.40, 0.00),(54.00, 5.00),(60.55, 4.00),(197.93, 1.00),(248.73, 2.00),(291.30, 3.00)
            for (int i = 0; i < AngleList.Count; i++)
            {
                babyVList[(int)AngleList[i].y].transform.SetSiblingIndex(i);

                float NextAngle = 0.0f;
                if (i >= AngleList.Count - 1)
                {
                    NextAngle = 360.0f + AngleList[0].x - AngleList[i].x;
                }
                else
                {
                    NextAngle = AngleList[i + 1].x - AngleList[i].x;
                }
                float CountRound = 360.0f / ((float)AngleList.Count);
                Vanillite babyVS = babyVList[(int)AngleList[i].y].GetComponent<Vanillite>();
                Vanillish babyVM = babyVList[(int)AngleList[i].y].GetComponent<Vanillish>();
                if (babyVS != null)
                {
                    if (NextAngle < CountRound) { babyVS.SurroundRotationSpeed = 16.0f - (Mathf.Abs(NextAngle - CountRound) / CountRound) * 16.0f; }
                    else if (NextAngle > CountRound) { babyVS.SurroundRotationSpeed = 16.0f + (Mathf.Abs(NextAngle - CountRound) / CountRound) * 60.0f; }
                    else { babyVS.SurroundRotationSpeed = 16.0f; }
                    babyVS.SurroundRotationSpeed *= (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
                }
                else if (babyVM != null)
                {
                    if (NextAngle < CountRound) { babyVM.SurroundRotationSpeed = 16.0f - (Mathf.Abs(NextAngle - CountRound) / CountRound) * 16.0f; }
                    else if (NextAngle > CountRound) { babyVM.SurroundRotationSpeed = 16.0f + (Mathf.Abs(NextAngle - CountRound) / CountRound) * 60.0f; }
                    else { babyVM.SurroundRotationSpeed = 16.0f; }
                    babyVM.SurroundRotationSpeed *= (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
                }
            }
        }
    }

    //========================子迷你冰环绕=================================

    //■■■■■■■■■■■■■■■■■■■■状态■■■■■■■■■■■■■■■■■■■■■■
}
