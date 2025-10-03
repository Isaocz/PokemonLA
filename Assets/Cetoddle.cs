using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cetoddle : Empty
{

    MyAstarAI AI;//寻路A*ai




    Vector2 Director;//敌人朝向
    Vector3 LastPosition;//计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行

    float DanceStartCDTimer;//跳尾巴舞使用后下次使用所需的的cd冷却计时器
    float TIMEofDanceStartCD = 1.5f;//跳尾巴舞使用后下次使用所需的的cd冷却时间

    float CDTimer;//使用技能后原地发呆的计时器
    float TIMEofDanceOverCD = 5.0f;//跳尾巴舞使用后原地发呆的的cd冷却时间
    float TIMEofSingOverCD = 9.0f;//唱歌使用后原地发呆的的cd冷却时间

    //跳舞时的技能圈
    public CetoddleDanceCircle danceCircle;
    CetoddleDanceCircle danceCircleObj;

    //唱歌时的技能圈
    public EmptyEchoedVoice singCircle;
    EmptyEchoedVoice singCircleObj;

    enum State //状态机
    {
        Normal,//平常状态，追击敌人
        Sing,//唱歌状态
        Dance,//跳舞（摇尾巴）状态
        CDState//技能结束后冷却状态
    };
    State NowState;//当前状态


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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验


        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //获取寻路ai
        AI = transform.GetComponent<MyAstarAI>();
        //启动计算方向携程
        StartCoroutine(CheckLook());
        //初始化状态机
        NowState = State.Normal;

        animator.SetFloat("Speed", 0.0f);
        animator.SetFloat("LookX", 0.0f);
        animator.SetFloat("LookY", -1.0f);



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
        }
    }

    private void FixedUpdate()
    {
        ResetPlayer();//如果玩家组件丢失，重新获取
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//如果被魅惑，计算魅惑时间
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            EmptyBeKnock();//判定是否被击退

            //不处于睡眠 冰冻 沉默状态时
            if (!isSleepDone &&!isEmptyFrozenDone && !isSilence && !(isFearDone && NowState != State.Normal))
            {
                if (!isCanNotMoveWhenParalysis)
                {
                    //一般状态时计算跳舞cd时间
                    if (NowState == State.Normal)
                    {
                        if (DanceStartCDTimer > 0) { DanceStartCDTimer -= Time.deltaTime; if (DanceStartCDTimer <= 0) { DanceStartCDTimer = 0; } }//计算cd
                        if (DanceStartCDTimer == 0 && (AI.targetPosition.position - transform.position).magnitude <= 4.2f && !AI.isCanNotMove && !isFearDone)
                        {
                            DanceStartCDTimer = TIMEofDanceStartCD;
                            AI.isCanNotMove = true;
                            animator.SetBool("Dance" , true);
                            NowState = State.Dance;
                            animator.SetFloat("Speed", 0);
                        }
                    }

                    //跳舞结束后，计算发呆cd时间
                    if (NowState == State.CDState)
                    {
                        if (CDTimer > 0) { CDTimer -= Time.deltaTime; if (CDTimer <= 0) { CDTimer = 0; } }//计算cd
                        if (CDTimer == 0 && !isFearDone)
                        {
                            AI.isCanNotMove = false;
                            NowState = State.Normal;
                        }
                    }
                }

            }
            else if (isFearDone && NowState != State.Normal)
            {
                if (NowState == State.Dance)
                {
                    animator.SetBool("Dance", false);
                    NowState = State.Normal;
                }
                if (NowState == State.Sing)
                {
                    animator.SetBool("Sing", false);
                    NowState = State.CDState;
                }
                if (danceCircleObj != null) { Destroy(danceCircleObj.gameObject); }
                if (singCircleObj != null) { Destroy(singCircleObj.gameObject); }
                animator.SetBool("Dance", false);
                animator.SetBool("Sing", false);
                AI.isCanNotMove = false;
                NowState = State.Normal;
            }
            //睡眠 冰冻 沉默时不进行动作
            else
            {
                if (danceCircleObj != null) { Destroy(danceCircleObj.gameObject); }
                if (singCircleObj != null) { Destroy(singCircleObj.gameObject); }
                if (NowState == State.Sing) { SetSingOver(); }
                animator.SetBool("Dance", false);
                animator.SetBool("Sing", false);
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && !AI.isCanNotMove && NowState == State.Normal)
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

    public void CallCanMove()
    {
        //AI.canMove = true;
        animator.SetFloat("Speed", 0.0f);
    }


    //=========================摇尾巴跳舞相关=============================

    public void SetDanceOver()
    {
        if (NowState == State.Dance)
        {
            animator.SetBool("Dance", false);
            NowState = State.CDState;
            CDTimer = TIMEofDanceOverCD;
            
        }
    }

    //开始跳舞 放置技能圈
    public void StartDance()
    {
        if (animator.GetBool("Dance")) {
            danceCircleObj = Instantiate(danceCircle, this.transform.position, Quaternion.identity, transform);
            danceCircleObj.ParentEmpty = this;
        }
    }

    //=========================摇尾巴跳舞相关=============================









    //=========================唱歌回声相关=============================

    /// <summary>
    /// 使用回声
    /// </summary>
    public override void UseEchoedVoice(int echoedVoiceLevel)
    {
        Debug.Log(name + "+" + transform.position + "+" + echoedVoiceLevel);
        animator.SetBool("Sing" , true);
        NowState = State.Sing;
        AI.isCanNotMove = true;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("LookX", 0);
        animator.SetFloat("LookY", -1);

        singCircleObj = Instantiate(singCircle, this.transform.position, Quaternion.identity, transform);
        singCircleObj.SetEchoedVoiceLevel(echoedVoiceLevel);
        singCircleObj.ParentEmpty = this;
    }

    public void SetSingOver()
    {
        if (NowState == State.Sing)
        {
            animator.SetBool("Sing", false);
            NowState = State.CDState;
            CDTimer = TIMEofSingOverCD;

        }
    }


    /// <summary>
    /// 判定当前敌人是否可以回声
    /// </summary>
    public override bool isEchoedVoiceisReady()
    {
        if (NowState == State.Normal && !isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) { return true; }
        else { return false; }
    }

    //=========================唱歌回声相关=============================
}
