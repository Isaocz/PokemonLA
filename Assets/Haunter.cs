using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��˹ͨ�ж��߼�
//���� ����ը�� ���� ����ը��
//���� ��Ӱצ ��Ӱצ
//���� Ӱ����
//���� ���
public class Haunter : Empty
{
    /// <summary>
    /// ��˹ͨ��״̬��
    /// </summary>
    enum HaunterState
    {
        Idle,       //����
        SludgeBomb, //����ը��
        ShadowBall, //Ӱ����
        ShadowClaw, //��Ӱץ
        Rush,       //���
        Invisible   //����״̬
    };
    /// <summary>
    /// ��˹ͨ��ǰ״̬
    /// </summary>
    HaunterState NowState;

    /// <summary>
    /// ״̬���к� 0��һ������ը�� 1�ڶ�������ը�� 2��һ�ΰ�Ӱץ 3�ڶ��ΰ�Ӱץ 4Ӱ���� 5���
    /// </summary>
    int StateIndex;



    /// <summary>
    /// ���˳���
    /// </summary>
    Vector2 Director;
    /// <summary>
    /// Ŀ��λ��
    /// </summary>
    Vector2 TargetPosition;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ghost;//���˵�һ����
        EmptyType02 = PokemonType.TypeEnum.Poison;//���˵ڶ�����
        player = GameObject.FindObjectOfType<PlayerControler>();//��ȡ���
        Emptylevel = SetLevel(player.Level, MaxLevel);//�趨���˵ȼ�
        EmptyHpForLevel(Emptylevel);//�趨Ѫ��
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//�趨������
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//�趨�ع�
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.2f; ;//�趨������
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.2f; ;//�趨�ط�
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//�趨�ٶ�
        Exp = BaseExp * Emptylevel / 7;//�趨���ܺ��ȡ�ľ���

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //���ó���Ϊ��
        Director = Vector2.down;
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);
        //��ʼ��ȴ�ڼ�
        IdleEnter(TIME_OF_IDLE_START);
        //�趨״̬���к�
        StateIndex = 0;
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
            
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//�����������ʧ�����»�ȡ
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//������Ȼ󣬼����Ȼ�ʱ��
        if (!isDie && !isBorn)//��������������״̬�����ڳ���״̬ʱ
        {
            EmptyBeKnock();//�ж��Ƿ񱻻���
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {

                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }

                if (!isFearDone) {
                    //״̬��
                    switch (NowState)
                    {
                        //����״̬
                        case HaunterState.Idle:
                            IdleTimer -= Time.deltaTime;
                            if (IdleTimer <= 0)
                            {
                                IdleOver();
                                InvisibleByState();
                            }
                            break;
                        //����״̬
                        case HaunterState.Invisible:
                            InvisibleTimer -= Time.deltaTime;
                            if (InvisibleTimer <= 0 && animator.GetBool("Invisible"))
                            {
                                animator.SetBool("Invisible", false);
                                MoveToPosition();
                                BackGroundMusic.StaticBGM.FadeIn(1.0f, 2.0f);
                            }
                            break;
                        //����ը��״̬
                        case HaunterState.SludgeBomb:
                            SludgeBombTimer -= Time.deltaTime;
                            if (SludgeBombTimer <= 0 && animator.GetBool("Atk"))
                            {
                                animator.SetBool("Atk", false);
                            }
                            break;
                        //��Ӱצ״̬
                        case HaunterState.ShadowClaw:
                            ShadowClawTimer -= Time.deltaTime;
                            if (isShadowClawStart && Vector3.Distance(start, transform.position) <= 2.5f)
                            {
                                rigidbody2D.position = new Vector2(
                                    Mathf.Clamp(rigidbody2D.position.x
                                        + (float)Director.x * Time.deltaTime * speed * 10.5f,                    //����*�ٶ�
                                    ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x, //��Сֵ
                                    ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x),//���ֵ
                                    Mathf.Clamp(rigidbody2D.position.y
                                        + (float)Director.y * Time.deltaTime * speed * 10.5f,                     //����*�ٶ� 
                                    ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y,  //��Сֵ
                                    ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y));//���ֵ
                            }
                            if (ShadowClawTimer <= 0 && animator.GetBool("Atk"))
                            {
                                animator.SetBool("Atk", false);
                            }
                            break;
                        //Ӱ����״̬
                        case HaunterState.ShadowBall:
                            if (animator.GetBool("Atk") && ShadowBallList.Count == SHADOWBALL_COUNT + (isEmptyConfusionDone ? -2 : 0))
                            {
                                animator.SetBool("Atk", false);
                                LunchAllShadowBall();
                            }
                            break;
                        case HaunterState.Rush:
                            ShadowRushTimer -= Time.deltaTime;
                            if (isShadowRushStart)
                            {
                                rigidbody2D.position = new Vector2(
                                    Mathf.Clamp(rigidbody2D.position.x
                                        + (float)Director.x * Time.deltaTime * speed * 10.5f,                    //����*�ٶ�
                                    ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x, //��Сֵ
                                    ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x),//���ֵ
                                    Mathf.Clamp(rigidbody2D.position.y
                                        + (float)Director.y * Time.deltaTime * speed * 10.5f,                     //����*�ٶ� 
                                    ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y,  //��Сֵ
                                    ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y));//���ֵ
                            }
                            if ((ShadowRushTimer <= 0 && animator.GetBool("Atk")) || (
                                Mathf.Approximately(rigidbody2D.position.x, ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x) ||
                                Mathf.Approximately(rigidbody2D.position.x, ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x) ||
                                Mathf.Approximately(rigidbody2D.position.y, ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y) ||
                                Mathf.Approximately(rigidbody2D.position.y, ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y)
                                ))
                            {
                                animator.SetBool("Atk", false);
                            }
                            break;
                    } 
                }
                else
                {
                    switch (NowState)
                    {
                        //����״̬
                        case HaunterState.Idle:
                            IdleTimer -= Time.deltaTime;
                            if (IdleTimer <= 0)
                            {
                                IdleOver();
                                InvisibleByState();
                            }
                            break;
                        //����״̬
                        case HaunterState.Invisible:
                            InvisibleTimer -= Time.deltaTime;
                            if (InvisibleTimer <= 0 && animator.GetBool("Invisible"))
                            {
                                animator.SetBool("Invisible", false);
                                MoveToPosition();
                                BackGroundMusic.StaticBGM.FadeIn(1.0f, 2.0f);
                            }
                            break;
                    }
                    
                }
            }
            if ((isSleepDone || isSilence) && NowState != HaunterState.Idle)
            {
                if (NowState == HaunterState.Invisible)
                {
                    animator.SetBool("Invisible", false);
                    BackGroundMusic.StaticBGM.FadeIn(1.0f, 2.0f);
                    Invincible = false;
                    InvisibleTimer = 0;
                    if (isSleepDone) { IdleTimer = TIME_OF_IDLE_NORMALCD; }
                    else if (isSilence) { IdleTimer = 0.5f; }
                    NowState = HaunterState.Idle;
                    animator.SetTrigger("Sleep");
                }
                else
                {
                    animator.SetBool("Atk", false);
                    AtkOver();
                    animator.SetTrigger("Sleep");
                    ShadowBallList.Clear();
                    if (NowState == HaunterState.ShadowBall )
                    {
                        LunchAllShadowBall();
                    }
                }
            }
            if (isFearDone && NowState != HaunterState.Idle && NowState != HaunterState.Invisible)
            {
                animator.SetBool("Atk", false);
                AtkOver();
                animator.SetTrigger("Sleep");
                ShadowBallList.Clear();
                if (NowState == HaunterState.ShadowBall)
                {
                    LunchAllShadowBall();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {


        if (NowState == HaunterState.ShadowClaw)
        {
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                float alpha = 1.0f;
                if (playerControler != null)
                {
                    alpha = isPlayerState(playerControler) ? 1.5f : 1.0f;
                    playerControler.KnockOutPoint = 3;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, DMAGE_SHADOWCLAW * alpha, 0, 0, PokemonType.TypeEnum.Ghost);
                Debug.Log(DMAGE_SHADOWCLAW * alpha);
                GetCTEffect(other.transform);
            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, DMAGE_SHADOWCLAW, 0, 0, PokemonType.TypeEnum.Ghost);
                GetCTEffect(other.transform);
            }
        }
        else if (NowState == HaunterState.Rush)
        {
            if (other.transform.tag == ("Room") && animator.GetBool("Atk")) 
            {
                animator.SetBool("Atk", false);
            }
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                float alpha = 1.0f;
                if (playerControler != null)
                {
                    alpha = isPlayerState(playerControler) ? 1.5f : 1.0f;
                    playerControler.KnockOutPoint = 3;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, DMAGE_SHADOWRUSH * alpha, 0, 0, PokemonType.TypeEnum.Ghost);
                Debug.Log(DMAGE_SHADOWRUSH * alpha);
                GetCTEffect(other.transform);
            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, DMAGE_SHADOWRUSH, 0, 0, PokemonType.TypeEnum.Ghost);
                GetCTEffect(other.transform);
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












    //=============================������ͨ==============================

    /// <summary>
    /// �����Ķ���
    /// </summary>
    public void AtkActive()
    {
        switch (StateIndex)
        {
            case 0:
                SludgeBombLunch();
                break;
            case 1:
                SludgeBombLunch();
                break;

            case 2:
                ShadowClawLunch();
                break;
            case 3:
                ShadowClawLunch();
                break;

            case 4:
                ShadowBallLunch();
                break;
            case 5:
                ShadowRushLunch();
                break;
        }
    }

    /// <summary>
    /// ����������Ķ���
    /// </summary>
    public void AtkOver()
    {
        switch (StateIndex)
        {
            case 0:
                SludgeBombOver();
                break;
            case 1:
                SludgeBombOver();
                break;

            case 2:
                ShadowClawOver();
                break;
            case 3:
                ShadowClawOver();
                break;

            case 4:
                ShadowBallOver();
                break;
            case 5:
                ShadowRushOver();
                break;
        }
    }


    /// <summary>
    /// ��ɫ�Ƿ����쳣״̬
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    bool isPlayerState(PlayerControler player)
    {
        return (player.isToxicDone) || (player.isBurnDone) || (player.isParalysisDone) || (player.isPlayerFrozenDone) || (player.isSleepDone);
    }

    //=============================������ͨ==============================

























    //=============================����״̬==============================
    //һ������ʱ��
    //static float TIME_OF_INVISIBLE_NORMAL = 3.0f;
    //һ������ʱ��
    static float TIME_OF_INVISIBLE_SLUDGEBOMB = 3.0f;
    //һ������ʱ��
    static float TIME_OF_INVISIBLE_SHADOWCLAW = 3.0f;
    //һ������ʱ��
    static float TIME_OF_INVISIBLE_SHADOWBALL = 3.0f;
    //һ������ʱ��
    static float TIME_OF_INVISIBLE_RUSH = 1.2f;


    /// <summary>
    /// ����״̬��ʱ��
    /// </summary>
    float InvisibleTimer;


    void InvisibleByState()
    {
        switch (StateIndex)
        {
            case 0:
                InvisibleEnter(TIME_OF_INVISIBLE_SLUDGEBOMB);
                break;
            case 1:
                InvisibleEnter(TIME_OF_INVISIBLE_SLUDGEBOMB);
                break;
            case 2:
                InvisibleEnter(TIME_OF_INVISIBLE_SHADOWCLAW);
                break;
            case 3:
                InvisibleEnter(TIME_OF_INVISIBLE_SHADOWCLAW);
                break;
            case 4:
                InvisibleEnter(TIME_OF_INVISIBLE_SHADOWBALL);
                break;
            case 5:
                InvisibleEnter(TIME_OF_INVISIBLE_RUSH);
                break;
        }
    }

    /// <summary>
    /// ��������״̬
    /// </summary>
    /// <param name="time">����ʱ��</param>
    void InvisibleEnter(float time)
    {
        Invincible = true;
        InvisibleTimer = time;
        animator.SetBool("Invisible", true);
        NowState = HaunterState.Invisible;

    }

    void BGMSlience()
    {
        BackGroundMusic.StaticBGM.FadeOut(0.1f, 2.0f);
    }

    /// <summary>
    /// ��������״̬
    /// </summary>
    void InvisibleOver()
    {
        Invincible = false;
        InvisibleTimer = 0;
        if (isFearDone)
        {
            IdleEnter(TIME_OF_IDLE_NORMALCD);
            return;
        }

        switch (StateIndex)
        {
            case 0:
                SludgeBombEnter();
                break;
            case 1:
                SludgeBombEnter();
                break;

            case 2:
                ShadowClawEnter();
                break;
            case 3:
                ShadowClawEnter();
                break;

            case 4:
                ShadowBallEnter();
                break;
            case 5:
                ShadowRushEnter();
                break;
        }
    }




    /// <summary>
    /// ������ƶ�
    /// </summary>
    public void MoveToPosition()
    {
        Vector2 target;
        if (StateIndex == 0 || StateIndex == 1)//0��һ������ը�� 1�ڶ�������ը��
        {
            target = MoveToMedium();
        }
        else if (StateIndex == 4)//4Ӱ����
        {
            target = MoveToFar();
        }
        else// 2��һ�ΰ�Ӱץ 3�ڶ��ΰ�Ӱץ5���
        {
            if (isEmptyConfusionDone)
            {
                target = MoveToMedium();
            }
            else
            {
                target = MoveToBack();
            }
        }
        if (isFearDone) { target = FaerMove(); }
        transform.position = target;
        Director = _mTool.MainVector2((TargetPosition - (Vector2)transform.position));
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);

    }


    /// <summary>
    /// �ƶ���Զ����
    /// Vector2summary>
    Vector2 MoveToFar()
    {
        if (TargetPosition.x < ParentPokemonRoom.transform.position.x)
        { return new Vector2(9.5f, 0.25f) + (Vector2)ParentPokemonRoom.transform.position; }
        else
        { return new Vector2(-9.5f, 0.25f) + (Vector2)ParentPokemonRoom.transform.position; }
    }


    static float MIN_MEDIUM_RADIUS = 4.2f;
    static float MAX_MEDIUM_RADIUS = 5.8f;
    /// <summary>
    /// �ƶ����о���
    /// </summary>
    Vector2 MoveToMedium()
    {
        Vector2 output;
        do
        {
            // ����뾶�ͽǶ�
            float radius = Random.Range(MIN_MEDIUM_RADIUS, MAX_MEDIUM_RADIUS);
            float angle = Random.Range(0, Mathf.PI * 2);

            // ���������
            float x = Mathf.Cos(angle) * radius + TargetPosition.x;
            float y = Mathf.Sin(angle) * radius + TargetPosition.y;

            output = new Vector3(x, y);


            // �������Ƿ��ڰ뾶��Χ��
            if (ParentPokemonRoom.isPointInRoon(output, 0.75f))
            {
                return output;
            }
        } while (true);
    }


    /// <summary>
    /// �ƶ���Ŀ���
    /// </summary>
    Vector2 MoveToBack()
    {
        Vector2 output = Vector2.up;
        if (!isEmptyInfatuationDone)
        {
            output = -player.look * 1.7f + TargetPosition + Vector2.up * 0.3f;
            if (ParentPokemonRoom.isPointInRoon(output, 0.75f))
            {
                //Debug.Log("look" + ParentPokemonRoom.isPointInRoon(output, 0.75f));
                return output;
            }
        }
        float WallWeight = 0.75f;
        while (WallWeight >= 0)
        {
            List<Vector2> vList = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            while (vList.Count != 0)
            {
                output = vList[Random.Range(0, vList.Count)];
                vList.Remove(output);
                output = output * 1.7f + TargetPosition + Vector2.up * 0.3f;
                if (ParentPokemonRoom.isPointInRoon(output, WallWeight))
                {
                    return output;
                }
            }
            WallWeight -= 0.25f;
        }
        return output;
    }


    Vector2 FaerMove()
    {
        Vector2 output = new Vector2(((Random.Range(0.0f, 1.0f) > 0.5f) ? -1 : 1) , ((Random.Range(0.0f, 1.0f) > 0.5f) ? -1 : 1)) * 100.0f;
        output = new Vector2(
            Mathf.Clamp(output.x + transform.parent.position.x,                    //����*�ٶ�
            ParentPokemonRoom.RoomSize[2] + 1.3f + transform.parent.position.x, //��Сֵ
            ParentPokemonRoom.RoomSize[3] - 1.3f + transform.parent.position.x),//���ֵ
            Mathf.Clamp(output.y + transform.parent.position.y,                     //����*�ٶ� 
            ParentPokemonRoom.RoomSize[1] + 1.3f + transform.parent.position.y,  //��Сֵ
            ParentPokemonRoom.RoomSize[0] - 1.3f + transform.parent.position.y));//���ֵ
        return output;
    }


    //=============================����״̬==============================

























    //=============================����״̬========================================

    //���������ȴʱ��
    static float TIME_OF_IDLE_START = 0.2f;
    //ʹ����һ�㼼�ܺ����ȴʱ��
    static float TIME_OF_IDLE_NORMALCD = 4.0f;
    //ʹ��������ը�������ȴʱ��
    static float TIME_OF_IDLE_SLUDGEBOMBCD = 4.5f;
    //ʹ����Ӱ��������ȴʱ��
    static float TIME_OF_IDLE_SHADOWBALLCD = 1.0f;
    //ʹ���갵Ӱץ�����ȴʱ��
    static float TIME_OF_IDLE_SHADOWCLAWCD = 1.0f;
    //ʹ�����̺����ȴʱ��
    static float TIME_OF_IDLE_RUSHCD = 10.0f;

    /// <summary>
    /// ����ʱ���ʱ��
    /// </summary>
    float IdleTimer;

    /// <summary>
    /// ���뷢��״̬
    /// </summary>
    /// <param name="idletime">����ʱ��</param>
    void IdleEnter(float idletime)
    {
        if (idletime != TIME_OF_IDLE_START) {
            StateIndex++;
            if (StateIndex > 5) { StateIndex = 0; }
        }
        IdleTimer = idletime;
        NowState = HaunterState.Idle;
    }

    /// <summary>
    /// ��������״̬
    /// </summary>
    void IdleOver()
    {
        IdleTimer = 0;
    }

    //=============================����״̬==============================




















    //=========����ը��״̬==========

    //����ը���Ĺ���
    static float TIME_OF_ATK_SLUDGEBOMB = 0.2f;

    /// <summary>
    /// ����ը��ʱ���ʱ��
    /// </summary>
    float SludgeBombTimer;

    /// <summary>
    /// ����ը��Ԥ�Ƽ�
    /// </summary>
    public HaunterSludgeBomb sludgeBomb;

    /// <summary>
    /// ��ʼ����ը��״̬
    /// </summary>
    void SludgeBombEnter()
    {
        animator.SetBool("Atk", true);
        SludgeBombTimer = TIME_OF_ATK_SLUDGEBOMB;
        NowState = HaunterState.SludgeBomb;
    }

    /// <summary>
    /// ��������ը��״̬
    /// </summary>
    void SludgeBombOver()
    {
        SludgeBombTimer = 0;
        IdleEnter(TIME_OF_IDLE_SLUDGEBOMBCD);
    }

    /// <summary>
    /// ��������ը��
    /// </summary>
    void SludgeBombLunch()
    {
        float Angle = isEmptyConfusionDone ? 45.0f : 22.0f;
        Vector2 dir1 = (TargetPosition - (Vector2)transform.position).normalized;
        Vector2 dir2 = Quaternion.AngleAxis( Angle, Vector3.forward) * dir1;
        Vector2 dir3 = Quaternion.AngleAxis(-Angle, Vector3.forward) * dir1;
        float distence = Vector2.Distance(TargetPosition, (Vector2)transform.position);
        HaunterSludgeBomb sb1 = Instantiate(sludgeBomb, transform.position + Vector3.up * 0.3f + (Vector3)dir1 * 1.2f, Quaternion.identity, ParentPokemonRoom.transform);
        HaunterSludgeBomb sb2 = Instantiate(sludgeBomb, transform.position + Vector3.up * 0.3f + (Vector3)dir2 * 1.2f, Quaternion.identity, ParentPokemonRoom.transform);
        HaunterSludgeBomb sb3 = Instantiate(sludgeBomb, transform.position + Vector3.up * 0.3f + (Vector3)dir3 * 1.2f, Quaternion.identity, ParentPokemonRoom.transform);
        sb1.SetNewSludgeBomb(this, dir1, distence * 0.8f);
        sb2.SetNewSludgeBomb(this, dir2, distence * 1.5f);
        sb3.SetNewSludgeBomb(this, dir3, distence * 1.5f);
    }

    //=========����ը��״̬==========






























    //=========��Ӱצ״̬==========
    //��Ӱצ�Ĺ���
    static float TIME_OF_ATK_SHADOWCLAW = 0.45f;

    //��Ӱצ�Ĺ�����
    public static int DMAGE_SHADOWCLAW = 80;

    /// <summary>
    /// ��Ӱצʱ���ʱ��
    /// </summary>
    float ShadowClawTimer;

    /// <summary>
    /// ��ӰצԤ�Ƽ�
    /// </summary>
    public HaunterShadowClaw shadowClaw;

    //��Ӱצ�Ƿ�����
    bool isShadowClawStart;

    /// <summary>
    /// ��ʼ��Ӱצ״̬
    /// </summary>
    void ShadowClawEnter()
    {
        animator.SetBool("Atk", true);
        ShadowClawTimer = TIME_OF_ATK_SHADOWCLAW;
        NowState = HaunterState.ShadowClaw;
        Debug.Log("XXX" + ShadowClawTimer);
    }

    /// <summary>
    /// ������Ӱצ״̬
    /// </summary>
    void ShadowClawOver()
    {
        ShadowClawTimer = 0;
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        isShadowClawStart = false;
        IdleEnter(TIME_OF_IDLE_SHADOWCLAWCD);
    }

    Vector3 start;

    /// <summary>
    /// ���䰵Ӱצ
    /// </summary>
    void ShadowClawLunch()
    {
        HaunterShadowClaw sc = Instantiate(shadowClaw, transform.position, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Director, Vector2.right) + 90.0f), transform);
        sc.ParentHaunter = this;
        //������Ӱ
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
        isShadowClawStart = true;
        start = transform.position;
        Debug.Log("XXX" + ShadowClawTimer);
    }

    //=========��Ӱצ״̬==========






























    //=========Ӱ����״̬==========

    //Ӱ����������Ƕ�
    static float SHADOWBALL_MAX_ANGLE = 50.0f;
    //Ӱ����������Ƕ�
    static float SHADOWBALL_SPACING = 0.08f;
    //Ӱ���������
    static int SHADOWBALL_COUNT = 9;
    //Ӱ�����ٶ�
    static float SHADOWBALL_SPEED = 9.0f;


    /// <summary>
    /// Ӱ����Ԥ�Ƽ�
    /// </summary>
    public HaunterShadowBall shadowBall;

    List<HaunterShadowBall> ShadowBallList = new List<HaunterShadowBall> { };

    /// <summary>
    /// ��ʼӰ����״̬
    /// </summary>
    void ShadowBallEnter()
    {
        animator.SetBool("Atk", true);
        NowState = HaunterState.ShadowBall;
    }

    /// <summary>
    /// ����Ӱ����״̬
    /// </summary>
    void ShadowBallOver()
    {
        IdleEnter(TIME_OF_IDLE_SHADOWBALLCD);
    }

    /// <summary>
    /// ����Ӱ����
    /// </summary>
    void ShadowBallLunch()
    {
        //sb1.LaunchNotForce(Director, 10);
        ShadowBallList.Clear();
        Vector2 v = new Vector2(Director.x, ((TargetPosition - (Vector2)transform.position).y > 0) ? 1.0f : -1.0f);
        //Debug.Log(v + string.Join("," , ).ToString());
        List<float> AngleList = ShadowBallAngle(SHADOWBALL_COUNT, SHADOWBALL_MAX_ANGLE, v);
        StartCoroutine(InstantiateShadowBall(AngleList, SHADOWBALL_SPACING));
    }



    IEnumerator InstantiateShadowBall(List<float> AngleList, float Spacing)
    {
        for (int i = 0; i < AngleList.Count; i++)
        {
            Vector3 v = Quaternion.AngleAxis(AngleList[i], Vector3.forward) * Vector3.right;
            HaunterShadowBall sb1 = Instantiate(shadowBall, transform.position + v * 2.5f, Quaternion.identity, ParentPokemonRoom.transform);
            sb1.empty = this;
            ShadowBallList.Add(sb1);
            sb1.direction = v;
            yield return new WaitForSeconds(Spacing);
        }
        StopCoroutine(InstantiateShadowBall(AngleList, Spacing));
    }

    /// <summary>
    /// �������е�Ӱ����
    /// </summary>
    void LunchAllShadowBall()
    {
        for (int i = 0; i < ShadowBallList.Count; i++)
        {
            ShadowBallList[i].LaunchNotForce(ShadowBallList[i].direction , SHADOWBALL_SPEED);
        }
    }


    /// <summary>
    /// Ӱ�������еķ���Ƕ�
    /// </summary>
    /// <param name="Count">Ӱ������Ĵ���</param>
    /// <param name="MaxAngle">Ӱ��������Ƕ�</param>
    /// <param name="StartQuadrant">Ӱ������ĳ�ʼ����</param>
    /// <returns></returns>
    List<float> ShadowBallAngle(int Count , float MaxAngle , Vector2 StartQuadrant)
    {
        int SBCount = isEmptyConfusionDone ? (Count - 2) : Count;
        List<float> output = new List<float> { };
        float PerAngle = (MaxAngle * 2.0f) / (float)(SBCount - 1);
        float StartAngle = MaxAngle;
        if      (StartQuadrant.x > 0 && StartQuadrant.y > 0) { PerAngle = -PerAngle; }
        else if (StartQuadrant.x > 0 && StartQuadrant.y < 0) { StartAngle = -StartAngle; }
        else if (StartQuadrant.x < 0 && StartQuadrant.y > 0) { StartAngle = -(StartAngle + 180.0f); }
        else if (StartQuadrant.x < 0 && StartQuadrant.y < 0) { StartAngle = (StartAngle + 180.0f); PerAngle = -PerAngle; }
        for (int i = 0; i < SBCount; i++)
        {
            output.Add(StartAngle + ((float)i)* PerAngle);
        }
        return output;
    }


    //=========Ӱ����״̬==========














    //=========���״̬==========

    //��Ӱ��̵Ĺ�����
    public static int DMAGE_SHADOWRUSH = 110;
    //��Ӱ��̵Ĺ���ʱ��
    static float TIME_OF_ATK_SHADOWRUSH = 4.0f;

    /// <summary>
    /// ��Ӱ���ʱ���ʱ��
    /// </summary>
    float ShadowRushTimer;

    /// <summary>
    /// ��Ӱ���Ԥ�Ƽ�
    /// </summary>
    public HaunterShadowClaw shadowRush;

    //��Ӱ����Ƿ�����
    bool isShadowRushStart;

    /// <summary>
    /// ��ʼ��Ӱ���״̬
    /// </summary>
    void ShadowRushEnter()
    {
        animator.SetBool("Atk", true);
        ShadowRushTimer = TIME_OF_ATK_SHADOWRUSH;
        NowState = HaunterState.Rush;
    }

    /// <summary>
    /// ������Ӱ���״̬
    /// </summary>
    void ShadowRushOver()
    {
        ShadowClawTimer = 0;
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        isShadowRushStart = false;
        ShadowBallList.Clear();
        IdleEnter(TIME_OF_IDLE_RUSHCD);
    }


    /// <summary>
    /// ���䰵Ӱ���
    /// </summary>
    void ShadowRushLunch()
    {
        HaunterShadowClaw sc = Instantiate(shadowClaw, transform.position, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Director, Vector2.right) + 90.0f), transform);
        sc.ParentHaunter = this;
        //������Ӱ
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
        isShadowRushStart = true;
    }
    //=========���״̬==========



















}
