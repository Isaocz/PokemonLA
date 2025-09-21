using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//多多冰-》有多多冰伙伴
//      -》无多多冰伙伴 --》冰光-》摇晃（有从属迷你冰发射 无从属迷你冰冰雾）


public class Vanillish : Empty
{
    //是否有多多冰伙伴
    public enum HavePartnerState
    {
        No,     //没有伙伴
        Partner,   //有伙伴
        Father  //有双倍多多冰父
    }
    public HavePartnerState havePartnerState;



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
        Idle,  //发呆
        Beam,  //射激光
        Shake, //摇晃
    }
    PartnerState partnerState;



    //有父辈时的状态
    public enum FatherState
    {
        Idle,     //发呆
        Back,     //返回母体
        Surround, //环绕
    }
    public FatherState fatherState;



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
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Idle;
        IdleStart_Single(TIME_START_SINGLE);



        //初始化方向
        Director = new Vector2(-1, -1);
        SetDirector(Director);
        //设置搜索状态为真
        //NeedSearchParent = true;
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
                        animator.ResetTrigger("Sleep");
                        switch (singleState)
                        {
                            //发呆
                            case SingleState.Idle:
                                IdleTimer_Single -= Time.deltaTime;
                                //发呆结束后或恐惧时转进移动状态
                                if (IdleTimer_Single <= 0)
                                {
                                    IdleOver_Single();
                                    if (havePartnerState == HavePartnerState.No) { BeamStart_Single(TIME_BEAM); }
                                }
                                break;                      
                            //激光
                            case SingleState.Beam:

                                if (!isFearDone) {
                                    BeamTimer_Single -= Time.deltaTime;
                                }
                                //Debug.Log(BeamTimer_Single);
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
                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE_SINGLE || isFearDone)
                                    {
                                        //移动
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha,       //方向*速度
                                            ParentPokemonRoom.RoomSize[2] + 3.0f + transform.parent.position.x, //最小值
                                            ParentPokemonRoom.RoomSize[3] - 3.0f + transform.parent.position.x),//最大值
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha,        //方向*速度 
                                            ParentPokemonRoom.RoomSize[1] + 3.0f + transform.parent.position.y,  //最小值
                                            ParentPokemonRoom.RoomSize[0] - 3.0f + transform.parent.position.y));//最大值
                                    }
                                    //激光
                                    if (!isFearDone && BeamTimer_Single <= TIME_BEAM - DELAY_TIME_BEAM && NowLunchIceBeam == null)
                                    {
                                        LunchBeam();
                                    }
                                    if (isFearDone && NowLunchIceBeam != null)
                                    {
                                        StopBeam();
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
                                if (isFearDone)
                                {
                                    ShakeOver();
                                    IdleStart_Single(TIME_START_SINGLE);
                                }
                                break;
                        }
                        //排序迷你冰
                        SortChildren();
                    }
                    if (((isSleepDone) && singleState != SingleState.Idle))
                    {
                        animator.SetTrigger("Sleep");
                        if (singleState == SingleState.Beam)
                        {
                            BeamOver_Single();
                            StopBeam();
                        }
                        else if (singleState == SingleState.Shake)
                        {
                            ShakeOver();
                        }
                        IdleStart_Single(TIME_START_SINGLE);
                    }
                    break;
                //有伙伴
                case HavePartnerState.Partner:
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        switch (partnerState)
                        {
                            //发呆
                            case PartnerState.Idle:
                                IdleTimer_Partner -= Time.deltaTime;
                                //发呆结束后或恐惧时转进移动状态
                                if (IdleTimer_Partner <= 0)
                                {
                                    IdleOver_Partner();
                                    BeamStart_Partner((360.0f/BEAM_ROTATION_SPEED_PARTNER) * 2.0f);
                                }
                                break;
                            //激光
                            case PartnerState.Beam:
                                BeamTimer_Partner -= Time.deltaTime;
                                if (BeamTimer_Partner >= 0)
                                {
                                    //设置方向
                                    Vector2 MoveDirector = (TargetPosition - (Vector2)transform.position).normalized;
                                    //恐惧时移动
                                    if (isFearDone)
                                    {
                                        //设置方向
                                        MoveDirector = MoveDirector * 2;
                                    }
                                    //不恐惧时移动并释放激光
                                    if (rigidbody2D != null && Vector2.Distance(TargetPosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE_PARTNER)
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
                                    BeamOver_Partner();
                                    ShakeStart_Partner(0);
                                    StopBeam();
                                }
                                break;
                        }
                    }
                    if ((isSleepDone) && partnerState != PartnerState.Idle)
                    {
                        animator.SetTrigger("Sleep");
                        switch (partnerState)
                        {
                            case PartnerState.Idle:
                                IdleOver_Partner();
                                break;
                            case PartnerState.Beam:
                                BeamOver_Partner();
                                break;
                            case PartnerState.Shake:
                                ShakeOver();
                                break;
                        }
                        IdleStart_Partner(TIME_START_PARTNER);
                        isReverseBeam = !isReverseBeam;
                    }
                    //排序迷你冰
                    SortChildren();
                    if (partnerEmpty == null || positionInPartnership == PositionInPartnershipEnum.NoPartner || isFearDone)
                    {
                        animator.SetTrigger("Sleep");
                        switch (partnerState)
                        {
                            case PartnerState.Idle:
                                IdleOver_Partner();
                                break;
                            case PartnerState.Beam:
                                BeamOver_Partner();
                                break;
                            case PartnerState.Shake:
                                ShakeOver();
                                break;
                        }
                        RemovePartnership();
                        partnerEmpty = null;
                        positionInPartnership = PositionInPartnershipEnum.NoPartner;
                        IdleStart_Single(TIME_START_SINGLE);
                    }
                    break;
                case HavePartnerState.Father:
                    if (ParentEmptyByChild == null)
                    {
                        SearchVanillishParentOrPartner();
                    }
                    else
                    {
                        if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                        {
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
                                    if (Vector2.Distance(ParentPosition, (Vector2)transform.position) > SURROUND_DISTENCE_Father)
                                    {
                                        //设置方向
                                        Vector2 MoveDirector = (ParentPosition - (Vector2)transform.position).normalized;
                                        //不恐惧时移动
                                        if (!isFearDone)
                                        {
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
                                    }
                                    //小于等于时进入迅游状态
                                    else
                                    {
                                        BackOver_Father();
                                        SurroundStart_Father();
                                    }
                                    break;
                                case FatherState.Surround:
                                    //环绕
                                    Surround(surroundRotationSpeed);
                                    //激光
                                    if (!isFearDone && NowLunchIceBeam == null)
                                    {
                                        LunchBeam();
                                    }
                                    break;

                            }
                            //排序迷你冰
                            SortChildren();
                        }
                        if (isSleepDone || isFearDone || isEmptyInfatuationDone)
                        {
                            animator.SetTrigger("Sleep");
                            switch (fatherState)
                            {
                                case FatherState.Idle:
                                    IdleOver_Father();
                                    break;
                                case FatherState.Back:
                                    BackOver_Father();
                                    break;
                                case FatherState.Surround:
                                    SurroundOver_Father();
                                    break;
                            }
                            IdleStart_Single(0.0f);
                        }
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
            EmptyBeKnock();//判定是否被击退

            //Debug.Log(NeedSearchParent);
            if (NeedSearchParent)
            {
                NeedSearchParent = false;
                SearchVanillishParentOrPartner();
            }
            
            //有伙伴时确定目标位置为房间的一侧
            if (PartnerEmpty != null && positionInPartnership != PositionInPartnershipEnum.NoPartner)
            {
                Vector2 U = new Vector2(0, ParentPokemonRoom.RoomSize[0] * 0.5f);
                Vector2 D = new Vector2(0, ParentPokemonRoom.RoomSize[1] * 0.5f);
                Vector2 L = new Vector2(ParentPokemonRoom.RoomSize[2] * 0.5f, 0);
                Vector2 R = new Vector2(ParentPokemonRoom.RoomSize[3] * 0.5f, 0);
                //确认位置Index
                //大哥确认位置
                if (positionInPartnership != PositionInPartnershipEnum.BigBrother)
                {
                    //房间纵轴比横轴长
                    if ((ParentPokemonRoom.RoomSize[0]- ParentPokemonRoom.RoomSize[1]) > (ParentPokemonRoom.RoomSize[3] - ParentPokemonRoom.RoomSize[2]) ) 
                    {
                        if (Vector2.Distance((Vector2)ParentPokemonRoom.transform.position+U , (Vector2)transform.position) < Vector2.Distance((Vector2)ParentPokemonRoom.transform.position + D, (Vector2)transform.position))
                        {
                            TargetPositionIndex_Partner = 0;
                        }
                        else
                        {
                            TargetPositionIndex_Partner = 1;
                        }
                    }
                    else
                    {
                        if (Vector2.Distance((Vector2)ParentPokemonRoom.transform.position + L, (Vector2)transform.position) < Vector2.Distance((Vector2)ParentPokemonRoom.transform.position + R, (Vector2)transform.position))
                        {
                            TargetPositionIndex_Partner = 2;
                        }
                        else
                        {
                            TargetPositionIndex_Partner = 3;
                        }
                    }
                }
                //小弟被动确认位置
                else
                {
                    switch (partnerEmpty.GetComponent<Vanillish>().TargetPositionIndex_Partner)
                    {
                        case 0: TargetPositionIndex_Partner = 1; break;
                        case 1: TargetPositionIndex_Partner = 0; break;
                        case 2: TargetPositionIndex_Partner = 3; break;
                        case 3: TargetPositionIndex_Partner = 2; break;
                    }
                }
                switch (TargetPositionIndex_Partner)
                {
                    case 0: TargetPosition = (Vector2)ParentPokemonRoom.transform.position + U; break;
                    case 1: TargetPosition = (Vector2)ParentPokemonRoom.transform.position + D; break;
                    case 2: TargetPosition = (Vector2)ParentPokemonRoom.transform.position + L; break;
                    case 3: TargetPosition = (Vector2)ParentPokemonRoom.transform.position + R; break;
                }
            }
            else
            {
                //根据魅惑情况确实目标位置
                if (!isEmptyInfatuationDone || _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile()).Count <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }

            }
            //Debug.Log(this.name + "+" + TargetPosition + "+" + TargetPositionIndex_Partner);

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
    /// 子迷你冰
    /// </summary>
    public Vanillite ChildVanillite;

    /// <summary>
    /// 设置多多冰的动画机方向
    /// </summary>
    public void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    public override void DieEvent()
    {
        //清楚激光
        if (NowLunchIceBeam != null)
        {
            if (NowLunchIceBeam.transform.parent != null) { NowLunchIceBeam.transform.parent = null; }
            NowLunchIceBeam.StopBeam();
        }
        Instantiate(ChildVanillite, transform.position + Vector3.right, Quaternion.identity, ParentPokemonRoom.EmptyFile());
        Instantiate(ChildVanillite, transform.position - Vector3.right, Quaternion.identity, ParentPokemonRoom.EmptyFile());
        ParentPokemonRoom.isClear += 2;
        base.DieEvent();
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


    /// <summary>
    /// 是否需要寻找父辈
    /// </summary>
    bool NeedSearchParent;

    /// <summary>
    /// 搜索伙伴或者父辈
    /// </summary>
    public HavePartnerState SearchVanillishParentOrPartner()
    {
        if (!isDie && !isBorn && !isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone && !isEmptyInfatuationDone)
        {
            Vaniluxe father = SearchParentByDistence<Vaniluxe>();
            //有父
            if (father != null)
            {
                //Debug.Log("Father");
                ChildBackHome(father);//回父亲家
                havePartnerState = HavePartnerState.Father;
                fatherState = FatherState.Idle;
                IdleStart_Father(TIME_START_FATHER);
                return HavePartnerState.Father;
            }
            else
            {
                ChildLeaveHome();//保持单身
                SearchPartnerInRoomByDistence<Vanillish>();
                //有伙伴
                Debug.Log(name + "Partner" + PartnerEmpty + PositionInPartnership);
                if (PartnerEmpty != null && positionInPartnership != PositionInPartnershipEnum.NoPartner)
                {
                    havePartnerState = HavePartnerState.Partner;
                    partnerState = PartnerState.Idle;
                    IdleStart_Partner(TIME_START_PARTNER);
                    if (PartnerEmpty.PartnerEmpty == null || PartnerEmpty.positionInPartnership == PositionInPartnershipEnum.NoPartner || havePartnerState != HavePartnerState.Partner ) 
                    {
                        PartnerEmpty.GetComponent<Vanillish>().SearchVanillishParentOrPartner();
                    }
                    return HavePartnerState.Partner;
                }
                //无伙伴
                else
                {
                    //Debug.Log("Single");
                    havePartnerState = HavePartnerState.No;
                    singleState = SingleState.Idle;
                    IdleStart_Single(TIME_START_SINGLE);
                    return HavePartnerState.No;
                }
            }
        }
        return HavePartnerState.No;
    }



    /// <summary>
    /// 动画调用的摇晃结束
    /// </summary>
    public void ShakeOver()
    {
        //Debug.Log(havePartnerState);
        switch (havePartnerState)
        {
            case HavePartnerState.No:
                ShakeOver_Single();
                break;
            case HavePartnerState.Partner:
                ShakeOver_Partner();
                break;
            case HavePartnerState.Father:
                //TODO
                break;

        }
    }


    //复写出家
    public override void ChildLeaveHome()
    {
        //恢复碰撞体
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.ResetOneChildCollision(this);
        }
        if (ParentEmptyByChild != null && ParentEmptyByChild.ChildrenList.Contains(this))
        {
            ParentEmptyByChild.ChildrenList.Remove(this);
        }
        //设定父对象和家
        if (ParentEmptyByChild != null)
        {
            //transform.parent = ParentPokemonRoom.EmptyFile();
            ParentEmptyByChild = null;
        }
    }

    //复写回家
    public override void ChildBackHome(Empty parent)
    {
        //设定父对象和家
        ParentEmptyByChild = parent;
        //transform.parent = ParentEmptyByChild.ChildHome;
        if (ParentEmptyByChild != null && !ParentEmptyByChild.ChildrenList.Contains(this))
        {
            ParentEmptyByChild.ChildrenList.Add(this);
        }
        //Debug.Log(ParentEmptyByChild);
        //忽略碰撞体
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.IgnoreOneChildCollision(this);
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
    static float TIME_AFTER_SHAKE_SINGLE = 3.5f;

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
        SearchVanillishParentOrPartner();
    }
    //========================发呆=================================










    //========================射激光=================================

    //无伙伴模式的激光角速度
    public static float BEAM_ROTATION_SPEED_SINGLE = 15.0f;

    //发射激光的延迟
    static float DELAY_TIME_BEAM = 0.45f;

    //发射激光的时间
    static float TIME_BEAM = 15.00f;

    //停止移动的距离
    static float MOVE_STOP_DISTENCE_SINGLE = 3.8f;

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
        //Debug.Log("Lunch");
        NowLunchIceBeam = Instantiate(IceBeam , transform.position , Quaternion.Euler(0,0,BeamLunchRotation) , transform);
        NowLunchIceBeam.ParentVanillish = this;
    }

    void StopBeam()
    {
        if (NowLunchIceBeam != null) {
            NowLunchIceBeam.StopBeam();
            NowLunchIceBeam = null;
        }
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
    /// 细雪
    /// </summary>
    public VanillishPodesSnow PodesSnow;

    /// <summary>
    /// 摇晃开始
    /// </summary>
    public void ShakeStart_Single(float _Timer)
    {
        animator.SetTrigger("Shake");
        ShakeTimer_Single = _Timer;
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Shake;
        LunchIceMist();
        Timer.Start(this, 0.4f, () => { LunchVanillite(); });
        
    }

    void LunchIceMist()
    {
        if (!isFearDone)
        {
            VanillishPodesSnow ps = Instantiate(PodesSnow, transform.position + Vector3.up, Quaternion.identity);
            ps.empty = this;
        }
    }

    void LunchVanillite()
    {
        if (!isFearDone)
        {
            if (ChildrenList.Count != 0)
            {
                for(int i = 0; i < ChildrenList.Count;i++)
                {
                    Vanillite BabyV = ChildrenList[i].GetComponent<Vanillite>();
                    if (BabyV != null && BabyV.FamilyState == Vanillite.VanilaFamilyState.Father && BabyV.fatherState == Vanillite.FatherState.Surround)
                    {
                        BabyV.SurroundOver_Father();
                        BabyV.LunchStart_Father(((Vector2)(BabyV.transform.position - transform.position)).normalized);
                        ResetOneChildCollision(BabyV);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 摇晃结束
    /// </summary>
    public void ShakeOver_Single()
    {
        ShakeTimer_Single = 0.0f;
        IdleStart_Single(TIME_AFTER_SHAKE_SINGLE);
    }
    //========================摇晃=================================
















    //========================子迷你冰环绕=================================

    void SortChildren()
    {
        if (ChildrenList.Count != 0)
        {
            //夹角列表
            List<Vector2> AngleList = new List<Vector2> { };
            //正在环绕的子对象的序列
            List<Vanillite> babyVList = new List<Vanillite> { };

            //遍历所有迷你冰 确认是否环绕
            for (int i = 0; i < ChildrenList.Count; i++)
            {
                Vanillite babyV = ChildrenList[i].GetComponent<Vanillite>();
                if (babyV != null && babyV.FamilyState == Vanillite.VanilaFamilyState.Father && babyV.fatherState == Vanillite.FatherState.Surround)
                {
                    babyVList.Add(babyV);
                }
            }

            //遍历所有环绕的迷你冰 获得迷你冰夹角
            for (int i = 0; i < babyVList.Count; i++)
            {
                float angle = _mTool.Angle_360Y(((Vector2)babyVList[i].transform.position - (Vector2)transform.position).normalized, Vector2.right);
                AngleList.Add(new Vector2(angle, i));
            }

            AngleList.Sort((a, b) => a.x.CompareTo(b.x));
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
                    NextAngle = AngleList[i+1].x - AngleList[i].x;
                }
                float CountRound = 360.0f / ((float)AngleList.Count);
                if      (NextAngle < CountRound) { babyVList[(int)AngleList[i].y].SurroundRotationSpeed = 20.0f - (Mathf.Abs(NextAngle-CountRound) / CountRound) * 20.0f; }
                else if (NextAngle > CountRound) { babyVList[(int)AngleList[i].y].SurroundRotationSpeed = 20.0f + (Mathf.Abs(NextAngle - CountRound) / CountRound) * 60.0f; }
                else                                                    { babyVList[(int)AngleList[i].y].SurroundRotationSpeed = 20.0f; }
                babyVList[(int)AngleList[i].y].SurroundRotationSpeed *= (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
            }
        }
    }

    //========================子迷你冰环绕=================================







    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■无伙伴■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■




















    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■有伙伴■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    //========================发呆=================================

    //开始后的冷却时间
    static float TIME_START_PARTNER = 0.5f;
    //冲刺后的冷却时间
    static float TIME_AFTER_SHAKE_PARTNER = 5.5f;

    /// <summary>
    /// 发呆计时器
    /// </summary>
    float IdleTimer_Partner = 0;

    /// <summary>
    /// 发呆开始
    /// </summary>
    public void IdleStart_Partner(float Timer)
    {
        IdleTimer_Partner = Timer;
        havePartnerState = HavePartnerState.Partner;
        partnerState = PartnerState.Idle;
    }

    /// <summary>
    /// 发呆结束
    /// </summary>
    public void IdleOver_Partner()
    {
        IdleTimer_Partner = 0.0f;
    }
    //========================发呆=================================










    //========================射激光=================================


    //伙伴模式的激光角速度
    public static float BEAM_ROTATION_SPEED_PARTNER = 20.0f;

    //停止移动的距离
    static float MOVE_STOP_DISTENCE_PARTNER = 0.2f;

    /// <summary>
    /// 射激光计时器
    /// </summary>
    float BeamTimer_Partner = 0;

    /// <summary>
    /// 激光发射初始角度
    /// </summary>
    float BeamLunchRotation_Partner;

    /// <summary>
    /// 发射激光所处的位置的序列号 0上 1下 2左 3右
    /// </summary>
    int TargetPositionIndex_Partner;

    /// <summary>
    /// 是否反转激光旋转方向和速度
    /// </summary>
    public bool isReverseBeam;


    void SetBeamLunchRotation()
    {
        switch (TargetPositionIndex_Partner)
        {
            case 0:
                BeamLunchRotation = 110.0f;
                break;
            case 1:
                BeamLunchRotation = 290.0f;
                break;
            case 2:
                BeamLunchRotation = 200.0f;
                break;
            case 3:
                BeamLunchRotation = 20.0f;
                break;
        }
    }

    /// <summary>
    /// 射激光开始
    /// </summary>
    public void BeamStart_Partner(float beamTime)
    {
        BeamTimer_Partner = beamTime;
        havePartnerState = HavePartnerState.Partner;
        partnerState = PartnerState.Beam;
        BeamLunchRotation = 0;
    }


    /// <summary>
    /// 射激光结束
    /// </summary>
    public void BeamOver_Partner()
    {
        BeamTimer_Partner = 0.0f;
        //反转激光方向和速度
        isReverseBeam = !isReverseBeam;
        StopBeam();
    }
    //========================射激光=================================















    //========================摇晃=================================

    /// <summary>
    /// 摇晃计时器
    /// </summary>
    float ShakeTimer_Partner = 0;

    /// <summary>
    /// 摇晃开始
    /// </summary>
    public void ShakeStart_Partner(float _Timer)
    {
        animator.SetTrigger("Shake");
        ShakeTimer_Partner = _Timer;
        havePartnerState = HavePartnerState.Partner;
        partnerState = PartnerState.Shake;
        LunchIceMist();
        Timer.Start(this, 0.4f, () => { LunchVanillite(); });

    }

    /// <summary>
    /// 摇晃结束
    /// </summary>
    public void ShakeOver_Partner()
    {
        ShakeTimer_Partner = 0.0f;
        //Debug.Log(TIME_AFTER_SHAKE_PARTNER);
        IdleStart_Partner(TIME_AFTER_SHAKE_PARTNER);
    }
    //========================摇晃=================================

    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■有伙伴■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
























    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■有父辈■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    //========================发呆=================================

    //开始后的冷却时间
    static float TIME_START_FATHER = 0.2f;

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
        havePartnerState = HavePartnerState.Father;
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
        havePartnerState = HavePartnerState.Father;
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
    static float SURROUND_DISTENCE_Father = 3.5f;


    /// <summary>
    /// 环绕角速度
    /// </summary>
    public float SurroundRotationSpeed
    {
        get { return surroundRotationSpeed; }
        set { surroundRotationSpeed = value; }
    }
    float surroundRotationSpeed = 20.0f;


    /// <summary>
    /// 环绕开始
    /// </summary>
    public void SurroundStart_Father()
    {
        havePartnerState = HavePartnerState.Father;
        fatherState = FatherState.Surround;
        //静止刚体
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        //设定父对象和家
        transform.parent = ParentEmptyByChild.ChildHome;
        BeamLunchRotation = _mTool.Angle_360Y(((Vector2)transform.position - (Vector2)ParentEmptyByChild.transform.position).normalized, Vector2.right);
    }

    public void Surround(float RotationSpeed)
    {
        RotationSpeed *= isEmptyConfusionDone ? 0.5f : 1.0f;
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
                       + Mathf.Cos(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE_Father,       //方向*速度
                   ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //最小值
                   ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//最大值
                   Mathf.Clamp(ParentPosition.y
                       + Mathf.Sin(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE_Father,        //方向*速度 
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
        if (ParentEmptyByChild != null)
        {
            transform.parent = ParentPokemonRoom.EmptyFile();
        }
        //恢复刚体
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        StopBeam();
    }
    //========================环绕=================================


    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■有父辈■■■■■■■■■■■■■■■■■■■■■■
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
}
