using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//鬼斯通行动逻辑
//隐身 污泥炸弹 隐身 污泥炸弹
//隐身 暗影爪 暗影爪
//隐身 影子球
//隐身 冲刺
public class Haunter : Empty
{
    /// <summary>
    /// 鬼斯通的状态机
    /// </summary>
    enum HaunterState
    {
        Idle,       //发呆
        SludgeBomb, //污泥炸弹
        ShadowBall, //影子球
        ShadowClaw, //暗影抓
        Rush,       //冲刺
        Invisible   //隐身状态
    };
    /// <summary>
    /// 鬼斯通当前状态
    /// </summary>
    HaunterState NowState;

    /// <summary>
    /// 状态序列号 0第一次污泥炸弹 1第二次污泥炸弹 2第一次暗影抓 3第二次暗影抓 4影子球 5冲刺
    /// </summary>
    int StateIndex;



    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;
    /// <summary>
    /// 目标位置
    /// </summary>
    Vector2 TargetPosition;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ghost;//敌人第一属性
        EmptyType02 = PokemonType.TypeEnum.Poison;//敌人第二属性
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        Emptylevel = SetLevel(player.Level, MaxLevel);//设定敌人等级
        EmptyHpForLevel(Emptylevel);//设定血量
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//设定攻击力
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//设定特攻
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.2f; ;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.2f; ;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //设置朝向为下
        Director = Vector2.down;
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);
        //开始冷却期间
        IdleEnter(TIME_OF_IDLE_START);
        //设定状态序列号
        StateIndex = 0;
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
            
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//如果玩家组件丢失，重新获取
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//如果被魅惑，计算魅惑时间
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            EmptyBeKnock();//判定是否被击退
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {

                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }

                if (!isFearDone) {
                    //状态机
                    switch (NowState)
                    {
                        //发呆状态
                        case HaunterState.Idle:
                            IdleTimer -= Time.deltaTime;
                            if (IdleTimer <= 0)
                            {
                                IdleOver();
                                InvisibleByState();
                            }
                            break;
                        //隐身状态
                        case HaunterState.Invisible:
                            InvisibleTimer -= Time.deltaTime;
                            if (InvisibleTimer <= 0 && animator.GetBool("Invisible"))
                            {
                                animator.SetBool("Invisible", false);
                                MoveToPosition();
                                BackGroundMusic.StaticBGM.FadeIn(1.0f, 2.0f);
                            }
                            break;
                        //污泥炸弹状态
                        case HaunterState.SludgeBomb:
                            SludgeBombTimer -= Time.deltaTime;
                            if (SludgeBombTimer <= 0 && animator.GetBool("Atk"))
                            {
                                animator.SetBool("Atk", false);
                            }
                            break;
                        //暗影爪状态
                        case HaunterState.ShadowClaw:
                            ShadowClawTimer -= Time.deltaTime;
                            if (isShadowClawStart && Vector3.Distance(start, transform.position) <= 2.5f)
                            {
                                rigidbody2D.position = new Vector2(
                                    Mathf.Clamp(rigidbody2D.position.x
                                        + (float)Director.x * Time.deltaTime * speed * 10.5f,                    //方向*速度
                                    ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x, //最小值
                                    ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x),//最大值
                                    Mathf.Clamp(rigidbody2D.position.y
                                        + (float)Director.y * Time.deltaTime * speed * 10.5f,                     //方向*速度 
                                    ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y,  //最小值
                                    ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y));//最大值
                            }
                            if (ShadowClawTimer <= 0 && animator.GetBool("Atk"))
                            {
                                animator.SetBool("Atk", false);
                            }
                            break;
                        //影子球状态
                        case HaunterState.ShadowBall:
                            if (animator.GetBool("Atk") && ShadowBallList.Count == SHADOWBALL_COUNT + (isEmptyConfusionDone ? -2 : 0))
                            {
                                animator.SetBool("Atk", false);
                                LunchAllShadowBall();
                            }
                            break;
                        case HaunterState.Rush:
                            ShadowRushTimer -= Time.deltaTime;
                            if (isShadowRushStart)
                            {
                                rigidbody2D.position = new Vector2(
                                    Mathf.Clamp(rigidbody2D.position.x
                                        + (float)Director.x * Time.deltaTime * speed * 10.5f,                    //方向*速度
                                    ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x, //最小值
                                    ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x),//最大值
                                    Mathf.Clamp(rigidbody2D.position.y
                                        + (float)Director.y * Time.deltaTime * speed * 10.5f,                     //方向*速度 
                                    ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y,  //最小值
                                    ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y));//最大值
                            }
                            if ((ShadowRushTimer <= 0 && animator.GetBool("Atk")) || (
                                Mathf.Approximately(rigidbody2D.position.x, ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x) ||
                                Mathf.Approximately(rigidbody2D.position.x, ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x) ||
                                Mathf.Approximately(rigidbody2D.position.y, ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y) ||
                                Mathf.Approximately(rigidbody2D.position.y, ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y)
                                ))
                            {
                                animator.SetBool("Atk", false);
                            }
                            break;
                    } 
                }
                else
                {
                    switch (NowState)
                    {
                        //发呆状态
                        case HaunterState.Idle:
                            IdleTimer -= Time.deltaTime;
                            if (IdleTimer <= 0)
                            {
                                IdleOver();
                                InvisibleByState();
                            }
                            break;
                        //隐身状态
                        case HaunterState.Invisible:
                            InvisibleTimer -= Time.deltaTime;
                            if (InvisibleTimer <= 0 && animator.GetBool("Invisible"))
                            {
                                animator.SetBool("Invisible", false);
                                MoveToPosition();
                                BackGroundMusic.StaticBGM.FadeIn(1.0f, 2.0f);
                            }
                            break;
                    }
                    
                }
            }
            if ((isSleepDone || isSilence) && NowState != HaunterState.Idle)
            {
                if (NowState == HaunterState.Invisible)
                {
                    animator.SetBool("Invisible", false);
                    BackGroundMusic.StaticBGM.FadeIn(1.0f, 2.0f);
                    Invincible = false;
                    InvisibleTimer = 0;
                    if (isSleepDone) { IdleTimer = TIME_OF_IDLE_NORMALCD; }
                    else if (isSilence) { IdleTimer = 0.5f; }
                    NowState = HaunterState.Idle;
                    animator.SetTrigger("Sleep");
                }
                else
                {
                    animator.SetBool("Atk", false);
                    AtkOver();
                    animator.SetTrigger("Sleep");
                    ShadowBallList.Clear();
                    if (NowState == HaunterState.ShadowBall )
                    {
                        LunchAllShadowBall();
                    }
                }
            }
            if (isFearDone && NowState != HaunterState.Idle && NowState != HaunterState.Invisible)
            {
                animator.SetBool("Atk", false);
                AtkOver();
                animator.SetTrigger("Sleep");
                ShadowBallList.Clear();
                if (NowState == HaunterState.ShadowBall)
                {
                    LunchAllShadowBall();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {


        if (NowState == HaunterState.ShadowClaw)
        {
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                float alpha = 1.0f;
                if (playerControler != null)
                {
                    alpha = isPlayerState(playerControler) ? 1.5f : 1.0f;
                    playerControler.KnockOutPoint = 3;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, DMAGE_SHADOWCLAW * alpha, 0, 0, PokemonType.TypeEnum.Ghost);
                Debug.Log(DMAGE_SHADOWCLAW * alpha);
                GetCTEffect(other.transform);
            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, DMAGE_SHADOWCLAW, 0, 0, PokemonType.TypeEnum.Ghost);
                GetCTEffect(other.transform);
            }
        }
        else if (NowState == HaunterState.Rush)
        {
            if (other.transform.tag == ("Room") && animator.GetBool("Atk")) 
            {
                animator.SetBool("Atk", false);
            }
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                float alpha = 1.0f;
                if (playerControler != null)
                {
                    alpha = isPlayerState(playerControler) ? 1.5f : 1.0f;
                    playerControler.KnockOutPoint = 3;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, DMAGE_SHADOWRUSH * alpha, 0, 0, PokemonType.TypeEnum.Ghost);
                Debug.Log(DMAGE_SHADOWRUSH * alpha);
                GetCTEffect(other.transform);
            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, DMAGE_SHADOWRUSH, 0, 0, PokemonType.TypeEnum.Ghost);
                GetCTEffect(other.transform);
            }
        }
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












    //=============================攻击共通==============================

    /// <summary>
    /// 攻击的动作
    /// </summary>
    public void AtkActive()
    {
        switch (StateIndex)
        {
            case 0:
                SludgeBombLunch();
                break;
            case 1:
                SludgeBombLunch();
                break;

            case 2:
                ShadowClawLunch();
                break;
            case 3:
                ShadowClawLunch();
                break;

            case 4:
                ShadowBallLunch();
                break;
            case 5:
                ShadowRushLunch();
                break;
        }
    }

    /// <summary>
    /// 攻击结束后的动作
    /// </summary>
    public void AtkOver()
    {
        switch (StateIndex)
        {
            case 0:
                SludgeBombOver();
                break;
            case 1:
                SludgeBombOver();
                break;

            case 2:
                ShadowClawOver();
                break;
            case 3:
                ShadowClawOver();
                break;

            case 4:
                ShadowBallOver();
                break;
            case 5:
                ShadowRushOver();
                break;
        }
    }


    /// <summary>
    /// 角色是否处于异常状态
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    bool isPlayerState(PlayerControler player)
    {
        return (player.isToxicDone) || (player.isBurnDone) || (player.isParalysisDone) || (player.isPlayerFrozenDone) || (player.isSleepDone);
    }

    //=============================攻击共通==============================

























    //=============================隐身状态==============================
    //一般隐身时间
    //static float TIME_OF_INVISIBLE_NORMAL = 3.0f;
    //一般隐身时间
    static float TIME_OF_INVISIBLE_SLUDGEBOMB = 3.0f;
    //一般隐身时间
    static float TIME_OF_INVISIBLE_SHADOWCLAW = 3.0f;
    //一般隐身时间
    static float TIME_OF_INVISIBLE_SHADOWBALL = 3.0f;
    //一般隐身时间
    static float TIME_OF_INVISIBLE_RUSH = 1.2f;


    /// <summary>
    /// 隐身状态计时器
    /// </summary>
    float InvisibleTimer;


    void InvisibleByState()
    {
        switch (StateIndex)
        {
            case 0:
                InvisibleEnter(TIME_OF_INVISIBLE_SLUDGEBOMB);
                break;
            case 1:
                InvisibleEnter(TIME_OF_INVISIBLE_SLUDGEBOMB);
                break;
            case 2:
                InvisibleEnter(TIME_OF_INVISIBLE_SHADOWCLAW);
                break;
            case 3:
                InvisibleEnter(TIME_OF_INVISIBLE_SHADOWCLAW);
                break;
            case 4:
                InvisibleEnter(TIME_OF_INVISIBLE_SHADOWBALL);
                break;
            case 5:
                InvisibleEnter(TIME_OF_INVISIBLE_RUSH);
                break;
        }
    }

    /// <summary>
    /// 进入隐身状态
    /// </summary>
    /// <param name="time">隐身时间</param>
    void InvisibleEnter(float time)
    {
        Invincible = true;
        InvisibleTimer = time;
        animator.SetBool("Invisible", true);
        NowState = HaunterState.Invisible;

    }

    void BGMSlience()
    {
        BackGroundMusic.StaticBGM.FadeOut(0.1f, 2.0f);
    }

    /// <summary>
    /// 结束隐身状态
    /// </summary>
    void InvisibleOver()
    {
        Invincible = false;
        InvisibleTimer = 0;
        if (isFearDone)
        {
            IdleEnter(TIME_OF_IDLE_NORMALCD);
            return;
        }

        switch (StateIndex)
        {
            case 0:
                SludgeBombEnter();
                break;
            case 1:
                SludgeBombEnter();
                break;

            case 2:
                ShadowClawEnter();
                break;
            case 3:
                ShadowClawEnter();
                break;

            case 4:
                ShadowBallEnter();
                break;
            case 5:
                ShadowRushEnter();
                break;
        }
    }




    /// <summary>
    /// 隐身后移动
    /// </summary>
    public void MoveToPosition()
    {
        Vector2 target;
        if (StateIndex == 0 || StateIndex == 1)//0第一次污泥炸弹 1第二次污泥炸弹
        {
            target = MoveToMedium();
        }
        else if (StateIndex == 4)//4影子球
        {
            target = MoveToFar();
        }
        else// 2第一次暗影抓 3第二次暗影抓5冲刺
        {
            if (isEmptyConfusionDone)
            {
                target = MoveToMedium();
            }
            else
            {
                target = MoveToBack();
            }
        }
        if (isFearDone) { target = FaerMove(); }
        transform.position = target;
        Director = _mTool.MainVector2((TargetPosition - (Vector2)transform.position));
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);

    }


    /// <summary>
    /// 移动到远距离
    /// Vector2summary>
    Vector2 MoveToFar()
    {
        if (TargetPosition.x < ParentPokemonRoom.transform.position.x)
        { return new Vector2(9.5f, 0.25f) + (Vector2)ParentPokemonRoom.transform.position; }
        else
        { return new Vector2(-9.5f, 0.25f) + (Vector2)ParentPokemonRoom.transform.position; }
    }


    static float MIN_MEDIUM_RADIUS = 4.2f;
    static float MAX_MEDIUM_RADIUS = 5.8f;
    /// <summary>
    /// 移动到中距离
    /// </summary>
    Vector2 MoveToMedium()
    {
        Vector2 output;
        do
        {
            // 随机半径和角度
            float radius = Random.Range(MIN_MEDIUM_RADIUS, MAX_MEDIUM_RADIUS);
            float angle = Random.Range(0, Mathf.PI * 2);

            // 计算点坐标
            float x = Mathf.Cos(angle) * radius + TargetPosition.x;
            float y = Mathf.Sin(angle) * radius + TargetPosition.y;

            output = new Vector3(x, y);


            // 检查距离是否在半径范围内
            if (ParentPokemonRoom.isPointInRoon(output, 0.75f))
            {
                return output;
            }
        } while (true);
    }


    /// <summary>
    /// 移动到目标后背
    /// </summary>
    Vector2 MoveToBack()
    {
        Vector2 output = Vector2.up;
        if (!isEmptyInfatuationDone)
        {
            output = -player.look * 1.7f + TargetPosition + Vector2.up * 0.3f;
            if (ParentPokemonRoom.isPointInRoon(output, 0.75f))
            {
                //Debug.Log("look" + ParentPokemonRoom.isPointInRoon(output, 0.75f));
                return output;
            }
        }
        float WallWeight = 0.75f;
        while (WallWeight >= 0)
        {
            List<Vector2> vList = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            while (vList.Count != 0)
            {
                output = vList[Random.Range(0, vList.Count)];
                vList.Remove(output);
                output = output * 1.7f + TargetPosition + Vector2.up * 0.3f;
                if (ParentPokemonRoom.isPointInRoon(output, WallWeight))
                {
                    return output;
                }
            }
            WallWeight -= 0.25f;
        }
        return output;
    }


    Vector2 FaerMove()
    {
        Vector2 output = new Vector2(((Random.Range(0.0f, 1.0f) > 0.5f) ? -1 : 1) , ((Random.Range(0.0f, 1.0f) > 0.5f) ? -1 : 1)) * 100.0f;
        output = new Vector2(
            Mathf.Clamp(output.x + transform.parent.position.x,                    //方向*速度
            ParentPokemonRoom.RoomSize[2] + 1.3f + transform.parent.position.x, //最小值
            ParentPokemonRoom.RoomSize[3] - 1.3f + transform.parent.position.x),//最大值
            Mathf.Clamp(output.y + transform.parent.position.y,                     //方向*速度 
            ParentPokemonRoom.RoomSize[1] + 1.3f + transform.parent.position.y,  //最小值
            ParentPokemonRoom.RoomSize[0] - 1.3f + transform.parent.position.y));//最大值
        return output;
    }


    //=============================隐身状态==============================

























    //=============================发呆状态========================================

    //出生后的冷却时间
    static float TIME_OF_IDLE_START = 0.2f;
    //使用完一般技能后的冷却时间
    static float TIME_OF_IDLE_NORMALCD = 4.0f;
    //使用完污泥炸弹后的冷却时间
    static float TIME_OF_IDLE_SLUDGEBOMBCD = 4.5f;
    //使用完影子球后的冷却时间
    static float TIME_OF_IDLE_SHADOWBALLCD = 1.0f;
    //使用完暗影抓后的冷却时间
    static float TIME_OF_IDLE_SHADOWCLAWCD = 1.0f;
    //使用完冲刺后的冷却时间
    static float TIME_OF_IDLE_RUSHCD = 10.0f;

    /// <summary>
    /// 发呆时间计时器
    /// </summary>
    float IdleTimer;

    /// <summary>
    /// 进入发呆状态
    /// </summary>
    /// <param name="idletime">发呆时间</param>
    void IdleEnter(float idletime)
    {
        if (idletime != TIME_OF_IDLE_START) {
            StateIndex++;
            if (StateIndex > 5) { StateIndex = 0; }
        }
        IdleTimer = idletime;
        NowState = HaunterState.Idle;
    }

    /// <summary>
    /// 结束发呆状态
    /// </summary>
    void IdleOver()
    {
        IdleTimer = 0;
    }

    //=============================发呆状态==============================




















    //=========污泥炸弹状态==========

    //污泥炸弹的攻击
    static float TIME_OF_ATK_SLUDGEBOMB = 0.2f;

    /// <summary>
    /// 污泥炸弹时间计时器
    /// </summary>
    float SludgeBombTimer;

    /// <summary>
    /// 污泥炸弹预制件
    /// </summary>
    public HaunterSludgeBomb sludgeBomb;

    /// <summary>
    /// 开始污泥炸弹状态
    /// </summary>
    void SludgeBombEnter()
    {
        animator.SetBool("Atk", true);
        SludgeBombTimer = TIME_OF_ATK_SLUDGEBOMB;
        NowState = HaunterState.SludgeBomb;
    }

    /// <summary>
    /// 结束污泥炸弹状态
    /// </summary>
    void SludgeBombOver()
    {
        SludgeBombTimer = 0;
        IdleEnter(TIME_OF_IDLE_SLUDGEBOMBCD);
    }

    /// <summary>
    /// 发射污泥炸弹
    /// </summary>
    void SludgeBombLunch()
    {
        float Angle = isEmptyConfusionDone ? 45.0f : 22.0f;
        Vector2 dir1 = (TargetPosition - (Vector2)transform.position).normalized;
        Vector2 dir2 = Quaternion.AngleAxis( Angle, Vector3.forward) * dir1;
        Vector2 dir3 = Quaternion.AngleAxis(-Angle, Vector3.forward) * dir1;
        float distence = Vector2.Distance(TargetPosition, (Vector2)transform.position);
        HaunterSludgeBomb sb1 = Instantiate(sludgeBomb, transform.position + Vector3.up * 0.3f + (Vector3)dir1 * 1.2f, Quaternion.identity, ParentPokemonRoom.transform);
        HaunterSludgeBomb sb2 = Instantiate(sludgeBomb, transform.position + Vector3.up * 0.3f + (Vector3)dir2 * 1.2f, Quaternion.identity, ParentPokemonRoom.transform);
        HaunterSludgeBomb sb3 = Instantiate(sludgeBomb, transform.position + Vector3.up * 0.3f + (Vector3)dir3 * 1.2f, Quaternion.identity, ParentPokemonRoom.transform);
        sb1.SetNewSludgeBomb(this, dir1, distence * 0.8f);
        sb2.SetNewSludgeBomb(this, dir2, distence * 1.5f);
        sb3.SetNewSludgeBomb(this, dir3, distence * 1.5f);
    }

    //=========污泥炸弹状态==========






























    //=========暗影爪状态==========
    //暗影爪的攻击
    static float TIME_OF_ATK_SHADOWCLAW = 0.45f;

    //暗影爪的攻击力
    public static int DMAGE_SHADOWCLAW = 80;

    /// <summary>
    /// 暗影爪时间计时器
    /// </summary>
    float ShadowClawTimer;

    /// <summary>
    /// 暗影爪预制件
    /// </summary>
    public HaunterShadowClaw shadowClaw;

    //暗影爪是否启动
    bool isShadowClawStart;

    /// <summary>
    /// 开始暗影爪状态
    /// </summary>
    void ShadowClawEnter()
    {
        animator.SetBool("Atk", true);
        ShadowClawTimer = TIME_OF_ATK_SHADOWCLAW;
        NowState = HaunterState.ShadowClaw;
        Debug.Log("XXX" + ShadowClawTimer);
    }

    /// <summary>
    /// 结束暗影爪状态
    /// </summary>
    void ShadowClawOver()
    {
        ShadowClawTimer = 0;
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        isShadowClawStart = false;
        IdleEnter(TIME_OF_IDLE_SHADOWCLAWCD);
    }

    Vector3 start;

    /// <summary>
    /// 发射暗影爪
    /// </summary>
    void ShadowClawLunch()
    {
        HaunterShadowClaw sc = Instantiate(shadowClaw, transform.position, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Director, Vector2.right) + 90.0f), transform);
        sc.ParentHaunter = this;
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
        isShadowClawStart = true;
        start = transform.position;
        Debug.Log("XXX" + ShadowClawTimer);
    }

    //=========暗影爪状态==========






























    //=========影子球状态==========

    //影子球发射的最大角度
    static float SHADOWBALL_MAX_ANGLE = 50.0f;
    //影子球发射的最大角度
    static float SHADOWBALL_SPACING = 0.08f;
    //影子球发射次数
    static int SHADOWBALL_COUNT = 9;
    //影子球速度
    static float SHADOWBALL_SPEED = 9.0f;


    /// <summary>
    /// 影子球预制件
    /// </summary>
    public HaunterShadowBall shadowBall;

    List<HaunterShadowBall> ShadowBallList = new List<HaunterShadowBall> { };

    /// <summary>
    /// 开始影子球状态
    /// </summary>
    void ShadowBallEnter()
    {
        animator.SetBool("Atk", true);
        NowState = HaunterState.ShadowBall;
    }

    /// <summary>
    /// 结束影子球状态
    /// </summary>
    void ShadowBallOver()
    {
        IdleEnter(TIME_OF_IDLE_SHADOWBALLCD);
    }

    /// <summary>
    /// 发射影子球
    /// </summary>
    void ShadowBallLunch()
    {
        //sb1.LaunchNotForce(Director, 10);
        ShadowBallList.Clear();
        Vector2 v = new Vector2(Director.x, ((TargetPosition - (Vector2)transform.position).y > 0) ? 1.0f : -1.0f);
        //Debug.Log(v + string.Join("," , ).ToString());
        List<float> AngleList = ShadowBallAngle(SHADOWBALL_COUNT, SHADOWBALL_MAX_ANGLE, v);
        StartCoroutine(InstantiateShadowBall(AngleList, SHADOWBALL_SPACING));
    }



    IEnumerator InstantiateShadowBall(List<float> AngleList, float Spacing)
    {
        for (int i = 0; i < AngleList.Count; i++)
        {
            Vector3 v = Quaternion.AngleAxis(AngleList[i], Vector3.forward) * Vector3.right;
            HaunterShadowBall sb1 = Instantiate(shadowBall, transform.position + v * 2.5f, Quaternion.identity, ParentPokemonRoom.transform);
            sb1.empty = this;
            ShadowBallList.Add(sb1);
            sb1.direction = v;
            yield return new WaitForSeconds(Spacing);
        }
        StopCoroutine(InstantiateShadowBall(AngleList, Spacing));
    }

    /// <summary>
    /// 发射所有的影子球
    /// </summary>
    void LunchAllShadowBall()
    {
        for (int i = 0; i < ShadowBallList.Count; i++)
        {
            ShadowBallList[i].LaunchNotForce(ShadowBallList[i].direction , SHADOWBALL_SPEED);
        }
    }


    /// <summary>
    /// 影子球所有的发射角度
    /// </summary>
    /// <param name="Count">影子球发射的次数</param>
    /// <param name="MaxAngle">影子球的最大角度</param>
    /// <param name="StartQuadrant">影子球发射的初始象限</param>
    /// <returns></returns>
    List<float> ShadowBallAngle(int Count , float MaxAngle , Vector2 StartQuadrant)
    {
        int SBCount = isEmptyConfusionDone ? (Count - 2) : Count;
        List<float> output = new List<float> { };
        float PerAngle = (MaxAngle * 2.0f) / (float)(SBCount - 1);
        float StartAngle = MaxAngle;
        if      (StartQuadrant.x > 0 && StartQuadrant.y > 0) { PerAngle = -PerAngle; }
        else if (StartQuadrant.x > 0 && StartQuadrant.y < 0) { StartAngle = -StartAngle; }
        else if (StartQuadrant.x < 0 && StartQuadrant.y > 0) { StartAngle = -(StartAngle + 180.0f); }
        else if (StartQuadrant.x < 0 && StartQuadrant.y < 0) { StartAngle = (StartAngle + 180.0f); PerAngle = -PerAngle; }
        for (int i = 0; i < SBCount; i++)
        {
            output.Add(StartAngle + ((float)i)* PerAngle);
        }
        return output;
    }


    //=========影子球状态==========














    //=========冲刺状态==========

    //暗影冲刺的攻击力
    public static int DMAGE_SHADOWRUSH = 110;
    //暗影冲刺的攻击时间
    static float TIME_OF_ATK_SHADOWRUSH = 4.0f;

    /// <summary>
    /// 暗影冲刺时间计时器
    /// </summary>
    float ShadowRushTimer;

    /// <summary>
    /// 暗影冲刺预制件
    /// </summary>
    public HaunterShadowClaw shadowRush;

    //暗影冲刺是否启动
    bool isShadowRushStart;

    /// <summary>
    /// 开始暗影冲刺状态
    /// </summary>
    void ShadowRushEnter()
    {
        animator.SetBool("Atk", true);
        ShadowRushTimer = TIME_OF_ATK_SHADOWRUSH;
        NowState = HaunterState.Rush;
    }

    /// <summary>
    /// 结束暗影冲刺状态
    /// </summary>
    void ShadowRushOver()
    {
        ShadowClawTimer = 0;
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        isShadowRushStart = false;
        ShadowBallList.Clear();
        IdleEnter(TIME_OF_IDLE_RUSHCD);
    }


    /// <summary>
    /// 发射暗影冲刺
    /// </summary>
    void ShadowRushLunch()
    {
        HaunterShadowClaw sc = Instantiate(shadowClaw, transform.position, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Director, Vector2.right) + 90.0f), transform);
        sc.ParentHaunter = this;
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
        isShadowRushStart = true;
    }
    //=========冲刺状态==========



















}
