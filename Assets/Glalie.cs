using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glalie : Empty
{
    //����״̬��
    enum State
    {
        Idle, //ԭ�ط���
        Bite, //��ҧ
    }
    State NowState;

    float BiteRushTimer;

    float RUSHTIME;

    public bool isBiteMove;

    Vector2 Director = new Vector2(1,1);//���˳���

    //��̵Ĵ���
    int BiteRushCount;
    //����̴���
    int MAX_RUSHCOUNT = 3;


    /// <summary>
    /// ������ų�������Ԥ�Ƽ�
    /// </summary>
    public GlalieIceShard iceShard;

    /// <summary>
    /// ҧ��Ķ���Ԥ�Ƽ�
    /// </summary>
    public GameObject Crunch;
    /// <summary>
    /// ҧ��Ԥ�Ƽ�
    /// </summary>
    public List<GameObject> CrunchOBJ = new List<GameObject> { };
    /// <summary>
    /// ������ʱ��
    /// </summary>
    public float IdleTimer;


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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);//�趨������
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);//�趨�ط�
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//�趨�ٶ�
        Exp = BaseExp * Emptylevel / 7;//�趨���ܺ��ȡ�ľ���

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //�����ƶ�����
        Director = new Vector2(((transform.localPosition.x) <= 0 ? 1 : -1), ((transform.localPosition.y) <= 0 ? 1 : -1));
        //���ö���������
        animator.SetFloat("LookX" , Director.x);
        animator.SetFloat("LookY" , Director.y);

        IdleState(0.3f);

        DestoryEvent += DieBlest;



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
            if (!isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone)
            {
                switch (NowState)
                {
                    case State.Bite:
                        if (isBiteMove)
                        {
                            //������Ӱ
                            if (ShadowCoroutine == null)
                            {
                                RUSHTIME = setrushtime();
                                StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
                                CrunchOBJ.Add(Instantiate(Crunch, transform.position + Vector3.up * 0.6f, Quaternion.identity, transform));
                            }
                            //���
                            rigidbody2D.position = new Vector2(
                                Mathf.Clamp(rigidbody2D.position.x
                                    + (float)Director.x * Time.deltaTime * speed * 5.5f,                    //����*�ٶ�
                                ParentPokemonRoom.RoomSize[2] - 0.0f + transform.parent.position.x, //��Сֵ
                                ParentPokemonRoom.RoomSize[3] + 0.0f + transform.parent.position.x),//���ֵ
                                Mathf.Clamp(rigidbody2D.position.y
                                    + (float)Director.y * Time.deltaTime * speed * 5.5f,                     //����*�ٶ� 
                                ParentPokemonRoom.RoomSize[1] - 0.0f + transform.parent.position.y,  //��Сֵ
                                ParentPokemonRoom.RoomSize[0] + 0.0f + transform.parent.position.y));//���ֵ

                            //��ʱ��
                            BiteRushTimer += Time.deltaTime;
                            if (BiteRushTimer >= ((isEmptyConfusionDone)?RUSHTIME : 0.15f))
                            {
                                SetRushDir();
                                if (ShadowCoroutine != null)
                                {
                                    StopShadowCoroutine();
                                }
                                BiteRushTimer = 0;
                                BiteRushCount++;
                                isBiteMove = false;
                                if (BiteRushCount >= MAX_RUSHCOUNT)
                                {
                                    animator.SetInteger("isRushOver" , 2);
                                    //StartCoroutine(IdleForSecoends(9.0f));
                                }
                                else
                                {
                                    animator.SetInteger("isRushOver", 1);
                                }
                            }

                        }
                        else
                        {
                            //�رղ�Ӱ
                            if (ShadowCoroutine != null)
                            {
                                StopShadowCoroutine();
                            }
                            
                        }
                        break;
                    case State.Idle:
                        IdleTimer -= Time.deltaTime;
                        if (IdleTimer <= 0.0f)
                        {
                            StartRush();
                            IdleTimer = 0.0f;
                        }
                        break;
                }
            }
            if ((isSleepDone || isFearDone) && NowState != State.Idle)
            {
                IdleState(3.0f);
                animator.SetInteger("isRushOver", 2);
                animator.SetTrigger("BiteOver");
                //�رղ�Ӱ
                if (ShadowCoroutine != null)
                {
                    StopShadowCoroutine();
                }
            }

            //���ö���������
            animator.SetFloat("LookX", Director.x);
            animator.SetFloat("LookY", Director.y);
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
            if (isBiteMove)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 80, 0, 0, PokemonType.TypeEnum.Dark);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 7.0f;
                    playerControler.KnockOutDirection = (new Vector2(Director.y, Director.x)).normalized;
                }
            }
            else
            {
                EmptyTouchHit(other.gameObject);//���������˺�
            }
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))//���Ȼ� ��������������ײʱ
        {
            if (isBiteMove)
            {
                Empty e = other.gameObject.GetComponent<Empty>();
                Pokemon.PokemonHpChange(this.gameObject, e.gameObject, 80, 0, 0, PokemonType.TypeEnum.Dark);
            }
            else
            {
                InfatuationEmptyTouchHit(other.gameObject);//�����Ȼ�����˺�
            }
        }
    }

    float setrushtime()
    {
        if (isEmptyConfusionDone) { return Random.Range(0.04f, 0.24f); }
        else                      { return 0.15f; }
    }

    /// <summary>
    /// ���뷢��״̬
    /// </summary>
    public void IdleState(float Idletime)
    {
        NowState = State.Idle;
        isBiteMove = false;
        BiteRushCount = 0;
        animator.ResetTrigger("Bite");
        IdleTimer = Idletime;
    }
    
    
    public void StartRush()
    {
        NowState = State.Bite;
        animator.SetTrigger("Bite");
        RUSHTIME = setrushtime();
        //�����ƶ�����
        Director = new Vector2(((transform.localPosition.x) <= 0 ? 1 : -1), ((transform.localPosition.y) <= 0 ? 1 : -1));
        //���ö���������
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);
    }

    /*
    /// <summary>
    /// ������ȴ
    /// </summary>
    /// <param name="idleTime">����ʱ��</param>
    /// <returns></returns>
    IEnumerator IdleForSecoends(float idleTime)
    {
        NowState = State.Idle;
        isBiteMove = false;
        BiteRushCount = 0;
        animator.ResetTrigger("Bite");
        yield return new WaitForSeconds(idleTime);
        NowState = State.Bite;
        animator.SetTrigger("Bite");
        //�����ƶ�����
        Director = new Vector2(((transform.localPosition.x) <= 0 ? 1 : -1), ((transform.localPosition.y) <= 0 ? 1 : -1));
        //���ö���������
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);
    }
    */

    public void BiteShakeStart()
    {
        NowState = State.Bite;
        isBiteMove = false;
        
    }

    public void BiteRushStart()
    {
        NowState = State.Bite;
        animator.SetInteger("isRushOver", 0);
        isBiteMove = true;
        
    }





    /// <summary>
    /// ���ó�̷���
    /// </summary>
    void SetRushDir()
    {
        Vector2 dir1 = Quaternion.AngleAxis( 90, Vector3.forward) * Director;
        Vector2 dir2 = Quaternion.AngleAxis(-90, Vector3.forward) * Director;
        RaycastHit2D raycastHit2D1 = Physics2D.Raycast(transform.position, dir1);
        RaycastHit2D raycastHit2D2 = Physics2D.Raycast(transform.position, dir2);
        if (raycastHit2D1.distance >= raycastHit2D2.distance)
        {
            Director = dir1;
            Debug.Log(dir1);
        }
        else
        {
            Director = dir2;
            Debug.Log(dir2);
        }

        

        //���ö���������
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);

        
        
    }



    /// <summary>
    /// ����������ը
    /// </summary>
    void DieBlest()
    {
        if (IsDeadrattle) {
            GlalieIceShard i1 = Instantiate(iceShard, transform.position, Quaternion.Euler(0, 0, 90), ParentPokemonRoom.transform);
            i1.SetNewIceShard(0, this);
            GlalieIceShard i2 = Instantiate(iceShard, transform.position, Quaternion.Euler(0, 0, 210), ParentPokemonRoom.transform);
            i2.SetNewIceShard(0, this);
            GlalieIceShard i3 = Instantiate(iceShard, transform.position, Quaternion.Euler(0, 0, 330), ParentPokemonRoom.transform);
            i3.SetNewIceShard(0, this);
        }
    }

}
