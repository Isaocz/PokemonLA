using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cetoddle : Empty
{

    MyAstarAI AI;//Ѱ·A*ai




    Vector2 Director;//���˳���
    Vector3 LastPosition;//���㵱ǰ�ٶ�,����ʱ�����õ���һʱ�䵥λ��λ������,ͨ��Я��ִ��

    float DanceStartCDTimer;//��β����ʹ�ú��´�ʹ������ĵ�cd��ȴ��ʱ��
    float TIMEofDanceStartCD = 1.5f;//��β����ʹ�ú��´�ʹ������ĵ�cd��ȴʱ��

    float CDTimer;//ʹ�ü��ܺ�ԭ�ط����ļ�ʱ��
    float TIMEofDanceOverCD = 5.0f;//��β����ʹ�ú�ԭ�ط����ĵ�cd��ȴʱ��
    float TIMEofSingOverCD = 9.0f;//����ʹ�ú�ԭ�ط����ĵ�cd��ȴʱ��

    //����ʱ�ļ���Ȧ
    public CetoddleDanceCircle danceCircle;
    CetoddleDanceCircle danceCircleObj;

    //����ʱ�ļ���Ȧ
    public EmptyEchoedVoice singCircle;
    EmptyEchoedVoice singCircleObj;

    enum State //״̬��
    {
        Normal,//ƽ��״̬��׷������
        Sing,//����״̬
        Dance,//���裨ҡβ�ͣ�״̬
        CDState//���ܽ�������ȴ״̬
    };
    State NowState;//��ǰ״̬


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
        //��ȡѰ·ai
        AI = transform.GetComponent<MyAstarAI>();
        //�������㷽��Я��
        StartCoroutine(CheckLook());
        //��ʼ��״̬��
        NowState = State.Normal;

        animator.SetFloat("Speed", 0.0f);
        animator.SetFloat("LookX", 0.0f);
        animator.SetFloat("LookY", -1.0f);



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
        }
    }

    private void FixedUpdate()
    {
        ResetPlayer();//�����������ʧ�����»�ȡ
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//������Ȼ󣬼����Ȼ�ʱ��
        if (!isDie && !isBorn)//��������������״̬�����ڳ���״̬ʱ
        {
            EmptyBeKnock();//�ж��Ƿ񱻻���

            //������˯�� ���� ��Ĭ״̬ʱ
            if (!isSleepDone &&!isEmptyFrozenDone && !isSilence && !(isFearDone && NowState != State.Normal))
            {
                if (!isCanNotMoveWhenParalysis)
                {
                    //һ��״̬ʱ��������cdʱ��
                    if (NowState == State.Normal)
                    {
                        if (DanceStartCDTimer > 0) { DanceStartCDTimer -= Time.deltaTime; if (DanceStartCDTimer <= 0) { DanceStartCDTimer = 0; } }//����cd
                        if (DanceStartCDTimer == 0 && (AI.targetPosition.position - transform.position).magnitude <= 4.2f && !AI.isCanNotMove && !isFearDone)
                        {
                            DanceStartCDTimer = TIMEofDanceStartCD;
                            AI.isCanNotMove = true;
                            animator.SetBool("Dance" , true);
                            NowState = State.Dance;
                            animator.SetFloat("Speed", 0);
                        }
                    }

                    //��������󣬼��㷢��cdʱ��
                    if (NowState == State.CDState)
                    {
                        if (CDTimer > 0) { CDTimer -= Time.deltaTime; if (CDTimer <= 0) { CDTimer = 0; } }//����cd
                        if (CDTimer == 0 && !isFearDone)
                        {
                            AI.isCanNotMove = false;
                            NowState = State.Normal;
                        }
                    }
                }

            }
            else if (isFearDone && NowState != State.Normal)
            {
                if (NowState == State.Dance)
                {
                    animator.SetBool("Dance", false);
                    NowState = State.Normal;
                }
                if (NowState == State.Sing)
                {
                    animator.SetBool("Sing", false);
                    NowState = State.CDState;
                }
                if (danceCircleObj != null) { Destroy(danceCircleObj.gameObject); }
                if (singCircleObj != null) { Destroy(singCircleObj.gameObject); }
                animator.SetBool("Dance", false);
                animator.SetBool("Sing", false);
                AI.isCanNotMove = false;
                NowState = State.Normal;
            }
            //˯�� ���� ��Ĭʱ�����ж���
            else
            {
                if (danceCircleObj != null) { Destroy(danceCircleObj.gameObject); }
                if (singCircleObj != null) { Destroy(singCircleObj.gameObject); }
                if (NowState == State.Sing) { SetSingOver(); }
                animator.SetBool("Dance", false);
                animator.SetBool("Sing", false);
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public IEnumerator CheckLook()
    {
        while (true)
        {
            //һ��״̬ʱ�����ٶȺͳ���
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && !AI.isCanNotMove && NowState == State.Normal)
            {
                //���ݵ�ǰλ�ú���һ��FixedUpdate����ʱ��λ�ò�����ٶ�
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //���ݵ�ǰλ�ú���һ��FixedUpdate����ʱ��λ�ò���㳯�� �������������
                Director = _mTool.MainVector2((transform.position - LastPosition));
                animator.SetFloat("LookX", Director.x);
                animator.SetFloat("LookY", Director.y);
                //Debug.Log(Director);
                //����λ��
                LastPosition = transform.position;
            }
            yield return new WaitForSeconds(0.1f);
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

    public void CallCanMove()
    {
        //AI.canMove = true;
        animator.SetFloat("Speed", 0.0f);
    }


    //=========================ҡβ���������=============================

    public void SetDanceOver()
    {
        if (NowState == State.Dance)
        {
            animator.SetBool("Dance", false);
            NowState = State.CDState;
            CDTimer = TIMEofDanceOverCD;
            
        }
    }

    //��ʼ���� ���ü���Ȧ
    public void StartDance()
    {
        if (animator.GetBool("Dance")) {
            danceCircleObj = Instantiate(danceCircle, this.transform.position, Quaternion.identity, transform);
            danceCircleObj.ParentEmpty = this;
        }
    }

    //=========================ҡβ���������=============================









    //=========================����������=============================

    /// <summary>
    /// ʹ�û���
    /// </summary>
    public override void UseEchoedVoice(int echoedVoiceLevel)
    {
        Debug.Log(name + "+" + transform.position + "+" + echoedVoiceLevel);
        animator.SetBool("Sing" , true);
        NowState = State.Sing;
        AI.isCanNotMove = true;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("LookX", 0);
        animator.SetFloat("LookY", -1);

        singCircleObj = Instantiate(singCircle, this.transform.position, Quaternion.identity, transform);
        singCircleObj.SetEchoedVoiceLevel(echoedVoiceLevel);
        singCircleObj.ParentEmpty = this;
    }

    public void SetSingOver()
    {
        if (NowState == State.Sing)
        {
            animator.SetBool("Sing", false);
            NowState = State.CDState;
            CDTimer = TIMEofSingOverCD;

        }
    }


    /// <summary>
    /// �ж���ǰ�����Ƿ���Ի���
    /// </summary>
    public override bool isEchoedVoiceisReady()
    {
        if (NowState == State.Normal && !isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) { return true; }
        else { return false; }
    }

    //=========================����������=============================
}
