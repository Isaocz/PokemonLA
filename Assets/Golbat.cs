using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golbat : Empty
{

    //�����״̬��
    enum State
    {
        Normal,  //һ��Ѳ��״̬
        Sonic,  //�ͷų�����״̬
        Rush,   //���״̬
        CDIdle, //������ȴ�����ڼ�
    }
    State NowState;



    /// <summary>
    /// Ŀ��λ��
    /// </summary>
    Vector3 TargetPosition;


    /// <summary>
    /// �ƶ�����
    /// </summary>
    Vector3 MoveDirector;

    /// <summary>
    /// ײǽ����
    /// </summary>
    int TurnCount;
    /// <summary>
    /// ײǽCD cd�ڼ�ײǽ����ת��
    /// </summary>
    bool isTurnCD;

    /// <summary>
    /// ������
    /// </summary>
    public ZubatSupersonic Sonic;
    /// <summary>
    /// �ͷų������ľ���
    /// </summary>
    float SONIC_DISTANCE = 6.7f;

    /// <summary>
    /// �Ƿ�ʼ���
    /// </summary>
    bool isRush;
    /// <summary>
    /// ��̷���
    /// </summary>
    Vector2 RushDir;
    /// <summary>
    /// ���ʱ���ʱ��
    /// </summary>
    float RushTimer = 0.0f;


    /// <summary>
    /// ����
    /// </summary>
    public GameObject PosionBite;
    /// <summary>
    /// ����������ʵ��
    /// </summary>
    GameObject PosionBiteGOBJ;
    /// <summary>
    /// ��������ȴʱ��
    /// </summary>
    public float TIME_PosionBiteCD = 8.0f;
    /// <summary>
    /// ��ȴʱ���ʱ��
    /// </summary>
    float CDTimer;


    Vector2 Director;//���˳���
    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Poison;//���˵�һ����
        EmptyType02 = PokemonType.TypeEnum.Flying;//���˵ڶ�����
        player = GameObject.FindObjectOfType<PlayerControler>();//��ȡ���
        Emptylevel = SetLevel(player.Level, MaxLevel);//�趨���˵ȼ�
        EmptyHpForLevel(Emptylevel);//�趨Ѫ��
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//�趨������
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//�趨�ع�
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);//�趨������
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);//�趨�ط�
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//�趨�ٶ�
        Exp = BaseExp * Emptylevel / 7;//�趨���ܺ��ȡ�ľ���

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //��ʼ��
        SetNormalState();
    }

    void SetNormalState()
    {
        //��ʼ��״̬��
        NowState = State.Normal;
        StopShadowCoroutine();
        CDTimer = 0.0f;
        RushTimer = 0.0f;
        isRush = false;
        TurnCount = 0;
        isTurnCD = false;
        if (PosionBiteGOBJ != null) { Destroy(PosionBiteGOBJ.gameObject); }
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

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (animator.speed == 0) { animator.speed = 1; }
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }
                switch (NowState)
                {
                    case State.Normal:

                        //�ƶ�����Ϊ�������������ߵ�����90��
                        MoveDirector = ((Vector2)TargetPosition - (Vector2)transform.position).normalized;
                        float Angle = _mTool.Angle_360Y(MoveDirector, Vector2.right);
                        //����Ϊ�ƶ������б�������滯
                        if (Angle >= 0 && Angle < 90) { Director.x = 1; Director.y = 1; }
                        else if (Angle >= 90 && Angle < 180) { Director.x = -1; Director.y = 1; }
                        else if (Angle >= 180 && Angle < 270) { Director.x = -1; Director.y = -1; }
                        else { Director.x = 1; Director.y = -1; }
                        animator.SetFloat("LookX", Director.x); animator.SetFloat("LookY", Director.y);

                        //========�ͷų���������========
                        //����Һʹ����֮��ĽǶȴ�Լ��45��ʱ �Ҿ���С��SONIC_DISTANCEʱ���ͷų�Խ��
                        //��Һʹ����֮��ĽǶ�
                        float ABSAngleForPlayerAndGolbat = Mathf.Abs(((Mathf.Abs(MoveDirector.x)) / (Mathf.Abs(MoveDirector.y))) - 1);
                        if (!isFearDone)
                        {
                            if (ABSAngleForPlayerAndGolbat <= 0.1f && (TargetPosition - transform.position).magnitude <= SONIC_DISTANCE)
                            {
                                animator.SetTrigger("Sonic");
                                TurnCount = 0;
                                NowState = State.Sonic;
                            }
                        }




                        //========�ƶ�����========
                        //��ϸ���ƶ����� ����ײǽ��������ת �־�ʱ��Զ��
                        MoveDirector = Quaternion.AngleAxis(
                            (isFearDone ? (TurnCount % 2 == 0 ? 1 : -1) * 105 : (TurnCount % 2 == 0 ? 1 : -1) * (90 - (TurnCount * 7.5f))), Vector3.forward) * MoveDirector;    
                        rigidbody2D.position = new Vector2(
                            Mathf.Clamp(rigidbody2D.position.x 
                                + (float)MoveDirector.x * Time.deltaTime * speed                //����*�ٶ�
                                * (Mathf.Pow(1.05f, TurnCount)) * (isFearDone ? 1.4f : 1),      //ײǽ����Խ��Խ�� �־�ʱ����
                            ParentPokemonRoom.RoomSize[2] - 1.5f + transform.parent.position.x, //��Сֵ
                            ParentPokemonRoom.RoomSize[3] + 1.5f + transform.parent.position.x),//���ֵ
                            Mathf.Clamp(rigidbody2D.position.y
                                + (float)MoveDirector.y * Time.deltaTime * speed                 //����*�ٶ� 
                                * (Mathf.Pow(1.05f, TurnCount)) * (isFearDone ? 1.4f : 1),       //ײǽ����Խ��Խ�� �־�ʱ���� 
                            ParentPokemonRoom.RoomSize[1] - 1.5f + transform.parent.position.y,  //��Сֵ
                            ParentPokemonRoom.RoomSize[0] + 1.5f + transform.parent.position.y));//���ֵ

                        

                        break;

                    //���״̬
                    case State.Rush:
                        //����ƶ�
                        if (isRush)
                        {
                            RushTimer += Time.deltaTime;
                            rigidbody2D.position = new Vector2(
                            Mathf.Clamp(rigidbody2D.position.x
                                + (float)RushDir.x * Time.deltaTime * speed * 3.4f,                    //����*�ٶ�
                            ParentPokemonRoom.RoomSize[2] - 0.5f + transform.parent.position.x, //��Сֵ
                            ParentPokemonRoom.RoomSize[3] + 0.5f + transform.parent.position.x),//���ֵ
                            Mathf.Clamp(rigidbody2D.position.y
                                + (float)RushDir.y * Time.deltaTime * speed * 3.4f,                     //����*�ٶ� 
                            ParentPokemonRoom.RoomSize[1] - 0.5f + transform.parent.position.y,  //��Сֵ
                            ParentPokemonRoom.RoomSize[0] + 0.5f + transform.parent.position.y));//���ֵ
                        }
                        if (RushTimer >= 1.8f)
                        {
                            RushOver();
                        }

                        break;
                    case State.CDIdle:
                        //������̽��������󣬼��㷢��cdʱ��
                        if (NowState == State.CDIdle)
                        {
                            if (CDTimer > 0) { CDTimer -= Time.deltaTime; if (CDTimer <= 0) { CDTimer = 0; } }//����cd
                            if (CDTimer == 0 && !isFearDone)
                            {
                                SetNormalState();
                            }
                        }
                        break;

                }
            }
            if ((isSleepDone || isSilence || isFearDone ) && NowState != State.Normal)
            {
                SetNormalState();
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
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))//δ���Ȼ� ���������ײʱ
        {
            //����ڼ䶾���˺�
            if (NowState == State.Rush)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 50, 0, 0, PokemonType.TypeEnum.Poison);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 7.0f;
                    playerControler.KnockOutDirection = (new Vector2(RushDir.y , RushDir.x)).normalized;
                    if (Random.Range(0.0f, 1.0f) > 0.0f) { playerControler.ToxicFloatPlus(0.4f); }
                }
            }
            else
            {
                EmptyTouchHit(other.gameObject);//���������˺�
            }
        }
        else if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//���Ȼ� ��������������ײʱ
        {
            //����ڼ䶾���˺�
            if (NowState == State.Rush)
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, 50, 0, 0, PokemonType.TypeEnum.Poison);
                e.EmptyToxicDone(1f, 5, 0.5f);
            }
            else
            {
                InfatuationEmptyTouchHit(other.gameObject);//�����Ȼ�����˺�
            }
        }
        else if (other.transform.tag == ("Room"))
        {
            //Ѳ��״̬ײǽ�۷�
            if (NowState == State.Normal)
            {
                if (!isTurnCD)
                {
                    Debug.Log(other.gameObject.name);
                    TurnCount++;
                    isTurnCD = true;
                    StartCoroutine(TurnColdDown());
                }
            }
            //����ڼ�ײǽ�������
            if (NowState == State.Rush)
            {
                RushOver();
            }
        }
    }


    /// <summary>
    /// ��̽���
    /// </summary>
    void RushOver()
    {
        isRush = false;
        RushTimer = 0.0f;
        StopShadowCoroutine();
        NowState = State.CDIdle;
        CDTimer = TIME_PosionBiteCD;
    }


    /// <summary>
    /// ײǽ��ȴ
    /// </summary>
    /// <returns></returns>
    IEnumerator TurnColdDown()
    {
        yield return new WaitForSeconds(0.1f);
        isTurnCD = false;
    }



    /// <summary>
    /// �ͷų����� ��ͨ������������
    /// </summary>
    public void SuperSonic()
    {
        if (!isDie && !isBorn && !isEmptyFrozenDone && !isSleepDone && !isFearDone && !isSilence)
        {
            Vector2 SonicDir = _mTool.TiltMainVector2((Vector2)TargetPosition - (Vector2)transform.position);
            //����
            if (isEmptyConfusionDone) { SonicDir = (Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.forward) * SonicDir).normalized; }
            ZubatSupersonic s = Instantiate(Sonic, rigidbody2D.position + new Vector2(0, 0.6f) + SonicDir * 0.75f, Quaternion.identity);
            s.transform.rotation = Quaternion.Euler(0, 0, (TargetPosition.y - transform.position.y <= 0 ? -1 : 1) * Vector2.Angle(SonicDir, new Vector2(1, 0)));
            s.ParentZubat = this;
            s.transform.GetChild(0).GetComponent<ZubatSupersonic>().ParentZubat = this;
        }
    }

    /// <summary>
    /// ����������״̬ ��ʼ���� ��ͨ������������
    /// </summary>
    public void SuperSonicOver()
    {
        NowState = State.Rush;
        isRush = true;
        RushDir = _mTool.MainVector2((Vector2)TargetPosition - (Vector2)transform.position);
        //����
        if (isEmptyConfusionDone) { RushDir = (Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.forward) * RushDir).normalized; }
        StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        StartCoroutine(InstantiatePosionBite(0.20f));
    }

    IEnumerator InstantiatePosionBite(float waittime)
    {

            yield return new WaitForSeconds(waittime);
        if (isRush && !isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence  && !isFearDone)
        {
            PosionBiteGOBJ = Instantiate(PosionBite, transform.position + Vector3.up * 0.6f, Quaternion.identity, transform);
        }
    }





}
