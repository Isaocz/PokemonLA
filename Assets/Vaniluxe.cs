using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaniluxe : Empty
{
    //״̬��
    enum State
    {
        Idle,    //����״̬
        Normal,  //һ��״̬
    }
    State NowState;


    /// <summary>
    /// ���˳���
    /// </summary>
    Vector2 Director;

    /// <summary>
    /// ���˵�Ŀ�������
    /// </summary>
    public Vector2 TARGET_POSITION
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }
    Vector2 TargetPosition;


    /// <summary>
    /// ˫�����������ĵ㣬ƫ�ƺ�᷵��
    /// </summary>
    Vector2 HomePosition;

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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.2f;//�趨������
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.2f;//�趨�ط�
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//�趨�ٶ�
        Exp = BaseExp * Emptylevel / 7;//�趨���ܺ��ȡ�ľ���

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //Ŀ��λ��Ϊ������
        HomePosition = transform.position;

        //��ʼ����
        IdleStart(TIME_START);



        StartOverEvent();
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
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis)
            {
                switch (NowState)
                {
                    //����
                    case State.Idle:
                        IdleTimer -= Time.deltaTime;
                        //�����������־�ʱת���ƶ�״̬
                        if (IdleTimer <= 0)
                        {
                            IdleOver();
                            NormalStart(180.0f / BEAM_ROTATION_SPEED);
                        }
                        break;
                    //����
                    case State.Normal:
                        NormalTimer -= Time.deltaTime * WeatherSpeedAlpha;
                        if (NormalTimer >= 0)
                        {
                            //���÷���
                            Vector2 MoveDirector = (HomePosition - (Vector2)transform.position).normalized;
                            //�־�ʱ�ƶ�
                            if (isFearDone)
                            {
                                //���÷���
                                MoveDirector = MoveDirector * 2;
                            }
                            //���־�ʱ�ƶ����ͷż���
                            if (Vector2.Distance(HomePosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE)
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
                            NormalOver();
                            IdleStart(TIME_IDLE_AFTER_NORMAL);
                            StopBeam();
                        }
                        break;
                }
                //������������

            }
            if ((isSleepDone || isFearDone) && NowState != State.Idle)
            {
                NormalOver();
                IdleStart(TIME_START);
                StopBeam();
            }
            SortChildren();
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

    public Vanillish ChildVanillish;

    /// <summary>
    /// ����˫�������Ķ���������
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
            NowLunchIceBeam.StopBeam();
            if (NowLunchIceBeam.transform.parent != null) { NowLunchIceBeam.transform.parent = null; }
        }
        Instantiate(ChildVanillish, transform.position + Vector3.right, Quaternion.identity, ParentPokemonRoom.EmptyFile());
        Instantiate(ChildVanillish, transform.position - Vector3.right, Quaternion.identity, ParentPokemonRoom.EmptyFile());
        ParentPokemonRoom.isClear += 2;
        base.DieEvent();
    }




    //������������������������������������������ͨ��������������������������������������������






















    //����������������������������������������״̬��������������������������������������������

    //========================����=================================

    //��ʼ�����ȴʱ��
    static float TIME_START = 0.5f;
    //һ��״̬�����ȴʱ��
    static float TIME_IDLE_AFTER_NORMAL = 5.0f;

    /// <summary>
    /// ������ʱ��
    /// </summary>
    float IdleTimer = 0;

    /// <summary>
    /// ������ʼ
    /// </summary>
    public void IdleStart(float Timer)
    {
        IdleTimer = Timer;
        NowState = State.Idle;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void IdleOver()
    {
        IdleTimer = 0.0f;
    }
    //========================����=================================










    //========================һ��״̬=================================

    //ֹͣ�ƶ��ľ���
    static float MOVE_STOP_DISTENCE = 0.3f;

    //�������ת���ٶ�
    public static float BEAM_ROTATION_SPEED = 48.0f;

    //�������ͷż��
    static float ICICLE_CRASH_COUNT = 2;

    //����
    public VaniluxeIceBeam IceBeam;

    //����
    public VaniluxeIcicleCrash IcicleCrash;

    //�������λ��
    Vector2 LastTargetPosition;

    //���ڷ���ı���
    public VaniluxeIceBeam NowLunchIceBeam
    {
        get { return nowLunchIceBeam; }
        set { nowLunchIceBeam = value; }
    }
    VaniluxeIceBeam nowLunchIceBeam;

    /// <summary>
    /// һ��״̬��ʱ��
    /// </summary>
    float NormalTimer = 0;

    /// <summary>
    /// ���ⷢ���ʼ�Ƕ�
    /// </summary>
    float BeamLunchRotation;

    /// <summary>
    /// �Ƿ�ת������ת����
    /// </summary>
    public bool isReverseBeam;




    /// <summary>
    /// �ƶ���ʼ
    /// </summary>
    public void NormalStart(float Timer)
    {
        NormalTimer = Timer;
        NowState = State.Normal;
        StartCoroutine(StartLunchIcicelCrash());
    }

    /// <summary>
    /// �ƶ�����
    /// </summary>
    public void NormalOver()
    {
        NormalTimer = 0.0f;
        isReverseBeam = !isReverseBeam;
        StopCoroutine(StartLunchIcicelCrash());
    }

    /// <summary>
    /// ���伤��
    /// </summary>
    void LunchBeam()
    {
        NowLunchIceBeam = Instantiate(IceBeam, transform.position, Quaternion.Euler(0, 0, BeamLunchRotation), transform);
        NowLunchIceBeam.ParentVaniluex = this;
    }


    /// <summary>
    /// ֹͣ����
    /// </summary>
    void StopBeam()
    {
        if (NowLunchIceBeam != null)
        {
            NowLunchIceBeam.StopBeam();
            NowLunchIceBeam = null;
        }
    }

    void SetBeamLunchRotation()
    {
        BeamLunchRotation = isReverseBeam ? 90 : 270;
    }


    void LunchIcicelCrash(Vector2 targetPosition)
    {
        VaniluxeIcicleCrash ic = Instantiate(IcicleCrash , targetPosition, Quaternion.identity);
        ic.empty = this;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <returns></returns>
    IEnumerator StartLunchIcicelCrash()
    {

        Debug.Log(isSleepDone);
        float cd = (180.0f / BEAM_ROTATION_SPEED) / (float)ICICLE_CRASH_COUNT;
        LastTargetPosition = TargetPosition;
        for (int i = 0; i <= ICICLE_CRASH_COUNT; i++)
        {
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone)
            {
                if (isEmptyConfusionDone || isEmptyInfatuationDone)
                {
                    LunchIcicelCrash(TargetPosition);
                }
                else
                {
                    LunchIcicelCrash(PredictTargetPosition(LastTargetPosition, TargetPosition));
                }
            }
            LastTargetPosition = TargetPosition;
            yield return new WaitForSeconds(cd);
        }
    }

    /// <summary>
    /// Ԥ�����λ��
    /// </summary>
    Vector2 PredictTargetPosition( Vector2 Last , Vector2 Now )
    {
        float distance1 = Vector2.Distance(transform.position, Last);
        float distance2 = Vector2.Distance(transform.position, Now);
        float distance = Mathf.Clamp(((1.8f * distance2) - 0.8f * distance1), 3.0f, 20.0f);
        float Angle1 = _mTool.Angle_360Y(((Vector3)Last - transform.position), Vector3.right);
        float Angle2 = _mTool.Angle_360Y(((Vector3)Now - transform.position), Vector3.right);
        float Angle = (1.8f * Angle2) - 0.8f * Angle1;
        //Debug.Log(distance1 +"+"+ distance2 + "+" + distance + "+" + Angle1 + "+" + Angle2 + "+" + Angle);
        Vector2 output = (transform.position + Quaternion.AngleAxis(Angle, Vector3.forward) * Vector2.right * distance);
        output = new Vector2(Mathf.Clamp(output.x, ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[2], ParentPokemonRoom.transform.position.x + ParentPokemonRoom.RoomSize[3]),
                            (Mathf.Clamp(output.y, ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[1], ParentPokemonRoom.transform.position.y + ParentPokemonRoom.RoomSize[0])));
        return output;
    }

    //========================һ��״̬=================================













    //========================�������or��������=================================

    void SortChildren()
    {
        if (ChildrenList.Count != 0)
        {
            //�н��б�
            List<Vector2> AngleList = new List<Vector2> { };
            //���ڻ��Ƶ��Ӷ��������
            List<Empty> babyVList = new List<Empty> { };



            //������������� ȷ���Ƿ���
            for (int i = 0; i < ChildrenList.Count; i++)
            {
                Vanillite babyVS = ChildrenList[i].GetComponent<Vanillite>();
                if (babyVS != null && babyVS.FamilyState == Vanillite.VanilaFamilyState.Grandfather && babyVS.grandfatherState == Vanillite.GrandfatherState.Surround)
                {
                    babyVList.Add(babyVS);
                }
                Vanillish babyVM = ChildrenList[i].GetComponent<Vanillish>();
                if (babyVM != null && babyVM.havePartnerState == Vanillish.HavePartnerState.Father && babyVM.fatherState == Vanillish.FatherState.Surround)
                {
                    babyVList.Add(babyVM);
                }
            }

            //�������л��Ƶ������ ���������н�
            for (int i = 0; i < babyVList.Count; i++)
            {
                float angle = _mTool.Angle_360Y(((Vector2)babyVList[i].transform.position - (Vector2)transform.position).normalized, Vector2.right);
                AngleList.Add(new Vector2(angle, i));
            }

            AngleList.Sort((a, b) => a.x.CompareTo(b.x));
            //Debug.Log(string.Join(",", ChildrenList));
            //Debug.Log(string.Join(",", babyVList));
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
                    NextAngle = AngleList[i + 1].x - AngleList[i].x;
                }
                float CountRound = 360.0f / ((float)AngleList.Count);
                Vanillite babyVS = babyVList[(int)AngleList[i].y].GetComponent<Vanillite>();
                Vanillish babyVM = babyVList[(int)AngleList[i].y].GetComponent<Vanillish>();
                if (babyVS != null)
                {
                    if (NextAngle < CountRound) { babyVS.SurroundRotationSpeed = 16.0f - (Mathf.Abs(NextAngle - CountRound) / CountRound) * 16.0f; }
                    else if (NextAngle > CountRound) { babyVS.SurroundRotationSpeed = 16.0f + (Mathf.Abs(NextAngle - CountRound) / CountRound) * 60.0f; }
                    else { babyVS.SurroundRotationSpeed = 16.0f; }
                    babyVS.SurroundRotationSpeed *= (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
                }
                else if (babyVM != null)
                {
                    if (NextAngle < CountRound) { babyVM.SurroundRotationSpeed = 16.0f - (Mathf.Abs(NextAngle - CountRound) / CountRound) * 16.0f; }
                    else if (NextAngle > CountRound) { babyVM.SurroundRotationSpeed = 16.0f + (Mathf.Abs(NextAngle - CountRound) / CountRound) * 60.0f; }
                    else { babyVM.SurroundRotationSpeed = 16.0f; }
                    babyVM.SurroundRotationSpeed *= (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f;
                }
            }
        }
    }

    //========================�����������=================================

    //����������������������������������������״̬��������������������������������������������
}
