using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�����-���޶��� --��ϸѩ-�����-������-��
//      -���ж���
//      -����˫������

public class Vanillite : Empty
{

    //������ĸ�����״̬
    public enum VanilaFamilyState
    {
        Single,       //û�и���
        Father,       //�Զ���Ϊ����
        Grandfather,  //��˫˫����Ϊ����
    }
    public VanilaFamilyState FamilyState;

    //�����û�и���ʱ��״̬��
    public enum SingleState
    {
        Idle,     //����
        Move,     //�ƶ�
        Blow,     //������
        Rush,     //���
    }
    public SingleState singleState;

    //������Զ���Ϊ����ʱ��״̬��
    public enum FatherState
    {
        Idle,     //����
        Lunch,    //������
        Back,     //����ĸ��
        Surround, //����
    }
    public FatherState fatherState;

    //�������˫������Ϊ����ʱ��״̬��
    public enum GrandfatherState
    {
    }
    public GrandfatherState grandfatherState;


    /// <summary>
    /// ���˳���
    /// </summary>
    Vector2 Director;

    /// <summary>
    /// ���˵�Ŀ�������
    /// </summary>
    Vector2 TargetPosition;

    Vector3 LastPosition;//���㵱ǰ�ٶ�,����ʱ�����õ���һʱ�䵥λ��λ������,ͨ��Я��ִ��

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ice;//���˵�һ����
        EmptyType02 = PokemonType.TypeEnum.No;//���˵ڶ�����
        player = GameObject.FindObjectOfType<PlayerControler>();//��ȡ���
        Emptylevel = SetLevel(player.Level, MaxLevel);//�趨���˵ȼ�
        EmptyHpForLevel(Emptylevel);//�趨Ѫ��
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//�趨������
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//�趨�ع�
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint)/* * 1.2f */;//�趨������
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint)/* * 1.2f */;//�趨�ط�
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//�趨�ٶ�
        Exp = BaseExp * Emptylevel / 7;//�趨���ܺ��ȡ�ľ���

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //�������㷽��Я��
        StartCoroutine(CheckLook());


        //��ʼ��״̬�����޸�����
        ChildLeaveHome();//���ֵ���
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Idle;
        IdleStart_Single(TIME_START_SINGLE);

        //��ʼ������
        Director = new Vector2(-1,-1);
        SetDirector(Director);
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

            //����ѩ�� ��������
            float WeatherSpeedAlpha = (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
            switch (FamilyState)
            {
                case VanilaFamilyState.Single:
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        animator.ResetTrigger("Sleep");
                        switch (singleState)
                        {
                            case SingleState.Idle:
                                IdleTimer_Single -= Time.deltaTime;
                                //�����������־�ʱת���ƶ�״̬
                                if (IdleTimer_Single <= 0 || isFearDone)
                                {
                                    IdleOver_Single();
                                    MoveStart_Single();
                                }
                                break;
                            case SingleState.Move:
                                //���÷���
                                Vector2 MoveDirector = (TargetPosition - (Vector2)transform.position).normalized;
                                //�־�ʱ�ƶ�
                                if (isFearDone)
                                {
                                    //���÷���
                                    MoveDirector = -MoveDirector;
                                    //�ƶ�
                                    rigidbody2D.position = new Vector2(
                                        Mathf.Clamp(rigidbody2D.position.x
                                            + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha * 2.0f,       //����*�ٶ�
                                        ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //��Сֵ
                                        ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//���ֵ
                                        Mathf.Clamp(rigidbody2D.position.y
                                            + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha * 2.0f,        //����*�ٶ� 
                                        ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //��Сֵ
                                        ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//���ֵ
                                    Director = _mTool.TiltMainVector2(MoveDirector);
                                    SetDirector(Director);
                                }
                                else
                                {
                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE)
                                    {
                                        //�ƶ�
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha,       //����*�ٶ�
                                            ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //��Сֵ
                                            ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//���ֵ
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha,        //����*�ٶ� 
                                            ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //��Сֵ
                                            ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//���ֵ
                                        Director = _mTool.TiltMainVector2(MoveDirector);
                                        SetDirector(Director);
                                    }
                                    else
                                    {
                                        MoveOver_Single();
                                        RushStart_Single(RUSH_TIME);
                                    }
                                }
                                break;
                            case SingleState.Rush:
                                RushTimer_Single -= Time.deltaTime;
                                if (RushTimer_Single <= 0 || isFearDone)
                                {
                                    RushOver_Single();
                                    BlowStart_Single();
                                }
                                else
                                {
                                    rigidbody2D.position = new Vector2(
                                        Mathf.Clamp(rigidbody2D.position.x
                                            + (float)RushDirector.x * Time.deltaTime * speed * 6.5f * WeatherSpeedAlpha,       //����*�ٶ�
                                        ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //��Сֵ
                                        ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//���ֵ
                                        Mathf.Clamp(rigidbody2D.position.y
                                            + (float)RushDirector.y * Time.deltaTime * speed * 6.5f * WeatherSpeedAlpha,        //����*�ٶ� 
                                        ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //��Сֵ
                                        ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//���ֵ
                                }
                                break;
                            case SingleState.Blow:
                                break;
                        }
                    }
                    if ((isSleepDone || isSilence) && singleState != SingleState.Idle)
                    {
                        animator.SetTrigger("Sleep");
                        Debug.Log("Sleep");
                        switch (singleState)
                        {
                            case SingleState.Move:
                                MoveOver_Single();
                                IdleStart_Single(0.2f);
                                break;
                            case SingleState.Blow:
                                BlowOver_Single();
                                IdleStart_Single(0.2f);
                                break;
                            case SingleState.Rush:
                                RushOver_Single();
                                IdleStart_Single(0.2f);
                                break;
                        }
                    }
                    break;
                case VanilaFamilyState.Father:
                    if (ParentEmptyByChild != null) {
                        //������λ��
                        Vector2 ParentPosition = ((Vector2)ParentEmptyByChild.transform.position);
                        switch (fatherState)
                        {
                            case FatherState.Idle:
                                IdleTimer_Father -= Time.deltaTime;
                                //�����������־�ʱת���ƶ�״̬
                                if (IdleTimer_Father <= 0)
                                {
                                    IdleOver_Father();
                                    BackStart_Father(0);
                                }
                                break;
                            case FatherState.Back:
                                //���븸���ľ������һ��ʱ�ӽ�����
                                if (Vector2.Distance(ParentPosition, (Vector2)transform.position) > SURROUND_DISTENCE) {
                                    //���÷���
                                    Vector2 MoveDirector = (ParentPosition - (Vector2)transform.position).normalized;
                                    //���־�ʱ�ƶ�
                                    if (!isFearDone)
                                    {
                                        //�ƶ�
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha * 1.3f,       //����*�ٶ�
                                            ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //��Сֵ
                                            ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//���ֵ
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha * 1.3f,        //����*�ٶ� 
                                            ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //��Сֵ
                                            ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//���ֵ
                                        Director = _mTool.TiltMainVector2(MoveDirector);
                                        SetDirector(Director);
                                    }
                                }
                                //С�ڵ���ʱ����Ѹ��״̬
                                else
                                {
                                    BackOver_Father();
                                    SurroundStart_Father(0.0f);
                                }
                                break;
                            case FatherState.Surround:
                                //����
                                Surround(surroundRotationSpeed);
                                break;
                            case FatherState.Lunch:
                                break;
                        }
                    }
                    break;
                case VanilaFamilyState.Grandfather:
                    switch (grandfatherState)
                    {
                        default: break;
                    }
                    break;
            }
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//�����������ʧ�����»�ȡ
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//������Ȼ󣬼����Ȼ�ʱ��
        if (!isDie && !isBorn)//��������������״̬�����ڳ���״̬ʱ
        {
            //Debug.Log(NeedSearchParent);
            //Ѱ�Ҹ���
            if (NeedSearchParent)
            {
                NeedSearchParent = false;
                SearchVanilliteParent();
            }

            //��������Ѳ��״̬ʱ �ж��Ƿ񱻻��� 
            if (FamilyState == VanilaFamilyState.Single) { EmptyBeKnock(); }
            
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
        //�޸���״̬ ���
        if (FamilyState == VanilaFamilyState.Single && singleState == SingleState.Rush)
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











    //������������������������������������������ͨ��������������������������������������������
    /// <summary>
    /// ���ö����Ķ���������
    /// </summary>
    void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    public IEnumerator CheckLook()
    {
        while (true)
        {
            //һ��״̬ʱ�����ٶȺͳ���
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && 
                (FamilyState == VanilaFamilyState.Single && singleState == SingleState.Move)//TODO
             )
            {
                //���ݵ�ǰλ�ú���һ��FixedUpdate����ʱ��λ�ò�����ٶ�
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //���ݵ�ǰλ�ú���һ��FixedUpdate����ʱ��λ�ò���㳯�� �������������
                //SetDirector(_mTool.MainVector2((transform.position - LastPosition)));
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

    /// <summary>
    /// �Ƿ���ҪѰ�Ҹ���
    /// </summary>
    bool NeedSearchParent = false;

    /// <summary>
    /// Ѱ�Ҹ���
    /// </summary>
    public void SearchVanilliteParent()
    {
        Vaniluxe grandfather = SearchParentByDistence<Vaniluxe>();
        //���游
        if (grandfather != null)
        {
            Debug.Log("GrandFather");
            ChildBackHome(grandfather);//���游��
            FamilyState = VanilaFamilyState.Grandfather;
        }
        //���游
        else
        {
            Vanillish father = SearchParentByDistence<Vanillish>();
            //�и�
            if (father != null)
            {
                Debug.Log("Father");
                ChildBackHome(father);//�ظ��׼�
                FamilyState = VanilaFamilyState.Single;
                fatherState = FatherState.Idle;
                IdleStart_Father(TIME_START_FATHER);
            }
            //�޸�
            else
            {
                Debug.Log("Single");
                ChildLeaveHome();//���ֵ���
                FamilyState = VanilaFamilyState.Single;
                singleState = SingleState.Idle;
                IdleStart_Single(TIME_START_SINGLE);
            }
        }
    }


    
    //��д����
    public override void ChildLeaveHome()
    {
        base.ChildLeaveHome();
        //�ָ���ײ��
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.ResetOneChildCollision(this);
        }
    }

    //��д�ؼ�
    public override void ChildBackHome(Empty parent)
    {
        base.ChildBackHome(parent);
        //������ײ��
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.IgnoreOneChildCollision(this);
        }
    }


    //������������������������������������������ͨ��������������������������������������������






























    //������������������������������������������������������������������������������������������
    //�����������������������������������������޸�����������������������������������������������
    //������������������������������������������������������������������������������������������





    //========================����=================================

    //��ʼ�����ȴʱ��
    static float TIME_START_SINGLE = 0.5f;
    //��̺����ȴʱ��
    static float TIME_AFTER_BLOW_SINGLE = 4.3f;

    /// <summary>
    /// ������ʱ��
    /// </summary>
    float IdleTimer_Single = 0;

    /// <summary>
    /// ������ʼ
    /// </summary>
    public void IdleStart_Single(float Timer)
    {
        IdleTimer_Single = Timer;
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Idle;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void IdleOver_Single()
    {
        IdleTimer_Single = 0.0f;
    }
    //========================����=================================










    //========================�ƶ�=================================

    //ֹͣ�ƶ��ľ���
    static float MOVE_STOP_DISTENCE = 3.8f;

    /// <summary>
    /// �ƶ���ʱ��
    /// </summary>
    float MoveTimer_Single = 0;

    /// <summary>
    /// �ƶ���ʼ
    /// </summary>
    public void MoveStart_Single()
    {
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Move;
    }

    /// <summary>
    /// �ƶ�����
    /// </summary>
    public void MoveOver_Single()
    {
        MoveTimer_Single = 0.0f;
    }
    //========================�ƶ�=================================







    //========================���=================================

    //��̵�ʱ��
    static float RUSH_TIME = 0.18f;

    /// <summary>
    /// ��̼�ʱ��
    /// </summary>
    float RushTimer_Single = 0;

    Vector2 RushDirector = Vector2.zero;

    /// <summary>
    /// ��̿�ʼ
    /// </summary>
    public void RushStart_Single(float time)
    {
        RushTimer_Single = time;
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Rush;
        StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        RushDirector = (TargetPosition - (Vector2)transform.position).normalized;
        SetDirector(_mTool.TiltMainVector2(RushDirector));
    }

    /// <summary>
    /// ��̽���
    /// </summary>
    public void RushOver_Single()
    {
        RushTimer_Single = 0.0f;
        StopShadowCoroutine();
        RushDirector = Vector2.zero;
    }
    //========================���=================================





    //========================��ѩ=================================

    /// <summary>
    /// ϸѩ
    /// </summary>
    public VanillitePodesSnow PodesSnow;

    /// <summary>
    /// ��ѩ��ʱ��
    /// </summary>
    float BlowTimer_Single = 0;

    /// <summary>
    /// ��ѩ��ʼ
    /// </summary>
    public void BlowStart_Single()
    {
        animator.SetTrigger("Blow");
        FamilyState = VanilaFamilyState.Single;
        singleState = SingleState.Blow;
    }

    //����ϸѩ
    public void LunchPodesSnow()
    {
        if (!isFearDone) {
            Vector2 LunchDir = (TargetPosition - (Vector2)transform.position).normalized;
            if (isEmptyConfusionDone)
            {
                LunchDir = Quaternion.AngleAxis( ((Random.Range(0,1)>0.5f)? -60.0f : 60.0f ), Vector3.forward) * LunchDir;
            }
            VanillitePodesSnow ps = Instantiate(PodesSnow, transform.position + (Vector3)LunchDir * 0.7f, Quaternion.Euler(0, 0, _mTool.Angle_360Y((Vector3)LunchDir, Vector3.right)));
            SetDirector(_mTool.TiltMainVector2(LunchDir));
            ps.empty = this;
        }
    }

    /// <summary>
    /// ��ѩ����
    /// </summary>
    public void BlowOver_Single()
    {
        BlowTimer_Single = 0.0f;
        IdleStart_Single(TIME_AFTER_BLOW_SINGLE);
    }
    //========================��ѩ=================================


    //������������������������������������������������������������������������������������������
    //�����������������������������������������޸�����������������������������������������������
    //������������������������������������������������������������������������������������������























    //������������������������������������������������������������������������������������������
    //�������������������������������������Զ���Ϊ��������������������������������������������
    //������������������������������������������������������������������������������������������



    //========================����=================================

    //��ʼ�����ȴʱ��
    static float TIME_START_FATHER = 0.0f;

    /// <summary>
    /// ������ʱ��
    /// </summary>
    float IdleTimer_Father = 0;

    /// <summary>
    /// ������ʼ
    /// </summary>
    public void IdleStart_Father(float Timer)
    {
        IdleTimer_Father = Timer;
        FamilyState = VanilaFamilyState.Father;
        fatherState = FatherState.Idle;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void IdleOver_Father()
    {
        IdleTimer_Father = 0.0f;
    }
    //========================����=================================











    //========================����ĸ��=================================

    /// <summary>
    /// ����ĸ���ʱ��
    /// </summary>
    float BackTimer_Father = 0;

    /// <summary>
    /// ����ĸ�忪ʼ
    /// </summary>
    public void BackStart_Father(float Timer)
    {
        BackTimer_Father = Timer;
        FamilyState = VanilaFamilyState.Father;
        fatherState = FatherState.Back;
    }

    /// <summary>
    /// ����ĸ�����
    /// </summary>
    public void BackOver_Father()
    {
        BackTimer_Father = 0.0f;
    }
    //========================����ĸ��=================================











    //========================����=================================

    //���ƾ���
    static float SURROUND_DISTENCE = 2.0f;

 
    public float SurroundRotationSpeed
    {
        get { return surroundRotationSpeed; }
        set { surroundRotationSpeed = value; }
    }
    float surroundRotationSpeed = 35.0f;

    /// <summary>
    /// ���Ƽ�ʱ��
    /// </summary>
    float SurroundTimer_Father = 0;

    /// <summary>
    /// ���ƿ�ʼ
    /// </summary>
    public void SurroundStart_Father(float Timer)
    {
        SurroundTimer_Father = Timer;
        FamilyState = VanilaFamilyState.Father;
        fatherState = FatherState.Surround;
        //��ֹ����
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Surround( float RotationSpeed )
    {
        //������λ��
        Vector2 ParentPosition = ((Vector2)ParentEmptyByChild.transform.position);
        //�����ƶ�
        //��ǰ�Ƕ�
        float NowAngle = _mTool.Angle_360Y(((Vector2)transform.position - ParentPosition).normalized, Vector2.right);
        //Debug.Log("Vector2" + ((Vector2)transform.position - ParentPosition).normalized);
        //Debug.Log("NowAngle" + NowAngle);
        float PlusAngle = (NowAngle + RotationSpeed * Time.deltaTime) % (360.0f);
        //Debug.Log("PlusAngle" + PlusAngle);
        //Debug.Log("Xcos" + Mathf.Cos(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE);
        //Debug.Log("Ysin" + Mathf.Sin(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE);
        transform.position = new Vector2(
                   Mathf.Clamp(ParentPosition.x
                       + Mathf.Cos(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE,       //����*�ٶ�
                   ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //��Сֵ
                   ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//���ֵ
                   Mathf.Clamp(ParentPosition.y
                       + Mathf.Sin(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE,        //����*�ٶ� 
                   ParentPokemonRoom.RoomSize[1] - 0.0f + ParentPokemonRoom.transform.position.y,  //��Сֵ
                   ParentPokemonRoom.RoomSize[0] + 0.0f + ParentPokemonRoom.transform.position.y));//���ֵ
        //���÷���
        SetDirector(_mTool.TiltMainVector2((Vector2)transform.position - ParentPosition));
    }

    /// <summary>
    /// ���ƽ���
    /// </summary>
    public void SurroundOver_Father()
    {
        SurroundTimer_Father = 0.0f;
        //�ָ�����
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
    //========================����=================================











    //========================������=================================

    /// <summary>
    /// �������ʱ��
    /// </summary>
    float LunchTimer_Father = 0;

    /// <summary>
    /// �����俪ʼ
    /// </summary>
    public void LunchStart_Father(float Timer)
    {
        LunchTimer_Father = Timer;
        FamilyState = VanilaFamilyState.Father;
        fatherState = FatherState.Lunch;
    }

    /// <summary>
    /// ���������
    /// </summary>
    public void LunchOver_Father()
    {
        LunchTimer_Father = 0.0f;
    }
    //========================������=================================






    //������������������������������������������������������������������������������������������
    //�������������������������������������Զ���Ϊ��������������������������������������������
    //������������������������������������������������������������������������������������������






















    //������������������������������������������������������������������������������������������
    //������������������������������������˫������Ϊ������������������������������������������
    //������������������������������������������������������������������������������������������





    //������������������������������������������������������������������������������������������
    //������������������������������������˫������Ϊ������������������������������������������
    //������������������������������������������������������������������������������������������

}
