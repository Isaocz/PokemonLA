using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ҹ�
//����---������ҹ�---�ȴ����ҹֻع�---����12�κ�---����һ��������---����ѩ��
public class Avalugg : Empty
{
    //���ҹֵ�״̬��
    public enum AvaluggState
    {
        Idle,
        LunchBergmite,
        Jump,
    }
    public AvaluggState NowState;


    /// <summary>
    /// �����б�
    /// </summary>
    public List<Bergmite> BergmiteList = new List<Bergmite> { };

    /// <summary>
    /// �����ڱ���ʱ�������ڵĸ�����
    /// </summary>
    public Transform ChildBergmiteHome;

    /// <summary>
    /// ���������ڱ��ҹ����ϵ����λ��
    /// </summary>
    static List<Vector2> BergmitePosition = new List<Vector2> { new Vector2(-0.6f, -0.77f) , new Vector2(0.6f, -0.77f), new Vector2(-1.22f, -0.4f), new Vector2(1.22f, -0.4f), new Vector2(-0.55f, 0.0f), new Vector2(0.55f, 0.0f) };
    /// <summary>
    /// ����λ���Ƿ��б���
    /// </summary>
    public List<Bergmite> PositionIsBergmiteExist = new List<Bergmite> { null , null, null, null, null, null };
    /// <summary>
    /// ����λ�õı���
    /// </summary>
    public List<AvaluggFrozenMistCollision> FrozenMistList = new List<AvaluggFrozenMistCollision> { };
    /// <summary>
    /// ѩ��
    /// </summary>
    public AvaluggAvalancheCollider avalanche;

