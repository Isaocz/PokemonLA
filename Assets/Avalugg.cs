using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//冰岩怪
//发呆---发射冰岩怪---等待冰岩怪回归---发射12次后---大跳一起发射六个---引起雪崩
public class Avalugg : Empty
{
    //冰岩怪的状态机
    public enum AvaluggState
    {
        Idle,
        LunchBergmite,
        Jump,
    }
    public AvaluggState NowState;


    /// <summary>
    /// 冰宝列表
    /// </summary>
    public List<Bergmite> BergmiteList = new List<Bergmite> { };

    /// <summary>
    /// 冰宝在背上时冰宝所在的父对象
    /// </summary>
    public Transform ChildBergmiteHome;

    /// <summary>
    /// 六个冰宝在冰岩怪身上的相对位置
    /// </summary>
    static List<Vector2> BergmitePosition = new List<Vector2> { new Vector2(-0.6f, -0.77f) , new Vector2(0.6f, -0.77f), new Vector2(-1.22f, -0.4f), new Vector2(1.22f, -0.4f), new Vector2(-0.55f, 0.0f), new Vector2(0.55f, 0.0f) };
    /// <summary>
    /// 六个位置是否有冰宝
    /// </summary>
    public List<Bergmite> PositionIsBergmiteExist = new List<Bergmite> { null , null, null, null, null, null };
    /// <summary>
    /// 六个位置的冰雾
    /// </summary>
    public List<AvaluggFrozenMistCollision> FrozenMistList = new List<AvaluggFrozenMistCollision> { };
    /// <summary>
    /// 雪崩
    /// </summary>
    public AvaluggAvalancheCollider avalanche;

