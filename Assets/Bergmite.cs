using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����
//�б��ҹ� ���ϱ��ҹֵı�---�ڱ��Ϸ���---�ӱ�������---����---��·�ӽ����ҹ�---���ϱ��ҹֵı�
//�ޱ��ҹ� ���뺦��״̬ �����������


public class Bergmite : Empty
{
    /// <summary>
    /// �ױ��ҹ�
    /// </summary>
    public Avalugg ParentAvalugg;

    /// <summary>
    /// ����״̬��
    /// </summary>
    public enum BergmiteState
    {
        Idle,//����
        Run,//��·
        Jump,//���ϱ��ҹ�
        Drop,//���±��ҹ�
        IdleInParent,//�ڱ��ҹ����Ϸ���
    };
    public BergmiteState NowState;

    /// <summary>
    /// �������ڱ��ҹ����ϵ�λ��
    /// </summary>
    public Vector2 ParentPosition;

    /// <summary>
    /// ����
    /// </summary>
    public BergmiteIceShard IceShard;

    Vector2 Director;//��������
    /// <summary>
    /// Ŀ��λ��
    /// </summary>
    Vector2 TargetPosition;

    Vector3 LastPosition;//���㵱ǰ�ٶ�,����ʱ�����õ���һʱ�䵥λ��λ������,ͨ��Я��ִ��

    //��������Ч��1��2
    ParticleSystem FrozenMistPS1;
    ParticleSystem FrozenMistPS2;

    //����ʱ������Ŀ��
    Vector2 FearTarget = Vector2.zero;



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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.2f ;//�趨������
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.2f ;//�趨�ط�
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//�趨�ٶ�
        Exp = BaseExp * Emptylevel / 7;//�趨���ܺ��ȡ�ľ���

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        FrozenMistPS1 = transform.GetChild(4).GetChild(0).GetComponent<ParticleSystem>();
        FrozenMistPS2 = transform.GetChild(4).GetChild(1).GetComponent<ParticleSystem>();
        //�������㷽��Я��
        StartCoroutine(CheckLook());
        //���ó���
        SetDirector(Vector2.down);
        //���Ѻ󷢴�
        IdleStart(IDLE_OF_AWAKE);
        StopFrozenMistPS();


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

