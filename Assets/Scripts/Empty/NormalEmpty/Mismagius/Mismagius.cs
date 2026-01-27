using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// !!!!!!!!!!!!!!!!梦妖魔 因为特殊性只放在标准房间中!!!!!!!!!!!!!!!!!!!!
/// </summary>
public class Mismagius : Empty
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
        Idle,              //发呆
        Move,              //移动接近
        CloneShadow,       //分身
        TakeTurnsLaunch,   //转圈发射弹幕
        EatDream,          //食梦
    }
    MainState NowState;



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
        StartCoroutine(CheckLook());
        IdleStart(TIME_IDLE_START , MainState.Move); 
        SetDirector(new Vector2(-1,-1));

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
                //Debug.Log(InfatuationTargetEmpty);
                //进入食梦状态
                if ( ( (!isEmptyInfatuationDone && player.isSleepDone ) || (isEmptyInfatuationDone && InfatuationTargetEmpty != null && InfatuationTargetEmpty.isSleepDone))
                    && NowState != MainState.EatDream && !isFearDone)
                {
                    if (CloneBodyList.Count != 0)
                    {
                        List<MismagiusCloneBody> l = new List<MismagiusCloneBody> { };
                        for (int i = 0; i < CloneBodyList.Count; i++)
                        {
                            l.Add(CloneBodyList[i]);
                        }
                        for (int i = 0; i < l.Count; i++)
                        {
                            l[i].SetCloneShadowOver(false);
                        }
                        l.Clear();
                    }
                    //Debug.Log(NowState);
                    switch (NowState)
                    {
                        case MainState.Idle: IdleOver(); break;
                        case MainState.Move: MoveOver(); break;
                        case MainState.CloneShadow: CloneShadowOver(); break;
                        case MainState.TakeTurnsLaunch: TakeTurnsLaunchOver(); break;
                    }
                    
                    EatDreamStart();
                }
                switch (NowState)
                {
                    //发呆状态
                    case MainState.Idle:
                        IdleTimer -= Time.deltaTime;//发呆计时器时间减少

                        if ( IdleTimer <= 0 || isFearDone)         //计时器时间到时间，结束发呆状态
                        {
                            if (isFearDone) { NextState_IdleOver = MainState.Move; RemoveAllCloneShadow(); }
                            IdleOver();
                            switch (NextState_IdleOver)
                            {
                                //下一个状态为发射弹幕时切换为发射弹幕
                                case MainState.TakeTurnsLaunch:
                                    TakeTurnsLaunchStart();
                                    break;
                                //下一个状态为其他时切换为移动
                                default:
                                    MoveStart();
                                    break;
                            }
                            
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                    //移动接近状态
                    case MainState.Move:
                        //MoveTimer -= Time.deltaTime;//移动接近计时器时间减少
                        //if ( MoveTimer <= 0 )         //计时器时间到时间，结束移动接近状态
                        //{
                        //    MoveOver();
                        //    //TODO添加下一个状态的开始方法
                        //}
                        if (!isFearDone)
                        {
                            Vector2 MoveDir = (TargetPosition - (Vector2)transform.position).normalized;
                            MoveBySpeedAndDir(MoveDir, speed, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                            //Debug.Log(Vector2.Distance(TargetPosition, (Vector2)transform.position) + "+" + Mathf.Clamp((TargetPosition - (Vector2)ParentPokemonRoom.transform.position).y, 1.0f, 100.0f));
                            if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= Mathf.Clamp(Mathf.Abs((TargetPosition - (Vector2)ParentPokemonRoom.transform.position).y), 1.0f, 100.0f))
                            {
                                MoveOver();
                                CloneShadowStart();
                            }
                        }
                        else
                        {
                            Vector2 MoveDir = -(TargetPosition - (Vector2)transform.position).normalized;
                            if (Vector2.Distance(TargetPosition , (Vector2)transform.position) <= 6.0f)
                            {
                                MoveBySpeedAndDir(MoveDir, speed, 2.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                            }
                        }
                        break;
                    //分身状态
                    case MainState.CloneShadow:
                        //CloneShadowTimer -= Time.deltaTime;//分身计时器时间减少
                        //if ( CloneShadowTimer <= 0 )         //计时器时间到时间，结束分身状态
                        //{
                        //    CloneShadowOver();
                        //    //TODO添加下一个状态的开始方法
                        //}
                        MoveBySpeedAndDir( (RushTarget - (Vector2)transform.position).normalized ,RushSpeed , 1.0f , 0.0f, 0.0f, 0.0f, 0.0f);
                        if ( (Vector2.Distance((Vector2)transform.position, RushTarget) <= 0.6f) || isFearDone )
                        {
                            if (isFearDone) { RemoveAllCloneShadow(); }
                            CloneShadowOver();
                            IdleStart(TIME_IDLE_CLONESHADOW, MainState.TakeTurnsLaunch);
                        }
                        break;
                    //转圈发射弹幕状态
                    case MainState.TakeTurnsLaunch:
                        TakeTurnsLaunchTimer += Time.deltaTime;     //转圈发射弹幕计时器时间减少
                        TakeTurnsLaunchStateTimer -= Time.deltaTime;//转圈发射弹幕状态计时器时间减少
                        if ( TakeTurnsLaunchTimer >= (float)RealIndex * TIME_TAKETURNLAUNCH_INTERVAL )         //计时器时间到时间，发射弹幕
                        {
                            animator.SetTrigger("Atk");
                            TakeTurnsLaunchTimer -= (float)COUNT_CLONESHADOW * TIME_TAKETURNLAUNCH_INTERVAL;
                            LunchDir = (CloneBodyCenter - (Vector2)transform.position).normalized;
                            LunchOneShadowBall(LunchDir, Mismagius.SPEED_SHADOWBALL);
                            //TakeTurnsLaunchOver();
                            //TODO添加下一个状态的开始方法
                        }
                        if (TakeTurnsLaunchStateTimer <= 0 || isFearDone)         //计时器时间到时间，结束发射弹幕状态
                        {
                            if (isFearDone) { RemoveAllCloneShadow(); }
                            TakeTurnsLaunchOver();
                            IdleStart(TIME_IDLE_TAKETURNSLAUNCH , MainState.Idle);
                        }
                        break;
                    //食梦状态
                    case MainState.EatDream:
                        //EatDreamTimer -= Time.deltaTime;//食梦计时器时间减少
                        //if ( EatDreamTimer <= 0 )         //计时器时间到时间，结束食梦状态
                        //{
                        //    EatDreamOver();
                        //    //TODO添加下一个状态的开始方法
                        //}
                        int i = MoveBySpeedAndDir(EatDreamDir, speed, SPEED_ALPHA_EATDREAM, 0.0f, 0.0f, 0.0f, 0.0f);
                        if (i != -1 || isFearDone)
                        {
                            if (isFearDone) { RemoveAllCloneShadow(); }
                            //结束食梦
                            EatDreamOver();
                            IdleStart(TIME_IDLE_EATDREAM, MainState.Move);
                        }
                        break;
                }
            }
            //■■结束判断状态机
            if ((isEmptyFrozenDone || isSleepDone ||isSilence) && NowState != MainState.Idle)
            {
                animator.SetTrigger("Sleep");
                animator.SetFloat("Speed" , 0.0f);
                RemoveAllCloneShadow();
                switch (NowState)
                {
                    case MainState.Move:
                        MoveOver();
                        break;
                    case MainState.TakeTurnsLaunch:
                        TakeTurnsLaunchOver();
                        break;
                    case MainState.CloneShadow:
                        CloneShadowOver();
                        break;
                    case MainState.EatDream:
                        EatDreamOver();
                        break;
                }
                IdleStart(TIME_IDLE_START , MainState.Move);
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
        //食梦状态
        if (NowState == MainState.EatDream)
        {
            //结束食梦
            if (other.transform.tag == ("Room") )
            {
                EatDreamOver();
                IdleStart(TIME_IDLE_EATDREAM , MainState.Move);
            }
            //碰撞伤害
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//未被魅惑 且与玩家碰撞时
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                int BeforeHp = -1;
                int AfterHp = -1;
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 7.0f;
                    playerControler.KnockOutDirection = (new Vector2(Director.y, Director.x)).normalized;
                    //获取受伤前血量
                    BeforeHp = playerControler.Hp;
                }
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 0, SPDMAGE_EATDREAM, 0, PokemonType.TypeEnum.Psychic);
                //获取受伤后血量
                if (playerControler != null){AfterHp = playerControler.Hp;}
                //如果造成了伤害且攻击了玩家 吸血
                if (BeforeHp != -1 && AfterHp != -1 && BeforeHp > AfterHp) { Pokemon.PokemonHpChange(null, this.gameObject, 0, 0, (BeforeHp - AfterHp)/2 , PokemonType.TypeEnum.No); }

                //生成食梦特效
                if (!isAnimationEffectBorn)
                {
                    isAnimationEffectBorn = false;
                    Instantiate(EatDreamEffect, transform.position, Quaternion.identity);
                }
            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//被魅惑 且与其他敌人碰撞时
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                int BeforeHp = -1;
                int AfterHp = -1;
                //获取受伤前血量
                if (e != null) { BeforeHp = e.EmptyHp; }
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, 0, SPDMAGE_EATDREAM, 0, PokemonType.TypeEnum.Psychic);
                //获取受伤后血量
                if (e != null) { AfterHp = e.EmptyHp; }
                //如果造成了伤害且攻击了玩家 吸血
                if (BeforeHp != -1 && AfterHp != -1 && BeforeHp > AfterHp) { Pokemon.PokemonHpChange(null, this.gameObject, 0, 0, (BeforeHp - AfterHp) / 2, PokemonType.TypeEnum.No); }

                //生成食梦特效
                if (!isAnimationEffectBorn)
                {
                    isAnimationEffectBorn = false;
                    Instantiate(EatDreamEffect, transform.position, Quaternion.identity);
                }
            }
        }
        //非食梦状态
        else
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
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence)
            {
                if ((NowState == MainState.Move))
                {
                    //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                    animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                    //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                    Director = _mTool.TiltMainVector2((transform.position - LastPosition));
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
    /// 输出-1时表示没有到达运动边界
    /// 输出0123时分别代表到达上下左右的运动边界
    /// </summary>
    /// <param name="dir">移动方向</param>
    /// <param name="Speed">移动速度</param>
    /// <param name="SpeedAlpha">移动速度的加成系数（乘算）</param>
    /// <param name="RoomUpAlpha">房间上边界的限制系数</param>
    /// <param name="RoomDownAlpha">房间下边界的限制系数</param>
    /// <param name="RoomLeftAlpha">房间右边界的限制系数</param>
    /// <param name="RoomRightAlpha">房间左边界的限制系数</param>
    public int MoveBySpeedAndDir(Vector2 dir, float Speed, float SpeedAlpha, float RoomUpAlpha, float RoomDownAlpha, float RoomLeftAlpha, float RoomRightAlpha)
    {
        int output = -1;
        rigidbody2D.position = new Vector2(
            Mathf.Clamp(rigidbody2D.position.x
                + (float)dir.x * Time.deltaTime * Speed * SpeedAlpha,                    //方向*速度
            ParentPokemonRoom.RoomSize[2] - RoomLeftAlpha + transform.parent.position.x, //最小值
            ParentPokemonRoom.RoomSize[3] + RoomRightAlpha + transform.parent.position.x),//最大值
            Mathf.Clamp(rigidbody2D.position.y
                + (float)dir.y * Time.deltaTime * Speed * SpeedAlpha,                     //方向*速度 
            ParentPokemonRoom.RoomSize[1] - RoomDownAlpha + transform.parent.position.y,  //最小值
            ParentPokemonRoom.RoomSize[0] + RoomUpAlpha + transform.parent.position.y));//最大值
        if (rigidbody2D.position.y >= ParentPokemonRoom.RoomSize[0] + RoomUpAlpha + transform.parent.position.y) { output = 0; }
        if (rigidbody2D.position.y <= ParentPokemonRoom.RoomSize[1] - RoomDownAlpha + transform.parent.position.y) { output = 1; }
        if (rigidbody2D.position.x <= ParentPokemonRoom.RoomSize[2] - RoomLeftAlpha + transform.parent.position.x) { output = 2; }
        if (rigidbody2D.position.x >= ParentPokemonRoom.RoomSize[3] + RoomRightAlpha + transform.parent.position.x) { output = 3; }
        
        
        
        return output;
    }




    public override void HitEvent()
    {
        base.HitEvent();
        //发射弹幕期间被击中大硬直
        if (NowState == MainState.TakeTurnsLaunch || NowState == MainState.CloneShadow)
        {
            RemoveAllCloneShadow();
            switch (NowState)
            {
                case MainState.TakeTurnsLaunch:
                    TakeTurnsLaunchOver(); break;
                case MainState.CloneShadow:
                    CloneShadowOver(); break;
            }
            
            IdleStart(TIME_IDLE_TAKETURNSLAUNCH_BIGCD, MainState.Move);

        }
    }


    public override void DieEvent()
    {
        if (CloneBodyList.Count != 0)
        {
            RemoveAllCloneShadow();
            switch (NowState)
            {
                case MainState.TakeTurnsLaunch:
                    TakeTurnsLaunchOver(); break;
                case MainState.CloneShadow:
                    CloneShadowOver(); break;
            }
        }
        base.DieEvent();
    }


    //InsertSubStateChange


    //清空分身
    public void RemoveAllCloneShadow()
    {
        if (CloneBodyList.Count > 0) {
            List<MismagiusCloneBody> l = new List<MismagiusCloneBody> { };
            for (int i = 0; i < CloneBodyList.Count; i++)
            {
                l.Add(CloneBodyList[i]);
            }
            for (int i = 0; i < l.Count; i++)
            {
                l[i].SetCloneShadowOver(false);
            }
            l.Clear();
        }
    }
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■














    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //=========================发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_START = 0.5f; //TODO需修改时间

    //移动接近后的冷却时间
    static float TIME_IDLE_MOVE = 0.0f; //TODO需修改时间


    //分身后的冷却时间
    static float TIME_IDLE_CLONESHADOW = 0.2f; //TODO需修改时间


    //转圈发射弹幕后的冷却时间
    static float TIME_IDLE_TAKETURNSLAUNCH = 1.8f; //TODO需修改时间


    //食梦后的冷却时间
    static float TIME_IDLE_EATDREAM = 3.5f; //TODO需修改时间

    //移动接近后的冷却时间
    static float TIME_IDLE_TAKETURNSLAUNCH_BIGCD = 7.5f; //TODO需修改时间




    /// <summary>
    /// 发呆计时器
    /// <summary>
    float IdleTimer = 0;

    /// <summary>
    /// 发呆后的下一个状态
    /// </summary>
    MainState NextState_IdleOver;

    /// <summary>
    /// 发呆开始
    /// <summary>
    public void IdleStart(float Timer , MainState nextState)
    {
        IdleTimer = Timer;
        NowState = MainState.Idle;
        NextState_IdleOver = nextState;
    }

    /// <summary>
    /// 发呆结束
    /// <summary>
    public void IdleOver()
    {
        IdleTimer = 0;
    }


    //=========================发呆============================


















    //=========================移动接近============================




    /// <summary>
    /// 移动接近计时器
    /// <summary>
    //float MoveTimer = 0;

    /// <summary>
    /// 移动接近开始
    /// <summary>
    public void MoveStart(/*float Timer*/)
    {
        //MoveTimer = Timer;
        NowState = MainState.Move;
    }

    /// <summary>
    /// 移动接近结束
    /// <summary>
    public void MoveOver()
    {
        //MoveTimer = 0;
    }


    //=========================移动接近============================





















    //=========================分身============================

    /// <summary>
    /// 分身散开的固定时间
    /// </summary>
    static float TIME_CLONESHADOW_BLAST = 0.4f;

    /// <summary>
    /// 生成分身的个数（包括本体）
    /// </summary>
    public static int COUNT_CLONESHADOW = 16;

    /// <summary>
    /// 分身
    /// </summary>
    public MismagiusCloneBody CloneShadow;

    /// <summary>
    /// 分身列表
    /// </summary>
    public List<MismagiusCloneBody> CloneBodyList = new List<MismagiusCloneBody> { };

    /// <summary>
    /// 分身圈的中心点
    /// </summary>
    public Vector2 CloneBodyCenter
    {
        get { return cloneBodyCenter; }
        set { cloneBodyCenter = value; }
    }
    Vector2 cloneBodyCenter;

    /// <summary>
    /// 分身圈的半径
    /// </summary>
    public float CloneBodyRadius 
    {
        get { return cloneBodyRadius; }
        set { cloneBodyRadius = value; }
    }
    float cloneBodyRadius;

    /// <summary>
    /// 分身计时器
    /// <summary>
    //float CloneShadowTimer = 0;

    /// <summary>
    /// 本体所在的序列号
    /// </summary>
    int RealIndex = 0;

    /// <summary>
    /// 冲刺目的地
    /// </summary>
    Vector2 RushTarget = Vector2.zero;

    /// <summary>
    /// 冲刺速度
    /// </summary>
    float RushSpeed = 0.0f;




    /// <summary>
    /// 分身开始
    /// <summary>
    public void CloneShadowStart(/*float Timer*/)
    {
        //CloneShadowTimer = Timer;
        NowState = MainState.CloneShadow;
        
        CloneBodyCenter = new Vector2((TargetPosition - (Vector2)ParentPokemonRoom.transform.position).x, ParentPokemonRoom.transform.position.y);
        CloneBodyRadius = 7.0f;
        RealIndex = 0;
        RushTarget = Vector2.zero;
        RushSpeed = 0.0f;
        BornCloneShadow();
        GetComponent<Collider2D>().enabled = false;
        animator.SetBool("EatDream", true);
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.08f, 1.6f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
    }

    /// <summary>
    /// 分身结束
    /// <summary>
    public void CloneShadowOver()
    {
        //CloneShadowTimer = 0;
        //CloneBodyCenter = Vector2.zero;
        //CloneBodyRadius = 0;
        //RealIndex = 0;
        RushTarget = Vector2.zero;
        RushSpeed = 0.0f;
        GetComponent<Collider2D>().enabled = true;
        animator.SetBool("EatDream", false);
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }


    /// <summary>
    /// 生成分身
    /// </summary>
    public void BornCloneShadow()
    {
        //分身和本体的次数
        int CloneShadowCount = COUNT_CLONESHADOW;
        //清空列表
        CloneBodyList.Clear();
        CloneBodyList = new List<MismagiusCloneBody> { };
        //随机本体所在序列号
        RealIndex = Random.Range(0, CloneShadowCount);
        //每个分身之间的角度
        float PerAngle = 360.0f / ((float)(CloneShadowCount));
       

        //生成分身
        for (int i = 0; i < COUNT_CLONESHADOW; i++)
        {
            //设置分身
            if (i != RealIndex) {
                MismagiusCloneBody clone = Instantiate(CloneShadow, transform.position, Quaternion.identity, transform.parent);
                CloneBodyList.Add(clone);
                clone.SetCloneBody(this);
                clone.RushTarget =  CloneBodyCenter + (Vector2)((Quaternion.AngleAxis(i * PerAngle, Vector3.forward) * Vector2.right) * CloneBodyRadius);
                clone.RushTarget = new Vector2(
                    Mathf.Clamp(clone.RushTarget.x,                                     //目的地
                    ParentPokemonRoom.RoomSize[2] + transform.parent.position.x,  //最小值
                    ParentPokemonRoom.RoomSize[3] + transform.parent.position.x), //最大值
                    Mathf.Clamp(clone.RushTarget.y,                                     //目的地
                    ParentPokemonRoom.RoomSize[1] + transform.parent.position.y,  //最小值
                    ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));//最大值
                //Debug.Log(clone.RushTarget);
                clone.SetDirector( _mTool.TiltMainVector2((clone.RushTarget - cloneBodyCenter).normalized));
                //clone.RushStart();
                //冲刺速度
                clone.RushSpeed = Vector2.Distance(clone.RushTarget, transform.position) / TIME_CLONESHADOW_BLAST;
                clone.CloneBodyIndex = i;
                clone.DispearTime = (float)COUNT_SHADOWBALL_TURNS * (float)COUNT_CLONESHADOW * TIME_TAKETURNLAUNCH_INTERVAL;
            }
            //设置本体
            else
            {
                RushTarget = CloneBodyCenter + (Vector2)((Quaternion.AngleAxis(i * PerAngle, Vector3.forward) * Vector2.right) * CloneBodyRadius);
                RushTarget = new Vector2(
                    Mathf.Clamp(RushTarget.x,                                     //目的地
                    ParentPokemonRoom.RoomSize[2] + transform.parent.position.x,  //最小值
                    ParentPokemonRoom.RoomSize[3] + transform.parent.position.x), //最大值
                    Mathf.Clamp(RushTarget.y,                                     //目的地
                    ParentPokemonRoom.RoomSize[1] + transform.parent.position.y,  //最小值
                    ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));//最大值
                //Debug.Log(RushTarget);
                //冲刺速度
                RushSpeed = Vector2.Distance(RushTarget, transform.position) / TIME_CLONESHADOW_BLAST;
            }
        }
    }


    //=========================分身============================









    //=========================转圈发射弹幕============================


    /// <summary>
    /// 发射影子球的论次数
    /// </summary>
    static int COUNT_SHADOWBALL_TURNS = 4;

    /// <summary>
    /// 发射影子球角度
    /// </summary>
    public Vector2 LunchDir;

    /// <summary>
    /// 假影子球
    /// </summary>
    public MismagiusShadowBalll sb;



    /// <summary>
    /// 轮流发射影子球的时间间隔
    /// </summary>
    public static float TIME_TAKETURNLAUNCH_INTERVAL = 0.17f;

    /// <summary>
    /// 发射影子球的速度
    /// </summary>
    public static float SPEED_SHADOWBALL = 3.0f;

    /// <summary>
    /// 转圈发射弹幕计时器
    /// <summary>
    float TakeTurnsLaunchTimer = 0;

    /// <summary>
    /// 转圈发射弹幕状态计时器
    /// <summary>
    float TakeTurnsLaunchStateTimer = 0;

    /// <summary>
    /// 转圈发射弹幕开始
    /// <summary>
    public void TakeTurnsLaunchStart(/*float Timer*/)
    {
        TakeTurnsLaunchStateTimer = (float)COUNT_SHADOWBALL_TURNS * (float)COUNT_CLONESHADOW * TIME_TAKETURNLAUNCH_INTERVAL;
        TakeTurnsLaunchTimer = 0;
        NowState = MainState.TakeTurnsLaunch;
        SetDirector(_mTool.TiltMainVector2((cloneBodyCenter - (Vector2)transform.position).normalized));
    }

    /// <summary>
    /// 转圈发射弹幕结束
    /// <summary>
    public void TakeTurnsLaunchOver()
    {
        CloneBodyList.Clear();
        CloneBodyList = new List<MismagiusCloneBody> { };
        TakeTurnsLaunchTimer = 0;
        TakeTurnsLaunchStateTimer = 0;
    }

    /// <summary>
    /// 发射一个影子球
    /// </summary>
    void LunchOneShadowBall(Vector2 Dir, float speed)
    {
        Dir = Quaternion.AngleAxis((isEmptyConfusionDone ? (Random.Range(-30, 30)) : 0), Vector3.forward) * Dir.normalized;
        MismagiusShadowBalll s = Instantiate(sb, (Vector3)Dir * 1.0f + transform.position + Vector3.up * 0.4f, Quaternion.identity);
        s.empty = this;
        s.LaunchNotForce(Dir, speed);
    }
    //=========================转圈发射弹幕============================










    //=========================食梦============================

    public GameObject EatDreamEffect;

    /// <summary>
    /// 食梦速度加成
    /// </summary>
    static float SPEED_ALPHA_EATDREAM = 4.3f;

    /// <summary>
    /// 食梦伤害
    /// </summary>
    static int SPDMAGE_EATDREAM = 100;

    /// <summary>
    /// 动画特效是否生成
    /// </summary>
    bool isAnimationEffectBorn = false;


    /// <summary>
    /// 食梦方向
    /// </summary>
    Vector2 EatDreamDir = Vector2.zero;

    /// <summary>
    /// 食梦计时器
    /// <summary>
    //float EatDreamTimer = 0;

    /// <summary>
    /// 食梦开始
    /// <summary>
    public void EatDreamStart(/*float Timer*/)
    {
        //EatDreamTimer = Timer;
        NowState = MainState.EatDream;
        EatDreamDir = Quaternion.AngleAxis((isEmptyConfusionDone ? (Random.Range(-30, 30)) : 0), Vector3.forward) * (TargetPosition - (Vector2)transform.position).normalized;
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.06f, 1.6f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f)); ;
        }
        animator.SetBool("EatDream", true);
        SetDirector(_mTool.TiltMainVector2(EatDreamDir));
        isAnimationEffectBorn = false;
    }

    /// <summary>
    /// 食梦结束
    /// <summary>
    public void EatDreamOver()
    {
        //EatDreamTimer = 0;
        EatDreamDir = Vector2.zero;
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        animator.SetBool("EatDream" , false);
        isAnimationEffectBorn = false;
    }


    //=========================食梦============================




    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    
}
