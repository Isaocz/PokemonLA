using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEmpty : Empty
{
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
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint)/* * 1.2f */;//�趨������
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint)/* * 1.2f */;//�趨�ط�
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//�趨�ٶ�
        Exp = BaseExp * Emptylevel / 7;//�趨���ܺ��ȡ�ľ���

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();



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

            /**
            //�����Ȼ����ȷʵĿ��λ��
            if (!isEmptyInfatuationDone || _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile()).Count <= 1 || InfatuationForDistanceEmpty() == null)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }
            **/
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





    /**
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
    //������������������������������������������ͨ��������������������������������������������
    **/






    /*boss
        
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

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Steel;
        EmptyType02 = Type.TypeEnum.Psychic;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.5f;
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.5f;
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isDie && !isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();


        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
    }











    */


}
