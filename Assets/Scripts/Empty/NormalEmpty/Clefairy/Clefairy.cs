using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//皮皮
//Dance全体跳舞 月光
//Skill 重力 加盾 撒娇
//轮唱
public class Clefairy : Empty
{
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




    //==============================状态机枚举===================================

    /// <summary>
    /// 主状态
    /// </summary>
    enum MainState
    {
        Idle,   //发呆
        Sing,   //唱歌
        Dance,  //跳舞回血
        Skill,  //随机使用技能

    }
    MainState NowState;

    //==============================状态机枚举===================================



    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Fairy;//敌人第一属性
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

        //设置初始方向
        SetDirector(new Vector2(-1, -1));


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

            //Debug.Log(name + "+" + (!isEmptyFrozenDone)+"+"+(!isSleepDone) + "+" + (!isSilence) + "+" + (!isCanNotMoveWhenParalysis));
            //■■开始判断状态机 当处于冰冻 睡眠 致盲 麻痹状态时状态机停运
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) /* TODO状态机停运的额外条件 */
            {
                float ConfusionAlpha = isEmptyConfusionDone ? 0.75f : 1.0f;
                switch (NowState)
                {
                    //发呆状态
                    case MainState.Idle:
                        IdleTimer -= Time.deltaTime * ConfusionAlpha;//发呆计时器时间减少
                        //Debug.Log(name + IdleTimer);
                        if ( IdleTimer <= 0 )         //计时器时间到时间，结束发呆状态
                        {
                            IdleOver();
                            SingStart(TIME_SING_JUDGE);
                        }
                        break;
                    //唱歌状态
                    case MainState.Sing:
                        SingTimer -= Time.deltaTime * ConfusionAlpha;//唱歌计时器时间减少
                        if ( SingTimer <= 0 )         //计时器时间到时间，结束唱歌状态
                        {
                            SingOver();
                            
                        }
                        break;
                    //跳舞回血状态
                    case MainState.Dance:
                        DanceTimer -= Time.deltaTime * ConfusionAlpha;//跳舞回血计时器时间减少
                        if ( DanceTimer <= 0 )         //计时器时间到时间，结束跳舞回血状态
                        {
                            DanceOver();
                            //TODO添加下一个状态的开始方法
                        }
                        break;
                    //随机使用技能状态
                    case MainState.Skill:
                        SkillTimer -= Time.deltaTime * ConfusionAlpha;//随机使用技能计时器时间减少
                        if ( SkillTimer <= 0 )         //计时器时间到时间，结束随机使用技能状态
                        {
                            SkillOver();
                            IdleStart(TIME_IDLE_SING);
                        }
                        break;
                }
            }
            if ((isEmptyFrozenDone || isSleepDone || isSilence || isFearDone) && NowState != MainState.Idle)
            {
                DestorySingObj();
                DestorySkill();
                animator.SetTrigger("Sleep");
                switch (NowState)
                {
                    //唱歌状态
                    case MainState.Sing:
                        SingOver();
                        break;
                    //跳舞回血状态
                    case MainState.Dance:
                        DanceOver();
                        break;
                    //随机使用技能状态
                    case MainState.Skill:
                        SkillOver();
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

            /**
            //根据魅惑情况确实目标位置
            if (!isEmptyInfatuationDone || _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile()).Count <= 1 || InfatuationForDistanceEmpty() == null)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }
            **/
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
        if (NowState != MainState.Sing) {
            Director = director;
            animator.SetFloat("LookX", director.x);
            animator.SetFloat("LookY", director.y);
        }
    }


    //InsertSubStateChange
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■
    













    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //=========================发呆============================


    //开始后的冷却时间
    static float TIME_IDLE_START = 0.5f; //TODO需修改时间

    //唱歌后的冷却时间
    static float TIME_IDLE_SING = 3.0f; //TODO需修改时间


    //跳舞回血后的冷却时间
    static float TIME_IDLE_DANCE = 3.0f; //TODO需修改时间


    //随机使用技能后的冷却时间
    static float TIME_IDLE_SKILL = 0.0f; //TODO需修改时间



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






    //=========================唱歌============================


    /// <summary>
    /// 唱歌时间
    /// </summary>
    static float TIME_SING = 4.5f;

    /// <summary>
    /// 唱歌的判定时间
    /// </summary>
    static float TIME_SING_JUDGE = 0.5f;

    /// <summary>
    /// 是否使用了回音
    /// </summary>
    bool isUsedEchoedVoice = false;

    /// <summary>
    /// 唱歌计时器
    /// <summary>
    float SingTimer = 0;

    //唱歌时的技能圈
    public EmptyEchoedVoice singCircle;
    EmptyEchoedVoice singCircleObj;

    /// <summary>
    /// 使用回声
    /// </summary>
    public override void UseEchoedVoice(int echoedVoiceLevel)
    {
        SingTimer += TIME_SING;
        isUsedEchoedVoice = true;
        Debug.Log(name + "+" + transform.position + "+" + echoedVoiceLevel);
        animator.SetTrigger("Sing");
        //SingStart(TIME_SING);
        SetDirector(new Vector2(-1,-1));

        singCircleObj = Instantiate(singCircle, this.transform.position, Quaternion.identity, transform);
        singCircleObj.SetEchoedVoiceLevel(echoedVoiceLevel);
        singCircleObj.ParentEmpty = this;
    }

    /// <summary>
    /// 消除唱歌实体
    /// </summary>
    void DestorySingObj()
    {
        if (singCircleObj != null)
        {
            Destroy(singCircleObj.gameObject);
        }
    }

    /// <summary>
    /// 唱歌开始
    /// <summary>
    public void SingStart(float Timer)
    {
        SingTimer = Timer;
        NowState = MainState.Sing;
    }

    /// <summary>
    /// 唱歌结束
    /// <summary>
    public void SingOver()
    {
        SingTimer = 0;
        DestorySingObj();
        if (isUsedEchoedVoice)
        {
            IdleStart(TIME_IDLE_SING);
        }
        else
        {
            SkillStart(TIME_SKILL);
        }
        isUsedEchoedVoice = false;
    }

    /// <summary>
    /// 判定当前敌人是否可以回声
    /// </summary>
    public override bool isEchoedVoiceisReady()
    {
        if (NowState == MainState.Sing && !isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) { return true; }
        else { return false; }
    }

    //=========================唱歌============================






    //=========================跳舞回血============================




    /// <summary>
    /// 跳舞回血计时器
    /// <summary>
    float DanceTimer = 0;

    /// <summary>
    /// 跳舞回血开始
    /// <summary>
    public void DanceStart(float Timer)
    {
        DanceTimer = Timer;
        NowState = MainState.Dance;
    }

    /// <summary>
    /// 跳舞回血结束
    /// <summary>
    public void DanceOver()
    {
        DanceTimer = 0;
    }


    //=========================跳舞回血============================






    //=========================随机使用技能============================

    /// <summary>
    /// 皮皮撒娇
    /// </summary>
    public ClefairyCharm charm;
    public ClefairyCharm charmObj;

    /// <summary>
    /// 皮皮重力
    /// </summary>
    public ClefairyGravity gravity;
    public ClefairyGravity gravityObj;

    /// <summary>
    /// 皮皮月光
    /// </summary>
    public ClefairyMoonlight moonlight;
    public ClefairyMoonlight moonlightObj;

    /// <summary>
    /// 皮皮护盾
    /// </summary>
    public ClefairyShield shield;
    public ClefairyShield shieldObj;





    static float TIME_SKILL = 4.5f;

    /// <summary>
    /// 随机使用技能计时器
    /// <summary>
    float SkillTimer = 0;

    /// <summary>
    /// 随机使用技能开始
    /// <summary>
    public void SkillStart(float Timer)
    {
        SkillTimer = Timer;
        NowState = MainState.Skill;
        UseSkill();
    }

    /// <summary>
    /// 随机使用技能结束
    /// <summary>
    public void SkillOver()
    {
        SkillTimer = 0;
        DestorySkill();
    }





    /// <summary>
    /// 使用随机技能
    /// </summary>
    void UseSkill()
    {
        animator.SetBool("Dance", true);
        int i = Random.Range(0, 4);
        //int i = 2;
        switch (i)
        {
            /// 皮皮撒娇
            case 0:
                charmObj = Instantiate(charm, transform.position, Quaternion.identity, transform);
                charmObj.ParentEmpty = this;
                break;
            /// 皮皮重力
            case 1:
                gravityObj = Instantiate(gravity, transform.position, Quaternion.identity, transform);
                gravityObj.ParentEmpty = this;
                break;
            /// 皮皮月光
            case 2:
                moonlightObj = Instantiate(moonlight, transform.position, Quaternion.identity, transform);
                moonlightObj.ParentEmpty = this;
                break;
            /// 皮皮护盾
            case 3:
                shieldObj = Instantiate(shield, transform.position, Quaternion.identity, transform);
                shieldObj.ParentEmpty = this;
                break;
        }


    }

    /// <summary>
    /// 销毁技能
    /// </summary>
    void DestorySkill()
    {
        Debug.Log("Destory");
        
        animator.SetBool("Dance", false);
        if (charmObj != null) { Debug.Log(charmObj.name); Destroy(charmObj.gameObject); }
        if (gravityObj != null) { Debug.Log(gravityObj.name); Destroy(gravityObj.gameObject);  }
        if (moonlightObj != null) { Debug.Log(moonlightObj.name); Destroy(moonlightObj.gameObject);}
        if (shieldObj != null) { Debug.Log(shieldObj.name); Destroy(shieldObj.gameObject);}
    }

    //=========================随机使用技能============================




    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■

}
