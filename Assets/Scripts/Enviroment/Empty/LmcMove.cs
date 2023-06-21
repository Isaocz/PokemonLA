using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LmcMove : Empty
{



    //����2�����ͱ�������ʾ�������������Ϊ1��������Ϊ-1����һ����ʾ��ʼ��x����
    float distance;
    int isLeft = -1;
    float FirstX;


    // Start is called before the first frame update
    void Start()
    {
        //������ë�棩��ʼ����ë����ƶ�����
        distance = 5.0f;

        //---���������е������ࣩ---����ʼ�������ƶ��ٶȣ�����
        speed = 2.0f;
        EmptyType01 = Type.TypeEnum.Bug;
        EmptyType02 = 0;

        //---���������е������ࣩ---����ȡ���
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();

        //---���������е������ࣩ---�����õȼ�
        Emptylevel = SetLevel(player.Level,15);
        //---���������е������ࣩ---����ʼ������ֵ�Ϳɻ�þ���
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint= AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp*Emptylevel/7;


        //---���������е������ࣩ---����ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //������ë�棩��ë��ĳ�ʼλ��
        FirstX = rigidbody2D.position.x;
        

    }

    // Update is called once per frame
    void Update()
    {
        //---���������е������ࣩ---�������ҽ��������»�ȡ���
        ResetPlayer();

        //---���������е������ࣩ---�����������ɶ�����Ϻ�
        if (!isBorn)
        {
            //ÿ֡�������Ƿ�����
            EmptyDie();
            //ÿ֡�����˲����Ƿ���
            StateMaterialChange();
            //ÿ֡�������Ƿ��ж�
            UpdateEmptyChangeHP();
        }
    }

    private void FixedUpdate()
    {
        //---���������е������ࣩ---�������ҽ��������»�ȡ���
        ResetPlayer();
        if (!isBorn)
        {
            //������ë�棩��ë����ƶ���ʽ
            if (!isDie && !isHit)
            {
                
                //��ȡ��ǰ�������꣬����ǰx����ͳ�ʼx�������С���趨�õ��ƶ�����ʱ�������ƶ� ������ʱ�����ó�ʼλ�ã�����ת
                Vector2 position = rigidbody2D.position;
                if (Mathf.Abs(position.x - FirstX) < distance)
                {
                    position.x = position.x + speed * isLeft * Time.deltaTime;
                    rigidbody2D.position = position;

                }
                else
                {
                    FirstX = position.x;
                    isLeft = -isLeft;
                    animator.SetTrigger("turn");
                }
            }
            //---���������е������ࣩ---���������Ƿ񱻻���
            EmptyBeKnock();
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        //---���������е������ࣩ---����������Һ������ײ�˺�
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
    }
}
