using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frosmoth : Empty
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


    // 移动粒子效果
    public ParticleSystem PS1;
    public ParticleSystem PS2;



    //==============================状态机枚举===================================

    /// <summary>
    /// 主状态
    /// </summary>
    enum MainState
    {
        Idle, //发呆
        Run,  //移动
        Atk,  //攻击
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
    { MainState.TODO, new[] { SubState.TODO,... },
    { MainState.TODO, new[] { SubState.TODO,... },
    ....
};
**/
    //==============================状态机枚举===================================



    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ice;//敌人第一属性
        EmptyType02 = PokemonType.TypeEnum.Bug;//敌人第二属性
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
        //开始发呆
        IdleStart(TIME_IDLE_START);

        PasueMistPS();

        SetDirector(Vector2.down);

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
                animator.ResetTrigger("Sleep");
                switch (NowState)
                {
                    //发呆状态
                    case MainState.Idle:
                        IdleTimer -= Time.deltaTime;//发呆计时器时间减少
                        if ( IdleTimer <= 0 )         //计时器时间到时间，结束发呆状态
                        {
                            IdleOver();
                            RunStart();
                        }
                        if (isFearDone)
                        {
                            AtkOver();
                            RunStart();
                        }
                        break;
                    //移动状态
                    case MainState.Run:
                        //沿水平方向移动
                        float AtkRange = isEmptyConfusionDone ? 1.3f : 0.3f;
                        if (MoveIsHorizontal)
                        {
                            if (Mathf.Abs(transform.position.x - TargetPosition.x) < AtkRange) {
                                if (!isFearDone) {
                                    RunOver();
                                    AtkStart(TIME_ATK());
                                }
                            }
                            else
                            {
                                if (transform.position.x > TargetPosition.x) { Director = Vector2.left; }
                                if (transform.position.x < TargetPosition.x) { Director = Vector2.right; }
                            }
                        }
                        //沿垂直方向移动
                        else
                        {
                            if (Mathf.Abs(transform.position.y - TargetPosition.y) < AtkRange)
                            {
                                if (!isFearDone) {
                                    RunOver();
                                    AtkStart(TIME_ATK());
                                }
                            }
                            else
                            {
                                if (transform.position.y > TargetPosition.y) { Director = Vector2.down; }
                                if (transform.position.y < TargetPosition.y) { Director = Vector2.up; }
                            }
                        }
                        if (isFearDone)
                        {
                            Director = -Director;
                        }
                        //移动
                        MoveBySpeedAndDir(Director , speed , 1.0f , 0.3f, 0.3f, 0.3f, 0.3f);
                        SetDirector(Director);

                        break;
                    //攻击状态
                    case MainState.Atk:
                        AtkTimer -= Time.deltaTime;//攻击计时器时间减少
                        if (BlizzardCountLevel < BlizzardLevelByDistence.Count) {
                            if ((TIME_ATK() - AtkTimer) >= (BlizzardInterval * ((float)BlizzardCountLevel) + BlizzardStartDelay)) {
                                LunchBlizzard(BlizzardCountLevel);
                                BlizzardCountLevel++;
                            }
                        }

                        if ( AtkTimer <= 0 )         //计时器时间到时间，结束攻击状态
                        {
                            AtkOver();
                            IdleStart(TIME_IDLE_ATK);
                            //TODO添加下一个状态的开始方法
                        }
                        if (isFearDone)
                        {
                            AtkOver();
                            RunStart();
                        }
                        break;
                }
            }

            if ((isEmptyFrozenDone || isSleepDone || isSilence) && NowState != MainState.Idle )
            {
                animator.SetTrigger("Sleep");
                switch (NowState)
                {
                    case MainState.Atk:
                        AtkOver();
                        break;
                    case MainState.Run:
                        RunOver();
                        break;
                }
                IdleStart(TIME_IDLE_START);
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
            //一般或走路状态时更改速度和朝向
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && (NowState == MainState.Idle || NowState == MainState.Run) )
            {
                //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                //Director = _mTool.MainVector2((transform.position - LastPosition));
                //animator.SetFloat("LookX", Director.x);
                //animator.SetFloat("LookY", Director.y);
                //Debug.Log(Director);
                //重置位置
                LastPosition = transform.position;
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




    //InsertSubStateChange
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■














    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //=========================发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_START = 1.2f; //TODO需修改时间

    //移动后的冷却时间
    static float TIME_IDLE_RUN = 3.0f; //TODO需修改时间


    //攻击后的冷却时间
    static float TIME_IDLE_ATK = 7.0f; //TODO需修改时间



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
    /// 是否沿水平方向移动
    /// </summary>
    bool MoveIsHorizontal = true;

    /// <summary>
    /// 移动计时器
    /// <summary>
    //float RunTimer = 0;

    /// <summary>
    /// 移动开始
    /// <summary>
    public void RunStart( )
    {
        //RunTimer = Timer;
        NowState = MainState.Run;
        StartMistPS();
    }

    /// <summary>
    /// 移动结束
    /// <summary>
    public void RunOver()
    {
        //RunTimer = 0;
        MoveIsHorizontal = !MoveIsHorizontal;
        PasueMistPS();
    }



    /// <summary>
    /// 开始发射移动粒子
    /// </summary>
    public void StartMistPS()
    {
        var e1 = PS1.emission;
        var e2 = PS2.emission;
        e1.enabled = true;
        e2.enabled = true;
    }


    //停止发射移动粒子
    public void PasueMistPS()
    {
        var e1 = PS1.emission;
        var e2 = PS2.emission;
        e1.enabled = false;
        e2.enabled = false;
    }


    //=========================移动============================






    //=========================攻击============================

    /// <summary>
    /// 暴风雪的发射间隔
    /// </summary>
    static float BlizzardInterval = 0.55f;

    /// <summary>
    /// 暴风雪的开始延迟
    /// </summary>
    static float BlizzardStartDelay = 0.2f;

    /// <summary>
    /// 根据距离生成的暴风雪的等级
    /// </summary>
    public static List<int> BlizzardLevelByDistence = new List<int> { 0 ,0 ,1 ,2 ,3 };

    /// <summary>
    /// 根据距离生成的暴风雪的等级
    /// </summary>
    public static List<float> BlizzardRadius = new List<float> { 1.55f, 1.55f, 1.9f, 2.25f, 2.6f };

    /// <summary>
    /// 生成暴风雪位置的偏移量
    /// </summary>
    public static List<float> BlizzardPosition = new List<float> { 2.55f, 5.65f, 9.1f, 13.25f, 18.1f };

    /// <summary>
    /// 暴风雪预制体
    /// </summary>
    public FrosmothBlizzard FB;
    /// <summary>
    /// 生成的暴风雪实体列表
    /// </summary>
    //List<FrosmothBlizzard> FBObjList = new List<FrosmothBlizzard> { };

    /// <summary>
    /// 当前的暴风雪发射等级
    /// </summary>
    int BlizzardCountLevel = 0;

    //发射暴雪的持续时间
    float TIME_ATK()
    {
        return BlizzardInterval * ((float)BlizzardLevelByDistence.Count) + BlizzardStartDelay;
    }

    /// <summary>
    /// 发射方向
    /// </summary>
    Vector2 LunchDir;

    /// <summary>
    /// 攻击计时器
    /// <summary>
    float AtkTimer = 0;

    /// <summary>
    /// 攻击开始
    /// <summary>
    public void AtkStart(float Timer)
    {
        BlizzardCountLevel = 0;
        animator.SetBool("Atk" , true);
        AtkTimer = Timer;
        NowState = MainState.Atk;
        LunchDir = _mTool.MainVector2(TargetPosition - (Vector2)transform.position);
        SetDirector(LunchDir);
    }

    /// <summary>
    /// 攻击结束
    /// <summary>
    public void AtkOver()
    {
        AtkTimer = 0;
        animator.SetBool("Atk", false);
        
    }




    /// <summary>
    /// 发射暴风雪
    /// </summary>
    public void LunchBlizzard(int i)
    {
        //判断暴风雪位置是否在限定位置内
        Vector2 p1 = transform.position + (Vector3)LunchDir * (BlizzardPosition[i] - BlizzardRadius[i]);
        Vector2 p2 = transform.position - (Vector3)LunchDir * (BlizzardPosition[i] - BlizzardRadius[i]);

        //横向发射
        if (LunchDir.y == 0)
        {
            if (!(p1.x >= ParentPokemonRoom.RoomSize[3] || p1.x <= ParentPokemonRoom.RoomSize[2] ) )
            {
                //生成暴风雪实例
                FrosmothBlizzard fb1 = Instantiate(FB, transform.position, Quaternion.identity);
                fb1.transform.position += (Vector3)LunchDir * BlizzardPosition[i];
                if (isEmptyConfusionDone) { fb1.transform.position += Quaternion.AngleAxis(90, Vector3.forward) * LunchDir * Random.Range(-2.5f, 2.5f); }
                fb1.BlizzardLevel = BlizzardLevelByDistence[i];
                fb1.setBlizzardSize();
                fb1.ParentEmpty = this;
                //FBObjList.Add(fb1);
            }
            if (!(p2.x >= ParentPokemonRoom.RoomSize[3] || p2.x <= ParentPokemonRoom.RoomSize[2]))
            {
                FrosmothBlizzard fb2 = Instantiate(FB, transform.position, Quaternion.identity);
                fb2.transform.position -= (Vector3)LunchDir * BlizzardPosition[i];
                if (isEmptyConfusionDone) { fb2.transform.position += Quaternion.AngleAxis(90, Vector3.forward) * LunchDir * Random.Range(-2.5f, 2.5f); }
                fb2.BlizzardLevel = BlizzardLevelByDistence[i];
                fb2.setBlizzardSize();
                fb2.ParentEmpty = this;
                //FBObjList.Add(fb2);
            }
        }
        //纵向发射
        else
        {
            if (!(p1.y >= ParentPokemonRoom.RoomSize[0] || p1.y <= ParentPokemonRoom.RoomSize[1]))
            {
                //生成暴风雪实例
                FrosmothBlizzard fb1 = Instantiate(FB, transform.position, Quaternion.identity);
                fb1.transform.position += (Vector3)LunchDir * BlizzardPosition[i];
                if (isEmptyConfusionDone) { fb1.transform.position += Quaternion.AngleAxis(90, Vector3.forward) * LunchDir * Random.Range(-2.5f, 2.5f); }
                fb1.BlizzardLevel = BlizzardLevelByDistence[i];
                fb1.setBlizzardSize();
                fb1.ParentEmpty = this;
                //FBObjList.Add(fb1);
            }
            if (!(p2.y >= ParentPokemonRoom.RoomSize[0] || p2.y <= ParentPokemonRoom.RoomSize[1]))
            {
                FrosmothBlizzard fb2 = Instantiate(FB, transform.position, Quaternion.identity);
                fb2.transform.position -= (Vector3)LunchDir * BlizzardPosition[i];
                if (isEmptyConfusionDone) { fb2.transform.position += Quaternion.AngleAxis(90, Vector3.forward) * LunchDir * Random.Range(-2.5f, 2.5f); }
                fb2.BlizzardLevel = BlizzardLevelByDistence[i];
                fb2.setBlizzardSize();
                fb2.ParentEmpty = this;
                //FBObjList.Add(fb2);
            }
        }




    }


    //=========================攻击============================




    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■





















}
