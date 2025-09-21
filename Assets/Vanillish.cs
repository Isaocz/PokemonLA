using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����-���ж������
//      -���޶������ --������-��ҡ�Σ��д������������ �޴������������


public class Vanillish : Empty
{
    //�Ƿ��ж������
    public enum HavePartnerState
    {
        No,     //û�л��
        Partner,   //�л��
        Father  //��˫��������
    }
    public HavePartnerState havePartnerState;



    //û�л��ʱ��״̬
    enum SingleState
    {
        Idle,  //����
        Beam,  //�伤��
        Shake, //ҡ��
    }
    SingleState singleState;



    //�л��ʱ��״̬
    enum PartnerState
    {
        Idle,  //����
        Beam,  //�伤��
        Shake, //ҡ��
    }
    PartnerState partnerState;



    //�и���ʱ��״̬
    public enum FatherState
    {
        Idle,     //����
        Back,     //����ĸ��
        Surround, //����
    }
    public FatherState fatherState;



    /// <summary>
    /// ���˳���
    /// </summary>
    Vector2 Director;

    public Vector2 TARGET_POSITION
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }
    Vector2 TargetPosition;

    Vector3 LastPosition;//���㵱ǰ�ٶ�,����ʱ�����õ���һʱ�䵥λ��λ������,ͨ��Я��ִ��

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
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Idle;
        IdleStart_Single(TIME_START_SINGLE);



        //��ʼ������
        Director = new Vector2(-1, -1);
        SetDirector(Director);
        //��������״̬Ϊ��
        //NeedSearchParent = true;
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
            switch (havePartnerState)
            {
                //�޻��
                case HavePartnerState.No:
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        animator.ResetTrigger("Sleep");
                        switch (singleState)
                        {
                            //����
                            case SingleState.Idle:
                                IdleTimer_Single -= Time.deltaTime;
                                //�����������־�ʱת���ƶ�״̬
                                if (IdleTimer_Single <= 0)
                                {
                                    IdleOver_Single();
                                    if (havePartnerState == HavePartnerState.No) { BeamStart_Single(TIME_BEAM); }
                                }
                                break;                      
                            //����
                            case SingleState.Beam:

                                if (!isFearDone) {
                                    BeamTimer_Single -= Time.deltaTime;
                                }
                                //Debug.Log(BeamTimer_Single);
                                if (BeamTimer_Single >= 0) {
                                    //���÷���
                                    Vector2 MoveDirector = (TargetPosition - (Vector2)transform.position).normalized;
                                    //�־�ʱ�ƶ�
                                    if (isFearDone)
                                    {
                                        //���÷���
                                        MoveDirector = -MoveDirector * 2;
                                    }
                                    //���־�ʱ�ƶ����ͷż���
                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE_SINGLE || isFearDone)
                                    {
                                        //�ƶ�
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha,       //����*�ٶ�
                                            ParentPokemonRoom.RoomSize[2] + 3.0f + transform.parent.position.x, //��Сֵ
                                            ParentPokemonRoom.RoomSize[3] - 3.0f + transform.parent.position.x),//���ֵ
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha,        //����*�ٶ� 
                                            ParentPokemonRoom.RoomSize[1] + 3.0f + transform.parent.position.y,  //��Сֵ
                                            ParentPokemonRoom.RoomSize[0] - 3.0f + transform.parent.position.y));//���ֵ
                                    }
                                    //����
                                    if (!isFearDone && BeamTimer_Single <= TIME_BEAM - DELAY_TIME_BEAM && NowLunchIceBeam == null)
                                    {
                                        LunchBeam();
                                    }
                                    if (isFearDone && NowLunchIceBeam != null)
                                    {
                                        StopBeam();
                                    }
                                }
                                else
                                {
                                    BeamOver_Single();
                                    ShakeStart_Single(0);
                                    StopBeam();
                                }


                                break;
                            //ҡ��
                            case SingleState.Shake:
                                if (isFearDone)
                                {
                                    ShakeOver();
                                    IdleStart_Single(TIME_START_SINGLE);
                                }
                                break;
                        }
                        //���������
                        SortChildren();
                    }
                    if (((isSleepDone) && singleState != SingleState.Idle))
                    {
                        animator.SetTrigger("Sleep");
                        if (singleState == SingleState.Beam)
                        {
                            BeamOver_Single();
                            StopBeam();
                        }
                        else if (singleState == SingleState.Shake)
                        {
                            ShakeOver();
                        }
                        IdleStart_Single(TIME_START_SINGLE);
                    }
                    break;
                //�л��
                case HavePartnerState.Partner:
                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        switch (partnerState)
                        {
                            //����
                            case PartnerState.Idle:
                                IdleTimer_Partner -= Time.deltaTime;
                                //�����������־�ʱת���ƶ�״̬
                                if (IdleTimer_Partner <= 0)
                                {
                                    IdleOver_Partner();
                                    BeamStart_Partner((360.0f/BEAM_ROTATION_SPEED_PARTNER) * 2.0f);
                                }
                                break;
                            //����
                            case PartnerState.Beam:
                                BeamTimer_Partner -= Time.deltaTime;
                                if (BeamTimer_Partner >= 0)
                                {
                                    //���÷���
                                    Vector2 MoveDirector = (TargetPosition - (Vector2)transform.position).normalized;
                                    //�־�ʱ�ƶ�
                                    if (isFearDone)
                                    {
                                        //���÷���
                                        MoveDirector = MoveDirector * 2;
                                    }
                                    //���־�ʱ�ƶ����ͷż���
                                    if (rigidbody2D != null && Vector2.Distance(TargetPosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE_PARTNER)
                                    {
                                        //�ƶ�
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha * 2.0f,       //����*�ٶ�
                                            ParentPokemonRoom.RoomSize[2] + 3.0f + transform.parent.position.x, //��Сֵ
                                            ParentPokemonRoom.RoomSize[3] - 3.0f + transform.parent.position.x),//���ֵ
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha * 2.0f,        //����*�ٶ� 
                                            ParentPokemonRoom.RoomSize[1] + 3.0f + transform.parent.position.y,  //��Сֵ
                                            ParentPokemonRoom.RoomSize[0] - 3.0f + transform.parent.position.y));//���ֵ
                                    }

                                    
                                    //����
                                    if (!isFearDone && NowLunchIceBeam == null)
                                    {
                                        SetBeamLunchRotation();
                                        LunchBeam();
                                    }
                                }
                                else
                                {
                                    BeamOver_Partner();
                                    ShakeStart_Partner(0);
                                    StopBeam();
                                }
                                break;
                        }
                    }
                    if ((isSleepDone) && partnerState != PartnerState.Idle)
                    {
                        animator.SetTrigger("Sleep");
                        switch (partnerState)
                        {
                            case PartnerState.Idle:
                                IdleOver_Partner();
                                break;
                            case PartnerState.Beam:
                                BeamOver_Partner();
                                break;
                            case PartnerState.Shake:
                                ShakeOver();
                                break;
                        }
                        IdleStart_Partner(TIME_START_PARTNER);
                        isReverseBeam = !isReverseBeam;
                    }
                    //���������
                    SortChildren();
                    if (partnerEmpty == null || positionInPartnership == PositionInPartnershipEnum.NoPartner || isFearDone)
                    {
                        animator.SetTrigger("Sleep");
                        switch (partnerState)
                        {
                            case PartnerState.Idle:
                                IdleOver_Partner();
                                break;
                            case PartnerState.Beam:
                                BeamOver_Partner();
                                break;
                            case PartnerState.Shake:
                                ShakeOver();
                                break;
                        }
                        RemovePartnership();
                        partnerEmpty = null;
                        positionInPartnership = PositionInPartnershipEnum.NoPartner;
                        IdleStart_Single(TIME_START_SINGLE);
                    }
                    break;
                case HavePartnerState.Father:
                    if (ParentEmptyByChild == null)
                    {
                        SearchVanillishParentOrPartner();
                    }
                    else
                    {
                        if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
                        {
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
                                    if (Vector2.Distance(ParentPosition, (Vector2)transform.position) > SURROUND_DISTENCE_Father)
                                    {
                                        //���÷���
                                        Vector2 MoveDirector = (ParentPosition - (Vector2)transform.position).normalized;
                                        //���־�ʱ�ƶ�
                                        if (!isFearDone)
                                        {
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
                                    }
                                    //С�ڵ���ʱ����Ѹ��״̬
                                    else
                                    {
                                        BackOver_Father();
                                        SurroundStart_Father();
                                    }
                                    break;
                                case FatherState.Surround:
                                    //����
                                    Surround(surroundRotationSpeed);
                                    //����
                                    if (!isFearDone && NowLunchIceBeam == null)
                                    {
                                        LunchBeam();
                                    }
                                    break;

                            }
                            //���������
                            SortChildren();
                        }
                        if (isSleepDone || isFearDone || isEmptyInfatuationDone)
                        {
                            animator.SetTrigger("Sleep");
                            switch (fatherState)
                            {
                                case FatherState.Idle:
                                    IdleOver_Father();
                                    break;
                                case FatherState.Back:
                                    BackOver_Father();
                                    break;
                                case FatherState.Surround:
                                    SurroundOver_Father();
                                    break;
                            }
                            IdleStart_Single(0.0f);
                        }
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
            EmptyBeKnock();//�ж��Ƿ񱻻���

            //Debug.Log(NeedSearchParent);
            if (NeedSearchParent)
            {
                NeedSearchParent = false;
                SearchVanillishParentOrPartner();
            }
            
            //�л��ʱȷ��Ŀ��λ��Ϊ�����һ��
            if (PartnerEmpty != null && positionInPartnership != PositionInPartnershipEnum.NoPartner)
            {
                Vector2 U = new Vector2(0, ParentPokemonRoom.RoomSize[0] * 0.5f);
                Vector2 D = new Vector2(0, ParentPokemonRoom.RoomSize[1] * 0.5f);
                Vector2 L = new Vector2(ParentPokemonRoom.RoomSize[2] * 0.5f, 0);
                Vector2 R = new Vector2(ParentPokemonRoom.RoomSize[3] * 0.5f, 0);
                //ȷ��λ��Index
                //���ȷ��λ��
                if (positionInPartnership != PositionInPartnershipEnum.BigBrother)
                {
                    //��������Ⱥ��᳤
                    if ((ParentPokemonRoom.RoomSize[0]- ParentPokemonRoom.RoomSize[1]) > (ParentPokemonRoom.RoomSize[3] - ParentPokemonRoom.RoomSize[2]) ) 
                    {
                        if (Vector2.Distance((Vector2)ParentPokemonRoom.transform.position+U , (Vector2)transform.position) < Vector2.Distance((Vector2)ParentPokemonRoom.transform.position + D, (Vector2)transform.position))
                        {
                            TargetPositionIndex_Partner = 0;
                        }
                        else
                        {
                            TargetPositionIndex_Partner = 1;
                        }
                    }
                    else
                    {
                        if (Vector2.Distance((Vector2)ParentPokemonRoom.transform.position + L, (Vector2)transform.position) < Vector2.Distance((Vector2)ParentPokemonRoom.transform.position + R, (Vector2)transform.position))
                        {
                            TargetPositionIndex_Partner = 2;
                        }
                        else
                        {
                            TargetPositionIndex_Partner = 3;
                        }
                    }
                }
                //С�ܱ���ȷ��λ��
                else
                {
                    switch (partnerEmpty.GetComponent<Vanillish>().TargetPositionIndex_Partner)
                    {
                        case 0: TargetPositionIndex_Partner = 1; break;
                        case 1: TargetPositionIndex_Partner = 0; break;
                        case 2: TargetPositionIndex_Partner = 3; break;
                        case 3: TargetPositionIndex_Partner = 2; break;
                    }
                }
                switch (TargetPositionIndex_Partner)
                {
                    case 0: TargetPosition = (Vector2)ParentPokemonRoom.transform.position + U; break;
                    case 1: TargetPosition = (Vector2)ParentPokemonRoom.transform.position + D; break;
                    case 2: TargetPosition = (Vector2)ParentPokemonRoom.transform.position + L; break;
                    case 3: TargetPosition = (Vector2)ParentPokemonRoom.transform.position + R; break;
                }
            }
            else
            {
                //�����Ȼ����ȷʵĿ��λ��
                if (!isEmptyInfatuationDone || _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile()).Count <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }

            }
            //Debug.Log(this.name + "+" + TargetPosition + "+" + TargetPositionIndex_Partner);

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










    //������������������������������������������ͨ��������������������������������������������

    /// <summary>
    /// �������
    /// </summary>
    public Vanillite ChildVanillite;

    /// <summary>
    /// ���ö����Ķ���������
    /// </summary>
    public void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    public override void DieEvent()
    {
        //�������
        if (NowLunchIceBeam != null)
        {
            if (NowLunchIceBeam.transform.parent != null) { NowLunchIceBeam.transform.parent = null; }
            NowLunchIceBeam.StopBeam();
        }
        Instantiate(ChildVanillite, transform.position + Vector3.right, Quaternion.identity, ParentPokemonRoom.EmptyFile());
        Instantiate(ChildVanillite, transform.position - Vector3.right, Quaternion.identity, ParentPokemonRoom.EmptyFile());
        ParentPokemonRoom.isClear += 2;
        base.DieEvent();
    }

    public IEnumerator CheckLook()
    {
        while (true)
        {
            //һ��״̬ʱ�����ٶȺͳ���
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence &&
                (havePartnerState == HavePartnerState.No && singleState == SingleState.Beam)
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
    bool NeedSearchParent;

    /// <summary>
    /// ���������߸���
    /// </summary>
    public HavePartnerState SearchVanillishParentOrPartner()
    {
        if (!isDie && !isBorn && !isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone && !isEmptyInfatuationDone)
        {
            Vaniluxe father = SearchParentByDistence<Vaniluxe>();
            //�и�
            if (father != null)
            {
                //Debug.Log("Father");
                ChildBackHome(father);//�ظ��׼�
                havePartnerState = HavePartnerState.Father;
                fatherState = FatherState.Idle;
                IdleStart_Father(TIME_START_FATHER);
                return HavePartnerState.Father;
            }
            else
            {
                ChildLeaveHome();//���ֵ���
                SearchPartnerInRoomByDistence<Vanillish>();
                //�л��
                Debug.Log(name + "Partner" + PartnerEmpty + PositionInPartnership);
                if (PartnerEmpty != null && positionInPartnership != PositionInPartnershipEnum.NoPartner)
                {
                    havePartnerState = HavePartnerState.Partner;
                    partnerState = PartnerState.Idle;
                    IdleStart_Partner(TIME_START_PARTNER);
                    if (PartnerEmpty.PartnerEmpty == null || PartnerEmpty.positionInPartnership == PositionInPartnershipEnum.NoPartner || havePartnerState != HavePartnerState.Partner ) 
                    {
                        PartnerEmpty.GetComponent<Vanillish>().SearchVanillishParentOrPartner();
                    }
                    return HavePartnerState.Partner;
                }
                //�޻��
                else
                {
                    //Debug.Log("Single");
                    havePartnerState = HavePartnerState.No;
                    singleState = SingleState.Idle;
                    IdleStart_Single(TIME_START_SINGLE);
                    return HavePartnerState.No;
                }
            }
        }
        return HavePartnerState.No;
    }



    /// <summary>
    /// �������õ�ҡ�ν���
    /// </summary>
    public void ShakeOver()
    {
        //Debug.Log(havePartnerState);
        switch (havePartnerState)
        {
            case HavePartnerState.No:
                ShakeOver_Single();
                break;
            case HavePartnerState.Partner:
                ShakeOver_Partner();
                break;
            case HavePartnerState.Father:
                //TODO
                break;

        }
    }


    //��д����
    public override void ChildLeaveHome()
    {
        //�ָ���ײ��
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.ResetOneChildCollision(this);
        }
        if (ParentEmptyByChild != null && ParentEmptyByChild.ChildrenList.Contains(this))
        {
            ParentEmptyByChild.ChildrenList.Remove(this);
        }
        //�趨������ͼ�
        if (ParentEmptyByChild != null)
        {
            //transform.parent = ParentPokemonRoom.EmptyFile();
            ParentEmptyByChild = null;
        }
    }

    //��д�ؼ�
    public override void ChildBackHome(Empty parent)
    {
        //�趨������ͼ�
        ParentEmptyByChild = parent;
        //transform.parent = ParentEmptyByChild.ChildHome;
        if (ParentEmptyByChild != null && !ParentEmptyByChild.ChildrenList.Contains(this))
        {
            ParentEmptyByChild.ChildrenList.Add(this);
        }
        //Debug.Log(ParentEmptyByChild);
        //������ײ��
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.IgnoreOneChildCollision(this);
        }
    }




    //������������������������������������������ͨ��������������������������������������������



















    //������������������������������������������������������������������������������������������
    //�����������������������������������������޻���������������������������������������������
    //������������������������������������������������������������������������������������������

    /// <summary>
    /// �ӵ����б�
    /// </summary>
    Empty SonList = new Empty { };

    //========================����=================================

    //��ʼ�����ȴʱ��
    static float TIME_START_SINGLE = 0.5f;
    //��̺����ȴʱ��
    static float TIME_AFTER_SHAKE_SINGLE = 3.5f;

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
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Idle;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void IdleOver_Single()
    {
        IdleTimer_Single = 0.0f;
        SearchVanillishParentOrPartner();
    }
    //========================����=================================










    //========================�伤��=================================

    //�޻��ģʽ�ļ�����ٶ�
    public static float BEAM_ROTATION_SPEED_SINGLE = 15.0f;

    //���伤����ӳ�
    static float DELAY_TIME_BEAM = 0.45f;

    //���伤���ʱ��
    static float TIME_BEAM = 15.00f;

    //ֹͣ�ƶ��ľ���
    static float MOVE_STOP_DISTENCE_SINGLE = 3.8f;

    /// <summary>
    /// ����
    /// </summary>
    public VanillishIceBeam IceBeam;

    /// <summary>
    /// ��ǰʵ��������ļ���
    /// </summary>
    public VanillishIceBeam NowLunchIceBeam;

    /// <summary>
    /// �伤���ʱ��
    /// </summary>
    float BeamTimer_Single = 0;

    /// <summary>
    /// ���ⷢ���ʼ�Ƕ�
    /// </summary>
    float BeamLunchRotation;

    /// <summary>
    /// �伤�⿪ʼ
    /// </summary>
    public void BeamStart_Single(float beamTimer)
    {
        BeamTimer_Single = beamTimer;
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Beam;
        BeamLunchRotation = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
    }

    void LunchBeam()
    {
        //Debug.Log("Lunch");
        NowLunchIceBeam = Instantiate(IceBeam , transform.position , Quaternion.Euler(0,0,BeamLunchRotation) , transform);
        NowLunchIceBeam.ParentVanillish = this;
    }

    void StopBeam()
    {
        if (NowLunchIceBeam != null) {
            NowLunchIceBeam.StopBeam();
            NowLunchIceBeam = null;
        }
    }

    /// <summary>
    /// �伤�����
    /// </summary>
    public void BeamOver_Single()
    {
        BeamTimer_Single = 0.0f;
    }
    //========================�伤��=================================












    //========================ҡ��=================================

    /// <summary>
    /// ҡ�μ�ʱ��
    /// </summary>
    float ShakeTimer_Single = 0;

    /// <summary>
    /// ϸѩ
    /// </summary>
    public VanillishPodesSnow PodesSnow;

    /// <summary>
    /// ҡ�ο�ʼ
    /// </summary>
    public void ShakeStart_Single(float _Timer)
    {
        animator.SetTrigger("Shake");
        ShakeTimer_Single = _Timer;
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Shake;
        LunchIceMist();
        Timer.Start(this, 0.4f, () => { LunchVanillite(); });
        
    }

    void LunchIceMist()
    {
        if (!isFearDone)
        {
            VanillishPodesSnow ps = Instantiate(PodesSnow, transform.position + Vector3.up, Quaternion.identity);
            ps.empty = this;
        }
    }

    void LunchVanillite()
    {
        if (!isFearDone)
        {
            if (ChildrenList.Count != 0)
            {
                for(int i = 0; i < ChildrenList.Count;i++)
                {
                    Vanillite BabyV = ChildrenList[i].GetComponent<Vanillite>();
                    if (BabyV != null && BabyV.FamilyState == Vanillite.VanilaFamilyState.Father && BabyV.fatherState == Vanillite.FatherState.Surround)
                    {
                        BabyV.SurroundOver_Father();
                        BabyV.LunchStart_Father(((Vector2)(BabyV.transform.position - transform.position)).normalized);
                        ResetOneChildCollision(BabyV);
                    }
                }
            }
        }
    }

    /// <summary>
    /// ҡ�ν���
    /// </summary>
    public void ShakeOver_Single()
    {
        ShakeTimer_Single = 0.0f;
        IdleStart_Single(TIME_AFTER_SHAKE_SINGLE);
    }
    //========================ҡ��=================================
















    //========================�����������=================================

    void SortChildren()
    {
        if (ChildrenList.Count != 0)
        {
            //�н��б�
            List<Vector2> AngleList = new List<Vector2> { };
            //���ڻ��Ƶ��Ӷ��������
            List<Vanillite> babyVList = new List<Vanillite> { };

            //������������� ȷ���Ƿ���
            for (int i = 0; i < ChildrenList.Count; i++)
            {
                Vanillite babyV = ChildrenList[i].GetComponent<Vanillite>();
                if (babyV != null && babyV.FamilyState == Vanillite.VanilaFamilyState.Father && babyV.fatherState == Vanillite.FatherState.Surround)
                {
                    babyVList.Add(babyV);
                }
            }

            //�������л��Ƶ������ ���������н�
            for (int i = 0; i < babyVList.Count; i++)
            {
                float angle = _mTool.Angle_360Y(((Vector2)babyVList[i].transform.position - (Vector2)transform.position).normalized, Vector2.right);
                AngleList.Add(new Vector2(angle, i));
            }

            AngleList.Sort((a, b) => a.x.CompareTo(b.x));
            //Debug.Log(string.Join(",", AngleList));

            //(21.40, 0.00),(54.00, 5.00),(60.55, 4.00),(197.93, 1.00),(248.73, 2.00),(291.30, 3.00)
            for (int i = 0; i < AngleList.Count; i++)
            {
                babyVList[(int)AngleList[i].y].transform.SetSiblingIndex(i);

                float NextAngle = 0.0f;
                if (i >= AngleList.Count - 1)
                {
                    NextAngle = 360.0f + AngleList[0].x - AngleList[i].x;
                }
                else
                {
                    NextAngle = AngleList[i+1].x - AngleList[i].x;
                }
                float CountRound = 360.0f / ((float)AngleList.Count);
                if      (NextAngle < CountRound) { babyVList[(int)AngleList[i].y].SurroundRotationSpeed = 20.0f - (Mathf.Abs(NextAngle-CountRound) / CountRound) * 20.0f; }
                else if (NextAngle > CountRound) { babyVList[(int)AngleList[i].y].SurroundRotationSpeed = 20.0f + (Mathf.Abs(NextAngle - CountRound) / CountRound) * 60.0f; }
                else                                                    { babyVList[(int)AngleList[i].y].SurroundRotationSpeed = 20.0f; }
                babyVList[(int)AngleList[i].y].SurroundRotationSpeed *= (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
            }
        }
    }

    //========================�����������=================================







    //������������������������������������������������������������������������������������������
    //�����������������������������������������޻���������������������������������������������
    //������������������������������������������������������������������������������������������




















    //������������������������������������������������������������������������������������������
    //�����������������������������������������л���������������������������������������������
    //������������������������������������������������������������������������������������������

    //========================����=================================

    //��ʼ�����ȴʱ��
    static float TIME_START_PARTNER = 0.5f;
    //��̺����ȴʱ��
    static float TIME_AFTER_SHAKE_PARTNER = 5.5f;

    /// <summary>
    /// ������ʱ��
    /// </summary>
    float IdleTimer_Partner = 0;

    /// <summary>
    /// ������ʼ
    /// </summary>
    public void IdleStart_Partner(float Timer)
    {
        IdleTimer_Partner = Timer;
        havePartnerState = HavePartnerState.Partner;
        partnerState = PartnerState.Idle;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void IdleOver_Partner()
    {
        IdleTimer_Partner = 0.0f;
    }
    //========================����=================================










    //========================�伤��=================================


    //���ģʽ�ļ�����ٶ�
    public static float BEAM_ROTATION_SPEED_PARTNER = 20.0f;

    //ֹͣ�ƶ��ľ���
    static float MOVE_STOP_DISTENCE_PARTNER = 0.2f;

    /// <summary>
    /// �伤���ʱ��
    /// </summary>
    float BeamTimer_Partner = 0;

    /// <summary>
    /// ���ⷢ���ʼ�Ƕ�
    /// </summary>
    float BeamLunchRotation_Partner;

    /// <summary>
    /// ���伤��������λ�õ����к� 0�� 1�� 2�� 3��
    /// </summary>
    int TargetPositionIndex_Partner;

    /// <summary>
    /// �Ƿ�ת������ת������ٶ�
    /// </summary>
    public bool isReverseBeam;


    void SetBeamLunchRotation()
    {
        switch (TargetPositionIndex_Partner)
        {
            case 0:
                BeamLunchRotation = 110.0f;
                break;
            case 1:
                BeamLunchRotation = 290.0f;
                break;
            case 2:
                BeamLunchRotation = 200.0f;
                break;
            case 3:
                BeamLunchRotation = 20.0f;
                break;
        }
    }

    /// <summary>
    /// �伤�⿪ʼ
    /// </summary>
    public void BeamStart_Partner(float beamTime)
    {
        BeamTimer_Partner = beamTime;
        havePartnerState = HavePartnerState.Partner;
        partnerState = PartnerState.Beam;
        BeamLunchRotation = 0;
    }


    /// <summary>
    /// �伤�����
    /// </summary>
    public void BeamOver_Partner()
    {
        BeamTimer_Partner = 0.0f;
        //��ת���ⷽ����ٶ�
        isReverseBeam = !isReverseBeam;
        StopBeam();
    }
    //========================�伤��=================================















    //========================ҡ��=================================

    /// <summary>
    /// ҡ�μ�ʱ��
    /// </summary>
    float ShakeTimer_Partner = 0;

    /// <summary>
    /// ҡ�ο�ʼ
    /// </summary>
    public void ShakeStart_Partner(float _Timer)
    {
        animator.SetTrigger("Shake");
        ShakeTimer_Partner = _Timer;
        havePartnerState = HavePartnerState.Partner;
        partnerState = PartnerState.Shake;
        LunchIceMist();
        Timer.Start(this, 0.4f, () => { LunchVanillite(); });

    }

    /// <summary>
    /// ҡ�ν���
    /// </summary>
    public void ShakeOver_Partner()
    {
        ShakeTimer_Partner = 0.0f;
        //Debug.Log(TIME_AFTER_SHAKE_PARTNER);
        IdleStart_Partner(TIME_AFTER_SHAKE_PARTNER);
    }
    //========================ҡ��=================================

    //������������������������������������������������������������������������������������������
    //�����������������������������������������л���������������������������������������������
    //������������������������������������������������������������������������������������������
























    //������������������������������������������������������������������������������������������
    //�����������������������������������������и�����������������������������������������������
    //������������������������������������������������������������������������������������������

    //========================����=================================

    //��ʼ�����ȴʱ��
    static float TIME_START_FATHER = 0.2f;

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
        havePartnerState = HavePartnerState.Father;
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
        havePartnerState = HavePartnerState.Father;
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
    static float SURROUND_DISTENCE_Father = 3.5f;


    /// <summary>
    /// ���ƽ��ٶ�
    /// </summary>
    public float SurroundRotationSpeed
    {
        get { return surroundRotationSpeed; }
        set { surroundRotationSpeed = value; }
    }
    float surroundRotationSpeed = 20.0f;


    /// <summary>
    /// ���ƿ�ʼ
    /// </summary>
    public void SurroundStart_Father()
    {
        havePartnerState = HavePartnerState.Father;
        fatherState = FatherState.Surround;
        //��ֹ����
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        //�趨������ͼ�
        transform.parent = ParentEmptyByChild.ChildHome;
        BeamLunchRotation = _mTool.Angle_360Y(((Vector2)transform.position - (Vector2)ParentEmptyByChild.transform.position).normalized, Vector2.right);
    }

    public void Surround(float RotationSpeed)
    {
        RotationSpeed *= isEmptyConfusionDone ? 0.5f : 1.0f;
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
                       + Mathf.Cos(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE_Father,       //����*�ٶ�
                   ParentPokemonRoom.RoomSize[2] - 0.0f + ParentPokemonRoom.transform.position.x, //��Сֵ
                   ParentPokemonRoom.RoomSize[3] + 0.0f + ParentPokemonRoom.transform.position.x),//���ֵ
                   Mathf.Clamp(ParentPosition.y
                       + Mathf.Sin(PlusAngle * Mathf.Deg2Rad) * SURROUND_DISTENCE_Father,        //����*�ٶ� 
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
        if (ParentEmptyByChild != null)
        {
            transform.parent = ParentPokemonRoom.EmptyFile();
        }
        //�ָ�����
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        StopBeam();
    }
    //========================����=================================


    //������������������������������������������������������������������������������������������
    //�����������������������������������������и�����������������������������������������������
    //������������������������������������������������������������������������������������������
}