            if (ParentAvalugg != null)
            {
                if (!isEmptyFrozenDone && !isSleepDone && !isSilence) {
                    switch (NowState)
                    {
                        case BergmiteState.Idle:
                            //����
                            if (!isCanNotMoveWhenParalysis)
                            {
                                IdleTimer += Time.deltaTime;
                                if (IdleTimer >= TimeOfIdle)
                                {
                                    IdleOver();
                                    RunStart();
                                }
                            }
                            break;
                        case BergmiteState.Run:
                            if (!isCanNotMoveWhenParalysis)
                            {
                                if (!isFearDone)
                                {
                                    if (!isEmptyInfatuationDone)
                                    {
                                        //���ױ��ҹ��ƶ�
                                        rigidbody2D.position += ((Vector2)(ParentAvalugg.transform.position - transform.position).normalized * Time.deltaTime * speed);
                                        //���ó���
                                        Director = _mTool.MainVector2((ParentAvalugg.transform.position - transform.position).normalized);
                                        SetDirector(Director);
                                        //������С��JumpInDistenceʱ ���ϱ�
                                        if (Vector3.Distance(ParentAvalugg.transform.position, transform.position) <= JumpInDistence)
                                        {
                                            //�ƶ����� ���ϱ��Ͼ����Լ�����ĵ�
                                            RunOver();
                                            ParentPosition = ParentAvalugg.SetAPositionHaveBergmite(this);
                                            JumpStart(ParentPosition + (Vector2)ParentAvalugg.transform.position);
                                        }
                                        if (FearTarget != Vector2.zero) { FearTarget = Vector2.zero; }
                                    }
                                    else
                                    {
                                        if (Vector2.Distance( (Vector2)player.transform.position , (Vector2)transform.position) >= 1.6f) {
                                            //������ƶ�
                                            rigidbody2D.position += ((Vector2)(player.transform.position - transform.position).normalized * Time.deltaTime * speed);
                                            //���ó���
                                            Director = _mTool.MainVector2((player.transform.position - transform.position).normalized);
                                            SetDirector(Director);
                                        }
                                    }
                                }
                                else//����ʱ������ƶ�
                                {
                                    if (FearTarget == Vector2.zero)
                                    {
                                        FearTarget = new Vector2(((Random.Range(0.0f, 1.0f) > 0.5f) ? (ParentPokemonRoom.RoomSize[2] + 1.0f) : (ParentPokemonRoom.RoomSize[3] - 1.0f)), ((Random.Range(0.0f, 1.0f) > 0.5f) ? (ParentPokemonRoom.RoomSize[1] + 1.0f) : (ParentPokemonRoom.RoomSize[0] - 1.0f)));
                                        FearTarget += (Vector2)ParentPokemonRoom.transform.position;
                                        //Debug.Log(FearTarget);
                                    }
                                    if (Vector3.Distance((Vector3)FearTarget, transform.position) >= 1.2f)
                                    {
                                        rigidbody2D.position += ((Vector2)((Vector3)FearTarget - transform.position).normalized * Time.deltaTime * speed * 2.0f);
                                        //���ó���
                                        Director = _mTool.MainVector2(((Vector3)FearTarget - transform.position).normalized);
                                    }
                                    SetDirector(Director);
                                }
                            }
                            break;
                        case BergmiteState.Jump:
                            //���ϱ��ҹֵı�
                            JumpTimer += Time.deltaTime;
                            if (JumpTimer >= 0.13333f && JumpTimer < 0.63333f)
                            {
                                rigidbody2D.position += (JumpDirector * Time.deltaTime * JumpDistence);
                            }
                            else if (JumpTimer >= 0.63333f && JumpTimer < 1.13333f)
                            {
                                rigidbody2D.position += (JumpDirector * Time.deltaTime * JumpDistence) + (Vector2.up * Time.deltaTime * 4.0f);
                            }
                            else if (JumpTimer >= 1.13333f && (Vector2)transform.localPosition != ParentPosition + Vector2.up * 2.0f)
                            {
                                transform.localPosition = ParentPosition + Vector2.up * 2.0f;
                            }
                            break;
                        case BergmiteState.IdleInParent:
                            //�ڱ��ҹֵı��Ϸ���
                            /**
                            IdleInParentTimer += Time.deltaTime;
                            if (IdleInParentTimer >= 3.0)
                            {
                                Drop(new Vector2(10.0f, -3.0f));
                            }
                            **/
                            break;
                        case BergmiteState.Drop:
                            //���±��ҹֵı�
                            DropTimer += Time.deltaTime;
                            if (DropTimer >= 0.13333f && DropTimer < 0.3f)
                            {
                                rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f) - (Vector2.up * Time.deltaTime * 3.0f);
                            }
                            else if (DropTimer >= 0.3f && DropTimer < 0.46666f)
                            {
                                rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f) - (Vector2.up * Time.deltaTime * 3.0f);
                            }
                            else if (DropTimer >= 0.46666f && DropTimer < 0.63333f)
                            {
                                rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f);
                            }
                            else if (DropTimer >= 0.63333f && rigidbody2D.position != DropTarget)
                            {
                                rigidbody2D.position = DropTarget;
                            }
                            break;
                    }
                } 
            }
            else
            {
                if (!isFearDone)
                {
                    Fear(1000.0f, 100.0f);
                }
                switch (NowState)
                {
                    case BergmiteState.Idle:
                        //����
                        IdleOver();
                        RunStart();
                        break;
                    case BergmiteState.Run:
                        if (!isCanNotMoveWhenParalysis)
                        {
                            if (FearTarget == Vector2.zero)
                            {
                                FearTarget = new Vector2(((Random.Range(0.0f, 1.0f) > 0.5f) ? (ParentPokemonRoom.RoomSize[2] + 1.0f) : (ParentPokemonRoom.RoomSize[3] - 1.0f)), ((Random.Range(0.0f, 1.0f) > 0.5f) ? (ParentPokemonRoom.RoomSize[1] + 1.0f) : (ParentPokemonRoom.RoomSize[0] - 1.0f)));
                                FearTarget += (Vector2)ParentPokemonRoom.transform.position;
                                //Debug.Log(FearTarget);
                            }
                            if (Vector3.Distance((Vector3)FearTarget, transform.position) >= 1.2f)
                            {
                                rigidbody2D.position += ((Vector2)((Vector3)FearTarget - transform.position).normalized * Time.deltaTime * speed * 2.0f);
                                //���ó���
                                Director = _mTool.MainVector2(((Vector3)FearTarget - transform.position).normalized);
                            }
                        }
                        SetDirector(Director);
                        break;
                    case BergmiteState.Jump:
                        //���ϱ��ҹֵı�
                        JumpTimer += Time.deltaTime;
                        if (JumpTimer >= 0.13333f && JumpTimer < 0.63333f)
                        {
                            rigidbody2D.position += (JumpDirector * Time.deltaTime * JumpDistence);
                        }
                        else if (JumpTimer >= 0.63333f && JumpTimer < 1.13333f)
                        {
                            rigidbody2D.position += (JumpDirector * Time.deltaTime * JumpDistence) + (Vector2.up * Time.deltaTime * 4.0f);
                        }
                        else if (JumpTimer >= 1.13333f && (Vector2)transform.localPosition != ParentPosition + Vector2.up * 2.0f)
                        {
                            transform.localPosition = ParentPosition + Vector2.up * 2.0f;
                        }
                        break;
                    case BergmiteState.IdleInParent:
                        //�ڱ��ҹֵı��Ϸ���
                        /**
                        IdleInParentTimer += Time.deltaTime;
                        if (IdleInParentTimer >= 3.0)
                        {
                            Drop(new Vector2(10.0f, -3.0f));
                        }
                        **/
                        break;
                    case BergmiteState.Drop:
                        //���±��ҹֵı�
                        DropTimer += Time.deltaTime;
                        if (DropTimer >= 0.13333f && DropTimer < 0.3f)
                        {
                            rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f) - (Vector2.up * Time.deltaTime * 3.0f);
                        }
                        else if (DropTimer >= 0.3f && DropTimer < 0.46666f)
                        {
                            rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f) - (Vector2.up * Time.deltaTime * 3.0f);
                        }
                        else if (DropTimer >= 0.46666f && DropTimer < 0.63333f)
                        {
                            rigidbody2D.position += (DropDirector * Time.deltaTime * DropDistence * 2.0f);
                        }
                        else if (DropTimer >= 0.63333f && rigidbody2D.position != DropTarget)
                        {
                            rigidbody2D.position = DropTarget;
                        }
                        break;
                }
            }
        }
    }

    public override void DieEvent()
    {
        base.DieEvent();
        Debug.Log("Die" + this.name);
        if (ParentAvalugg != null) { ParentAvalugg.ChildBergmiteDie(this); }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//�����������ʧ�����»�ȡ
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//������Ȼ󣬼����Ȼ�ʱ��
        if (!isDie && !isBorn)//��������������״̬�����ڳ���״̬ʱ
        {
            if (NowState != BergmiteState.Jump && NowState != BergmiteState.IdleInParent && NowState != BergmiteState.Drop ) { EmptyBeKnock(); } //�ж��Ƿ񱻻���
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
        if (NowState == BergmiteState.Drop)
        {
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 40, 0, 0, PokemonType.TypeEnum.Ice);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 3;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, 40, 0, 0, PokemonType.TypeEnum.Ice);
            }
        }
        else
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

    }





    //============================��ͨ=================================

    /// <summary>
    /// ���ñ����Ķ���������
    /// </summary>
    void SetDirector( Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX" , director.x);
        animator.SetFloat("LookY" , director.y);
    }

    public IEnumerator CheckLook()
    {
        while (true)
        {
            //һ��״̬ʱ�����ٶȺͳ���
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && NowState == BergmiteState.Run )
            {
                //���ݵ�ǰλ�ú���һ��FixedUpdate����ʱ��λ�ò�����ٶ�
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //���ݵ�ǰλ�ú���һ��FixedUpdate����ʱ��λ�ò���㳯�� �������������
                SetDirector(_mTool.MainVector2((transform.position - LastPosition)));
                //����λ��
                LastPosition = transform.position;
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    //============================��ͨ=================================








    //============================����=================================

    /// <summary>
    /// ���Ѻ󷢴���ʱ��
    /// </summary>
    static float IDLE_OF_AWAKE = 0.4f;

    /// <summary>
    /// �ӱ����������󷢴���ʱ��
    /// </summary>
    static float IDLE_OF_DROP = 8.8f;

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
        NowState = BergmiteState.Idle;
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

    //============================����=================================











    //============================��·=================================

    /// <summary>
    /// ��Ծ��ʼ
    /// </summary>
    public void RunStart()
    {
        NowState = BergmiteState.Run;
        //StartFrozenMistPS();
    }


    /// <summary>
    /// ������������Ч��
    /// </summary>
    void StartFrozenMistPS()
    {
        FrozenMistPS1.Play();
        FrozenMistPS2.Play();
    }

    /// <summary>
    /// ��ͣ��������Ч��
    /// </summary>
    void StopFrozenMistPS()
    {
        FrozenMistPS1.Stop();
        FrozenMistPS2.Stop();
    }

    /// <summary>
    /// ��Ծ����
    /// </summary>
    public void RunOver()
    {
        //StopFrozenMistPS();
    }

    //============================��·=================================



















    //============================�������ϱ��ҹֵı�=================================

    //����������ҹ־���ΪJumpInDistenceʱ �� ���ϱ�
    public static float JumpInDistence = 3.0f;

    /// <summary>
    /// ���ϵ�Ŀ��λ��
    /// </summary>
    Vector2 JumpTarget;

    /// <summary>
    /// ���ϵĽǶ�
    /// </summary>
    Vector2 JumpDirector;
    /// <summary>
    /// ���ϵľ���
    /// </summary>
    float JumpDistence;

    /// <summary>
    /// ���ϼ�ʱ��
    /// </summary>
    float JumpTimer = 0;

    /// <summary>
    /// ���Ͽ�ʼ
    /// </summary>
    public void JumpStart(Vector2 Target)
    {
        //��������ʼ��Ծ
        animator.SetTrigger("Jump");
        //״̬����Ծ
        NowState = BergmiteState.Jump;
        //����λ�ã��ǶȺ;���
        JumpTarget = Target;
        JumpDirector = (JumpTarget - (Vector2)transform.position).normalized;
        JumpDistence = Vector2.Distance(JumpTarget, (Vector2)transform.position);
        if (ParentAvalugg != null) {
            //�����ĸ������Ϊ���ҹ�
            transform.parent = ParentAvalugg.ChildBergmiteHome.transform;
            //������ײ
            ParentAvalugg.IgnoreOneChildCollision(this);
            //���ñ���
            ParentAvalugg.SetFrozenMistByChild();
        }
    }

    /// <summary>
    /// ���Ͻ���
    /// </summary>
    public void JumpOver()
    {
        JumpTimer = 0.0f;
        IdleInParentStart();
    }

    //============================�������ϱ��ҹֵı�=================================






















    //============================�ڱ��ҹֱ��Ϸ���=================================

    /// <summary>
    /// �ڱ��ҹֱ��Ϸ�����ʱ��
    /// </summary>
    float IdleInParentTimer = 0;

    /// <summary>
    /// �ڱ��ҹֱ��Ϸ�����ʼ
    /// </summary>
    public void IdleInParentStart()
    {
        NowState = BergmiteState.IdleInParent;
        SetDirector(Vector2.down);
    }

    /// <summary>
    /// �ڱ��ҹֱ��Ϸ������� ���±��ҹֵı�
    /// </summary>
    public void IdleInParentOver()
    {
        IdleInParentTimer = 0.0f;
    }

    //============================�ڱ��ҹֱ��Ϸ���=================================















    //============================�������±��ҹֵı�=================================

    /// <summary>
    /// ���µ�Ŀ��λ��
    /// </summary>
    Vector2 DropTarget ;

    /// <summary>
    /// ���µĽǶ�
    /// </summary>
    Vector2 DropDirector;

    /// <summary>
    /// ���µľ���
    /// </summary>
    float DropDistence;

    /// <summary>
    /// ���¼�ʱ��
    /// </summary>
    float DropTimer = 0;

    /// <summary>
    /// LunchCount ����ģʽ
    /// </summary>
    public int LunchCount;

    /// <summary>
    /// ���¿�ʼ
    /// </summary>
    public void DropStart(Vector2 Target , int lunchCount)
    {

        //��������ʼ����
        animator.SetTrigger("Drop");
        //״̬������
        NowState = BergmiteState.Drop;
        //����λ�ã��ǶȺ;���
        DropTarget = Target;
        DropDirector = (DropTarget - JumpTarget).normalized;
        DropDistence = Vector2.Distance(DropTarget, JumpTarget);
        //�����ĸ������Ϊ���ҹ�
        if (ParentAvalugg != null) {
            transform.parent = ParentAvalugg.transform.parent;
        }
        //���ñ���
        //ParentAvalugg.SetFrozenMistByChild();
        LunchCount = lunchCount;
    }


    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="Target">�����Ŀ��λ��</param>
    public void Drop(Vector2 TargetPos, int lunchCount)
    {
        //TargetPos = new Vector2(10.0f, 4.0f);
        IdleInParentOver();
        SetDirector((TargetPos - (Vector2)transform.position).normalized);
        DropStart(TargetPos, lunchCount);
        if (ParentAvalugg != null) { ParentAvalugg.SetAPositionDontHaveBergmite(ParentPosition); }
    }

    /// <summary>
    /// ˤ������
    /// </summary>
    public void DropGround()
    {
        LunchIceShard();
        
    }

    /// <summary>
    /// �������
    /// </summary>
    public void LunchIceShard()
    {
        CameraShake(0.3f, 2.5f, true);
        if (!isFearDone && !isSilence && !isSleepDone)
        {
            Debug.Log(Director);
            //Debug.Log(isEmptyConfusionDone);
            float alpha = 0;
            if (LunchCount % 2 == 1) { alpha = 90; }
            
            if (!isEmptyConfusionDone)
            {
                for (int i = 0; i < 6; i++)
                {
                    
                    BergmiteIceShard e1 = Instantiate(IceShard, transform.position + Quaternion.AngleAxis(alpha + i * 60 , Vector3.forward) * Vector3.right * 0.75f, Quaternion.Euler(0, 0, alpha + i * 60));
                    e1.empty = this;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    BergmiteIceShard e1 = Instantiate(IceShard, transform.position + Quaternion.AngleAxis(alpha + i * 90, Vector3.forward) * Vector3.right * 0.75f, Quaternion.Euler(0, 0, alpha + i * 90));
                    e1.empty = this;
                }
            }

        }
    }

    /// <summary>
    /// ���½���
    /// </summary>
    public void DropOver()
    {
        DropTimer = 0.0f;
        IdleStart(IDLE_OF_DROP);
        //������ײ
        if (ParentAvalugg != null) {
            ParentAvalugg.ResetOneChildCollision(this);
        }
        LunchCount = 0;
    }

    //============================�������±��ҹֵı�=================================



}