    Vector2 Director;//敌人朝向
    /// <summary>
    /// 目标位置
    /// </summary>
    Vector2 TargetPosition;


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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.2f;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.2f;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //忽略碰撞体
        //IgnoreCollisionParentChild();
        //苏醒后发呆
        IdleStart(IDLE_OF_AWAKE);
        //设置冰雾
        SetFrozenMistByChild();
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

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone) {
                switch (NowState)
                {
                    case AvaluggState.Idle:
                        //发呆
                        IdleTimer += Time.deltaTime;
                        if (!isEmptyInfatuationDone)
                        {//未被魅惑时 如果玩家冰冻且近距离 大跳
                            if (player.isPlayerFrozenDone && Vector3.Distance(transform.position, player.transform.position) < 6.0f)
                            {
                                IdleOver();
                                JumpStart();
                            }
                        }
                        if (GetAliveBergmite().Count > 0)
                        {
                            if (IdleTimer >= TimeOfIdle && GetBackBergmite().Count > 0)
                            {
                                if (LunchCount <= MAX_BERGMITE_LUNCH_COUNT)
                                {
                                    //Debug.Log(LunchCount + "+" + MAX_BERGMITE_LUNCH_COUNT + "+" + (LunchCount <= MAX_BERGMITE_LUNCH_COUNT));
                                    IdleOver();
                                    LunchStart();
                                }
                                else if (GetBackBergmite().Count == GetAliveBergmite().Count)
                                {
                                    IdleOver();
                                    JumpStart();
                                }
                            }
                        }
                        else
                        {
                            if (IdleTimer >= TimeOfIdle)
                            {
                                IdleOver();
                                JumpStart();
                            }
                        }

                        break;
                    case AvaluggState.LunchBergmite:
                        break;
                    case AvaluggState.Jump:
                        break;
                }
            }
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













    //=============================共通=================================

    /// <summary>
    /// 设置冰宝的idle和冰岩怪同步
    /// </summary>
    public void setChildBergmiteIdleTime()
    {
        foreach (Bergmite b in BergmiteList)
        {
            if (b != null && b.NowState == Bergmite.BergmiteState.IdleInParent)
            {
                b.GetComponent<Animator>().SetTrigger("Jump");
            }
        }
    }

    /// <summary>
    /// 冰宝死去
    /// </summary>
    public void ChildBergmiteDie(Bergmite b)
    {
        for (int i = 0; i < BergmiteList.Count; i++)
        {
            if (BergmiteList[i] != null && b.gameObject == BergmiteList[i].gameObject) { BergmiteList[i] = null; }
        }
        for (int i = 0; i < PositionIsBergmiteExist.Count; i++)
        {
            if (PositionIsBergmiteExist[i] != null && b.gameObject == PositionIsBergmiteExist[i].gameObject) { PositionIsBergmiteExist[i] = null; }
        }
        SetFrozenMistByChild();
    }

    /// <summary>
    /// 根据冰宝在背情况设置冰雾
    /// </summary>
    public void SetFrozenMistByChild()
    {
        for (int i = 0; i < 6; i++)
        {
            if      (PositionIsBergmiteExist[i] != null && !PositionIsBergmiteExist[i].isDie && !FrozenMistList[i].isPlay) { Debug.Log("Start"+ i); FrozenMistList[i].StartFrozenMist(); }
            else if (PositionIsBergmiteExist[i] == null &&  FrozenMistList[i].isPlay) {  FrozenMistList[i].StopFrozenMist(); }
            Debug.Log("Stop" + i + "+" + PositionIsBergmiteExist[i] + "+" + FrozenMistList[i].isPlay);
        }
    }


    /// <summary>
    /// 忽略所有冰宝和冰岩怪之间的碰撞
    /// </summary>
    public void IgnoreCollisionParentChild()
    {
        List<Collider2D> CList = new List<Collider2D> { };
        CList.Add(this.GetComponent<Collider2D>());
        foreach (Bergmite b in BergmiteList)
        {
            if (b.gameObject != null ) { CList.Add(b.transform.GetComponent<Collider2D>()); }
        }
        for (int i = 0; i < (CList.Count - 1); i++)
        {
            for (int j = i + 1; j < CList.Count; j++)
            {
                Physics2D.IgnoreCollision(CList[i], CList[j], true);
            }
        }
    }

    /// <summary>
    /// 忽略某个冰宝和其他冰宝以及冰岩怪的碰撞
    /// </summary>
    public void IgnoreOneChildCollision(Bergmite c)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), c.GetComponent<Collider2D>(), true);
        foreach (Bergmite b in BergmiteList)
        {
            if (b != null && b.gameObject != c.gameObject)
            {
                Physics2D.IgnoreCollision(b.GetComponent<Collider2D>(), c.GetComponent<Collider2D>(), true);
            }
        }
    }

    /// <summary>
    /// 恢复某个冰宝和其他冰宝以及冰岩怪的碰撞
    /// </summary>
    public void ResetOneChildCollision(Bergmite c)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), c.GetComponent<Collider2D>(), false);
        foreach (Bergmite b in BergmiteList)
        {
            if (b != null && b.gameObject != c.gameObject)
            {
                Physics2D.IgnoreCollision(b.GetComponent<Collider2D>(), c.GetComponent<Collider2D>(), false);
            }
        }
    }

    /// <summary>
    /// 获取某个冰宝可以跳上的最近距离，并设定某点已经有冰宝
    /// </summary>
    /// <param name="child"></param>
    /// <returns></returns>
    public Vector2 SetAPositionHaveBergmite( Bergmite child )
    {
        Vector2 output = BergmitePosition[0];
        float MinDis = 100.0f;
        int Index = -1;
        for (int i = 0; i < 6; i++)
        {
            //Debug.Log(PositionIsBergmiteExist.Count);
            if (PositionIsBergmiteExist[i] == null)
            {
                if (Vector2.Distance(BergmitePosition[i] + (Vector2)transform.position, (Vector2)child.transform.position) < MinDis)
                {
                    output = BergmitePosition[i];
                    MinDis = Vector2.Distance(output + (Vector2)transform.position, (Vector2)child.transform.position);
                    Index = i;
                }
            }
        }

        PositionIsBergmiteExist[Index] = child;
        return output;
    }


    /// <summary>
    /// 设定冰宝离开某点
    /// </summary>
    public void SetAPositionDontHaveBergmite(Vector2 position)
    {
        for (int i = 0; i < 6; i++)
        {
            if (position == BergmitePosition[i])
            {
                PositionIsBergmiteExist[i] = null;
            }
        }
    }

    /// <summary>
    /// 返回背上的冰宝队列
    /// </summary>
    /// <returns></returns>
    public List<Bergmite> GetBackBergmite()
    {
        List<Bergmite> output = new List<Bergmite> { };
        for (int i = 0; i < PositionIsBergmiteExist.Count; i++)
        {
            if (PositionIsBergmiteExist[i] != null && PositionIsBergmiteExist[i].NowState == Bergmite.BergmiteState.IdleInParent) { output.Add(PositionIsBergmiteExist[i]); }
        }
        return output;
    }

    /// <summary>
    /// 返回或者的冰宝队列
    /// </summary>
    /// <returns></returns>
    public List<Bergmite> GetAliveBergmite()
    {
        List<Bergmite> output = new List<Bergmite> { };
        for (int i = 0; i < BergmiteList.Count; i++)
        {
            if (BergmiteList[i] != null && !BergmiteList[i].isDie) { output.Add(BergmiteList[i]); }
        }
        return output;
    }

    /// <summary>
    /// 死亡时清空冰宝的父对象
    /// </summary>
    public override void DieEvent()
    {
        base.DieEvent();
        JumpLunchBergmite();
        for (int i = 0; i < BergmiteList.Count; i++)
        {
            if (BergmiteList[i] != null && !BergmiteList[i].isDie) {
                if(BergmiteList[i].transform.parent == ChildBergmiteHome){
                    BergmiteList[i].transform.parent = transform.parent;
                }
                if (BergmiteList[i].ParentAvalugg != null) { BergmiteList[i].ParentAvalugg = null; }
                ResetOneChildCollision(BergmiteList[i]);
            }
        }
    }

    //=============================共通=================================














    //=============================发呆=================================

    /// <summary>
    /// 发呆后发呆的时间
    /// </summary>
    static float IDLE_OF_AWAKE = 1.0f;

    /// <summary>
    /// 发射冰宝后发呆的时间
    /// </summary>
    static float IDLE_OF_LUNCHBERGMITE = 3.6f;

    /// <summary>
    /// 大跳后发呆的时间
    /// </summary>
    static float IDLE_OF_JUMP = 16.0f;

    /// <summary>
    /// 发呆计时器
    /// </summary>
    float IdleTimer = 0;

    /// <summary>
    /// 发呆的时间
    /// </summary>
    float TimeOfIdle;

    /// <summary>
    /// 发呆开始
    /// </summary>
    public void IdleStart(float IdleTime)
    {
        NowState = AvaluggState.Idle;
        IdleTimer = 0.0f;
        TimeOfIdle = IdleTime;
    }

    /// <summary>
    /// 发呆结束
    /// </summary>
    public void IdleOver()
    {
        IdleTimer = 0.0f;
        TimeOfIdle = 0.0f;
    }

    //=============================发呆=================================














    //=============================发射冰宝=================================

    /// <summary>
    /// 发射冰宝的最大次数
    /// </summary>
    public static int MAX_BERGMITE_LUNCH_COUNT = 1;

    /// <summary>
    /// 发射冰宝的次数
    /// </summary>
    public int LunchCount = 0;

    /// <summary>
    /// 角色的之前位置，用于预判玩家走位
    /// </summary>
    public Vector2 TargetLastPosition;

    /// <summary>
    /// 发射冰宝开始
    /// </summary>
    public void LunchStart()
    {
        NowState = AvaluggState.LunchBergmite;
        animator.SetTrigger("LittleJump");
        GetTargetLastPosition();
        LunchCount++;
    }

    /// <summary>
    /// 获取玩家的位置 用于预判
    /// </summary>
    void GetTargetLastPosition()
    {
        TargetLastPosition = TargetPosition;
    }

    /// <summary>
    /// 预测玩家位置
    /// </summary>
    Vector2 PredictTargetPosition()
    {
        float distance1 = Vector2.Distance(transform.position, TargetLastPosition);
        float distance2 = Vector2.Distance(transform.position, TargetPosition);
        float distance = Mathf.Clamp(((3.0f * distance2) - 2.0f * distance1) , 3.0f , 20.0f );
        float Angle1 = _mTool.Angle_360Y(((Vector3)TargetLastPosition - transform.position),Vector3.right);
        float Angle2 = _mTool.Angle_360Y(((Vector3)TargetPosition - transform.position),Vector3.right);
        float Angle = (3.0f * Angle2) - 2.0f * Angle1;
        //Debug.Log(distance1 +"+"+ distance2 + "+" + distance + "+" + Angle1 + "+" + Angle2 + "+" + Angle);
        Vector2 output = (transform.position + Quaternion.AngleAxis(Angle, Vector3.forward) * Vector2.right * distance);
        output = new Vector2(Mathf.Clamp(output.x, ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[2] , ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[3]),
                            (Mathf.Clamp(output.y, ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[1] , ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[0])));
        return output;
    }

    /// <summary>
    /// 发射冰宝
    /// </summary>
    public void Lunch()
    {
        //Debug.Log(TargetLastPosition +"+"+ TargetPosition +"+"+ PredictTargetPosition());
        //获取候补冰宝
        List<Bergmite> AL = GetBackBergmite();
        //获取目标位置
        Vector2 v = TargetPosition;
        if (isEmptyInfatuationDone)
        {
            v = TargetPosition;
        }
        else
        {
            if (!isEmptyConfusionDone) { v = PredictTargetPosition(); }
            else { v = TargetPosition; }
        }
        //获取距离目标最近的冰宝 发射
        AL[GetNearestBergmite(AL, v)].Drop(v,LunchCount);
        SetFrozenMistByChild();
    }

    /// <summary>
    /// 获取候补冰宝中距离目标最近的一个冰宝
    /// </summary>
    /// <param name="BergmiteList">候补冰宝</param>
    /// <param name="TargetPosition">目标</param>
    /// <returns></returns>
    public int GetNearestBergmite(List<Bergmite> BergmiteList , Vector2 TargetPosition)
    {
        int output = -1;
        float MinDistence = 100.0f;
        for (int i = 0; i < BergmiteList.Count; i++)
        {
            if (Vector2.Distance((Vector2)BergmiteList[i].transform.position, TargetPosition) < MinDistence)
            {
                MinDistence = Vector2.Distance((Vector2)BergmiteList[i].transform.position, TargetPosition);
                output = i;
            }
        }
        return output;
    }

    /// <summary>
    /// 发射冰宝结束
    /// </summary>
    public void LunchOver()
    {
        IdleStart(IDLE_OF_LUNCHBERGMITE);
    }
    //=============================发射冰宝=================================















    //=============================大跳发射冰宝=================================

    /// <summary>
    /// 发射冰宝开始
    /// </summary>
    public void JumpStart()
    {
        NowState = AvaluggState.Jump;
        animator.SetTrigger("BigJump");
        LunchCount = 0;
    }

    public void JumpGround()
    {
        JumpLunchBergmite();
        CameraShake(2.4f , 6.5f , true);
        SetFrozenMistByChild();
        LunchAvalanche();
    }

    /// <summary>
    /// 大跳发射冰宝
    /// </summary>
    public void JumpLunchBergmite()
    {
        List<Vector2> TargetPositionList = new List<Vector2> {
            new Vector2(-3.6f , -6.23538f) , new Vector2(3.6f , -6.23538f) ,new Vector2(-7.2f , 0.0f) ,
            new Vector2(7.2f , 0.0f) , new Vector2(-3.6f , 6.23538f) ,new Vector2(3.6f , 6.23538f) 
        };
        for (int i = 0; i < 6; i++)
        {
            //发射冰宝
            if (PositionIsBergmiteExist[i] != null && PositionIsBergmiteExist[i].NowState == Bergmite.BergmiteState.IdleInParent) { PositionIsBergmiteExist[i].Drop(TargetPositionList[i]+(Vector2)ParentPokemonRoom.transform.position,0); }
        }
    }

    void LunchAvalanche()
    {
        AvaluggAvalancheCollider a1 = Instantiate(avalanche, transform.position + Vector3.up * 4.5f, Quaternion.Euler(0, 0, 0));
        a1.empty = this;
    }

    /// <summary>
    /// 发射冰宝结束
    /// </summary>
    public void JumpOver()
    {
        IdleStart(IDLE_OF_JUMP);
    }

    //=============================大跳发射冰宝=================================
}
