using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glalie : Empty
{
    Vector2 Director;//���˳���
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
}
