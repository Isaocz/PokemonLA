using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����-���ж������
//      -���޶������ --������-��ҡ�Σ��д������������ �޴������������


public class Vanillish : Empty
{
    //�Ƿ��ж������
    enum HavePartnerState
    {
        No,     //û�л��
        Partner,   //�л��
        Father  //��˫��������
    }
    HavePartnerState havePartnerState;



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
    }
    PartnerState partnerState;



    //�и���ʱ��״̬
    enum FatherState
    {
    }
    FatherState fatherState;



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
        EmptyType01 = PokemonType.TypeEnum.Normal;//���˵�һ����
        EmptyType02 = PokemonType.TypeEnum.Normal;//���˵ڶ�����
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
                        switch (singleState)
                        {
                            //����
                            case SingleState.Idle:
                                IdleTimer_Single -= Time.deltaTime;
                                //�����������־�ʱת���ƶ�״̬
                                if (IdleTimer_Single <= 0)
                                {
                                    IdleOver_Single();
                                    BeamStart_Single(TIME_BEAM);
                                }
                                break;
                            //����
                            case SingleState.Beam:

                                BeamTimer_Single -= Time.deltaTime;
                                Debug.Log(BeamTimer_Single);
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
                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) > MOVE_STOP_DISTENCE)
                                    {
                                        //�ƶ�
                                        rigidbody2D.position = new Vector2(
                                            Mathf.Clamp(rigidbody2D.position.x
                                                + (float)MoveDirector.x * Time.deltaTime * speed * WeatherSpeedAlpha,       //����*�ٶ�
                                            ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x, //��Сֵ
                                            ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x),//���ֵ
                                            Mathf.Clamp(rigidbody2D.position.y
                                                + (float)MoveDirector.y * Time.deltaTime * speed * WeatherSpeedAlpha,        //����*�ٶ� 
                                            ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y,  //��Сֵ
                                            ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y));//���ֵ

                                        //����
                                        if (!isFearDone && BeamTimer_Single <= TIME_BEAM - DELAY_TIME_BEAM && NowLunchIceBeam == null)
                                        {
                                            LunchBeam();
                                        }
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
                                break;
                        }
                    }
                    break;
                case HavePartnerState.Partner:
                    break;
                case HavePartnerState.Father:
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










    //������������������������������������������ͨ��������������������������������������������
    /// <summary>
    /// ���ö����Ķ���������
    /// </summary>
    public void SetDirector(Vector2 director)
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
    static float TIME_AFTER_Shake_SINGLE = 3.5f;

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
    }
    //========================����=================================










    //========================�伤��=================================
    
    //���伤����ӳ�
    static float DELAY_TIME_BEAM = 0.45f;

    //���伤���ʱ��
    static float TIME_BEAM = 13.00f;

    //ֹͣ�ƶ��ľ���
    static float MOVE_STOP_DISTENCE = 3.8f;

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
        NowLunchIceBeam = Instantiate(IceBeam , transform.position , Quaternion.Euler(0,0,BeamLunchRotation) , transform);
        NowLunchIceBeam.ParentVanillish = this;
    }

    void StopBeam()
    {
        NowLunchIceBeam.StopBeam();
        NowLunchIceBeam = null;
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
    /// ҡ�ο�ʼ
    /// </summary>
    public void ShakeStart_Single(float Timer)
    {
        animator.SetTrigger("Shake");
        ShakeTimer_Single = Timer;
        havePartnerState = HavePartnerState.No;
        singleState = SingleState.Shake;
    }

    /// <summary>
    /// ҡ�ν���
    /// </summary>
    public void ShakeOver_Single()
    {
        ShakeTimer_Single = 0.0f;
        IdleStart_Single(TIME_AFTER_Shake_SINGLE);
    }
    //========================ҡ��=================================

    //������������������������������������������������������������������������������������������
    //�����������������������������������������޻���������������������������������������������
    //������������������������������������������������������������������������������������������




















    //������������������������������������������������������������������������������������������
    //�����������������������������������������л���������������������������������������������
    //������������������������������������������������������������������������������������������

    //������������������������������������������������������������������������������������������
    //�����������������������������������������л���������������������������������������������
    //������������������������������������������������������������������������������������������
























    //������������������������������������������������������������������������������������������
    //�����������������������������������������и�����������������������������������������������
    //������������������������������������������������������������������������������������������

    //������������������������������������������������������������������������������������������
    //�����������������������������������������и�����������������������������������������������
    //������������������������������������������������������������������������������������������
}
