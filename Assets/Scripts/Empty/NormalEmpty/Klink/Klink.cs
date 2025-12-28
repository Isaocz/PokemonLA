using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Klink : Empty
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
    public enum MainState
    {
        Single,     //单个齿轮 
        WithMGear,  //跟随中齿轮
        WithLGear,  //跟随大齿轮
    }
    public MainState FamilyState;


    /// <summary>
    /// 副状态单个齿轮
    /// </summary>
    public enum SubState
    {
        Idle_Single,         //单个齿轮发呆
        Move_Single,         //单个齿轮移动
        CircleAtk_Single,    //单个齿轮圆圈攻击
        Idle_WithMGear,      //跟随中齿轮发呆
        Surround_WithMGear,  //跟随中齿轮环绕
        Lunched_WithMGear,   //跟随中齿轮被发射
        MGearRushLunched_WithMGear,   //跟随中齿轮中齿轮冲刺后被发射
        Back_WithMGear,      //跟随中齿轮返回
        Idle_WithLGear,      //跟随大齿轮发呆
        Surround_WithLGear,  //跟随大齿轮环绕
        Back_WithLGear,      //跟随大齿轮返回
    }
    public SubState subState;



    /// <summary>
    /// 状态映射关系
    /// </summary>
    private static Dictionary<MainState, SubState[]> StateMap = new()
    {
        { MainState.Single, new[] { SubState.Idle_Single, SubState.Move_Single, SubState.CircleAtk_Single } },
        { MainState.WithMGear, new[] { SubState.Idle_WithMGear, SubState.Surround_WithMGear, SubState.Lunched_WithMGear, SubState.MGearRushLunched_WithMGear, SubState.Back_WithMGear } },
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

        //开局发呆
        Idle_SingleStart(TIME_IDLE_SINGLE_START);
        GetShield(maxHP/5);
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
                //●主状态：【单个齿轮】状态
                case MainState.Single:
                    // 当处于冰冻 睡眠 致盲 麻痹状态时主状态【单个齿轮】停运
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO【单个齿轮】状态停运的额外条件 */
                    {
                        //判断副状态
                        switch (subState)
                        {
                            //【单个齿轮发呆】状态
                            case SubState.Idle_Single:
                                Idle_SingleTimer -= Time.deltaTime;//【单个齿轮发呆】计时器时间减少
                                if (Idle_SingleTimer <= 0 || isFearDone)         //计时器时间到时间，结束【单个齿轮发呆】状态
                                {
                                    Idle_SingleOver();
                                    if (SearchParent() == MainState.Single)
                                    {
                                        Debug.Log("Single");
                                        Idle_SingleOver();
                                        Move_SingleStart();
                                    }
                                    else
                                    {
                                        Debug.Log("Father");
                                    }
                                }
                                break;
                            //【单个齿轮移动】状态
                            case SubState.Move_Single:
                                //移动方向
                                Vector2 dir = Vector2.zero;
                                //距离上下边界的距离
                                float UpBoard = Mathf.Abs((transform.position.y - (ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[0] - DISTENCE_Y_VMOVE)));
                                float DownBoard = Mathf.Abs((transform.position.y - (ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[1] + DISTENCE_Y_VMOVE)));

                                if (MoveSingleMode != MOVE_SINGLE_MODE.VMove)
                                {
                                    //关闭残影
                                    if (ShadowCoroutine != null)
                                    {
                                        StopShadowCoroutine();
                                    }

                                    //一般移动
                                    SpeedAlphaVMove = 1.0f;
                                    if (
                                        (Mathf.Abs((transform.position.y - (ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[0] - DISTENCE_Y_VMOVE))) < 0.05f) ||
                                        (Mathf.Abs((transform.position.y - (ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[1] + DISTENCE_Y_VMOVE))) < 0.05f)
                                        ) {
                                        Move_SingleTimer += Time.deltaTime;//【单个齿轮移动】计时器时间增加

                                    }
                                    //纵向移动，不在房间边缘时前往房间边缘，纵向移动时计时器不增加
                                    else
                                    {
                                        //齿轮距离房间上边界更近
                                        if (Mathf.Abs((ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[0]) - transform.position.y) < Mathf.Abs((ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[1]) - transform.position.y))
                                        {
                                            MoveSingleMode = MOVE_SINGLE_MODE.UpMove;
                                            if (transform.position.y > (ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[0] - DISTENCE_Y_VMOVE)) { dir += Vector2.down * (UpBoard + 0.5f); }
                                            else { dir += Vector2.up * (UpBoard + 0.5f); }
                                        }
                                        //齿轮距离房间下边界更近
                                        else
                                        {
                                            MoveSingleMode = MOVE_SINGLE_MODE.DownMove;
                                            if (transform.position.y > (ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[1] + DISTENCE_Y_VMOVE)) { dir += Vector2.down * (DownBoard + 0.5f); }
                                            else { dir += Vector2.up * (DownBoard + 0.5f); }
                                        }
                                    }

                                    //横向移动折返
                                    if (HMoveDir.x > 0 && ((transform.position.x >= (ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[3] - DISTENCE_Y_VMOVE)))) {
                                        HMoveDir = Vector2.left; isCanVMoveCount++; isCanHTurn = true;
                                    }
                                    if (HMoveDir.x < 0 && ((transform.position.x <= (ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[2] + DISTENCE_Y_VMOVE)))) {
                                        HMoveDir = Vector2.right; isCanVMoveCount++; isCanHTurn = true;
                                    }
                                    dir += HMoveDir;
                                    dir = dir.normalized;


                                    //低速模式时 到达时间后进入高速模式
                                    if (!isHighSpeed && Move_SingleTimer >= TIME_ENTER_HIGHSPEED_MOVE_SINGLE)
                                    {
                                        if (SearchParent() == MainState.Single)
                                        {
                                            HighSpeedModeEnter();
                                        }
                                    }
                                    //高速模式时 到达时间后进入低速模式
                                    if (isHighSpeed && Move_SingleTimer >= TIME_OVER_HIGHSPEED_MOVE_SINGLE)
                                    {
                                        if (SearchParent() == MainState.Single)
                                        {
                                            HighSpeedModeOver();
                                            Move_SingleTimer = 0;
                                        }
                                    }

                                    //在高速模式时检测到玩家进入攻击状态
                                    if (isHighSpeed)
                                    {
                                        if (!isFearDone)
                                        {
                                            //当横向移动到板边不能使用圆圈攻击
                                            if (
                                                transform.position.x >= (ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[3] - RADIUS_CIRCLEATK) ||
                                                transform.position.x <= (ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[2] + RADIUS_CIRCLEATK)
                                                )
                                            {

                                                if (isCanVMoveCount > 1)
                                                {

                                                    RaycastHit2D ray;
                                                    if (MoveSingleMode == MOVE_SINGLE_MODE.UpMove)
                                                    {
                                                        ray = Physics2D.Raycast(transform.position, Vector2.down, 20.0f, LayerMask.GetMask("Room", "Player", "PlayerFly", "PlayerJump", "Empty", "EmptyFly", "EmptyJump"));
                                                    }
                                                    else
                                                    {
                                                        ray = Physics2D.Raycast(transform.position, Vector2.up, 20.0f, LayerMask.GetMask("Room", "Player", "PlayerFly", "PlayerJump", "Empty", "EmptyFly", "EmptyJump"));
                                                    }
                                                    if (ray.collider && ((!isEmptyInfatuationDone && ray.collider.gameObject.tag == "Player") || (isEmptyInfatuationDone && ray.collider.gameObject.tag == "Empty")))
                                                    {
                                                        isCanVMoveCount = 0;
                                                        dir = Vector2.zero;
                                                        if (MoveSingleMode == MOVE_SINGLE_MODE.UpMove) { VMoveDir = Vector2.down; }
                                                        else { VMoveDir = Vector2.up; }
                                                        MoveSingleMode = MOVE_SINGLE_MODE.VMove;
                                                    }
                                                }
                                            }
                                            //当可以使用圆圈攻击
                                            else
                                            {
                                                if (useCircleAtkSingle)
                                                {
                                                    useCircleAtkSingle = false;
                                                    Move_SingleOver();
                                                    CircleAtk_SingleStart(T_CIRCLEATK);
                                                    if (MoveSingleMode == MOVE_SINGLE_MODE.UpMove) { CircleAtkDir.y = -1; }
                                                    else if (MoveSingleMode == MOVE_SINGLE_MODE.DownMove) { CircleAtkDir.y = 1; }
                                                    CircleAtkDir.x = dir.x;
                                                }
                                            }
                                        }
                                    }
                                    if (isFearDone && isCanHTurn)
                                    {
                                        
                                        RaycastHit2D ray;
                                        if (MoveSingleMode == MOVE_SINGLE_MODE.UpMove)
                                        {
                                            ray = Physics2D.Raycast(transform.position, Vector2.down, 20.0f, LayerMask.GetMask("Room", "Player", "PlayerFly", "PlayerJump", "Empty", "EmptyFly", "EmptyJump"));
                                        }
                                        else
                                        {
                                            ray = Physics2D.Raycast(transform.position, Vector2.up, 20.0f, LayerMask.GetMask("Room", "Player", "PlayerFly", "PlayerJump", "Empty", "EmptyFly", "EmptyJump"));
                                        }
                                        if (ray.collider && ((!isEmptyInfatuationDone && ray.collider.gameObject.tag == "Player") || (isEmptyInfatuationDone && ray.collider.gameObject.tag == "Empty")))
                                        {
                                            Debug.Log("aaa");
                                            isCanHTurn = false;
                                            HMoveDir = -HMoveDir;
                                            dir = HMoveDir;
                                        }
                                    }
                                }
                                else
                                {
                                    //确定方向和速度加成
                                    SpeedAlphaVMove = SPEED_ALPHA_VMOVE;
                                    dir = VMoveDir;
                                    //到达位置后停止移动
                                    if (dir.y < 0 && (transform.position.y <= (ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[1] + DISTENCE_Y_VMOVE))) { MoveSingleMode = MOVE_SINGLE_MODE.DownMove; }
                                    if (dir.y > 0 && (transform.position.y >= (ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[0] - DISTENCE_Y_VMOVE))) { MoveSingleMode = MOVE_SINGLE_MODE.UpMove; }
                                    //开启残影
                                    if (ShadowCoroutine == null)
                                    {
                                        StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                                    }
                                }

                                //移动
                                MoveBySpeedAndDir(dir, speed, SpeedAlpha * SpeedAlphaVMove * (isEmptyConfusionDone ? 0.5f :1.0f) , 0.2f, 0.2f, 0.2f, 0.2f);
                                //Move_SingleOver();
                                //CircleAtk_SingleStart();
                                //TODO添加下一个状态的开始方法
                                break;
                            //【单个齿轮圆圈攻击】状态
                            case SubState.CircleAtk_Single:
                                CircleAtk_SingleTimer -= Time.deltaTime;//【单个齿轮圆圈攻击】计时器时间减少
                                //移动
                                MoveBySpeedAndDir(Quaternion.AngleAxis( (-CircleAtkDir.y * CircleAtkDir.x) * (T_CIRCLEATK - CircleAtk_SingleTimer)*Omega_CIRCLEATK * (isEmptyConfusionDone ? 0.5f : 1.0f), Vector3.forward) * ((-CircleAtkDir.x) * Vector2.right), V_CIRCLEATK , 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                                //Debug.Log(CircleAtkDir);
                                if (CircleAtk_SingleTimer <= 0 || isFearDone)         //计时器时间到时间，结束【单个齿轮圆圈攻击】状态
                                {
                                    if (SearchParent() == MainState.Single)
                                    {
                                        CircleAtk_SingleOver();
                                        Idle_SingleStart(TIME_IDLE_SINGLE_START);
                                        //TODO添加下一个状态的开始方法
                                    }
                                }
                                break;
                        }
                    }
                    if ((isSleepDone || isSilence ) && subState != SubState.Idle_Single)
                    {
                        animator.SetTrigger("Sleep");
                        animator.SetBool("HighSpeed", false);
                        switch (subState)
                        {
                            case SubState.Move_Single:
                                Move_SingleOver();
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
                //●主状态：【跟随中齿轮】状态
                case MainState.WithMGear:
                    if (ParentEmptyByChild == null)
                    {
                        //父辈的位置
                        SearchParent();
                    }
                    else {
                        // 当处于冰冻 睡眠 致盲 麻痹状态时主状态【跟随中齿轮】停运
                        if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO【跟随中齿轮】状态停运的额外条件 */
                        {
                            //确定父位置
                            Vector2 ParentPosition = ((Vector2)ParentEmptyByChild.transform.position);

                            //判断副状态
                            switch (subState)
                            {
                                //【跟随中齿轮发呆】状态
                                case SubState.Idle_WithMGear:
                                    Idle_WithMGearTimer -= Time.deltaTime;//【跟随中齿轮发呆】计时器时间减少
                                    if (Idle_WithMGearTimer <= 0)         //计时器时间到时间，结束【跟随中齿轮发呆】状态
                                    {
                                        Idle_WithMGearOver();
                                        Back_WithMGearStart();
                                    }
                                    break;
                                //【跟随中齿轮环绕】状态
                                case SubState.Surround_WithMGear:
                                    //环绕
                                    Surround(surroundRotationSpeed);
                                    break;
                                //【跟随中齿轮被发射】状态
                                case SubState.Lunched_WithMGear:
                                    //Lunched_WithMGearTimer -= Time.deltaTime;//【跟随中齿轮被发射】计时器时间减少
                                    //if (Lunched_WithMGearTimer <= 0)         //计时器时间到时间，结束【跟随中齿轮被发射】状态
                                    //{
                                    //    Lunched_WithMGearOver();
                                    //    //TODO添加下一个状态的开始方法
                                    //}
                                    if (LunchTurnCount >= 3 &&  Vector3.Distance(transform.position , ParentEmptyByChild.transform.position) <= SURROUND_DISTENCE)
                                    {
                                        Debug.Log("Back");
                                        Lunched_WithMGearOver();
                                        Back_WithMGearStart();
                                    }
                                    MoveBySpeedAndDir(LunchDirector, speed, SpeedAlpha * SPEED_LUNCH_ALPHA, 3.4f, 3.4f, 2.1f, 2.1f);
                                    break;
                                //【跟随中齿轮中齿轮冲刺时被发射】状态
                                case SubState.MGearRushLunched_WithMGear:
                                    if(isSpeedDown_MGearRushLunched){
                                        MGearRushLunched_WithMGearTimer -= Time.deltaTime;//【跟随中齿轮中齿轮冲刺时被发射】计时器时间减少
                                    }
                                    if (MGearRushLunched_WithMGearTimer <= 0)         //计时器时间到时间，结束【跟随中齿轮中齿轮冲刺时被发射】状态
                                    {
                                        Lunched_WithMGearOver();
                                        Idle_WithMGearStart(TIME_IDLE_WITHMGEAR_MGEARRUSHLUNCHED_WITHMGEAR);
                                    }
                                    if (MGearRushLunchTurnCount >= 2 )
                                    {
                                        //开始减速
                                        if (animator.GetBool("HighSpeed"))
                                        {
                                            HighSpeedModeOver();
                                            //关闭残影
                                            if (ShadowCoroutine != null)
                                            {
                                                StopShadowCoroutine();
                                            }
                                        }
                                    }
                                    //Debug.Log(name + "+" + MGearRushLunchDirector);
                                    MoveBySpeedAndDir(MGearRushLunchDirector, speed, SpeedAlpha * MGearRushLunchSpeedAlpha, 3.4f, 3.4f, 2.1f, 2.1f);
                                    break;
                                //【跟随中齿轮返回】状态
                                case SubState.Back_WithMGear:
                                    //距离父辈环绕轨道的距离大于一定时接近父辈
                                    if (Mathf.Abs(Vector2.Distance(ParentPosition, (Vector2)transform.position) - SURROUND_DISTENCE) > 0.2f)
                                    {
                                        //设置方向
                                        Vector2 MoveDirector = (ParentPosition - (Vector2)transform.position).normalized;
                                        if (Vector2.Distance(ParentPosition, (Vector2)transform.position) < SURROUND_DISTENCE) { MoveDirector = -MoveDirector; }
                                        //不恐惧时移动
                                        if (!isFearDone)
                                        {
                                            //移动
                                            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                                            //Debug.Log(name + rigidbody2D);
                                            rigidbody2D.position = new Vector2(
                                                Mathf.Clamp(rigidbody2D.position.x
                                                    + (float)MoveDirector.x * Time.deltaTime * speed ,       //方向*速度
                                                ParentPokemonRoom.RoomSize[2] - 2.1f + ParentPokemonRoom.transform.position.x, //最小值
                                                ParentPokemonRoom.RoomSize[3] + 2.1f + ParentPokemonRoom.transform.position.x),//最大值
                                                Mathf.Clamp(rigidbody2D.position.y
                                                    + (float)MoveDirector.y * Time.deltaTime * speed ,        //方向*速度 
                                                ParentPokemonRoom.RoomSize[1] - 3.4f + ParentPokemonRoom.transform.position.y,  //最小值
                                                ParentPokemonRoom.RoomSize[0] + 3.4f + ParentPokemonRoom.transform.position.y));//最大值
                                            Director = _mTool.TiltMainVector2(MoveDirector);
                                            SetDirector(Director);
                                        }
                                    }
                                    //小于等于时进入迅游状态
                                    else
                                    {
                                        Back_WithMGearOver();
                                        Surround_WithMGearStart();
                                    }
                                    break;
                            }
                        }
                        if ((isEmptyFrozenDone || isSleepDone || isSilence) && subState != SubState.Idle_WithMGear )
                        {
                            animator.SetTrigger("Sleep");
                            animator.SetBool("HighSpeed", false);
                            switch (subState)
                            {
                                case SubState.Lunched_WithMGear:
                                    Lunched_WithMGearOver();
                                    break;
                                case SubState.MGearRushLunched_WithMGear:
                                    MGearRushLunched_WithMGearOver();
                                    break;
                                case SubState.Back_WithMGear:
                                    Back_WithMGearOver();
                                    break;
                                case SubState.Surround_WithMGear:
                                    Surround_WithMGearOver();
                                    break;
                            }
                            HighSpeedModeOver();
                            HighSpeedModeOut();
                            Idle_WithMGearStart(TIME_IDLE_WITHMGEAR_START);
                        }
                        //恐惧时进入单人状态
                        if (isFearDone || isEmptyInfatuationDone)
                        {
                            WithM2Single();
                        }
                    }
                    break;
                //●主状态：【跟随大齿轮】状态
                case MainState.WithLGear:
                    if (ParentEmptyByChild == null)
                    {
                        //父辈的位置
                        SearchParent();
                    }
                    else
                    {
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

            //根据目标位置设置方向
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) {
                if (TargetPosition.y > transform.position.y) { SetDirector(new Vector2(1.0f, 1.0f)); }
                else { SetDirector(new Vector2(-1.0f, -1.0f)); }
            }
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

    //碰撞
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
            case MainState.WithMGear:
                if (ParentEmptyByChild == null)
                {
                    //父辈的位置
                    SearchParent();
                }
                else
                {
                    switch (subState)
                    {
                        case SubState.Back_WithMGear:
                            //高速攻击
                            if (isHighSpeed)
                            {
                                HighSpeedTouch(other);
                            }
                            //低速攻击
                            else
                            {
                                NormalTouch(other);
                            }
                            break;
                        case SubState.Idle_WithMGear:
                            //高速攻击
                            if (isHighSpeed)
                            {
                                HighSpeedTouch(other);
                            }
                            //低速攻击
                            else
                            {
                                NormalTouch(other);
                            }
                            break;
                        case SubState.Lunched_WithMGear:
                            CircleAtkTouch(other);
                            break;
                        case SubState.MGearRushLunched_WithMGear:
                            //高速攻击
                            if (isHighSpeed)
                            {
                                CircleAtkTouch(other);
                            }
                            //低速攻击
                            else
                            {
                                NormalTouch(other);
                            }
                            break;
                        case SubState.Surround_WithMGear:
                            //环绕时中齿轮是否冲刺
                            if (surroundWithRushMGear)
                            {
                                Debug.Log("Rush");
                                ParentEmptyByChild.GetComponent<Klang>().CircleAtkTouch(other);
                            }
                            else
                            {
                                Debug.Log("NotRush");
                                //高速攻击
                                if (isHighSpeed)
                                {
                                    HighSpeedTouch(other);
                                }
                                //低速攻击
                                else
                                {
                                    NormalTouch(other);
                                }
                            }
                            break;
                    }
                }
                break;
            case MainState.WithLGear:
                break;
        }

        if (other.transform.tag == ("Empty"))
        {
            HMoveDir = -HMoveDir; isCanVMoveCount++; isCanHTurn = true;
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








    //=================================碰壁反弹=======================================


    private void OnCollisionStay2D(Collision2D other)
    {
        switch (FamilyState)
        {
            case MainState.Single:
                break;
            case MainState.WithMGear:
                if (ParentEmptyByChild == null)
                {
                    //父辈的位置
                    SearchParent();
                }
                else
                {
                    switch (subState)
                    {
                        case SubState.Back_WithMGear:
                            break;
                        case SubState.Idle_WithMGear:
                            break;
                        case SubState.Lunched_WithMGear:
                            if (other.transform.tag == ("Room"))
                            {
                                Rebound_BeLunch();
                            }
                            break;
                        case SubState.Surround_WithMGear:
                            break;
                        case SubState.MGearRushLunched_WithMGear:
                            if (other.transform.tag == ("Room"))
                            {
                                Rebound_BeLunch();
                            }
                            break;
                    }
                }
                break;
            case MainState.WithLGear:
                break;
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
            float[] DistenceList = new float[]{ 0.0f, 0.0f, 0.0f, 0.0f };
            RaycastHit2D RPRay = Physics2D.Raycast(EmptyCenter, Vector2.right, LayerMask.GetMask("Room"));
            RaycastHit2D UPRay = Physics2D.Raycast(EmptyCenter, Vector2.up, LayerMask.GetMask("Room"));
            RaycastHit2D LPRay = Physics2D.Raycast(EmptyCenter, Vector2.left, LayerMask.GetMask("Room"));
            RaycastHit2D DPRay = Physics2D.Raycast(EmptyCenter, Vector2.down, LayerMask.GetMask("Room"));
            if (RPRay) { DistenceList[0] = RPRay.distance;  Debug.DrawLine(EmptyCenter, RPRay.point, Color.red, 0.1f); }
            if (UPRay) { DistenceList[1] = UPRay.distance;  Debug.DrawLine(EmptyCenter, UPRay.point, Color.red, 0.1f); }
            if (LPRay) { DistenceList[2] = LPRay.distance;  Debug.DrawLine(EmptyCenter, LPRay.point, Color.red, 0.1f); }
            if (DPRay) { DistenceList[3] = DPRay.distance;  Debug.DrawLine(EmptyCenter, DPRay.point, Color.red, 0.1f); }
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
            HitVector = _mTool.MainVector2(Quaternion.AngleAxis(MinIndex * 90.0f , Vector3.forward) * HitVector);
        }

        //一般被发射
        if (subState == SubState.Lunched_WithMGear)
        {
            //Debug.Log(HitVector);
            LunchTurnCount++;
            if (LunchTurnCount == 1)
            {
                LunchDirector = new Vector2(LunchDirector.x * (HitVector.x == 0 ? 1 : -1), LunchDirector.y * (HitVector.y == 0 ? 1 : -1));
            }
            else
            {
                LunchDirector = LunchDir(LunchTurnCount);
            }
        }
        //跟随中齿轮冲刺后被发射
        else if (subState == SubState.MGearRushLunched_WithMGear)
        {
            MGearRushLunchTurnCount++;
            //第零次反弹被忽略 按照既定发射方向
            if (MGearRushLunchTurnCount != 0)
            {
                //被反弹
                MGearRushLunchDirector = new Vector2(MGearRushLunchDirector.x * (HitVector.x == 0 ? 1 : -1), MGearRushLunchDirector.y * (HitVector.y == 0 ? 1 : -1));
            }
            //减速后碰壁
            if (!isHighSpeed)
            {
                //取消冲刺
                if (MGearRushLunchSpeedAlpha != 1.0f)
                {
                    MGearRushLunchSpeedAlpha = 1.0f;
                    isSpeedDown_MGearRushLunched = true;
                }
            }

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
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence)
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
    /// 是否需要寻找父辈
    /// </summary>
    bool NeedSearchParent;


    /// <summary>
    /// 搜索父辈
    /// 有限搜索齿轮怪 随后搜索齿轮组 都没有时进入单身状态
    /// </summary>
    public MainState SearchParent()
    {
        if (!isDie && !isBorn && !isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone && !isEmptyInfatuationDone)
        {
            Klinklang parentFather = SearchParentByDistence<Klinklang>();
            //有齿轮怪
            if (parentFather != null && !parentFather.isFearDone)
            {
                //Debug.Log("齿轮怪");
                ChildBackHome(parentFather);//回齿轮怪家
                FamilyState = MainState.WithLGear;
                subState = SubState.Idle_WithLGear;
                Idle_WithLGearStart(TIME_IDLE_WITHLGEAR_START);
                return MainState.WithLGear;
            }
            else
            {
                Klang father = SearchParentByDistence<Klang>();
                //有齿轮组
                if (father != null && !father.isFearDone)
                {
                    //Debug.Log("齿轮组");
                    ChildBackHome(father);//回齿轮组家
                    FamilyState = MainState.WithMGear;
                    subState = SubState.Idle_WithMGear;
                    Idle_WithMGearStart(TIME_IDLE_WITHMGEAR_START);
                    return MainState.WithMGear;
                }
                //无父
                else
                {
                    //Debug.Log("Single");
                    ChildLeaveHome();//保持单身
                    FamilyState = MainState.Single;
                    subState = SubState.Idle_Single;
                    Idle_SingleStart(TIME_IDLE_SINGLE_START);
                    return MainState.Single;
                }
            }
        }
        return MainState.Single;
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
            ParentEmptyByChild.GetComponent<Klang>().HaveChild2Single();
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
            ParentEmptyByChild.GetComponent<Klang>().Single2HaveChild();
        }
        //Debug.Log(ParentEmptyByChild);
        //忽略碰撞体
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.IgnoreOneChildCollision(this);
        }
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


    public void WithM2Single()
    {
        switch (subState)
        {
            case SubState.Lunched_WithMGear:
                Lunched_WithMGearOver();
                break;
            case SubState.MGearRushLunched_WithMGear:
                MGearRushLunched_WithMGearOver();
                break;
            case SubState.Back_WithMGear:
                Back_WithMGearOver();
                break;
            case SubState.Surround_WithMGear:
                Surround_WithMGearOver();
                break;
        }
        HighSpeedModeOver();
        HighSpeedModeOut();
        Idle_SingleStart(TIME_IDLE_SINGLE_START);
        ChildLeaveHome();
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
        if (isHighSpeed == false) {
            isHighSpeed = true;
            SpeedAlpha = SPEED_ALPHA_HIGH;
        }
        if(isAtkUP == false){
            AtkChange(2, 0.0f);
            isAtkUP = true;
        }
    }

    /// <summary>
    /// 高速模式结束 开始减速
    /// </summary>
    public void HighSpeedModeOver()
    {
        if (isHighSpeed == true) {
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




    //==■==■==■==■==■==■==■主状态：单个齿轮状态■==■==■==■==■==■==■==



    //=========================单个齿轮发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_SINGLE_START = 0.5f; //TODO需修改时间

    //单个齿轮移动后的冷却时间
    static float TIME_IDLE_SINGLE_MOVE_SINGLE = 0.0f; //TODO需修改时间


    //单个齿轮圆圈攻击后的冷却时间
    static float TIME_IDLE_SINGLE_CIRCLEATK_SINGLE = 2.2f; //TODO需修改时间



    /// <summary>
    /// 单个齿轮发呆计时器
    /// <summary>
    float Idle_SingleTimer = 0;

    /// <summary>
    /// 单个齿轮发呆开始
    /// <summary>
    public void Idle_SingleStart(float Timer)
    {
        Idle_SingleTimer = Timer;
        ChangeSubState(SubState.Idle_Single);
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }

    /// <summary>
    /// 单个齿轮发呆结束
    /// <summary>
    public void Idle_SingleOver()
    {
        Idle_SingleTimer = 0;
    }


    //=========================单个齿轮发呆============================






    //=========================单个齿轮移动============================


    //单个齿轮移动时的移动时间
    //static float TIME_MOVE_SINGLE = 40.0f; //TODO需修改时间


    //单个齿轮移动时的进入高速模式需要的时间
    static float TIME_ENTER_HIGHSPEED_MOVE_SINGLE = 5.0f; //TODO需修改时间

    //单个齿轮移动时的结束高速模式需要的时间
    static float TIME_OVER_HIGHSPEED_MOVE_SINGLE = 12.5f; //TODO需修改时间

    enum MOVE_SINGLE_MODE
    {
        UpMove,   //在房间上边界移动
        DownMove, //在房间下边界移动
        VMove,    //竖向移动
    };
    MOVE_SINGLE_MODE MoveSingleMode;






    /// <summary>
    ///  竖向移动的方向hai
    /// </summary>
    Vector2 VMoveDir = Vector2.up;

    //移动时距离房间的边界
    static float DISTENCE_Y_VMOVE = 1.5f;

    //竖向移动时的速度加成量
    static float SPEED_ALPHA_VMOVE = 2.5f;

    //竖向移动时的速度加成
    float SpeedAlphaVMove = 1.0f;

    /// <summary>
    /// 是否可以竖向移动
    /// </summary>
    int isCanVMoveCount = 0;

    /// <summary>
    /// 是否可以横向转向
    /// </summary>
    bool isCanHTurn = false;




    /// <summary>
    /// 横向移动的分速度
    /// </summary>
    Vector2 HMoveDir = Vector2.left;

    /// <summary>
    /// 单个齿轮移动计时器
    /// <summary>
    float Move_SingleTimer = 0;





    /// <summary>
    /// 单个齿轮移动开始
    /// <summary>
    public void Move_SingleStart()
    {
        Move_SingleTimer = 0.0f;
        ChangeSubState(SubState.Move_Single);
        //随机决定横向方向
        HMoveDir = new Vector2( (Random.Range(0.0f , 1.0f) > 0.5f)? 1.0f : -1.0f , 0 );
        SpeedAlphaVMove = 1.0f;
    }

    /// <summary>
    /// 单个齿轮移动结束
    /// <summary>
    public void Move_SingleOver()
    {
        Move_SingleTimer = 0;
        SpeedAlphaVMove = 1.0f;
    }


    //=========================单个齿轮移动============================






    //=========================单个齿轮圆圈攻击============================


    //圆形攻击的半径
    static float RADIUS_CIRCLEATK = 3.0f;


    //圆形攻击的角速度
    static float Omega_CIRCLEATK = 720.0f;

    //圆形攻击的周期（转一圈）
    static float T_CIRCLEATK { get { return 360.0f/Omega_CIRCLEATK; } }

    //圆形攻击的线速度
    static float V_CIRCLEATK { get { return Mathf.Deg2Rad * Omega_CIRCLEATK * RADIUS_CIRCLEATK; } }


    /// <summary>
    /// 圆形攻击的速度
    /// </summary>
    Vector2 CircleAtkDir = new Vector2(0, 0);


    /// <summary>
    /// 是否发动圆圈攻击
    /// </summary>
    public bool UseCircleAtkSingle { get { return useCircleAtkSingle; } set { useCircleAtkSingle = value; } }
    bool useCircleAtkSingle;

    /// <summary>
    /// 圆圈攻击检测器是否启动
    /// </summary>
    public bool IsCircleAtkCheckerEnable() {
        if (subState == SubState.Move_Single && isHighSpeed && MoveSingleMode != MOVE_SINGLE_MODE.VMove)
        {
            return true;
        }
        else
        {
            useCircleAtkSingle = false;
            return false;
        }
        
    }





    /// <summary>
    /// 单个齿轮圆圈攻击计时器
    /// <summary>
    float CircleAtk_SingleTimer = 0;

    /// <summary>
    /// 单个齿轮圆圈攻击开始
    /// <summary>
    public void CircleAtk_SingleStart(float Timer)
    {
        CircleAtk_SingleTimer = Timer;
        ChangeSubState(SubState.CircleAtk_Single);
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.04f, 1.8f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
    }

    /// <summary>
    /// 单个齿轮圆圈攻击结束
    /// <summary>
    public void CircleAtk_SingleOver()
    {
        CircleAtk_SingleTimer = 0;
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        HighSpeedModeOver();
        CircleAtkDir = new Vector2(0, 0);
    }


    //=========================单个齿轮圆圈攻击============================



    //==■==■==■==■==■==■==■主状态：单个齿轮状态■==■==■==■==■==■==■==





    














    //==■==■==■==■==■==■==■主状态：跟随中齿轮状态■==■==■==■==■==■==■==



    //=========================跟随中齿轮发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_WITHMGEAR_START = 0.2f; //TODO需修改时间

    //跟随中齿轮环绕后的冷却时间
    static float TIME_IDLE_WITHMGEAR_SURROUND_WITHMGEAR = 0.0f; //TODO需修改时间


    //跟随中齿轮被发射后的冷却时间
    static float TIME_IDLE_WITHMGEAR_LUNCHED_WITHMGEAR = 0.0f; //TODO需修改时间


    //跟随中齿轮返回后的冷却时间
    static float TIME_IDLE_WITHMGEAR_BACK_WITHMGEAR = 0.0f; //TODO需修改时间

    //跟随中齿轮中齿轮冲刺后被发射后的冷却时间
    static float TIME_IDLE_WITHMGEAR_MGEARRUSHLUNCHED_WITHMGEAR = 7.0f; //TODO需修改时间



    /// <summary>
    /// 跟随中齿轮发呆计时器
    /// <summary>
    float Idle_WithMGearTimer = 0;

    /// <summary>
    /// 跟随中齿轮发呆开始
    /// <summary>
    public void Idle_WithMGearStart(float Timer)
    {
        Idle_WithMGearTimer = Timer;
        ChangeSubState(SubState.Idle_WithMGear);
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }

    /// <summary>
    /// 跟随中齿轮发呆结束
    /// <summary>
    public void Idle_WithMGearOver()
    {
        Idle_WithMGearTimer = 0;
    }


    //=========================跟随中齿轮发呆============================






    //=========================跟随中齿轮环绕============================

    //环绕距离
    static float SURROUND_DISTENCE = 2.0f;




    /// <summary>
    /// 是否环绕在正在冲刺的中齿轮
    /// </summary>
    public bool SurroundWithRushMGear
    {
        get { return surroundWithRushMGear; }
        set { surroundWithRushMGear = value; }
    }
    bool surroundWithRushMGear = false;



    public float SurroundRotationSpeed
    {
        get { return surroundRotationSpeed; }
        set { surroundRotationSpeed = value; }
    }
    float surroundRotationSpeed = 35.0f;



    /// <summary>
    /// 跟随中齿轮环绕开始
    /// <summary>
    public void Surround_WithMGearStart()
    {
        ChangeSubState(SubState.Surround_WithMGear); 
        //静止刚体
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        //设定父对象和家
        transform.parent = ParentEmptyByChild.ChildHome;
        ParentEmptyByChild.GetComponent<Klang>().Single2HaveChild();
        HighSpeedModeEnter();
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }

    /// <summary>
    /// 环绕
    /// </summary>
    /// <param name="RotationSpeed"></param>
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
        float PlusAngle = (NowAngle + RotationSpeed * Time.deltaTime * SpeedAlpha ) % (360.0f);
        //Debug.Log("PlusAngle" + PlusAngle);
        //Debug.Log("Xcos" + Mathf.Cos(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE);
        //Debug.Log("Ysin" + Mathf.Sin(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE);

        transform.position = new Vector2(
                   Mathf.Clamp(ParentPosition.x
                       + Mathf.Cos(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE,       //方向*速度
                   ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //最小值
                   ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//最大值
                   Mathf.Clamp(ParentPosition.y
                       + Mathf.Sin(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE,        //方向*速度 
                   ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //最小值
                   ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//最大值
        //Debug.Log(name + "+" + ParentPosition + "+" + transform.position);
        //设置方向
        SetDirector(new Vector2(-1,-1));
    }



    /// <summary>
    /// 跟随中齿轮环绕结束
    /// <summary>
    public void Surround_WithMGearOver()
    {
        if (ParentEmptyByChild != null)
        {
            transform.parent = ParentPokemonRoom.EmptyFile();
        }
        //恢复刚体
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }


    //=========================跟随中齿轮环绕============================









    //=========================跟随中齿轮被发射============================



    /// <summary>
    /// 发射的初始速度增加系数
    /// </summary>
    static float SPEED_LUNCH_ALPHA = 1.65f;



    /// <summary>
    /// 被发射的角度
    /// </summary>
    Vector2 LunchDirector = Vector2.zero;

    /// <summary>
    /// 发射的速度增加系数
    /// </summary>
    float LunchSpeedAlpha = 0;

    /// <summary>
    /// 跟随中齿轮被发射计时器
    /// <summary>
    //float Lunched_WithMGearTimer = 0;

    /// <summary>
    /// 被发射的反弹计数
    /// <summary>
    int LunchTurnCount = 0;


    /// <summary>
    /// 被发射 发射失败时返回false
    /// <summary>
    public bool BeLunch()
    {
        if (subState == SubState.Surround_WithMGear && isHighSpeed) {
            //结束环绕
            Surround_WithMGearOver();
            //开始发射
            Lunched_WithMGearStart();
            return true;
        }
        return false;
    }


    /// <summary>
    /// 跟随中齿轮被发射开始
    /// <summary>
    public void Lunched_WithMGearStart()
    {
        //Lunched_WithMGearTimer = Timer;
        ChangeSubState(SubState.Lunched_WithMGear);
        LunchTurnCount = 0;
        LunchDirector = LunchDir(LunchTurnCount);
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.04f, 1.8f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
    }


    /// <summary>
    /// 跟随中齿轮被发射结束
    /// <summary>
    public void Lunched_WithMGearOver()
    {
        //Lunched_WithMGearTimer = 0;
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }


    /// <summary>
    /// 小齿轮发射的角度
    /// </summary>
    public Vector2 LunchDir(int count)
    {
        Vector2 dir = (TargetPosition - (Vector2)transform.position).normalized;
        switch (count)
        {
            case 0:
                dir = Quaternion.AngleAxis((isEmptyConfusionDone || (ParentEmptyByChild != null && ParentEmptyByChild.isEmptyConfusionDone)) ? (Random.Range(-30.0f, 30.0f)) : 0, Vector3.forward) * (TargetPosition - (Vector2)transform.position).normalized;
                break;
            case 1:
                //dir = (TargetPosition - (Vector2)transform.position).normalized;
                break;
            case 2:
                dir = Quaternion.AngleAxis(isEmptyConfusionDone ? (Random.Range(-30.0f, 30.0f)) : 0, Vector3.forward) * (TargetPosition - (Vector2)transform.position).normalized;
                break;
            case 3:
                dir = ((Vector2)ParentEmptyByChild.transform.position - (Vector2)transform.position).normalized;
                break;
            default:
                dir = ((Vector2)ParentEmptyByChild.transform.position - (Vector2)transform.position).normalized;
                break;
        }
        return dir;
    }







    //=========================跟随中齿轮被发射============================













    //=========================跟随中齿轮中齿轮冲刺后被发射============================


    /// <summary>
    /// 跟随中齿轮中齿轮冲刺后被发射速度逐渐变为0需要的时间
    /// <summary>
    static float TIME_MGEARRUSHLUNCH_SPEEDDOWN = 2.8f;

    /// <summary>
    /// 跟随中齿轮被发射计时器
    /// <summary>
    float MGearRushLunched_WithMGearTimer = 0;

    /// <summary>
    /// 中齿轮冲刺后被发射的角度
    /// </summary>
    Vector2 MGearRushLunchDirector = Vector2.zero;

    /// <summary>
    /// 被发射的反弹计数
    /// <summary>
    int MGearRushLunchTurnCount = 0;

    /// <summary>
    /// 是否开始减速
    /// </summary>
    bool isSpeedDown_MGearRushLunched = false;

    /// <summary>
    /// 发射的速度增加系数
    /// </summary>
    float MGearRushLunchSpeedAlpha = 1.0f;

    /// <summary>
    /// 被发射 发射失败时返回false
    /// <summary>
    public bool BeLunch_MGearRush( Vector2 Dir)
    {
        Debug.Log(name + "+" + (subState == SubState.Surround_WithMGear) + "+" + isHighSpeed);
        if (subState == SubState.Surround_WithMGear )
        {
            if (!isHighSpeed && !animator.GetBool("HighSpeed")) { HighSpeedModeStart(); }
            //结束环绕
            Surround_WithMGearOver();
            //开始发射
            MGearRushLunchDirector = Dir;
            MGearRushLunched_WithMGearStart();
            return true;
        }
        return false;
    }


    /// <summary>
    /// 跟随中齿轮被发射开始
    /// <summary>
    public void MGearRushLunched_WithMGearStart()
    {
        MGearRushLunched_WithMGearTimer = TIME_MGEARRUSHLUNCH_SPEEDDOWN;
        ChangeSubState(SubState.MGearRushLunched_WithMGear);
        MGearRushLunchTurnCount = 0;
        isSpeedDown_MGearRushLunched = false;
        MGearRushLunchSpeedAlpha = SPEED_LUNCH_ALPHA;
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.04f, 1.8f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
    }


    /// <summary>
    /// 跟随中齿轮被发射结束
    /// <summary>
    public void MGearRushLunched_WithMGearOver()
    {
        MGearRushLunched_WithMGearTimer = 0;
        MGearRushLunchSpeedAlpha = 1.0f;
        isSpeedDown_MGearRushLunched = false;
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }









    //=========================跟随中齿轮中齿轮冲刺后被发射============================














    //=========================跟随中齿轮返回============================




    /// <summary>
    /// 跟随中齿轮返回计时器
    /// <summary>
    //float Back_WithMGearTimer = 0;

    /// <summary>
    /// 跟随中齿轮返回开始
    /// <summary>
    public void Back_WithMGearStart()
    {
        //Back_WithMGearTimer = Timer;
        ChangeSubState(SubState.Back_WithMGear);
    }

    /// <summary>
    /// 跟随中齿轮返回结束
    /// <summary>
    public void Back_WithMGearOver()
    {
        //Back_WithMGearTimer = 0;
    }


    //=========================跟随中齿轮返回============================



    //==■==■==■==■==■==■==■主状态：跟随中齿轮状态■==■==■==■==■==■==■==





























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