    Vector2 Director;//���˳���
    /// <summary>
    /// Ŀ��λ��
    /// </summary>
    Vector2 TargetPosition;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Normal;//���˵�һ����
        EmptyType02 = PokemonType.TypeEnum.Normal;//���˵ڶ�����
        player = GameObject.FindObjectOfType<PlayerControler>();//��ȡ���
        Emptylevel = SetLevel(player.Level, MaxLevel);//�趨���˵ȼ�
        EmptyHpForLevel(Emptylevel);//�趨Ѫ��
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//�趨������
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//�趨�ع�
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.2f;//�趨������
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.2f;//�趨�ط�
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//�趨�ٶ�
        Exp = BaseExp * Emptylevel / 7;//�趨���ܺ��ȡ�ľ���

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //������ײ��
        //IgnoreCollisionParentChild();
        //���Ѻ󷢴�
        IdleStart(IDLE_OF_AWAKE);
        //���ñ���
        SetFrozenMistByChild();
    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();//�����������ʧ�����»�ȡ
        if (!isDie && !isBorn)//��������������״̬�����ڳ���״̬ʱ
        {
            EmptyDie();//�ж��Ƿ�ִ������
            UpdateEmptyChangeHP();//�ж�����ֵ�Ƿ�仯
            StateMaterialChange();//�ж��Ƿ����״̬����

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone) {
                switch (NowState)
                {
                    case AvaluggState.Idle:
                        //����
                        IdleTimer += Time.deltaTime;
                        if (!isEmptyInfatuationDone)
                        {//δ���Ȼ�ʱ �����ұ����ҽ����� ����
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

        ResetPlayer();//�����������ʧ�����»�ȡ
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//������Ȼ󣬼����Ȼ�ʱ��
        if (!isDie && !isBorn)//��������������״̬�����ڳ���״̬ʱ
        {
            EmptyBeKnockByTransform();//�ж��Ƿ񱻻���
            //�����Ȼ����ȷʵĿ��λ��
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
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//δ���Ȼ� ���������ײʱ
        {
            EmptyTouchHit(other.gameObject);//���������˺�

        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//���Ȼ� ��������������ײʱ
        {
            InfatuationEmptyTouchHit(other.gameObject);//�����Ȼ�����˺�
        }
    }













    //=============================��ͨ=================================

    /// <summary>
    /// ���ñ�����idle�ͱ��ҹ�ͬ��
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
    /// ������ȥ
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
    /// ���ݱ����ڱ�������ñ���
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
    /// �������б����ͱ��ҹ�֮�����ײ
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
    /// ����ĳ�����������������Լ����ҹֵ���ײ
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
    /// �ָ�ĳ�����������������Լ����ҹֵ���ײ
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
    /// ��ȡĳ�������������ϵ�������룬���趨ĳ���Ѿ��б���
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
    /// �趨�����뿪ĳ��
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
    /// ���ر��ϵı�������
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
    /// ���ػ��ߵı�������
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
    /// ����ʱ��ձ����ĸ�����
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

    //=============================��ͨ=================================














    //=============================����=================================

    /// <summary>
    /// �����󷢴���ʱ��
    /// </summary>
    static float IDLE_OF_AWAKE = 1.0f;

    /// <summary>
    /// ��������󷢴���ʱ��
    /// </summary>
    static float IDLE_OF_LUNCHBERGMITE = 3.6f;

    /// <summary>
    /// �����󷢴���ʱ��
    /// </summary>
    static float IDLE_OF_JUMP = 16.0f;

    /// <summary>
    /// ������ʱ��
    /// </summary>
    float IdleTimer = 0;

    /// <summary>
    /// ������ʱ��
    /// </summary>
    float TimeOfIdle;

    /// <summary>
    /// ������ʼ
    /// </summary>
    public void IdleStart(float IdleTime)
    {
        NowState = AvaluggState.Idle;
        IdleTimer = 0.0f;
        TimeOfIdle = IdleTime;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void IdleOver()
    {
        IdleTimer = 0.0f;
        TimeOfIdle = 0.0f;
    }

    //=============================����=================================














    //=============================�������=================================

    /// <summary>
    /// ���������������
    /// </summary>
    public static int MAX_BERGMITE_LUNCH_COUNT = 1;

    /// <summary>
    /// ��������Ĵ���
    /// </summary>
    public int LunchCount = 0;

    /// <summary>
    /// ��ɫ��֮ǰλ�ã�����Ԥ�������λ
    /// </summary>
    public Vector2 TargetLastPosition;

    /// <summary>
    /// ���������ʼ
    /// </summary>
    public void LunchStart()
    {
        NowState = AvaluggState.LunchBergmite;
        animator.SetTrigger("LittleJump");
        GetTargetLastPosition();
        LunchCount++;
    }

    /// <summary>
    /// ��ȡ��ҵ�λ�� ����Ԥ��
    /// </summary>
    void GetTargetLastPosition()
    {
        TargetLastPosition = TargetPosition;
    }

    /// <summary>
    /// Ԥ�����λ��
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
    /// �������
    /// </summary>
    public void Lunch()
    {
        //Debug.Log(TargetLastPosition +"+"+ TargetPosition +"+"+ PredictTargetPosition());
        //��ȡ�򲹱���
        List<Bergmite> AL = GetBackBergmite();
        //��ȡĿ��λ��
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
        //��ȡ����Ŀ������ı��� ����
        AL[GetNearestBergmite(AL, v)].Drop(v,LunchCount);
        SetFrozenMistByChild();
    }

    /// <summary>
    /// ��ȡ�򲹱����о���Ŀ�������һ������
    /// </summary>
    /// <param name="BergmiteList">�򲹱���</param>
    /// <param name="TargetPosition">Ŀ��</param>
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
    /// �����������
    /// </summary>
    public void LunchOver()
    {
        IdleStart(IDLE_OF_LUNCHBERGMITE);
    }
    //=============================�������=================================















    //=============================�����������=================================

    /// <summary>
    /// ���������ʼ
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
    /// �����������
    /// </summary>
    public void JumpLunchBergmite()
    {
        List<Vector2> TargetPositionList = new List<Vector2> {
            new Vector2(-3.6f , -6.23538f) , new Vector2(3.6f , -6.23538f) ,new Vector2(-7.2f , 0.0f) ,
            new Vector2(7.2f , 0.0f) , new Vector2(-3.6f , 6.23538f) ,new Vector2(3.6f , 6.23538f) 
        };
        for (int i = 0; i < 6; i++)
        {
            //�������
            if (PositionIsBergmiteExist[i] != null && PositionIsBergmiteExist[i].NowState == Bergmite.BergmiteState.IdleInParent) { PositionIsBergmiteExist[i].Drop(TargetPositionList[i]+(Vector2)ParentPokemonRoom.transform.position,0); }
        }
    }

    void LunchAvalanche()
    {
        AvaluggAvalancheCollider a1 = Instantiate(avalanche, transform.position + Vector3.up * 4.5f, Quaternion.Euler(0, 0, 0));
        a1.empty = this;
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void JumpOver()
    {
        IdleStart(IDLE_OF_JUMP);
    }

    //=============================�����������=================================
}
