using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golbat : Empty
{

    //大嘴夫状态机
    enum State
    {
        Normal,  //一般巡逻状态
        Sonic,  //释放超音波状态
        Rush,   //冲刺状态
        CDIdle, //技能冷却发呆期间
    }
    State NowState;



    /// <summary>
    /// 目标位置
    /// </summary>
    Vector3 TargetPosition;


    /// <summary>
    /// 移动朝向
    /// </summary>
    Vector3 MoveDirector;

    /// <summary>
    /// 撞墙次数
    /// </summary>
    int TurnCount;
    /// <summary>
    /// 撞墙CD cd期间撞墙不会转向
    /// </summary>
    bool isTurnCD;

    /// <summary>
    /// 超音波
    /// </summary>
    public ZubatSupersonic Sonic;
    /// <summary>
    /// 释放超音波的距离
    /// </summary>
    float SONIC_DISTANCE = 6.7f;

    /// <summary>
    /// 是否开始冲刺
    /// </summary>
    bool isRush;
    /// <summary>
    /// 冲刺方向
    /// </summary>
    Vector2 RushDir;
    /// <summary>
    /// 冲刺时间计时器
    /// </summary>
    float RushTimer = 0.0f;


    /// <summary>
    /// 毒牙
    /// </summary>
    public GameObject PosionBite;
    /// <summary>
    /// 毒牙动画的实体
    /// </summary>
    GameObject PosionBiteGOBJ;
    /// <summary>
    /// 冲刺完的冷却时间
    /// </summary>
    public float TIME_PosionBiteCD = 8.0f;
    /// <summary>
    /// 冷却时间计时器
    /// </summary>
    float CDTimer;


    Vector2 Director;//敌人朝向
    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Poison;//敌人第一属性
        EmptyType02 = PokemonType.TypeEnum.Flying;//敌人第二属性
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        Emptylevel = SetLevel(player.Level, MaxLevel);//设定敌人等级
        EmptyHpForLevel(Emptylevel);//设定血量
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//设定攻击力
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//设定特攻
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //初始化
        SetNormalState();
    }

    void SetNormalState()
    {
        //初始化状态机
        NowState = State.Normal;
        StopShadowCoroutine();
        CDTimer = 0.0f;
        RushTimer = 0.0f;
        isRush = false;
        TurnCount = 0;
        isTurnCD = false;
        if (PosionBiteGOBJ != null) { Destroy(PosionBiteGOBJ.gameObject); }
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

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (animator.speed == 0) { animator.speed = 1; }
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }
                switch (NowState)
                {
                    case State.Normal:

                        //移动方向为与大嘴夫和玩家连线的正交90°
                        MoveDirector = ((Vector2)TargetPosition - (Vector2)transform.position).normalized;
                        float Angle = _mTool.Angle_360Y(MoveDirector, Vector2.right);
                        //朝向为移动方向的斜方向正规化
                        if (Angle >= 0 && Angle < 90) { Director.x = 1; Director.y = 1; }
                        else if (Angle >= 90 && Angle < 180) { Director.x = -1; Director.y = 1; }
                        else if (Angle >= 180 && Angle < 270) { Director.x = -1; Director.y = -1; }
                        else { Director.x = 1; Director.y = -1; }
                        animator.SetFloat("LookX", Director.x); animator.SetFloat("LookY", Director.y);

                        //========释放超音波部分========
                        //当玩家和大嘴夫之间的角度大约在45°时 且距离小于SONIC_DISTANCE时，释放超越波
                        //玩家和大嘴夫之间的角度
                        float ABSAngleForPlayerAndGolbat = Mathf.Abs(((Mathf.Abs(MoveDirector.x)) / (Mathf.Abs(MoveDirector.y))) - 1);
                        if (!isFearDone)
                        {
                            if (ABSAngleForPlayerAndGolbat <= 0.1f && (TargetPosition - transform.position).magnitude <= SONIC_DISTANCE)
                            {
                                animator.SetTrigger("Sonic");
                                TurnCount = 0;
                                NowState = State.Sonic;
                            }
                        }




                        //========移动部分========
                        //精细化移动方向 根据撞墙次数正反转 恐惧时会远离
                        MoveDirector = Quaternion.AngleAxis(
                            (isFearDone ? (TurnCount % 2 == 0 ? 1 : -1) * 105 : (TurnCount % 2 == 0 ? 1 : -1) * (90 - (TurnCount * 7.5f))), Vector3.forward) * MoveDirector;    
                        rigidbody2D.position = new Vector2(
                            Mathf.Clamp(rigidbody2D.position.x 
                                + (float)MoveDirector.x * Time.deltaTime * speed                //方向*速度
                                * (Mathf.Pow(1.05f, TurnCount)) * (isFearDone ? 1.4f : 1),      //撞墙次数越多越快 恐惧时更快
                            ParentPokemonRoom.RoomSize[2] - 1.5f + transform.parent.position.x, //最小值
                            ParentPokemonRoom.RoomSize[3] + 1.5f + transform.parent.position.x),//最大值
                            Mathf.Clamp(rigidbody2D.position.y
                                + (float)MoveDirector.y * Time.deltaTime * speed                 //方向*速度 
                                * (Mathf.Pow(1.05f, TurnCount)) * (isFearDone ? 1.4f : 1),       //撞墙次数越多越快 恐惧时更快 
                            ParentPokemonRoom.RoomSize[1] - 1.5f + transform.parent.position.y,  //最小值
                            ParentPokemonRoom.RoomSize[0] + 1.5f + transform.parent.position.y));//最大值

                        

                        break;

                    //冲刺状态
                    case State.Rush:
                        //冲刺移动
                        if (isRush)
                        {
                            RushTimer += Time.deltaTime;
                            rigidbody2D.position = new Vector2(
                            Mathf.Clamp(rigidbody2D.position.x
                                + (float)RushDir.x * Time.deltaTime * speed * 3.4f,                    //方向*速度
                            ParentPokemonRoom.RoomSize[2] - 0.5f + transform.parent.position.x, //最小值
                            ParentPokemonRoom.RoomSize[3] + 0.5f + transform.parent.position.x),//最大值
                            Mathf.Clamp(rigidbody2D.position.y
                                + (float)RushDir.y * Time.deltaTime * speed * 3.4f,                     //方向*速度 
                            ParentPokemonRoom.RoomSize[1] - 0.5f + transform.parent.position.y,  //最小值
                            ParentPokemonRoom.RoomSize[0] + 0.5f + transform.parent.position.y));//最大值
                        }
                        if (RushTimer >= 1.8f)
                        {
                            RushOver();
                        }

                        break;
                    case State.CDIdle:
                        //毒牙冲刺结束结束后，计算发呆cd时间
                        if (NowState == State.CDIdle)
                        {
                            if (CDTimer > 0) { CDTimer -= Time.deltaTime; if (CDTimer <= 0) { CDTimer = 0; } }//计算cd
                            if (CDTimer == 0 && !isFearDone)
                            {
                                SetNormalState();
                            }
                        }
                        break;

                }
            }
            if ((isSleepDone || isSilence || isFearDone ) && NowState != State.Normal)
            {
                SetNormalState();
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
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//未被魅惑 且与玩家碰撞时
        {
            //冲刺期间毒牙伤害
            if (NowState == State.Rush)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 50, 0, 0, PokemonType.TypeEnum.Poison);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 7.0f;
                    playerControler.KnockOutDirection = (new Vector2(RushDir.y , RushDir.x)).normalized;
                    if (Random.Range(0.0f, 1.0f) > 0.0f) { playerControler.ToxicFloatPlus(0.4f); }
                }
            }
            else
            {
                EmptyTouchHit(other.gameObject);//触发触碰伤害
            }
        }
        else if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//被魅惑 且与其他敌人碰撞时
        {
            //冲刺期间毒牙伤害
            if (NowState == State.Rush)
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, 50, 0, 0, PokemonType.TypeEnum.Poison);
                e.EmptyToxicDone(1f, 5, 0.5f);
            }
            else
            {
                InfatuationEmptyTouchHit(other.gameObject);//触发魅惑后触碰伤害
            }
        }
        else if (other.transform.tag == ("Room"))
        {
            //巡逻状态撞墙折返
            if (NowState == State.Normal)
            {
                if (!isTurnCD)
                {
                    Debug.Log(other.gameObject.name);
                    TurnCount++;
                    isTurnCD = true;
                    StartCoroutine(TurnColdDown());
                }
            }
            //冲刺期间撞墙结束冲刺
            if (NowState == State.Rush)
            {
                RushOver();
            }
        }
    }


    /// <summary>
    /// 冲刺结束
    /// </summary>
    void RushOver()
    {
        isRush = false;
        RushTimer = 0.0f;
        StopShadowCoroutine();
        NowState = State.CDIdle;
        CDTimer = TIME_PosionBiteCD;
    }


    /// <summary>
    /// 撞墙冷却
    /// </summary>
    /// <returns></returns>
    IEnumerator TurnColdDown()
    {
        yield return new WaitForSeconds(0.1f);
        isTurnCD = false;
    }



    /// <summary>
    /// 释放超音波 仅通过动画机调用
    /// </summary>
    public void SuperSonic()
    {
        if (!isDie && !isBorn && !isEmptyFrozenDone && !isSleepDone && !isFearDone && !isSilence)
        {
            Vector2 SonicDir = _mTool.TiltMainVector2((Vector2)TargetPosition - (Vector2)transform.position);
            //混乱
            if (isEmptyConfusionDone) { SonicDir = (Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.forward) * SonicDir).normalized; }
            ZubatSupersonic s = Instantiate(Sonic, rigidbody2D.position + new Vector2(0, 0.6f) + SonicDir * 0.75f, Quaternion.identity);
            s.transform.rotation = Quaternion.Euler(0, 0, (TargetPosition.y - transform.position.y <= 0 ? -1 : 1) * Vector2.Angle(SonicDir, new Vector2(1, 0)));
            s.ParentZubat = this;
            s.transform.GetChild(0).GetComponent<ZubatSupersonic>().ParentZubat = this;
        }
    }

    /// <summary>
    /// 结束超音波状态 开始毒牙 仅通过动画机调用
    /// </summary>
    public void SuperSonicOver()
    {
        NowState = State.Rush;
        isRush = true;
        RushDir = _mTool.MainVector2((Vector2)TargetPosition - (Vector2)transform.position);
        //混乱
        if (isEmptyConfusionDone) { RushDir = (Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.forward) * RushDir).normalized; }
        StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        StartCoroutine(InstantiatePosionBite(0.20f));
    }

    IEnumerator InstantiatePosionBite(float waittime)
    {

            yield return new WaitForSeconds(waittime);
        if (isRush && !isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence  && !isFearDone)
        {
            PosionBiteGOBJ = Instantiate(PosionBite, transform.position + Vector3.up * 0.6f, Quaternion.identity, transform);
        }
    }





}
